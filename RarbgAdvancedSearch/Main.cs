using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RarbgAdvancedSearch.RarbgPageParser;

namespace RarbgAdvancedSearch
{

    [Serializable]
    public partial class Main : Form
    {
        List<rarbgEntry> saved_listings;

        public Main()
        {
            saved_listings = new List<rarbgEntry>();
            InitializeComponent();
            Text += $" v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(btnSearch.Text == "STOP")
            {
                btnSearch.Text = "SEARCH";
                btnSearch.ForeColor = Color.DarkGreen;
                return;
            }

            dgvListings.Rows.Clear();
            btnSearch.Text = "STOP";
            btnSearch.ForeColor = Color.DarkRed;

            if (categ1.CheckedItems.Count > 0 || categ2.CheckedItems.Count > 0 || categ3.CheckedItems.Count > 0 || categ4.CheckedItems.Count > 0)
            {
                string category = string.Empty, order = string.Empty;

                foreach(int index in categ1.CheckedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.XXX}", $"{(int)RarbgCategory.Movies_x264_1080p}", $"{(int)RarbgCategory.Movies_x265_4k}", $"{(int)RarbgCategory.Movies_x265_1080}", $"{(int)RarbgCategory.Music_MP3}", $"{(int)RarbgCategory.Games_PS3}" }[index]);
                }
                foreach (int index in categ2.CheckedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.Movies_XVID}", $"{(int)RarbgCategory.Movies_x264_720}", $"{(int)RarbgCategory.Movs_x265_4k_HDR}", $"{(int)RarbgCategory.TV_Episodes}", $"{(int)RarbgCategory.Music_FLAC}", $"{(int)RarbgCategory.Games_XBOX_360}" }[index]);
                }
                foreach (int index in categ3.CheckedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.Movies_XVID_720}", $"{(int)RarbgCategory.Movies_x264_3D}", $"{(int)RarbgCategory.Movies_Full_BD}", $"{(int)RarbgCategory.TV_HD_Episodes}", $"{(int)RarbgCategory.Games_PC_ISO}", $"{(int)RarbgCategory.Software_PC_ISO}" }[index]);
                }
                foreach (int index in categ4.CheckedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.Movies_x264}", $"{(int)RarbgCategory.Movies_x264_4k}", $"{(int)RarbgCategory.Movies_BD_Remux}", $"{(int)RarbgCategory.TV_UHD_Episodes}", $"{(int)RarbgCategory.Games_PC_RIP}", $"{(int)RarbgCategory.Games_PS4}" }[index]);
                }

                if (chkSearchOrder.Checked)
                {
                    order = $"&order={cmbSearchOrder.Tag.ToString().Split(new[] { ',' })[cmbSearchOrder.SelectedIndex]}&by={dudSearchOrder.SelectedItem.ToString()}";
                }

                int entryCount = 0;
                RarbgPageParser parser = new RarbgPageParser();
                int maxPage = 1;

                var searchurl = $"https://rarbgenter.org/torrents.php?search={txtSearch.Text.Trim()}{category}{order}";

                UsageStats.Log("search", searchurl);
                retry_get:
                string html = Utils.HttpClient.Get($"{searchurl}&page={99999999}");
                if (html.StartsWith("https://") && html.Contains("threat_defence"))
                {
                    tstStatus.Text = "Captcha validation..";
                    showMessage("Rarbg needs you to validate a captcha.\nPlease validate the captcha and press enter.", "Captcha Required");
                    browser b = new browser();
                    b.navigateTo(html);
                    this.Controls.Add(b);
                    b.BringToFront();
                    b.Dock = DockStyle.Fill;
                    while(this.Controls.Contains(b))
                    {
                        Application.DoEvents();
                        Thread.Sleep(1);
                    }
                    if (!b.IsDisposed)
                        b.Dispose();
                    goto retry_get;
                }

                if (!chkPageLimit.Checked && parser.getLastPageNum(html, ref maxPage))
                {
                    chkPageLimit.Checked = true;
                    nudPageLimit.Value = maxPage;
                }

                int pageNum;
                saved_listings.Clear();
                for (pageNum = 1; btnSearch.Text != "SEARCH"; pageNum++)
                {
                    string response = Utils.HttpClient.Get($"{searchurl}&page={pageNum}");
                    if(response.Contains("We have too many requests from your ip in the past 24h."))
                    {
                        showMessage("Failed to gather listings, IP temporarily banned by server for 2 hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        pageNum--;
                        btnSearch.Text = "SEARCH";
                        btnSearch.ForeColor = Color.DarkGreen;
                        break;
                    }
                    parser.parsePage(response);

                    if (parser.listings.Count > 0)
                    {
                        saved_listings.AddRange(parser.listings);
                        populateGrid(parser.listings, pageNum, ref entryCount);
                    }                        
                    else
                        pageNum--;

                    Application.DoEvents();

                    if (parser.listings.Count == 0 || (nudPageLimit.Enabled && pageNum == nudPageLimit.Value))
                    {
                        btnSearch.Text = "SEARCH";
                        btnSearch.ForeColor = Color.DarkGreen;
                        break;
                    }

                    if (nudPageLimit.Enabled)
                    {
                        tstProgress.Value = (int)((double)(pageNum / nudPageLimit.Value) * tstProgress.Maximum);
                    }

                    for (int k = 0; k < 10; k++)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        if (btnSearch.Text == "SEARCH")
                            break;
                    }
                }

                tstStatus.Text = $"Done.. Page {pageNum}, {entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";                
                tstProgress.Value = tstProgress.Maximum;
                reloadListingSorting();
            }
            else
            {
                btnSearch.Text = "SEARCH";
                btnSearch.ForeColor = Color.DarkGreen;
                showMessage("Please select a category first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }         
        }

        private void reloadListingSorting()
        {
            if(dgvListings.SortedColumn != null)
            {
                dgvListings.Sort(dgvListings.SortedColumn, dgvListings.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
        }

        private DialogResult showMessage(string text, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            return MessageBox.Show(this, text, caption, buttons, icon, defaultButton);
        }

        private void exportEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved_listings.Count > 0)
            {
                var uniqueCategories = (from dbo in saved_listings select dbo.category).Distinct().Aggregate("", (current, next) => current + "_" + next).TrimStart(new[] { '_'});
                string filename = $"{uniqueCategories}_export_{DateTime.Now.ToString().Replace("/","_").Replace(":","_").Replace(" ","_")}";
                
                               
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog1.Title = "Save Listing";
                //saveFileDialog1.CheckFileExists = true;
                saveFileDialog1.FileName = filename;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.Filter = "Xml files (*.xml)|*.xml";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Utils.Serialize(saveFileDialog1.FileName, saved_listings);
                    showMessage($"Listing saved succesfully to\n{saveFileDialog1.FileName}\n{saved_listings.Count} Entries Saved", "Export Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
            else
                showMessage($"There's nothing to export!", "Export Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void importEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saved_listings.Count > 0)
            {
                if (showMessage("This action will clear all current listings, do you wish to proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Title = "Load Listing",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dgvListings.Rows.Clear();
                saved_listings = Utils.Deserialize<List<rarbgEntry>>(openFileDialog1.FileName);
                int entryCount = 0;
                populateGrid(saved_listings, saved_listings.Count/25, ref entryCount, true);
                tstStatus.Text = $"Done.. Page {saved_listings.Count / 25}, {entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
                reloadListingSorting();
                showMessage($"Listing loaded succesfully!\n{entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed", "Import Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.DoEvents();
            }
        }

        private void populateGrid(List<rarbgEntry> entries, int pageCount, ref int entryCount, bool fromXML = false)
        {
            int totalEntryCount = entries.Count;
            foreach (var entry in entries)
            {
                if(entry.genre.Count > 0)
                    if(entry.genre.Any(g => !clbGenre.Items.Contains(g)))
                    {
                        clbGenre.Items.AddRange(entry.genre.Where(g => !clbGenre.Items.Contains(g)).ToArray());
                    }

                if (txtSearch.Text.Trim().Length > 0)
                {
                    if (!(entry.name.ToUpper().Contains(txtSearch.Text.Trim().ToUpper())))
                    {
                        goto skip_filters;
                        //entryCount++;
                        //continue;
                    }
                }

                //Custom Filter
                if (chkMinImdb.Checked || chkMinYear.Checked || chkMaxYear.Checked || chkMinUpDate.Checked || chkGenre.Checked)
                {
                    if(!chkMinImdb.Checked || entry.imdbRating >= (double)nudMinImdb.Value)
                    {
                        if(!chkMinYear.Checked || entry.year >= dtpMinYear.Value.Year)
                        {
                            if (!chkMaxYear.Checked || entry.year <= dtpMaxYear.Value.Year)
                            {
                                if(!chkMinUpDate.Checked || entry.dateAdded >= dtpMinUpDate.Value)
                                {
                                    if (!chkGenre.Checked || clbGenre.CheckedItems.Count == 0 || entry.genre.Any( g => clbGenre.CheckedItems.Contains(g)))
                                    {
                                        dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating });
                                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                                    }                                    
                                }                               
                            }                           
                        }                        
                    }
                }
                else
                {
                    dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating });
                    dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                }

                skip_filters:
                if (fromXML)
                    tstStatus.Text = $"Working.. Page {(int)(((double)entryCount / totalEntryCount) * pageCount)}, {entryCount++} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
                else
                    tstStatus.Text = $"Working.. Page {pageCount}, {entryCount++} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";

                if (entryCount % 25 == 0)
                {
                    if(fromXML)
                    {
                        tstProgress.Value = (int)(((double)entryCount / totalEntryCount) * tstProgress.Maximum);
                    }
                    Application.DoEvents();
                }
            }

            if(fromXML)
            {
                tstProgress.Value = tstProgress.Maximum;
            }
        }


        private void chkPageLimit_CheckedChanged(object sender, EventArgs e)
        {
            nudPageLimit.Enabled = chkPageLimit.Checked;
        }

        private void chkSearchOrder_CheckedChanged(object sender, EventArgs e)
        {
            cmbSearchOrder.Enabled = dudSearchOrder.Enabled = chkSearchOrder.Checked;
            cmbSearchOrder.SelectedIndex = 0;
            dudSearchOrder.SelectedIndex = 0;
        }

        private void dgvListings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                rarbgEntry entry = (rarbgEntry)dgvListings.Rows[e.RowIndex].Tag;
                Process.Start($"https://rarbgenter.org{entry.url}");
            }            
        }

        private void chkMinImdb_CheckedChanged(object sender, EventArgs e)
        {
            nudMinImdb.Enabled = chkMinImdb.Checked;
        }
        
        private void reloadGrid()
        {
            dgvListings.Rows.Clear();
            int entryCount = 0;
            populateGrid(saved_listings, saved_listings.Count / 25, ref entryCount, true);
            tstStatus.Text = $"Done.. Page {saved_listings.Count / 25}, {entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
            reloadListingSorting();
        }

        private void chkMinYear_CheckedChanged(object sender, EventArgs e)
        {
            dtpMinYear.Enabled = chkMinYear.Checked;
        }

        private void chkMaxYear_CheckedChanged(object sender, EventArgs e)
        {
            dtpMaxYear.Enabled = chkMaxYear.Checked;
        }

        private void chkMinUpDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpMinUpDate.Enabled = chkMinUpDate.Checked;
        }

        private void btnReapplyFilter_Click(object sender, EventArgs e)
        {
            reloadGrid();
        }

        private void chkGenre_CheckedChanged(object sender, EventArgs e)
        {
            clbGenre.Enabled = chkGenre.Checked;
            clbGenre.ForeColor = chkGenre.Checked ? SystemColors.ControlText : SystemColors.ControlDark;
        }

        private void clb_click(object sender, EventArgs e)
        {
            var clb = sender as CheckedListBox;
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (clb.GetItemRectangle(i).Contains(clb.PointToClient(MousePosition)))
                {
                    switch (clb.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            clb.SetItemCheckState(i, CheckState.Unchecked);
                            break;
                        case CheckState.Indeterminate:
                        case CheckState.Unchecked:
                            clb.SetItemCheckState(i, CheckState.Checked);
                            break;
                    }
                }
            }
        }

        private void tsbRESET_Click(object sender, EventArgs e)
        {
            if (showMessage("This action will clear all current listings and disable all filters.\nDo you wish to proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            saved_listings.Clear();
            dgvListings.Rows.Clear();
            clbGenre.Items.Clear();
            categ1.CheckedIndices.Cast<int>().All(i => { categ1.SetItemChecked(i, false); return true; });
            categ2.CheckedIndices.Cast<int>().All(i => { categ2.SetItemChecked(i, false); return true; });
            categ3.CheckedIndices.Cast<int>().All(i => { categ3.SetItemChecked(i, false); return true; });
            categ4.CheckedIndices.Cast<int>().All(i => { categ4.SetItemChecked(i, false); return true; });
            chkMinImdb.Checked = chkMinYear.Checked = chkMaxYear.Checked = chkMinUpDate.Checked = chkSearchOrder.Checked = chkPageLimit.Checked = chkGenre.Checked = false;
            tstProgress.Value = 0;
            tstStatus.Text = "Idle..";
        }

        private void tsbDonate_Click(object sender, EventArgs e)
        {
            Process.Start($"https://www.paypal.me/ABhuttoo");
        }

        private void dgvListings_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dgvListings.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    ContextMenu m = new ContextMenu();
                    dgvListings.Rows[currentMouseOverRow].Selected = true;

                    rarbgEntry entry = ((rarbgEntry)dgvListings.Rows[currentMouseOverRow].Tag);

                    if(!string.IsNullOrEmpty(entry.imdb_id))
                        m.MenuItems.Add(new MenuItem("> Open IMDb Page", delegate { Process.Start($"https://www.imdb.com/title/{entry.imdb_id}"); }));

                    if (!string.IsNullOrEmpty(entry.url))
                        m.MenuItems.Add(new MenuItem("> Open RaRBG Page", delegate { Process.Start($"https://rarbgenter.org{entry.url}"); }));

                    if (m.MenuItems.Count > 0)
                        m.Show(dgvListings, new Point(e.X, e.Y));
                }
            }
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            About aboutpage = new About();
            this.Controls.Add(aboutpage);
            aboutpage.BringToFront();
            aboutpage.Dock = DockStyle.Fill;            
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSearch.Text == "STOP")
            {
                Application.Exit();
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}
