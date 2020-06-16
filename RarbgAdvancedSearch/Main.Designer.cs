namespace RarbgAdvancedSearch
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.categ1 = new System.Windows.Forms.CheckedListBox();
            this.categ2 = new System.Windows.Forms.CheckedListBox();
            this.categ3 = new System.Windows.Forms.CheckedListBox();
            this.categ4 = new System.Windows.Forms.CheckedListBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvListings = new System.Windows.Forms.DataGridView();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgUploader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Genre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgImdb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbGenre = new System.Windows.Forms.CheckedListBox();
            this.btnReapplyFilter = new System.Windows.Forms.Button();
            this.dudSearchOrder = new System.Windows.Forms.DomainUpDown();
            this.cmbSearchOrder = new System.Windows.Forms.ComboBox();
            this.chkMaxYear = new System.Windows.Forms.CheckBox();
            this.dtpMaxYear = new System.Windows.Forms.DateTimePicker();
            this.chkGenre = new System.Windows.Forms.CheckBox();
            this.chkSearchOrder = new System.Windows.Forms.CheckBox();
            this.chkMinYear = new System.Windows.Forms.CheckBox();
            this.dtpMinYear = new System.Windows.Forms.DateTimePicker();
            this.dtpMinUpDate = new System.Windows.Forms.DateTimePicker();
            this.nudPageLimit = new System.Windows.Forms.NumericUpDown();
            this.nudMinImdb = new System.Windows.Forms.NumericUpDown();
            this.chkPageLimit = new System.Windows.Forms.CheckBox();
            this.chkMinUpDate = new System.Windows.Forms.CheckBox();
            this.chkMinImdb = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tstProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tstStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.exportEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.importEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRESET = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiMarkedForDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDownloading = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDownloaded = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleted = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAllMarked = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDonate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.directDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbReadNotify = new System.Windows.Forms.ToolStripButton();
            this.pbTooltipImg = new System.Windows.Forms.PictureBox();
            this.pnlImdbInfo = new System.Windows.Forms.Panel();
            this.lblttName = new System.Windows.Forms.Label();
            this.lblttRatingCount = new System.Windows.Forms.Label();
            this.lblttRating = new System.Windows.Forms.Label();
            this.lblttGenre = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListings)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinImdb)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTooltipImg)).BeginInit();
            this.pnlImdbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // categ1
            // 
            this.categ1.FormattingEnabled = true;
            this.categ1.Items.AddRange(new object[] {
            "XXX",
            "Movies/x264/1080",
            "Movies/x265/4k",
            "Movies/x265/1080",
            "Music/MP3",
            "Games/PS3"});
            this.categ1.Location = new System.Drawing.Point(12, 29);
            this.categ1.Name = "categ1";
            this.categ1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.categ1.Size = new System.Drawing.Size(126, 94);
            this.categ1.TabIndex = 0;
            this.categ1.Click += new System.EventHandler(this.clb_click);
            // 
            // categ2
            // 
            this.categ2.FormattingEnabled = true;
            this.categ2.Items.AddRange(new object[] {
            "Movies/XVID",
            "Movies/x264/720",
            "Movs/x265/4k/HDR",
            "TV Episodes",
            "Music/FLAC",
            "Games/XBOX-360"});
            this.categ2.Location = new System.Drawing.Point(144, 29);
            this.categ2.Name = "categ2";
            this.categ2.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.categ2.Size = new System.Drawing.Size(126, 94);
            this.categ2.TabIndex = 0;
            this.categ2.Click += new System.EventHandler(this.clb_click);
            // 
            // categ3
            // 
            this.categ3.FormattingEnabled = true;
            this.categ3.Items.AddRange(new object[] {
            "Movies/XVID/720",
            "Movies/x264/3D",
            "Movies/Full BD",
            "TV HD Episodes",
            "Games/PC ISO",
            "Software/PC ISO"});
            this.categ3.Location = new System.Drawing.Point(276, 29);
            this.categ3.Name = "categ3";
            this.categ3.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.categ3.Size = new System.Drawing.Size(126, 94);
            this.categ3.TabIndex = 0;
            this.categ3.Click += new System.EventHandler(this.clb_click);
            // 
            // categ4
            // 
            this.categ4.FormattingEnabled = true;
            this.categ4.Items.AddRange(new object[] {
            "Movies/x264",
            "Movies/x264/4k",
            "Movies/BD Remux",
            "TV UHD Episodes",
            "Games/PC RIP",
            "Games/PS4"});
            this.categ4.Location = new System.Drawing.Point(408, 29);
            this.categ4.Name = "categ4";
            this.categ4.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.categ4.Size = new System.Drawing.Size(126, 94);
            this.categ4.TabIndex = 0;
            this.categ4.Click += new System.EventHandler(this.clb_click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnSearch.Location = new System.Drawing.Point(540, 29);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(379, 63);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvListings
            // 
            this.dgvListings.AllowUserToAddRows = false;
            this.dgvListings.AllowUserToDeleteRows = false;
            this.dgvListings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvListings.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvListings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.dgName,
            this.dgAdded,
            this.dgSize,
            this.dgS,
            this.dgL,
            this.dgUploader,
            this.Genre,
            this.dgYear,
            this.dgImdb,
            this.colStatus});
            this.dgvListings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvListings.Location = new System.Drawing.Point(12, 270);
            this.dgvListings.MultiSelect = false;
            this.dgvListings.Name = "dgvListings";
            this.dgvListings.ReadOnly = true;
            this.dgvListings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListings.Size = new System.Drawing.Size(908, 335);
            this.dgvListings.TabIndex = 2;
            this.toolTip.SetToolTip(this.dgvListings, "Double Click entry to Open in Browser");
            this.dgvListings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListings_CellContentClick);
            this.dgvListings.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListings_CellDoubleClick);
            this.dgvListings.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListings_CellMouseEnter);
            this.dgvListings.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListings_CellMouseLeave);
            this.dgvListings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvListings_MouseClick);
            // 
            // Category
            // 
            this.Category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Width = 74;
            // 
            // dgName
            // 
            this.dgName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgName.HeaderText = "Name";
            this.dgName.Name = "dgName";
            this.dgName.ReadOnly = true;
            this.dgName.Width = 60;
            // 
            // dgAdded
            // 
            this.dgAdded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgAdded.HeaderText = "Added";
            this.dgAdded.Name = "dgAdded";
            this.dgAdded.ReadOnly = true;
            this.dgAdded.Width = 63;
            // 
            // dgSize
            // 
            this.dgSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgSize.HeaderText = "Size";
            this.dgSize.Name = "dgSize";
            this.dgSize.ReadOnly = true;
            this.dgSize.Width = 52;
            // 
            // dgS
            // 
            this.dgS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgS.HeaderText = "S";
            this.dgS.Name = "dgS";
            this.dgS.ReadOnly = true;
            this.dgS.Width = 39;
            // 
            // dgL
            // 
            this.dgL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgL.HeaderText = "L";
            this.dgL.Name = "dgL";
            this.dgL.ReadOnly = true;
            this.dgL.Width = 38;
            // 
            // dgUploader
            // 
            this.dgUploader.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgUploader.HeaderText = "Uploader";
            this.dgUploader.Name = "dgUploader";
            this.dgUploader.ReadOnly = true;
            this.dgUploader.Width = 75;
            // 
            // Genre
            // 
            this.Genre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Genre.HeaderText = "Genre";
            this.Genre.Name = "Genre";
            this.Genre.ReadOnly = true;
            this.Genre.Width = 61;
            // 
            // dgYear
            // 
            this.dgYear.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgYear.HeaderText = "Year";
            this.dgYear.Name = "dgYear";
            this.dgYear.ReadOnly = true;
            this.dgYear.Width = 54;
            // 
            // dgImdb
            // 
            this.dgImdb.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgImdb.HeaderText = "Imdb";
            this.dgImdb.Name = "dgImdb";
            this.dgImdb.ReadOnly = true;
            this.dgImdb.Width = 55;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 62;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(540, 98);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(380, 25);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.clbGenre);
            this.groupBox1.Controls.Add(this.btnReapplyFilter);
            this.groupBox1.Controls.Add(this.dudSearchOrder);
            this.groupBox1.Controls.Add(this.cmbSearchOrder);
            this.groupBox1.Controls.Add(this.chkMaxYear);
            this.groupBox1.Controls.Add(this.dtpMaxYear);
            this.groupBox1.Controls.Add(this.chkGenre);
            this.groupBox1.Controls.Add(this.chkSearchOrder);
            this.groupBox1.Controls.Add(this.chkMinYear);
            this.groupBox1.Controls.Add(this.dtpMinYear);
            this.groupBox1.Controls.Add(this.dtpMinUpDate);
            this.groupBox1.Controls.Add(this.nudPageLimit);
            this.groupBox1.Controls.Add(this.nudMinImdb);
            this.groupBox1.Controls.Add(this.chkPageLimit);
            this.groupBox1.Controls.Add(this.chkMinUpDate);
            this.groupBox1.Controls.Add(this.chkMinImdb);
            this.groupBox1.Location = new System.Drawing.Point(12, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(907, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced Filters";
            // 
            // clbGenre
            // 
            this.clbGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbGenre.BackColor = System.Drawing.SystemColors.Window;
            this.clbGenre.Enabled = false;
            this.clbGenre.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.clbGenre.FormattingEnabled = true;
            this.clbGenre.Location = new System.Drawing.Point(589, 13);
            this.clbGenre.MultiColumn = true;
            this.clbGenre.Name = "clbGenre";
            this.clbGenre.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clbGenre.Size = new System.Drawing.Size(214, 109);
            this.clbGenre.TabIndex = 10;
            this.clbGenre.Click += new System.EventHandler(this.clb_click);
            // 
            // btnReapplyFilter
            // 
            this.btnReapplyFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReapplyFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReapplyFilter.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnReapplyFilter.Location = new System.Drawing.Point(809, 12);
            this.btnReapplyFilter.Name = "btnReapplyFilter";
            this.btnReapplyFilter.Size = new System.Drawing.Size(92, 110);
            this.btnReapplyFilter.TabIndex = 7;
            this.btnReapplyFilter.Text = "RELOAD FILTER";
            this.btnReapplyFilter.UseVisualStyleBackColor = true;
            this.btnReapplyFilter.Click += new System.EventHandler(this.btnReapplyFilter_Click);
            // 
            // dudSearchOrder
            // 
            this.dudSearchOrder.Enabled = false;
            this.dudSearchOrder.Items.Add("DESC");
            this.dudSearchOrder.Items.Add("ASC");
            this.dudSearchOrder.Location = new System.Drawing.Point(438, 60);
            this.dudSearchOrder.Name = "dudSearchOrder";
            this.dudSearchOrder.Size = new System.Drawing.Size(70, 20);
            this.dudSearchOrder.TabIndex = 9;
            // 
            // cmbSearchOrder
            // 
            this.cmbSearchOrder.Enabled = false;
            this.cmbSearchOrder.FormattingEnabled = true;
            this.cmbSearchOrder.Items.AddRange(new object[] {
            "Date Added",
            "Size",
            "Seeders",
            "Leechers"});
            this.cmbSearchOrder.Location = new System.Drawing.Point(311, 60);
            this.cmbSearchOrder.Name = "cmbSearchOrder";
            this.cmbSearchOrder.Size = new System.Drawing.Size(121, 21);
            this.cmbSearchOrder.TabIndex = 8;
            this.cmbSearchOrder.Tag = "data,size,seeders,leechers";
            // 
            // chkMaxYear
            // 
            this.chkMaxYear.AutoSize = true;
            this.chkMaxYear.Location = new System.Drawing.Point(23, 99);
            this.chkMaxYear.Name = "chkMaxYear";
            this.chkMaxYear.Size = new System.Drawing.Size(114, 17);
            this.chkMaxYear.TabIndex = 7;
            this.chkMaxYear.Text = "Max. Content Year";
            this.chkMaxYear.UseVisualStyleBackColor = true;
            this.chkMaxYear.CheckedChanged += new System.EventHandler(this.chkMaxYear_CheckedChanged);
            // 
            // dtpMaxYear
            // 
            this.dtpMaxYear.CustomFormat = "yyyy";
            this.dtpMaxYear.Enabled = false;
            this.dtpMaxYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMaxYear.Location = new System.Drawing.Point(140, 96);
            this.dtpMaxYear.Name = "dtpMaxYear";
            this.dtpMaxYear.ShowUpDown = true;
            this.dtpMaxYear.Size = new System.Drawing.Size(60, 20);
            this.dtpMaxYear.TabIndex = 6;
            this.dtpMaxYear.Value = new System.DateTime(3000, 1, 1, 0, 0, 0, 0);
            // 
            // chkGenre
            // 
            this.chkGenre.AutoSize = true;
            this.chkGenre.Location = new System.Drawing.Point(528, 62);
            this.chkGenre.Name = "chkGenre";
            this.chkGenre.Size = new System.Drawing.Size(55, 17);
            this.chkGenre.TabIndex = 5;
            this.chkGenre.Text = "Genre";
            this.chkGenre.UseVisualStyleBackColor = true;
            this.chkGenre.CheckedChanged += new System.EventHandler(this.chkGenre_CheckedChanged);
            // 
            // chkSearchOrder
            // 
            this.chkSearchOrder.AutoSize = true;
            this.chkSearchOrder.Location = new System.Drawing.Point(224, 62);
            this.chkSearchOrder.Name = "chkSearchOrder";
            this.chkSearchOrder.Size = new System.Drawing.Size(86, 17);
            this.chkSearchOrder.TabIndex = 5;
            this.chkSearchOrder.Text = "SearchOrder";
            this.chkSearchOrder.UseVisualStyleBackColor = true;
            this.chkSearchOrder.CheckedChanged += new System.EventHandler(this.chkSearchOrder_CheckedChanged);
            // 
            // chkMinYear
            // 
            this.chkMinYear.AutoSize = true;
            this.chkMinYear.Location = new System.Drawing.Point(23, 62);
            this.chkMinYear.Name = "chkMinYear";
            this.chkMinYear.Size = new System.Drawing.Size(111, 17);
            this.chkMinYear.TabIndex = 5;
            this.chkMinYear.Text = "Min. Content Year";
            this.toolTip.SetToolTip(this.chkMinYear, "Applies to Movies Only");
            this.chkMinYear.UseVisualStyleBackColor = true;
            this.chkMinYear.CheckedChanged += new System.EventHandler(this.chkMinYear_CheckedChanged);
            // 
            // dtpMinYear
            // 
            this.dtpMinYear.CustomFormat = "yyyy";
            this.dtpMinYear.Enabled = false;
            this.dtpMinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMinYear.Location = new System.Drawing.Point(140, 59);
            this.dtpMinYear.Name = "dtpMinYear";
            this.dtpMinYear.ShowUpDown = true;
            this.dtpMinYear.Size = new System.Drawing.Size(60, 20);
            this.dtpMinYear.TabIndex = 4;
            this.dtpMinYear.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            // 
            // dtpMinUpDate
            // 
            this.dtpMinUpDate.Enabled = false;
            this.dtpMinUpDate.Location = new System.Drawing.Point(335, 21);
            this.dtpMinUpDate.Name = "dtpMinUpDate";
            this.dtpMinUpDate.Size = new System.Drawing.Size(181, 20);
            this.dtpMinUpDate.TabIndex = 4;
            this.dtpMinUpDate.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            // 
            // nudPageLimit
            // 
            this.nudPageLimit.Enabled = false;
            this.nudPageLimit.Location = new System.Drawing.Point(342, 96);
            this.nudPageLimit.Maximum = new decimal(new int[] {
            -559939585,
            902409669,
            54,
            0});
            this.nudPageLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPageLimit.Name = "nudPageLimit";
            this.nudPageLimit.Size = new System.Drawing.Size(68, 20);
            this.nudPageLimit.TabIndex = 3;
            this.nudPageLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudMinImdb
            // 
            this.nudMinImdb.DecimalPlaces = 1;
            this.nudMinImdb.Enabled = false;
            this.nudMinImdb.Location = new System.Drawing.Point(140, 22);
            this.nudMinImdb.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMinImdb.Name = "nudMinImdb";
            this.nudMinImdb.Size = new System.Drawing.Size(60, 20);
            this.nudMinImdb.TabIndex = 3;
            // 
            // chkPageLimit
            // 
            this.chkPageLimit.AutoSize = true;
            this.chkPageLimit.Location = new System.Drawing.Point(224, 99);
            this.chkPageLimit.Name = "chkPageLimit";
            this.chkPageLimit.Size = new System.Drawing.Size(112, 17);
            this.chkPageLimit.TabIndex = 2;
            this.chkPageLimit.Text = "Page Search Limit";
            this.chkPageLimit.UseVisualStyleBackColor = true;
            this.chkPageLimit.CheckedChanged += new System.EventHandler(this.chkPageLimit_CheckedChanged);
            // 
            // chkMinUpDate
            // 
            this.chkMinUpDate.AutoSize = true;
            this.chkMinUpDate.Location = new System.Drawing.Point(224, 23);
            this.chkMinUpDate.Name = "chkMinUpDate";
            this.chkMinUpDate.Size = new System.Drawing.Size(109, 17);
            this.chkMinUpDate.TabIndex = 2;
            this.chkMinUpDate.Text = "Min. Upload Date";
            this.chkMinUpDate.UseVisualStyleBackColor = true;
            this.chkMinUpDate.CheckedChanged += new System.EventHandler(this.chkMinUpDate_CheckedChanged);
            // 
            // chkMinImdb
            // 
            this.chkMinImdb.AutoSize = true;
            this.chkMinImdb.Location = new System.Drawing.Point(23, 26);
            this.chkMinImdb.Name = "chkMinImdb";
            this.chkMinImdb.Size = new System.Drawing.Size(106, 17);
            this.chkMinImdb.TabIndex = 2;
            this.chkMinImdb.Text = "Min. Imdb Rating";
            this.toolTip.SetToolTip(this.chkMinImdb, "Applies to Movies & TV Shows Only");
            this.chkMinImdb.UseVisualStyleBackColor = true;
            this.chkMinImdb.CheckedChanged += new System.EventHandler(this.chkMinImdb_CheckedChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstProgress,
            this.tstStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 615);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(931, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip";
            // 
            // tstProgress
            // 
            this.tstProgress.Name = "tstProgress";
            this.tstProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // tstStatus
            // 
            this.tstStatus.Name = "tstStatus";
            this.tstStatus.Size = new System.Drawing.Size(32, 17);
            this.tstStatus.Text = "Idle..";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripSeparator2,
            this.toolStripSeparator1,
            this.tsbRESET,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.toolStripDropDownButton1,
            this.toolStripSeparator6,
            this.toolStripSeparator7,
            this.tsbDonate,
            this.toolStripSeparator8,
            this.toolStripSeparator9,
            this.tsbAbout,
            this.toolStripSeparator10,
            this.toolStripSeparator11,
            this.toolStripSplitButton2,
            this.tsbReadNotify});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(931, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportEntriesToolStripMenuItem,
            this.toolStripSeparator3,
            this.importEntriesToolStripMenuItem});
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(41, 22);
            this.toolStripSplitButton1.Text = "FILE";
            // 
            // exportEntriesToolStripMenuItem
            // 
            this.exportEntriesToolStripMenuItem.Name = "exportEntriesToolStripMenuItem";
            this.exportEntriesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exportEntriesToolStripMenuItem.Text = "Export Listing";
            this.exportEntriesToolStripMenuItem.Click += new System.EventHandler(this.exportEntriesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // importEntriesToolStripMenuItem
            // 
            this.importEntriesToolStripMenuItem.Name = "importEntriesToolStripMenuItem";
            this.importEntriesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.importEntriesToolStripMenuItem.Text = "Import Listing";
            this.importEntriesToolStripMenuItem.Click += new System.EventHandler(this.importEntriesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRESET
            // 
            this.tsbRESET.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRESET.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRESET.Name = "tsbRESET";
            this.tsbRESET.Size = new System.Drawing.Size(42, 22);
            this.tsbRESET.Text = "RESET";
            this.tsbRESET.Click += new System.EventHandler(this.tsbRESET_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMarkedForDownload,
            this.tsmiDownloading,
            this.tsmiDownloaded,
            this.tsmiDeleted,
            this.tsmiAllMarked});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton1.Text = "View";
            // 
            // tsmiMarkedForDownload
            // 
            this.tsmiMarkedForDownload.Name = "tsmiMarkedForDownload";
            this.tsmiMarkedForDownload.Size = new System.Drawing.Size(191, 22);
            this.tsmiMarkedForDownload.Text = "Marked For Download";
            this.tsmiMarkedForDownload.Click += new System.EventHandler(this.tstView_Click);
            // 
            // tsmiDownloading
            // 
            this.tsmiDownloading.Name = "tsmiDownloading";
            this.tsmiDownloading.Size = new System.Drawing.Size(191, 22);
            this.tsmiDownloading.Text = "Downloading";
            this.tsmiDownloading.Click += new System.EventHandler(this.tstView_Click);
            // 
            // tsmiDownloaded
            // 
            this.tsmiDownloaded.Name = "tsmiDownloaded";
            this.tsmiDownloaded.Size = new System.Drawing.Size(191, 22);
            this.tsmiDownloaded.Text = "Downloaded";
            this.tsmiDownloaded.Click += new System.EventHandler(this.tstView_Click);
            // 
            // tsmiDeleted
            // 
            this.tsmiDeleted.Name = "tsmiDeleted";
            this.tsmiDeleted.Size = new System.Drawing.Size(191, 22);
            this.tsmiDeleted.Text = "Deleted";
            this.tsmiDeleted.Click += new System.EventHandler(this.tstView_Click);
            // 
            // tsmiAllMarked
            // 
            this.tsmiAllMarked.Name = "tsmiAllMarked";
            this.tsmiAllMarked.Size = new System.Drawing.Size(191, 22);
            this.tsmiAllMarked.Text = "All Marked Entries";
            this.tsmiAllMarked.Click += new System.EventHandler(this.tstView_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDonate
            // 
            this.tsbDonate.Image = ((System.Drawing.Image)(resources.GetObject("tsbDonate.Image")));
            this.tsbDonate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDonate.Name = "tsbDonate";
            this.tsbDonate.Size = new System.Drawing.Size(72, 22);
            this.tsbDonate.Text = "DONATE";
            this.tsbDonate.Click += new System.EventHandler(this.tsbDonate_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(49, 22);
            this.tsbAbout.Text = "ABOUT";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directDownloadToolStripMenuItem});
            this.toolStripSplitButton2.ForeColor = System.Drawing.Color.Goldenrod;
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(77, 22);
            this.toolStripSplitButton2.Text = "★ SPECIAL";
            // 
            // directDownloadToolStripMenuItem
            // 
            this.directDownloadToolStripMenuItem.Name = "directDownloadToolStripMenuItem";
            this.directDownloadToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.directDownloadToolStripMenuItem.Text = "Direct Download List";
            this.directDownloadToolStripMenuItem.Click += new System.EventHandler(this.directDownloadToolStripMenuItem_Click);
            // 
            // tsbReadNotify
            // 
            this.tsbReadNotify.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbReadNotify.AutoToolTip = false;
            this.tsbReadNotify.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbReadNotify.ForeColor = System.Drawing.Color.Salmon;
            this.tsbReadNotify.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadNotify.Image")));
            this.tsbReadNotify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadNotify.Name = "tsbReadNotify";
            this.tsbReadNotify.Size = new System.Drawing.Size(192, 22);
            this.tsbReadNotify.Text = "You have ? unread notifications";
            this.tsbReadNotify.Visible = false;
            this.tsbReadNotify.Click += new System.EventHandler(this.tsbReadNotify_Click);
            // 
            // pbTooltipImg
            // 
            this.pbTooltipImg.Location = new System.Drawing.Point(0, 0);
            this.pbTooltipImg.Name = "pbTooltipImg";
            this.pbTooltipImg.Size = new System.Drawing.Size(207, 258);
            this.pbTooltipImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTooltipImg.TabIndex = 11;
            this.pbTooltipImg.TabStop = false;
            this.pbTooltipImg.MouseEnter += new System.EventHandler(this.pbTooltipImg_MouseEnter);
            // 
            // pnlImdbInfo
            // 
            this.pnlImdbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlImdbInfo.AutoSize = true;
            this.pnlImdbInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlImdbInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlImdbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImdbInfo.Controls.Add(this.label3);
            this.pnlImdbInfo.Controls.Add(this.label2);
            this.pnlImdbInfo.Controls.Add(this.label1);
            this.pnlImdbInfo.Controls.Add(this.lblttGenre);
            this.pnlImdbInfo.Controls.Add(this.lblttName);
            this.pnlImdbInfo.Controls.Add(this.lblttRatingCount);
            this.pnlImdbInfo.Controls.Add(this.lblttRating);
            this.pnlImdbInfo.Controls.Add(this.pbTooltipImg);
            this.pnlImdbInfo.Location = new System.Drawing.Point(558, 331);
            this.pnlImdbInfo.Name = "pnlImdbInfo";
            this.pnlImdbInfo.Size = new System.Drawing.Size(353, 263);
            this.pnlImdbInfo.TabIndex = 12;
            this.pnlImdbInfo.Visible = false;
            this.pnlImdbInfo.MouseEnter += new System.EventHandler(this.pnlImdbInfo_MouseEnter);
            // 
            // lblttName
            // 
            this.lblttName.AutoSize = true;
            this.lblttName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblttName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblttName.Location = new System.Drawing.Point(201, 9);
            this.lblttName.Name = "lblttName";
            this.lblttName.Size = new System.Drawing.Size(120, 21);
            this.lblttName.TabIndex = 12;
            this.lblttName.Text = "Unknown (????)";
            // 
            // lblttRatingCount
            // 
            this.lblttRatingCount.AutoSize = true;
            this.lblttRatingCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblttRatingCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblttRatingCount.Location = new System.Drawing.Point(268, 71);
            this.lblttRatingCount.Name = "lblttRatingCount";
            this.lblttRatingCount.Size = new System.Drawing.Size(80, 15);
            this.lblttRatingCount.TabIndex = 12;
            this.lblttRatingCount.Text = "999999 Users";
            // 
            // lblttRating
            // 
            this.lblttRating.AutoSize = true;
            this.lblttRating.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblttRating.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblttRating.Location = new System.Drawing.Point(268, 44);
            this.lblttRating.Name = "lblttRating";
            this.lblttRating.Size = new System.Drawing.Size(36, 15);
            this.lblttRating.TabIndex = 12;
            this.lblttRating.Text = "10/10";
            // 
            // lblttGenre
            // 
            this.lblttGenre.AutoSize = true;
            this.lblttGenre.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblttGenre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblttGenre.Location = new System.Drawing.Point(269, 97);
            this.lblttGenre.Name = "lblttGenre";
            this.lblttGenre.Size = new System.Drawing.Size(22, 15);
            this.lblttGenre.TabIndex = 13;
            this.lblttGenre.Text = "???";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label1.Location = new System.Drawing.Point(201, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "Rating:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label2.Location = new System.Drawing.Point(201, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 19);
            this.label2.TabIndex = 14;
            this.label2.Text = "Rated By:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.label3.Location = new System.Drawing.Point(201, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 19);
            this.label3.TabIndex = 15;
            this.label3.Text = "Genre:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 637);
            this.Controls.Add(this.pnlImdbInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvListings);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.categ4);
            this.Controls.Add(this.categ3);
            this.Controls.Add(this.categ2);
            this.Controls.Add(this.categ1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(947, 676);
            this.Name = "Main";
            this.Text = "Rarbg Advanced Search";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListings)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinImdb)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTooltipImg)).EndInit();
            this.pnlImdbInfo.ResumeLayout(false);
            this.pnlImdbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox categ1;
        private System.Windows.Forms.CheckedListBox categ2;
        private System.Windows.Forms.CheckedListBox categ3;
        private System.Windows.Forms.CheckedListBox categ4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvListings;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar tstProgress;
        private System.Windows.Forms.ToolStripStatusLabel tstStatus;
        private System.Windows.Forms.CheckBox chkMinImdb;
        private System.Windows.Forms.CheckBox chkMinYear;
        private System.Windows.Forms.DateTimePicker dtpMinYear;
        private System.Windows.Forms.DateTimePicker dtpMinUpDate;
        private System.Windows.Forms.NumericUpDown nudMinImdb;
        private System.Windows.Forms.CheckBox chkMinUpDate;
        private System.Windows.Forms.CheckBox chkMaxYear;
        private System.Windows.Forms.DateTimePicker dtpMaxYear;
        private System.Windows.Forms.NumericUpDown nudPageLimit;
        private System.Windows.Forms.CheckBox chkPageLimit;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DomainUpDown dudSearchOrder;
        private System.Windows.Forms.ComboBox cmbSearchOrder;
        private System.Windows.Forms.CheckBox chkSearchOrder;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbRESET;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem exportEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importEntriesToolStripMenuItem;
        private System.Windows.Forms.Button btnReapplyFilter;
        private System.Windows.Forms.CheckedListBox clbGenre;
        private System.Windows.Forms.CheckBox chkGenre;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbDonate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarkedForDownload;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownloading;
        private System.Windows.Forms.ToolStripMenuItem tsmiDownloaded;
        private System.Windows.Forms.ToolStripMenuItem tsmiAllMarked;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgL;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgUploader;
        private System.Windows.Forms.DataGridViewTextBoxColumn Genre;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgImdb;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripDropDownButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem directDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbReadNotify;
        private System.Windows.Forms.PictureBox pbTooltipImg;
        private System.Windows.Forms.Panel pnlImdbInfo;
        private System.Windows.Forms.Label lblttRating;
        private System.Windows.Forms.Label lblttName;
        private System.Windows.Forms.Label lblttRatingCount;
        private System.Windows.Forms.Label lblttGenre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

