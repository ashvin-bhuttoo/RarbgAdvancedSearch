using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CG.Web.MegaApiClient;
using Microsoft.VisualBasic.Logging;

namespace RarbgAdvancedSearch
{
    public partial class frmDownload : Form
    {
        private ContentTracker.ContentTrack g_ctrack;
        private ContentTracker g_ctracker;
        private Uri g_dlUri = null;
        private List<INode> g_dlNodes = new List<INode>();
        private long g_totalDownloadSizeBytes = 0, g_completedSize = 0;
        private INode g_rootNode;
        private MegaApiClient g_megaCli;
        private CancellationTokenSource g_cancelSrc = new CancellationTokenSource();
        private Task g_currentDownloadTask;
        private INode g_currentDownloadFile;
        private string g_downloadRootDir = string.Empty;

        public frmDownload(ref ContentTracker ctracker, ContentTracker.ContentTrack ctrack)
        {
            InitializeComponent();
            try
            {
                this.g_ctrack = ctrack;
                this.g_ctracker = ctracker;
                ctracker.savetrack(ctrack.entry, ContentTracker.Status.MarkedForDownload);
                g_dlUri = new Uri($"https://mega.nz/{ctrack.dd_url}");
                g_megaCli = new MegaApiClient();
            }
            catch(Exception ex)
            {
                rtbLog.AppendText($"Error: Download Failure \n{ex.Message + "\n" + ex.StackTrace}", Color.LightCoral, this);
                UsageStats.Log("download_failure", g_dlUri.AbsolutePath + "\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        void DisplayNodesRecursive(IEnumerable<INode> nodes, INode parent, ref long size, int level = 0)
        {
            IEnumerable<INode> children = nodes.Where(x => x.ParentId == parent.Id);
            foreach (INode child in children)
            {
                size += child.Size;
                string infos = $"- {child.Name} - {child.Size} bytes";                
                rtbLog.AppendText(infos.PadLeft(infos.Length + level, '\t') + "\n", Color.LightGreen, this);
                Application.DoEvents();
                if (child.Type == NodeType.Directory)
                {
                    DisplayNodesRecursive(nodes, child, ref size, level + 1);
                }
            }
        }

        string getNodePath(IEnumerable<INode> nodes, INode current, string path = "")
        {
            if(!string.IsNullOrEmpty(current.ParentId))
            {
                INode parent = nodes.Single(n => n.Id == current.ParentId);
                path = "\\" + parent.Name + ( current.Type == NodeType.File ? (  "\\" + current.Name) : "" ) + path;
                return getNodePath(nodes, parent, path);
            }

            return string.IsNullOrEmpty(path) ? current.Name : path;
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {
            txtDLdir.Text = Utils.Reg.dl_dir;
            g_downloadRootDir = txtDLdir.Text;

            if (g_dlUri == null)
                return;

            Task.Run(async () =>
            {
                try
                {
                    rtbLog.AppendText($"Fetching Download Details..\n", Color.LightGreen, this);
                   
                    this.PerformSafely(() => {
                        lblDLProgress.Text = "Waiting for download details..";
                        lblDLProgress.ForeColor = Color.DarkGoldenrod;
                    });

                    await g_megaCli.LoginAnonymousAsync().ContinueWith(task =>
                    {
                        IEnumerable<INode> nodes = g_megaCli.GetNodesFromLink(g_dlUri);
                        if (nodes.Count() > 0)
                        {
                            g_dlNodes = nodes.ToList();
                            INode parent = nodes.Single(n => n.Type == NodeType.Root);
                            g_rootNode = parent;
                            DisplayNodesRecursive(nodes, parent, ref g_totalDownloadSizeBytes);
                            this.PerformSafely(() => { lblSize.Text = $"{displaySizeStr(g_totalDownloadSizeBytes)}"; lblSize.ForeColor = Color.ForestGreen; });
                            this.PerformSafely(() => btnStartDownload.Enabled = true);
                            this.PerformSafely(() => { lblDLProgress.Text = "Please start download.."; lblDLProgress.ForeColor = Color.RoyalBlue; });
                        }
                        else
                        {
                            this.PerformSafely(() => lblDLProgress.Text = "Failed!");
                            rtbLog.AppendText($"Error: Bad Link, No Files found..\n", Color.LightCoral, this);
                            UsageStats.Log("download_failure", g_dlUri.AbsolutePath + ", nodes.Count() = 0");
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.PerformSafely(() => lblDLProgress.Text = "Failed!");
                    rtbLog.AppendText($"Error: Download Failure \n{ex.Message + "\n" + ex.StackTrace}\n", Color.LightCoral, this);
                    UsageStats.Log("download_failure", g_dlUri.AbsolutePath + "\n" + ex.Message + "\n" + ex.StackTrace);
                }
            });            
        }

        private string displaySizeStr(long bytes)
        {
            if((double)bytes/1024 < 1)
            {
                return $"{bytes} bytes";
            }
            bytes /= 1024;
            if ((double)bytes / 1024 < 1)
            {
                return $"{bytes} KB";
            }
            bytes /= 1024;
            if ((double)bytes / 1024 < 1)
            {
                return $"{bytes} MB";
            }
            return $"{Math.Round((double)bytes / 1024, 2)} GB";
        }

        private void btnStartDownload_Click(object sender, EventArgs e)
        {
            btnChgDir.Enabled = false;

            if(btnStartDownload.Text == "Start Download")
            {
                btnStartDownload.Text = "Cancel";
                btnStartDownload.ForeColor = Color.LightCoral;
            }           
            else if(btnStartDownload.Text == "Open Folder")
            {
                Process.Start(g_downloadRootDir);
                return;
            }
            else
            {
                btnStartDownload.Enabled = false;
                g_cancelSrc.Cancel();

                Task.Run(async () =>
                {
                    //10Sec cancellation timeout
                    for (int i = 0; i < 5000; i++)
                    {
                        Thread.Sleep(2);
                    }

                    g_currentDownloadTask = null;
                });

                lblDLProgress.Text = "Cancelling!";
                lblDLProgress.ForeColor = Color.OrangeRed;
                return;
            }

            if(g_dlNodes.Count > 0)
            {
                try
                {
                    g_downloadRootDir = $"{Utils.Reg.dl_dir}\\{getNodePath(g_dlNodes, g_rootNode)}";

                    //Delete Existing If Downloaded
                    if (Directory.Exists(g_downloadRootDir))
                    {
                        Directory.Delete($"{g_downloadRootDir}", true);
                    }

                    Directory.CreateDirectory(g_downloadRootDir);
                    rtbLog.AppendText($"Created download directory... \n{$"{g_downloadRootDir}"}\n", Color.LightGreen, this);

                    CancellationToken cancel_token = g_cancelSrc.Token;
                    foreach (INode node in g_dlNodes.Where(x => x.Type == NodeType.File))
                    {
                        g_currentDownloadFile = node;
                        rtbLog.AppendText($"Downloading {node.Name}.. ({displaySizeStr(node.Size)})\n", Color.LightGreen, this);
                        var progressIndicator = new Progress<double>(ReportProgress);

                        string dld_path = $"{Utils.Reg.dl_dir}{getNodePath(g_dlNodes, node)}";
                        System.IO.FileInfo file = new System.IO.FileInfo(dld_path);
                        file.Directory.Create();

                        g_currentDownloadTask = g_megaCli.DownloadFileAsync(node, dld_path, progressIndicator, cancel_token);
                        while (g_currentDownloadTask != null && !g_currentDownloadTask.IsCanceled && g_currentDownloadTask.Status != TaskStatus.Running)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        while (g_currentDownloadTask != null && !g_currentDownloadTask.IsCanceled && g_currentDownloadTask.Status == TaskStatus.Running)
                        {
                            Thread.Sleep(1);
                            Application.DoEvents();
                        }
                        if (g_currentDownloadTask == null || g_currentDownloadTask.IsCanceled)
                        {
                            g_ctracker.savetrack(g_ctrack.entry, ContentTracker.Status.NotSet);
                            pbDownload.SetState(3);
                            pbDownload.BackColor = Color.OrangeRed;
                            lblDLProgress.Text = "Download Cancelled!";
                            rtbLog.AppendText($"Download Cancelled!\n", Color.LightCoral, this);
                            if (Directory.Exists(g_downloadRootDir))
                            {
                                rtbLog.AppendText($"Deleting Downloads... ", Color.LightCoral, this);
                                try
                                {
                                    Directory.Delete($"{g_downloadRootDir}", true);
                                    rtbLog.AppendText($"Downloads Deleted!", Color.LightCoral, this);
                                }
                                catch (Exception) {
                                    Process.Start(Application.ExecutablePath, "OVERRIDE_PROCESS_CHECK");
                                    Application.Exit();
                                    Environment.Exit(Environment.ExitCode);
                                }                                
                            }
                            return;
                        }
                        g_completedSize += node.Size;
                        pbDownload.Value = (int)((g_completedSize / g_totalDownloadSizeBytes) * 1000);
                    }
                    g_ctracker.savetrack(g_ctrack.entry, ContentTracker.Status.Downloaded);
                    lblDLProgress.Text = "Complete!";
                    btnStartDownload.Text = "Open Folder";
                    btnStartDownload.ForeColor = Color.DodgerBlue;
                    rtbLog.AppendText($"Downloads Complete!", Color.LightGreen, this);
                    lblDLProgress.ForeColor = Color.DarkGreen;
                    pbDownload.Value = 1000;
                }
                catch(Exception ex)
                {                   
                    this.PerformSafely(() => { lblDLProgress.Text = "Error!"; lblDLProgress.ForeColor = Color.DarkRed; } );
                    rtbLog.AppendText($"Error: Download Failure \n{ex.Message + "\n" + ex.StackTrace}\n", Color.LightCoral, this);
                    UsageStats.Log("download_failure", g_dlUri.AbsolutePath + "\n" + ex.Message + "\n" + ex.StackTrace);
                    pbDownload.SetState(1);
                }
                
            }
        }

        private void btnChgDir_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if(Utils.IsDirectoryWritable(fbd.SelectedPath))
                    {
                        Utils.Reg.dl_dir = fbd.SelectedPath;
                        txtDLdir.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        if( MessageBox.Show(this, $"You do not have write access to the path\n{fbd.SelectedPath}\nDo you wish to choose another ?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            btnChgDir_Click(sender, e);
                        }
                    }
                }
            }
        }

        private async void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(g_currentDownloadTask != null && g_currentDownloadTask.Status == TaskStatus.Running)
            {
                if( MessageBox.Show(this, "Download has already started, do you wish to cancel?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        this.PerformSafely(() => { btnStartDownload_Click(null, null); });
                    });                        
                }
            }
        }

        private void ReportProgress(double obj)
        {
            long currentDownloadBytes = (long)(g_completedSize + (g_currentDownloadFile.Size * (obj / 100)));
            double download_percent = ((double)currentDownloadBytes / g_totalDownloadSizeBytes) * 100;
            pbDownload.Value = (int)(download_percent * 10);
            lblDLProgress.Text = $"Downloaded {displaySizeStr(currentDownloadBytes)} ({Math.Round(download_percent, 2)}%)";
        }

        //private const int CP_NOCLOSE_BUTTON = 0x200;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, Form _frmx)
        {
            _frmx.PerformSafely(() => {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
            });            
        }
    }

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }

    public static class CrossThreadExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void PerformSafely<T1>(this Control target, Action<T1> action, T1 parameter)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, parameter);
            }
            else
            {
                action(parameter);
            }
        }

        public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }
    }
}
