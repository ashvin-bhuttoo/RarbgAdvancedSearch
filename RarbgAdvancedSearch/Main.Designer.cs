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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMaxYear = new System.Windows.Forms.CheckBox();
            this.dtpMaxYear = new System.Windows.Forms.DateTimePicker();
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
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgUploader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgImdb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSearchOrder = new System.Windows.Forms.CheckBox();
            this.cmbSearchOrder = new System.Windows.Forms.ComboBox();
            this.dudSearchOrder = new System.Windows.Forms.DomainUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinImdb)).BeginInit();
            this.statusStrip.SuspendLayout();
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
            this.categ1.Location = new System.Drawing.Point(12, 12);
            this.categ1.Name = "categ1";
            this.categ1.Size = new System.Drawing.Size(126, 94);
            this.categ1.TabIndex = 0;
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
            this.categ2.Location = new System.Drawing.Point(144, 12);
            this.categ2.Name = "categ2";
            this.categ2.Size = new System.Drawing.Size(126, 94);
            this.categ2.TabIndex = 0;
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
            this.categ3.Location = new System.Drawing.Point(276, 12);
            this.categ3.Name = "categ3";
            this.categ3.Size = new System.Drawing.Size(126, 94);
            this.categ3.TabIndex = 0;
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
            this.categ4.Location = new System.Drawing.Point(408, 12);
            this.categ4.Name = "categ4";
            this.categ4.Size = new System.Drawing.Size(126, 94);
            this.categ4.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnSearch.Location = new System.Drawing.Point(540, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(190, 63);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgName,
            this.dgAdded,
            this.dgSize,
            this.dgS,
            this.dgL,
            this.dgUploader,
            this.dgYear,
            this.dgImdb});
            this.dataGridView1.Location = new System.Drawing.Point(12, 250);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(718, 206);
            this.dataGridView1.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(540, 81);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(190, 25);
            this.textBox1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dudSearchOrder);
            this.groupBox1.Controls.Add(this.cmbSearchOrder);
            this.groupBox1.Controls.Add(this.chkMaxYear);
            this.groupBox1.Controls.Add(this.dtpMaxYear);
            this.groupBox1.Controls.Add(this.chkSearchOrder);
            this.groupBox1.Controls.Add(this.chkMinYear);
            this.groupBox1.Controls.Add(this.dtpMinYear);
            this.groupBox1.Controls.Add(this.dtpMinUpDate);
            this.groupBox1.Controls.Add(this.nudPageLimit);
            this.groupBox1.Controls.Add(this.nudMinImdb);
            this.groupBox1.Controls.Add(this.chkPageLimit);
            this.groupBox1.Controls.Add(this.chkMinUpDate);
            this.groupBox1.Controls.Add(this.chkMinImdb);
            this.groupBox1.Location = new System.Drawing.Point(12, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 104);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced Filters";
            // 
            // chkMaxYear
            // 
            this.chkMaxYear.AutoSize = true;
            this.chkMaxYear.Location = new System.Drawing.Point(224, 64);
            this.chkMaxYear.Name = "chkMaxYear";
            this.chkMaxYear.Size = new System.Drawing.Size(114, 17);
            this.chkMaxYear.TabIndex = 7;
            this.chkMaxYear.Text = "Max. Content Year";
            this.chkMaxYear.UseVisualStyleBackColor = true;
            // 
            // dtpMaxYear
            // 
            this.dtpMaxYear.CustomFormat = "yyyy";
            this.dtpMaxYear.Enabled = false;
            this.dtpMaxYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMaxYear.Location = new System.Drawing.Point(341, 61);
            this.dtpMaxYear.Name = "dtpMaxYear";
            this.dtpMaxYear.ShowUpDown = true;
            this.dtpMaxYear.Size = new System.Drawing.Size(60, 20);
            this.dtpMaxYear.TabIndex = 6;
            // 
            // chkMinYear
            // 
            this.chkMinYear.AutoSize = true;
            this.chkMinYear.Location = new System.Drawing.Point(23, 64);
            this.chkMinYear.Name = "chkMinYear";
            this.chkMinYear.Size = new System.Drawing.Size(111, 17);
            this.chkMinYear.TabIndex = 5;
            this.chkMinYear.Text = "Min. Content Year";
            this.toolTip.SetToolTip(this.chkMinYear, "Applies to Movies Only");
            this.chkMinYear.UseVisualStyleBackColor = true;
            // 
            // dtpMinYear
            // 
            this.dtpMinYear.CustomFormat = "yyyy";
            this.dtpMinYear.Enabled = false;
            this.dtpMinYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMinYear.Location = new System.Drawing.Point(140, 61);
            this.dtpMinYear.Name = "dtpMinYear";
            this.dtpMinYear.ShowUpDown = true;
            this.dtpMinYear.Size = new System.Drawing.Size(60, 20);
            this.dtpMinYear.TabIndex = 4;
            // 
            // dtpMinUpDate
            // 
            this.dtpMinUpDate.Enabled = false;
            this.dtpMinUpDate.Location = new System.Drawing.Point(341, 21);
            this.dtpMinUpDate.Name = "dtpMinUpDate";
            this.dtpMinUpDate.Size = new System.Drawing.Size(142, 20);
            this.dtpMinUpDate.TabIndex = 4;
            // 
            // nudPageLimit
            // 
            this.nudPageLimit.Enabled = false;
            this.nudPageLimit.Location = new System.Drawing.Point(627, 20);
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
            this.chkPageLimit.Location = new System.Drawing.Point(509, 23);
            this.chkPageLimit.Name = "chkPageLimit";
            this.chkPageLimit.Size = new System.Drawing.Size(112, 17);
            this.chkPageLimit.TabIndex = 2;
            this.chkPageLimit.Text = "Page Search Limit";
            this.chkPageLimit.UseVisualStyleBackColor = true;
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
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstProgress,
            this.tstStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 466);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(741, 22);
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
            // dgName
            // 
            this.dgName.HeaderText = "Name";
            this.dgName.Name = "dgName";
            this.dgName.Width = 200;
            // 
            // dgAdded
            // 
            this.dgAdded.HeaderText = "Added";
            this.dgAdded.Name = "dgAdded";
            // 
            // dgSize
            // 
            this.dgSize.HeaderText = "Size";
            this.dgSize.Name = "dgSize";
            // 
            // dgS
            // 
            this.dgS.HeaderText = "S";
            this.dgS.Name = "dgS";
            this.dgS.Width = 50;
            // 
            // dgL
            // 
            this.dgL.HeaderText = "L";
            this.dgL.Name = "dgL";
            this.dgL.Width = 50;
            // 
            // dgUploader
            // 
            this.dgUploader.HeaderText = "Uploader";
            this.dgUploader.Name = "dgUploader";
            this.dgUploader.Width = 75;
            // 
            // dgYear
            // 
            this.dgYear.HeaderText = "Year";
            this.dgYear.Name = "dgYear";
            this.dgYear.Width = 50;
            // 
            // dgImdb
            // 
            this.dgImdb.HeaderText = "Imdb";
            this.dgImdb.Name = "dgImdb";
            this.dgImdb.Width = 50;
            // 
            // chkSearchOrder
            // 
            this.chkSearchOrder.AutoSize = true;
            this.chkSearchOrder.Location = new System.Drawing.Point(422, 64);
            this.chkSearchOrder.Name = "chkSearchOrder";
            this.chkSearchOrder.Size = new System.Drawing.Size(86, 17);
            this.chkSearchOrder.TabIndex = 5;
            this.chkSearchOrder.Text = "SearchOrder";
            this.chkSearchOrder.UseVisualStyleBackColor = true;
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
            this.cmbSearchOrder.Location = new System.Drawing.Point(509, 62);
            this.cmbSearchOrder.Name = "cmbSearchOrder";
            this.cmbSearchOrder.Size = new System.Drawing.Size(121, 21);
            this.cmbSearchOrder.TabIndex = 8;
            // 
            // dudSearchOrder
            // 
            this.dudSearchOrder.Enabled = false;
            this.dudSearchOrder.Items.Add("DESC");
            this.dudSearchOrder.Items.Add("ASC");
            this.dudSearchOrder.Location = new System.Drawing.Point(636, 62);
            this.dudSearchOrder.Name = "dudSearchOrder";
            this.dudSearchOrder.Size = new System.Drawing.Size(70, 20);
            this.dudSearchOrder.TabIndex = 9;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 488);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.categ4);
            this.Controls.Add(this.categ3);
            this.Controls.Add(this.categ2);
            this.Controls.Add(this.categ1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(757, 527);
            this.Name = "Main";
            this.Text = "Rarbg Advanced Search";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinImdb)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox categ1;
        private System.Windows.Forms.CheckedListBox categ2;
        private System.Windows.Forms.CheckedListBox categ3;
        private System.Windows.Forms.CheckedListBox categ4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgAdded;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgL;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgUploader;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgImdb;
        private System.Windows.Forms.DomainUpDown dudSearchOrder;
        private System.Windows.Forms.ComboBox cmbSearchOrder;
        private System.Windows.Forms.CheckBox chkSearchOrder;
    }
}

