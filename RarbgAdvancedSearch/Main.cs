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

                foreach(int index in categ1.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.XXX}", $"{(int)RarbgCategory.Movies_x264_1080p}", $"{(int)RarbgCategory.Movies_x265_4k}", $"{(int)RarbgCategory.Movies_x265_1080}", $"{(int)RarbgCategory.Music_MP3}", $"{(int)RarbgCategory.Games_PS3}" }[index]);
                }
                foreach (int index in categ2.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.Movies_XVID}", $"{(int)RarbgCategory.Movies_x264_720}", $"{(int)RarbgCategory.Movs_x265_4k_HDR}", $"{(int)RarbgCategory.TV_Episodes}", $"{(int)RarbgCategory.Music_FLAC}", $"{(int)RarbgCategory.Games_XBOX_360}" }[index]);
                }
                foreach (int index in categ3.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { $"{(int)RarbgCategory.Movies_XVID_720}", $"{(int)RarbgCategory.Movies_x264_3D}", $"{(int)RarbgCategory.Movies_Full_BD}", $"{(int)RarbgCategory.TV_HD_Episodes}", $"{(int)RarbgCategory.Games_PC_ISO}", $"{(int)RarbgCategory.Software_PC_ISO}" }[index]);
                }
                foreach (int index in categ4.SelectedIndices)
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
                if(!chkPageLimit.Checked && parser.getLastPageNum(Utils.HttpClient.Get($"https://rarbgenter.org/torrents.php?search={txtSearch.Text.Trim()}{category}{order}&page={9999999}"), ref maxPage))
                {
                    chkPageLimit.Checked = true;
                    nudPageLimit.Value = maxPage;
                }

                int pageNum;
                saved_listings.Clear();
                for (pageNum = 1; btnSearch.Text != "SEARCH"; pageNum++)
                {
                    string response = Utils.HttpClient.Get($"https://rarbgenter.org/torrents.php?search={txtSearch.Text.Trim()}{category}{order}&page={pageNum}");
                    if(response.Contains("We have too many requests from your ip in the past 24h."))
                    {
                        MessageBox.Show("Failed to gather listings, IP temporarily banned by server for 2 hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            else
            {
                btnSearch.Text = "SEARCH";
                btnSearch.ForeColor = Color.DarkGreen;
                showMessage("Error", "Please select a category first.", MessageBoxIcon.Error);
            }         
        }

        private void showMessage(string caption, string text, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            MessageBox.Show(text, caption, buttons, icon);
        }

        private void exportEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saved_listings.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog1.Title = "Save Listing";
                //saveFileDialog1.CheckFileExists = true;
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.Filter = "Xml files (*.xml)|*.xml";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Utils.Serialize(saveFileDialog1.FileName, saved_listings);
                    MessageBox.Show($"Listing saved succesfully to\n{saveFileDialog1.FileName}\n{saved_listings.Count} Entries Saved", "Export Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
        }

        private void importEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show($"Listing loaded succesfully!\n{entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed", "Import Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                //Custom Filter
                if(chkMinImdb.Checked || chkMinYear.Checked || chkMaxYear.Checked || chkMinUpDate.Checked || chkGenre.Checked)
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

                if(fromXML)
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
        }

        private void chkMinYear_CheckedChanged(object sender, EventArgs e)
        {
            dtpMinYear.Enabled = chkMinYear.Checked;
        }

        private void chkMaxYear_CheckedChanged(object sender, EventArgs e)
        {
            dtpMaxYear.Enabled = chkMaxYear.Checked;
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            saved_listings.Clear();
            dgvListings.Rows.Clear();
            clbGenre.Items.Clear();
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
            clbGenre.Enabled = chkGenre.Enabled;
        }
    }
}
