namespace yt_dl
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picLogo = new PictureBox();
            lblUrl = new Label();
            txtUrl = new TextBox();
            lblQuality = new Label();
            cmbQuality = new ComboBox();
            lblOutput = new Label();
            txtOutput = new TextBox();
            btnBrowse = new Button();
            lblFilename = new Label();
            txtFilename = new TextBox();
            btnCheckStatus = new Button();
            chkBypassRestrictions = new CheckBox();
            linkLabelHelp = new LinkLabel();
            picThumbnail = new PictureBox();
            lblNodeStatus = new Label();
            lblCookieStatus = new Label();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            btnDownload = new Button();
            btnListFormats = new Button();
            menuStrip = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            updateYtDlpToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).BeginInit();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // picLogo
            // 
            picLogo.Location = new Point(12, 36);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(40, 40);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // lblUrl
            // 
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(12, 89);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(85, 15);
            lblUrl.TabIndex = 1;
            lblUrl.Text = "YouTube URL:";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(112, 86);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "https://www.youtube.com/watch?v=...";
            txtUrl.Size = new Size(460, 23);
            txtUrl.TabIndex = 2;
            txtUrl.TextChanged += txtUrl_TextChanged;
            // 
            // lblQuality
            // 
            lblQuality.AutoSize = true;
            lblQuality.Location = new Point(12, 124);
            lblQuality.Name = "lblQuality";
            lblQuality.Size = new Size(51, 15);
            lblQuality.TabIndex = 3;
            lblQuality.Text = "Quality:";
            // 
            // cmbQuality
            // 
            cmbQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbQuality.FormattingEnabled = true;
            cmbQuality.Location = new Point(112, 121);
            cmbQuality.Name = "cmbQuality";
            cmbQuality.Size = new Size(200, 23);
            cmbQuality.TabIndex = 4;
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(12, 159);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(86, 15);
            lblOutput.TabIndex = 5;
            lblOutput.Text = "Output Folder:";
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(112, 156);
            txtOutput.Name = "txtOutput";
            txtOutput.PlaceholderText = "C:\\Users\\Username\\Downloads";
            txtOutput.Size = new Size(385, 23);
            txtOutput.TabIndex = 6;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(503, 156);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(69, 23);
            btnBrowse.TabIndex = 7;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // lblFilename
            // 
            lblFilename.AutoSize = true;
            lblFilename.Location = new Point(12, 194);
            lblFilename.Name = "lblFilename";
            lblFilename.Size = new Size(88, 15);
            lblFilename.TabIndex = 8;
            lblFilename.Text = "Filename (opt):";
            // 
            // txtFilename
            // 
            txtFilename.Location = new Point(112, 191);
            txtFilename.Name = "txtFilename";
            txtFilename.PlaceholderText = "Leave empty to use video title";
            txtFilename.Size = new Size(385, 23);
            txtFilename.TabIndex = 9;
            // 
            // btnCheckStatus
            // 
            btnCheckStatus.Location = new Point(503, 191);
            btnCheckStatus.Name = "btnCheckStatus";
            btnCheckStatus.Size = new Size(69, 23);
            btnCheckStatus.TabIndex = 10;
            btnCheckStatus.Text = "Status";
            btnCheckStatus.UseVisualStyleBackColor = true;
            btnCheckStatus.Click += btnCheckStatus_Click;
            // 
            // chkBypassRestrictions
            // 
            chkBypassRestrictions.AutoSize = true;
            chkBypassRestrictions.Location = new Point(12, 226);
            chkBypassRestrictions.Name = "chkBypassRestrictions";
            chkBypassRestrictions.Size = new Size(272, 19);
            chkBypassRestrictions.TabIndex = 11;
            chkBypassRestrictions.Text = "Bypass age restrictions (requires authentication)";
            chkBypassRestrictions.UseVisualStyleBackColor = true;
            // 
            // linkLabelHelp
            // 
            linkLabelHelp.AutoSize = true;
            linkLabelHelp.Location = new Point(290, 227);
            linkLabelHelp.Name = "linkLabelHelp";
            linkLabelHelp.Size = new Size(32, 15);
            linkLabelHelp.TabIndex = 12;
            linkLabelHelp.TabStop = true;
            linkLabelHelp.Text = "Help";
            linkLabelHelp.LinkClicked += linkLabelHelp_LinkClicked;
            // 
            // picThumbnail
            // 
            picThumbnail.BorderStyle = BorderStyle.FixedSingle;
            picThumbnail.Location = new Point(12, 251);
            picThumbnail.Name = "picThumbnail";
            picThumbnail.Size = new Size(160, 90);
            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;
            picThumbnail.TabIndex = 13;
            picThumbnail.TabStop = false;
            // 
            // lblNodeStatus
            // 
            lblNodeStatus.AutoSize = true;
            lblNodeStatus.Location = new Point(184, 256);
            lblNodeStatus.Name = "lblNodeStatus";
            lblNodeStatus.Size = new Size(117, 15);
            lblNodeStatus.TabIndex = 14;
            lblNodeStatus.Text = "Node.js: ✓ Installed";
            // 
            // lblCookieStatus
            // 
            lblCookieStatus.AutoSize = true;
            lblCookieStatus.Location = new Point(184, 281);
            lblCookieStatus.Name = "lblCookieStatus";
            lblCookieStatus.Size = new Size(119, 15);
            lblCookieStatus.TabIndex = 15;
            lblCookieStatus.Text = "cookies.txt: ✓ Found";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 353);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 16;
            lblStatus.Text = "Ready";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 371);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(477, 23);
            progressBar.TabIndex = 17;
            // 
            // btnDownload
            // 
            btnDownload.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDownload.Location = new Point(495, 353);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(77, 41);
            btnDownload.TabIndex = 18;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // btnListFormats
            // 
            btnListFormats.Location = new Point(318, 121);
            btnListFormats.Name = "btnListFormats";
            btnListFormats.Size = new Size(95, 23);
            btnListFormats.TabIndex = 19;
            btnListFormats.Text = "List Formats";
            btnListFormats.UseVisualStyleBackColor = true;
            btnListFormats.Click += btnListFormats_Click;
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(584, 24);
            menuStrip.TabIndex = 20;
            menuStrip.Text = "menuStrip";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { updateYtDlpToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // updateYtDlpToolStripMenuItem
            // 
            updateYtDlpToolStripMenuItem.Name = "updateYtDlpToolStripMenuItem";
            updateYtDlpToolStripMenuItem.Size = new Size(180, 22);
            updateYtDlpToolStripMenuItem.Text = "Update yt-dlp";
            updateYtDlpToolStripMenuItem.Click += updateYtDlpToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 406);
            Controls.Add(menuStrip);
            Controls.Add(btnListFormats);
            Controls.Add(btnDownload);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            Controls.Add(lblCookieStatus);
            Controls.Add(lblNodeStatus);
            Controls.Add(picThumbnail);
            Controls.Add(linkLabelHelp);
            Controls.Add(chkBypassRestrictions);
            Controls.Add(btnCheckStatus);
            Controls.Add(txtFilename);
            Controls.Add(lblFilename);
            Controls.Add(btnBrowse);
            Controls.Add(txtOutput);
            Controls.Add(lblOutput);
            Controls.Add(cmbQuality);
            Controls.Add(lblQuality);
            Controls.Add(txtUrl);
            Controls.Add(lblUrl);
            Controls.Add(picLogo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YouTube Downloader";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).EndInit();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picLogo;
        private Label lblUrl;
        private TextBox txtUrl;
        private Label lblQuality;
        private ComboBox cmbQuality;
        private Label lblOutput;
        private TextBox txtOutput;
        private Button btnBrowse;
        private Label lblFilename;
        private TextBox txtFilename;
        private Button btnCheckStatus;
        private CheckBox chkBypassRestrictions;
        private LinkLabel linkLabelHelp;
        private PictureBox picThumbnail;
        private Label lblNodeStatus;
        private Label lblCookieStatus;
        private Label lblStatus;
        private ProgressBar progressBar;
        private Button btnDownload;
        private Button btnListFormats;
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem updateYtDlpToolStripMenuItem;
    }
}
