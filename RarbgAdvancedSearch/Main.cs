using Newtonsoft.Json;
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
using static RarbgAdvancedSearch.ContentTracker;
using static RarbgAdvancedSearch.RarbgPageParser;

namespace RarbgAdvancedSearch
{

    [Serializable]
    public partial class Main : Form
    {
        List<rarbgEntry> saved_listings;
        List<ContentTrack> dd_list;
        ContentTracker ctracker = new ContentTracker();
        Color Not_Set = Color.Empty, 
            Marked_For_Dld = Color.FromArgb(222, 232, 86), 
            Downloading = Color.FromArgb(245, 186, 125), 
            Downloaded = Color.FromArgb(143, 255, 135), 
            Deleted = Color.FromArgb(247, 110, 72);

        Imdb imdB = new Imdb();
        private int gCurrentRow = -1;

        public Main()
        {
            dd_list = new List<ContentTrack>();
            saved_listings = new List<rarbgEntry>();
            InitializeComponent();
            Text += $" v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            byte[] response_bytes = { };

            if (btnSearch.Text == "STOP")
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
                string html = GetRarbgPage($"{searchurl}&page={99999999}", ref response_bytes);

                if (html.Length == 0)
                    return;

                if (!chkPageLimit.Checked && parser.getLastPageNum(html, ref maxPage))
                {
                    chkPageLimit.Checked = true;
                    nudPageLimit.Value = maxPage;
                }

                int pageNum;
                saved_listings.Clear();
                for (pageNum = 1; btnSearch.Text != "SEARCH"; pageNum++)
                {
                    string response = GetRarbgPage($"{searchurl}&page={pageNum}", ref response_bytes);
                    if(response == string.Empty)
                    {
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

        private string GetRarbgPage(string url, ref byte[] responseBytes, string referer = "")
        {
            retry_get:
            try
            {
                string html = Utils.HttpClient.Get(url, ref responseBytes, string.Empty, referer);
                if (html.StartsWith("https://") && html.Contains("threat_defence"))
                {
                    tstStatus.Text = "Captcha validation..";
                    showMessage("Rarbg needs you to validate a captcha.\nPlease validate the captcha and press enter.", "Captcha Required");
                    browser b = new browser();
                    b.navigateTo(html);
                    this.Controls.Add(b);
                    b.BringToFront();
                    b.Dock = DockStyle.Fill;
                    while (this.Controls.Contains(b))
                    {
                        Application.DoEvents();
                        Thread.Sleep(1);
                    }

                    if (b.userCancelled)
                    {
                        responseBytes = new byte[] { };

                        if (!b.IsDisposed)
                            b.Dispose();

                        showMessage("Captcha Cancelled.\nRarbgAdvancedSearch may not be ale to fetch new listings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        return string.Empty;
                    }

                    if (!b.IsDisposed)
                        b.Dispose();

                    goto retry_get;
                }

                if (html.Contains("We have too many requests from your ip in the past 24h."))
                {
                    showMessage("Failed fetch page, IP temporarily banned by Rarbg for 2 hours.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    html = string.Empty;
                }

                return html;
            }
            catch (Exception)
            {
                showMessage("Failed fetch rarbg page..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return string.Empty;           
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
                try
                {
                    populateGrid(saved_listings, saved_listings.Count / 25, ref entryCount, true);
                    tstStatus.Text = $"Done.. Page {saved_listings.Count / 25}, {entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
                    reloadListingSorting();
                    showMessage($"Listing loaded succesfully!\n{entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed", "Import Listing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception) { }
                Application.DoEvents();
            }
        }

        private void populateGrid(List<ContentTrack> dd_list)
        {
            if (!dgvListings.Columns.Contains("downloadCol"))
            {
                // Initialize the button column.
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "downloadCol";
                buttonColumn.HeaderText = "";
                buttonColumn.Text = "Download";

                // Use the Text property for the button text for all cells rather
                // than using each cell's value as the text for its own button.
                buttonColumn.UseColumnTextForButtonValue = true;

                // Add the button column to the control.
                dgvListings.Columns.Insert(0, buttonColumn);
            }            

            int entryCount = 0;
            foreach (var ctr in dd_list)
            {
                if (ctr.entry.genre.Count > 0)
                    if (ctr.entry.genre.Any(g => !clbGenre.Items.Contains(g)))
                    {
                        clbGenre.Items.AddRange(ctr.entry.genre.Where(g => !clbGenre.Items.Contains(g)).ToArray());
                    }

                if (txtSearch.Text.Trim().Length > 0)
                {
                    if (!ctr.entry.name.ToUpper().Contains(txtSearch.Text.Trim().ToUpper()) && !ctr.entry.name.ToUpper().Replace(".", " ").Contains(txtSearch.Text.Trim().ToUpper()))
                    {
                        goto entry_filtered;
                    }
                }

                Status stat = Status.NotSet;
                //Custom Filter
                if (chkMinImdb.Checked || chkMinYear.Checked || chkMaxYear.Checked || chkMinUpDate.Checked || chkGenre.Checked)
                {
                    if (!chkMinImdb.Checked || ctr.entry.imdbRating >= (double)nudMinImdb.Value)
                    {
                        if (!chkMinYear.Checked || ctr.entry.year >= dtpMinYear.Value.Year)
                        {
                            if (!chkMaxYear.Checked || ctr.entry.year <= dtpMaxYear.Value.Year)
                            {
                                if (!chkMinUpDate.Checked || ctr.entry.dateAdded >= dtpMinUpDate.Value)
                                {
                                    if (ctracker.contains(ctr.entry, ref stat))
                                    {
                                        Color backColor = Not_Set;
                                        switch (stat)
                                        {
                                            case Status.MarkedForDownload:                            
                                                backColor = Marked_For_Dld;
                                                break;
                                            case Status.Downloading:
                                                backColor = Downloading;
                                                break;
                                            case Status.Downloaded:
                                                backColor = Downloaded;
                                                break;
                                            case Status.Deleted:
                                                backColor = Deleted;
                                                break;
                                        }

                                        dgvListings.Rows.Add(new object[] { "Down", ctr.entry.category, ctr.entry.name, ctr.entry.dateAdded, Math.Round(ctr.entry.sizeInGb, 2), ctr.entry.seeders, ctr.entry.leechers, ctr.entry.uploader, string.Join(",", ctr.entry.genre), ctr.entry.year, ctr.entry.imdbRating, stat });
                                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = ctr;
                                        dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.BackColor = backColor;
                                        dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Empty;
                                    }
                                    else
                                    {
                                        dgvListings.Rows.Add(new object[] { "Down", ctr.entry.category, ctr.entry.name, ctr.entry.dateAdded, Math.Round(ctr.entry.sizeInGb, 2), ctr.entry.seeders, ctr.entry.leechers, ctr.entry.uploader, string.Join(",", ctr.entry.genre), ctr.entry.year, ctr.entry.imdbRating, Status.NotSet });
                                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = ctr;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    stat = Status.NotSet;
                    if (ctracker.contains(ctr.entry, ref stat))
                    {
                        Color backColor = Not_Set;
                        switch (stat)
                        {
                            case Status.MarkedForDownload:
                                backColor = Marked_For_Dld;
                                break;
                            case Status.Downloading:
                                backColor = Downloading;
                                break;
                            case Status.Downloaded:
                                backColor = Downloaded;
                                break;
                            case Status.Deleted:
                                backColor = Deleted;
                                break;
                        }

                        dgvListings.Rows.Add(new object[] { "Down", ctr.entry.category, ctr.entry.name, ctr.entry.dateAdded, Math.Round(ctr.entry.sizeInGb, 2), ctr.entry.seeders, ctr.entry.leechers, ctr.entry.uploader, string.Join(",", ctr.entry.genre), ctr.entry.year, ctr.entry.imdbRating, stat });
                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = ctr;
                        dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.BackColor = backColor;
                        dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Empty;
                    }
                    else
                    {
                        dgvListings.Rows.Add(new object[] { "Down", ctr.entry.category, ctr.entry.name, ctr.entry.dateAdded, Math.Round(ctr.entry.sizeInGb, 2), ctr.entry.seeders, ctr.entry.leechers, ctr.entry.uploader, string.Join(",", ctr.entry.genre), ctr.entry.year, ctr.entry.imdbRating, Status.NotSet });
                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = ctr;
                    }
                }

            entry_filtered:
                    tstStatus.Text = $"Working.. {++entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";

                if (entryCount % 5 == 0)
                {
                    tstProgress.Value = (int)(((double)entryCount / dd_list.Count) * tstProgress.Maximum);
                    Application.DoEvents();
                }
            }
            tstProgress.Value = tstProgress.Maximum;
        }

        private void populateGrid(List<rarbgEntry> entries, int pageCount, ref int entryCount, bool fromXML = false)
        {
            if (dgvListings.Columns.Contains("downloadCol"))
                dgvListings.Columns.Remove("downloadCol");

            int totalEntryCount = entries.Count;
            
            foreach (var entry in entries)
            {
                if (entry.genre.Count > 0)
                    if (entry.genre.Any(g => !clbGenre.Items.Contains(g)))
                    {
                        clbGenre.Items.AddRange(entry.genre.Where(g => !clbGenre.Items.Contains(g)).ToArray());
                    }

                if (txtSearch.Text.Trim().Length > 0)
                {
                    if (!entry.name.ToUpper().Contains(txtSearch.Text.Trim().ToUpper()) && !entry.name.ToUpper().Replace(".", " ").Contains(txtSearch.Text.Trim().ToUpper()))
                    {
                        goto entry_filtered;
                    }
                }

                //Custom Filter
                if (chkMinImdb.Checked || chkMinYear.Checked || chkMaxYear.Checked || chkMinUpDate.Checked || chkGenre.Checked)
                {
                    if (!chkMinImdb.Checked || entry.imdbRating >= (double)nudMinImdb.Value)
                    {
                        if (!chkMinYear.Checked || entry.year >= dtpMinYear.Value.Year)
                        {
                            if (!chkMaxYear.Checked || entry.year <= dtpMaxYear.Value.Year)
                            {
                                if (!chkMinUpDate.Checked || entry.dateAdded >= dtpMinUpDate.Value)
                                {
                                    if (!chkGenre.Checked || clbGenre.CheckedItems.Count == 0 || entry.genre.Any(g => clbGenre.CheckedItems.Contains(g)))
                                    {
                                        Status stat = Status.NotSet;
                                        if (ctracker.contains(entry, ref stat))
                                        {
                                            Color backColor = Not_Set;
                                            switch (stat)
                                            {
                                                case Status.MarkedForDownload:
                                                    backColor = Marked_For_Dld;
                                                    break;
                                                case Status.Downloading:
                                                    backColor = Downloading;
                                                    break;
                                                case Status.Downloaded:
                                                    backColor = Downloaded;
                                                    break;
                                                case Status.Deleted:
                                                    backColor = Deleted;
                                                    break;
                                            }

                                            dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating, stat });
                                            dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                                            dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.BackColor = backColor;
                                            dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Empty;
                                        }
                                        else
                                        {
                                            dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating, stat });
                                            dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Status stat = Status.NotSet;
                    if (ctracker.contains(entry, ref stat))
                    {
                        Color backColor = Color.Empty;
                        switch (stat)
                        {
                            case Status.MarkedForDownload:
                                backColor = Marked_For_Dld;
                                break;
                            case Status.Downloading:
                                backColor = Downloading;
                                break;
                            case Status.Downloaded:
                                backColor = Downloaded;
                                break;
                            case Status.Deleted:
                                backColor = Deleted;
                                break;
                        }

                        dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating, stat });
                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                        dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.BackColor = backColor;
                        //dgvListings.Rows[dgvListings.Rows.Count - 1].DefaultCellStyle.ForeColor = Not_Set;
                    }
                    else
                    {
                        dgvListings.Rows.Add(new object[] { entry.category, entry.name, entry.dateAdded, Math.Round(entry.sizeInGb, 2), entry.seeders, entry.leechers, entry.uploader, string.Join(",", entry.genre), entry.year, entry.imdbRating, stat });
                        dgvListings.Rows[dgvListings.Rows.Count - 1].Tag = entry;
                    }
                }

            entry_filtered:
                if (fromXML)
                    tstStatus.Text = $"Working.. Page {(int)(((double)entryCount / totalEntryCount) * pageCount)}, {entryCount++} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
                else
                    tstStatus.Text = $"Working.. Page {pageCount}, {entryCount++} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";

                if (entryCount % 25 == 0)
                {
                    if (fromXML)
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
                rarbgEntry entry = GetRarbgEntryFromRowTag(dgvListings.Rows[e.RowIndex].Tag);
                Process.Start($"https://rarbgenter.org{entry.url}");
            }            
        }

        private void chkMinImdb_CheckedChanged(object sender, EventArgs e)
        {
            nudMinImdb.Enabled = chkMinImdb.Checked;
        }
        
        private void reloadGrid()
        {
            if (dgvListings.Columns.Contains("downloadCol"))
            {
                dgvListings.Rows.Clear();
                populateGrid(dd_list);
                reloadListingSorting();
            }
            else
            {
                dgvListings.Rows.Clear();
                int entryCount = 0;
                populateGrid(saved_listings, saved_listings.Count / 25, ref entryCount, true);
                tstStatus.Text = $"Done.. Page {saved_listings.Count / 25}, {entryCount} Entries Loaded, {dgvListings.Rows.Count} Entries Displayed";
                reloadListingSorting();
            }            
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

            if (dgvListings.SortedColumn != null)
            {
                DataGridViewColumn col = dgvListings.SortedColumn;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            txtSearch.Text = string.Empty;
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

        private rarbgEntry GetRarbgEntryFromRowTag(object tag)
        {
            return dgvListings.Columns.Contains("downloadCol") ? ((ContentTrack)tag).entry : ((rarbgEntry)tag);
        }

        private void dgvListings_MouseClick(object sender, MouseEventArgs e)
        {
            byte[] response_bytes = { };
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dgvListings.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    ContextMenu m = new ContextMenu();
                    dgvListings.Rows[currentMouseOverRow].Selected = true;

                    rarbgEntry entry = GetRarbgEntryFromRowTag(dgvListings.Rows[currentMouseOverRow].Tag);

                    if(!string.IsNullOrEmpty(entry.imdb_id))
                        m.MenuItems.Add(new MenuItem("> Open IMDb Page", delegate { Process.Start($"https://www.imdb.com/title/{entry.imdb_id}"); }));

                    if (!string.IsNullOrEmpty(entry.url))
                    {
                        m.MenuItems.Add(new MenuItem("> Open RaRBG Page", delegate { Process.Start($"https://rarbgenter.org{entry.url}"); }));

                        Status stat = Status.NotSet;
                        ctracker.contains(entry, ref stat);
                        m.MenuItems.Add("> Set Status", new MenuItem[] {
                            new MenuItem(stat == Status.NotSet ? ">Not Set<" : "Not Set", delegate { bool saved = ctracker.savetrack(entry, Status.NotSet); dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = Not_Set; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.NotSet; UsageStats.Log("content_update", $"{entry.name} - {stat}"); }),
                            new MenuItem(stat == Status.MarkedForDownload ? ">Marked for Download<" : "Marked for Download", delegate {bool saved = ctracker.savetrack(entry, Status.MarkedForDownload); dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = saved ? Marked_For_Dld: Color.Empty; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.MarkedForDownload; UsageStats.Log("content_update", $"{entry.name} - {stat}"); }),
                            new MenuItem(stat == Status.Downloading ? ">Downloading<" : "Downloading", delegate {bool saved = ctracker.savetrack(entry, Status.Downloading); dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = saved ? Downloading: Color.Empty; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.Downloading; UsageStats.Log("content_update", $"{entry.name} - {stat}"); }),
                            new MenuItem(stat == Status.Downloaded ? ">Downloaded<" : "Downloaded", delegate {bool saved = ctracker.savetrack(entry, Status.Downloaded); dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = saved ? Downloaded : Color.Empty; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.Downloaded; UsageStats.Log("content_update", $"{entry.name} - {stat}"); }),
                            new MenuItem(stat == Status.Deleted ? ">Deleted<" : "Deleted", delegate {bool saved = ctracker.savetrack(entry, Status.Deleted); dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = saved ? Deleted : Color.Empty; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.Deleted; UsageStats.Log("content_update", $"{entry.name} - {stat}"); })
                        });

                        m.MenuItems.Add(new MenuItem("> Download using .Torrent File (opens using browser)", delegate {
                        //tstStatus.Text = "Looking for .Torrent file..";
                        string [] page_content = GetRarbgPage($"https://rarbgenter.org{entry.url}", ref response_bytes).Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);

                        if(page_content.Length > 0)
                        {
                            if(page_content.Any(s => s.Contains(".torrent")))
                            {
                                Process.Start($"https://rarbgenter.org{page_content.FirstOrDefault(s => s.Contains(".torrent"))}");

                                ////need to deal with cloudflare jscookie problem before saving torrent file directly can work.. Potential fix using CefSharp..
                                //GetRarbgPage($"https://rarbgenter.org{page_content.FirstOrDefault(s => s.Contains(".torrent"))}", ref response_bytes, $"https://rarbgenter.org{entry.url}");
                                //if(response_bytes.Length == 0)
                                //{
                                //    showMessage($"Could not download torrent directly, Opening in browser..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    Process.Start($"https://rarbgenter.org{page_content.FirstOrDefault(s => s.Contains(".torrent"))}");
                                //    return;
                                //}
                                    
                                //System.IO.FileInfo file = new System.IO.FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}/torrent/{entry.name}.torrent");
                                //file.Directory.Create();

                                //System.IO.File.WriteAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}/torrent/{entry.name}.torrent", response_bytes);
                                //tstStatus.Text = ".Torrent File Saved..";

                                //try
                                //{
                                //    Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}/torrent/{entry.name}.torrent");
                                //}
                                //catch (Exception)
                                //{
                                //    showMessage($"Could not locate your torrent application, please open the the torrent file using your torrent client.\nTorrent File: {AppDomain.CurrentDomain.BaseDirectory}\\torrent\\{entry.name}.torrent", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //    Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}/torrent");
                                //}
                                return;
                            }
                        }
                        //tstStatus.Text = "Failed to Download .Torrent file..";
                        }));

                        m.MenuItems.Add(new MenuItem("> Download using Magnet Link", delegate {
                            tstStatus.Text = "Looking for Magnet Link..";
                            string page = GetRarbgPage($"https://rarbgenter.org{entry.url}", ref response_bytes);
                            string[] page_content = page.Split(new[] { '"' });
                            if (page_content.Length > 0)
                            {
                                if (page_content.Any(s => s.StartsWith("magnet:?")))
                                {
                                    tstStatus.Text = "Magnet Link Found..";
                                    string magnet = page_content.FirstOrDefault(s => s.StartsWith("magnet:?"));
                                    try
                                    {
                                        Process.Start(magnet);
                                    }
                                    catch (Exception) {
                                        Clipboard.SetText(magnet);
                                        showMessage("Could not locate your torrent application, magnet link was copied to clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                     }
                                }
                                return;
                            }
                            tstStatus.Text = "Failed to acquire Magnet Link..";
                        }));

                        m.MenuItems.Add(new MenuItem("> Copy Magnet Link", delegate {
                            tstStatus.Text = "Looking for Magnet Link..";
                            string page = GetRarbgPage($"https://rarbgenter.org{entry.url}", ref response_bytes);
                            string[] page_content = page.Split(new[] { '"' });
                            if (page_content.Length > 0)
                            {
                                if (page_content.Any(s => s.StartsWith("magnet:?")))
                                {
                                    tstStatus.Text = "Magnet Link Found..";
                                    string magnet = page_content.FirstOrDefault(s => s.StartsWith("magnet:?"));
                                    Clipboard.SetText(magnet);
                                    showMessage("Magnet link was copied to clipboard.", "Magnet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                return;
                            }
                            tstStatus.Text = "Failed to acquire Magnet Link..";
                        }));
                    }

                    if (entry.category.ToString().Contains("Movies"))
                        m.MenuItems.Add(new MenuItem("> Open Youtube Trailer", delegate {
                            string [] entry_name = entry.name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                            string youtube_search_name = string.Empty;
                            foreach(var enm in entry_name)
                            {
                                youtube_search_name += enm + " ";
                                if (enm == $"{entry.year}")
                                {
                                    break;
                                }
                            }
                            Process.Start($"https://www.youtube.com/results?search_query={youtube_search_name.TrimEnd()}+trailer"); 
                        }));

                    if (entry.category.ToString().Contains("Movies_x265_1080") && !dgvListings.Columns.Contains("downloadCol"))
                        m.MenuItems.Add(new MenuItem("> Request Direct Download", delegate {
                            showMessage("Your Direct Download Request has been sent.\nYou will receive a notification in case it is available for download.", "Direct Download Request", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            var genre = entry.genre.Distinct().Aggregate("", (current, next) => current + "_" + next).TrimStart(new[] { '_' });
                            UsageStats.Log("DD_REQUEST", $"[{entry.name}][{entry.url}][{genre}][Size:{entry.sizeInGb}][Rating:{entry.imdbRating}]");
                        }));

                    //Admin stuff
                    if (UsageStats.machinecode == "1ED9F550CC070D9D2D029B6006AE8C4AED12E8A5")
                    {
                        m.MenuItems.Add(new MenuItem("> Set dd_URL", delegate {

                            Status stat = Status.NotSet;
                            string current_dd_url = string.Empty;
                            if (ctracker.contains(entry, ref stat))
                            {
                                current_dd_url = ctracker.tracks.FirstOrDefault(t => t.entry.name == entry.name).dd_url;
                            }
                            if (string.IsNullOrEmpty(current_dd_url))
                            {
                                if (dgvListings.Columns.Contains("downloadCol"))
                                {
                                    ContentTrack tmp_ctr = (ContentTrack)dgvListings.Rows[currentMouseOverRow].Tag;
                                    current_dd_url = tmp_ctr.dd_url;
                                }
                            }

                            string url = Microsoft.VisualBasic.Interaction.InputBox("Enter URL: ", "Set dd_Url ", current_dd_url);
                            url = url.Replace("https://mega.nz/", "");
                            if (!string.IsNullOrEmpty(url))
                            {
                                ContentTrack ctr = new ContentTrack() { entry = entry, stat = Status.Downloaded, dd_url = url };
                                ctracker.savetrack(ctr);
                                Clipboard.SetText(JsonConvert.SerializeObject(ctr) + ",");
                                dgvListings.Rows[currentMouseOverRow].DefaultCellStyle.BackColor = Downloaded; dgvListings.Rows[currentMouseOverRow].Cells["colStatus"].Value = Status.Downloaded;
                                if (dgvListings.Columns.Contains("dd_url"))
                                {
                                    dgvListings.Rows[currentMouseOverRow].Cells["dd_url"].Value = url;
                                }
                            }
                        }));

                        m.MenuItems.Add(new MenuItem("> Get dd_URL", delegate {
                            List<ContentTrack> dd_list = ctracker.tracks.Where(t => t.dd_url != null && t.dd_url.Length > 0).ToList();

                            Clipboard.SetText(JsonConvert.SerializeObject(dd_list).Compress());

                            showMessage($"ddList copied to clipboard.. {dd_list.Count} items", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));

                        m.MenuItems.Add(new MenuItem("> Show dd_URL", delegate {
                            List<ContentTrack> dd_list = ctracker.tracks.Where(t => t.dd_url != null && t.dd_url.Length > 0).ToList();
                            if(!dgvListings.Columns.Contains("dd_url"))
                            {
                                dgvListings.Columns.Add("dd_url", "dd_url");                                
                            }
                            foreach(DataGridViewRow row in dgvListings.Rows)
                            {
                                if(dd_list.Any(dd => dd.entry.name == GetRarbgEntryFromRowTag(row.Tag).name))
                                {
                                    row.Cells["dd_url"].Value = dd_list.FirstOrDefault(dd => dd.entry.name == GetRarbgEntryFromRowTag(row.Tag).name).dd_url;
                                }
                            }
                        }));
                    }

                    if (m.MenuItems.Count > 0)
                        m.Show(dgvListings, new Point(e.X, e.Y));
                }
            }
        }

        private void directDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessage("This action will clear all current listings, do you wish to proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            dgvListings.Rows.Clear();
            saved_listings.Clear();

            tstStatus.Text = "Fetching Direct Download List..";
            dd_list = ContentTracker.GetDDList();

            if(dd_list.Count == 1 && dd_list[0].dd_url == "deprecated")
            {
                showMessage("Your version of RarbgAdvancedSearch is outdated, please update..", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                return;
            }

            saved_listings.AddRange(dd_list.Select(ddl => ddl.entry));

            if (dd_list.Count > 0)
            {
                populateGrid(dd_list);
            }
            else
            {
                showMessage("Failed to fetch direct download list..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UsageStats.Log("fetch_ddlist_error", $"List is empty..");
            }

        }

        private void dgvListings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            //Download Button Handler
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                frmDownload frm = new frmDownload(ref ctracker, (ContentTrack)dgvListings.Rows[e.RowIndex].Tag, this);                
                frm.ShowDialog();

                if(!frm.IsDisposed)
                    frm.Dispose();

                reloadGrid();
            }
        }

        private void tsbReadNotify_Click(object sender, EventArgs e)
        {
            tsbReadNotify.Visible = false;
            Notifications notifpage = new Notifications();
            this.Controls.Add(notifpage);
            notifpage.BringToFront();
            notifpage.Dock = DockStyle.Fill;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            pnlImdbInfo.Location = new Point(576, 4);

            Task.Run(async () =>
            {
                while(true)
                {
                    UpdateNotifCounter();
                    Thread.Sleep(30000);
                }               
            });
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            About aboutpage = new About();
            this.Controls.Add(aboutpage);
            aboutpage.BringToFront();
            aboutpage.Dock = DockStyle.Fill;            
        }

        private void dgvListings_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            gCurrentRow = e.RowIndex;
            Thread.Sleep(10);
            DataGridView dgv_sender = sender as DataGridView;
            DataGridViewCell dgv_MouseOverCell = null;
            if (gCurrentRow == e.RowIndex && e.RowIndex >= 0 && e.ColumnIndex > 0 && e.RowIndex < dgv_sender.RowCount && e.ColumnIndex < dgv_sender.ColumnCount)
            {
                dgv_MouseOverCell = dgv_sender.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvListings.Rows[e.RowIndex].Selected = true;
            }

            Task.Run(async () =>
            {
                Thread.Sleep(300);
                
                if (dgv_MouseOverCell != null && gCurrentRow == e.RowIndex)
                {
                    rarbgEntry entry = GetRarbgEntryFromRowTag(dgvListings.Rows[e.RowIndex].Tag);
                    if(!string.IsNullOrEmpty(entry.imdb_id))
                    {                        
                        if(dgvListings.SelectedRows.Count > 0 && GetRarbgEntryFromRowTag(dgvListings.SelectedRows[0].Tag).imdb_id == entry.imdb_id)
                        {
                            var info = imdB.GetImdbInfo(entry.imdb_id);
                            if (!string.IsNullOrEmpty(info.Name) && dgvListings.SelectedRows.Count > 0 && GetRarbgEntryFromRowTag(dgvListings.SelectedRows[0].Tag).imdb_id == entry.imdb_id)
                            {
                                this.PerformSafely(() => {
                                    if(info.Image != null)
                                    {
                                        pbTooltipImg.Image = info.Image;
                                    }
                                    else
                                    {
                                        pbTooltipImg.ImageLocation = info.imgUrl;
                                    }
                                    lblttName.Text = $"{info.Name} ({info.DatePublished.Year})";
                                    lblttRating.Text = $"{info.RatingValue}/10";
                                    double ratingPercent = ((double)info.RatingValue / 8.5) * 100;
                                    lblttRating.ForeColor = Utils.GetBlendedColor((int)(ratingPercent > 100 ? 100 : ratingPercent));
                                    lblttRatingCount.Text = $"{info.RatingCount} Users";
                                    lblttGenre.Text = string.Empty;
                                    if(!string.IsNullOrEmpty(info.Genre.String))
                                    {
                                        lblttGenre.Text = $"{lblttGenre.Text}{info.Genre.String}\n";
                                    }
                                    else foreach(var g in info.Genre.StringArray)
                                    {
                                            lblttGenre.Text = $"{lblttGenre.Text}{g}\n";
                                    }
                                    pnlImdbInfo.Visible = true;
                                });
                            }                                
                        }                        
                    }                               
                }
            });            
        }

        private void dgvListings_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            pbTooltipImg.Image = null;
            pnlImdbInfo.Visible = false;
        }

        private void pbTooltipImg_MouseEnter(object sender, EventArgs e)
        {
            pbTooltipImg.Image = null;
            pnlImdbInfo.Visible = false;
        }

        private void pnlImdbInfo_MouseEnter(object sender, EventArgs e)
        {
            pbTooltipImg.Image = null;
            pnlImdbInfo.Visible = false;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSearch.Text == "STOP")
            {
                Application.Exit();
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void tstView_Click(object sender, EventArgs e)
        {
            if (dgvListings.Columns.Contains("downloadCol"))
                dgvListings.Columns.Remove("downloadCol");

            if (showMessage("This action will clear all current listings, do you wish to proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            saved_listings.Clear();
            dgvListings.Rows.Clear();            

            switch ((sender as ToolStripMenuItem).Name)
            {
                case "tsmiMarkedForDownload":
                    saved_listings.AddRange(ctracker.tracks.Where(t => t.stat == Status.MarkedForDownload).Select(x => x.entry).ToList());
                    break;
                case "tsmiDownloading":
                    saved_listings.AddRange(ctracker.tracks.Where(t => t.stat == Status.Downloading).Select(x => x.entry).ToList());
                    break;
                case "tsmiDownloaded":
                    saved_listings.AddRange(ctracker.tracks.Where(t => t.stat == Status.Downloaded).Select(x => x.entry).ToList());
                    break;
                case "tsmiDeleted":
                    saved_listings.AddRange(ctracker.tracks.Where(t => t.stat == Status.Deleted).Select(x => x.entry).ToList());
                    break;
                default:
                    saved_listings.AddRange(ctracker.tracks.Select(x => x.entry).ToList());
                    break;
            }

            reloadGrid();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                reloadGrid();
            }
        }

        private void UpdateNotifCounter()
        {
            int counter = Notifications.GetNotifCount();
            if (counter > 0)
            {
                this.PerformSafely(() => {
                    tsbReadNotify.Text = $"You have {counter} unread notifications.";
                    tsbReadNotify.Visible = true;
                });                
            }
            else
            {
                this.PerformSafely(() => {
                    tsbReadNotify.Visible = false;
                });
            }
        }
    }
}
