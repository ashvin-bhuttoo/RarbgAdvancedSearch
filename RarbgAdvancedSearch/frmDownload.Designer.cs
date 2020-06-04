namespace RarbgAdvancedSearch
{
    partial class frmDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownload));
            this.label1 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDLProgress = new System.Windows.Forms.Label();
            this.btnStartDownload = new System.Windows.Forms.Button();
            this.txtDLdir = new System.Windows.Forms.TextBox();
            this.btnChgDir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Download Size:";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSize.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.lblSize.Location = new System.Drawing.Point(135, 137);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(58, 15);
            this.lblSize.TabIndex = 0;
            this.lblSize.Text = "Unknown";
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.BackColor = System.Drawing.Color.DimGray;
            this.rtbLog.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.Location = new System.Drawing.Point(12, 155);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(681, 231);
            this.rtbLog.TabIndex = 1;
            this.rtbLog.Text = "";
            // 
            // pbDownload
            // 
            this.pbDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownload.Location = new System.Drawing.Point(12, 12);
            this.pbDownload.Maximum = 1000;
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(544, 55);
            this.pbDownload.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(220, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Download Progress: ";
            // 
            // lblDLProgress
            // 
            this.lblDLProgress.AutoSize = true;
            this.lblDLProgress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDLProgress.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.lblDLProgress.Location = new System.Drawing.Point(376, 137);
            this.lblDLProgress.Name = "lblDLProgress";
            this.lblDLProgress.Size = new System.Drawing.Size(57, 15);
            this.lblDLProgress.TabIndex = 0;
            this.lblDLProgress.Text = "Waiting...";
            // 
            // btnStartDownload
            // 
            this.btnStartDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartDownload.Enabled = false;
            this.btnStartDownload.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartDownload.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnStartDownload.Location = new System.Drawing.Point(562, 12);
            this.btnStartDownload.Name = "btnStartDownload";
            this.btnStartDownload.Size = new System.Drawing.Size(131, 55);
            this.btnStartDownload.TabIndex = 3;
            this.btnStartDownload.Text = "Start Download";
            this.btnStartDownload.UseVisualStyleBackColor = true;
            this.btnStartDownload.Click += new System.EventHandler(this.btnStartDownload_Click);
            // 
            // txtDLdir
            // 
            this.txtDLdir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDLdir.Location = new System.Drawing.Point(12, 96);
            this.txtDLdir.Name = "txtDLdir";
            this.txtDLdir.ReadOnly = true;
            this.txtDLdir.Size = new System.Drawing.Size(544, 20);
            this.txtDLdir.TabIndex = 4;
            // 
            // btnChgDir
            // 
            this.btnChgDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChgDir.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnChgDir.Location = new System.Drawing.Point(562, 94);
            this.btnChgDir.Name = "btnChgDir";
            this.btnChgDir.Size = new System.Drawing.Size(131, 23);
            this.btnChgDir.TabIndex = 5;
            this.btnChgDir.Text = "Change Directory";
            this.btnChgDir.UseVisualStyleBackColor = true;
            this.btnChgDir.Click += new System.EventHandler(this.btnChgDir_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Save to";
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 398);
            this.Controls.Add(this.btnChgDir);
            this.Controls.Add(this.txtDLdir);
            this.Controls.Add(this.btnStartDownload);
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.lblDLProgress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDownload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RarbgAdvancedSearch Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ProgressBar pbDownload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDLProgress;
        private System.Windows.Forms.Button btnStartDownload;
        private System.Windows.Forms.TextBox txtDLdir;
        private System.Windows.Forms.Button btnChgDir;
        private System.Windows.Forms.Label label3;
    }
}