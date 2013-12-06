namespace RPFTool
{
    partial class gameSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gameSelection));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl3 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_mp3 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rdr = new DevExpress.XtraEditors.SimpleButton();
            this.btn_mc = new DevExpress.XtraEditors.SimpleButton();
            this.btn_gtaV = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.IsSplitterFixed = true;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(315, 374);
            this.splitContainerControl1.SplitterPosition = 127;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.IsSplitterFixed = true;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.btn_mp3);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(315, 127);
            this.splitContainerControl2.SplitterPosition = 0;
            this.splitContainerControl2.TabIndex = 1;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl3.Horizontal = false;
            this.splitContainerControl3.IsSplitterFixed = true;
            this.splitContainerControl3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl3.Name = "splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add(this.btn_rdr);
            this.splitContainerControl3.Panel1.Text = "Panel1";
            this.splitContainerControl3.Panel2.Controls.Add(this.btn_mc);
            this.splitContainerControl3.Panel2.Text = "Panel2";
            this.splitContainerControl3.Size = new System.Drawing.Size(315, 242);
            this.splitContainerControl3.SplitterPosition = 127;
            this.splitContainerControl3.TabIndex = 0;
            this.splitContainerControl3.Text = "splitContainerControl3";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainerControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btn_gtaV);
            this.splitContainer1.Size = new System.Drawing.Size(315, 486);
            this.splitContainer1.SplitterDistance = 374;
            this.splitContainer1.TabIndex = 2;
            // 
            // btn_mp3
            // 
            this.btn_mp3.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_mp3.Appearance.Options.UseFont = true;
            this.btn_mp3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_mp3.Enabled = false;
            this.btn_mp3.Image = global::RPFTool.Properties.Resources.mp3;
            this.btn_mp3.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_mp3.Location = new System.Drawing.Point(0, 0);
            this.btn_mp3.Name = "btn_mp3";
            this.btn_mp3.Size = new System.Drawing.Size(315, 122);
            this.btn_mp3.TabIndex = 1;
            this.btn_mp3.Click += new System.EventHandler(this.btn_mp3_xbox_Click);
            // 
            // btn_rdr
            // 
            this.btn_rdr.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_rdr.Appearance.Options.UseFont = true;
            this.btn_rdr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_rdr.Enabled = false;
            this.btn_rdr.Image = global::RPFTool.Properties.Resources.RDR4;
            this.btn_rdr.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_rdr.Location = new System.Drawing.Point(0, 0);
            this.btn_rdr.Name = "btn_rdr";
            this.btn_rdr.Size = new System.Drawing.Size(315, 127);
            this.btn_rdr.TabIndex = 2;
            this.btn_rdr.Click += new System.EventHandler(this.btn_rdr_xbox_Click);
            // 
            // btn_mc
            // 
            this.btn_mc.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_mc.Appearance.Options.UseFont = true;
            this.btn_mc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_mc.Enabled = false;
            this.btn_mc.Image = global::RPFTool.Properties.Resources.MC;
            this.btn_mc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_mc.Location = new System.Drawing.Point(0, 0);
            this.btn_mc.Name = "btn_mc";
            this.btn_mc.Size = new System.Drawing.Size(315, 110);
            this.btn_mc.TabIndex = 3;
            this.btn_mc.Click += new System.EventHandler(this.btn_mc_xbox_Click);
            // 
            // btn_gtaV
            // 
            this.btn_gtaV.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btn_gtaV.Appearance.Options.UseFont = true;
            this.btn_gtaV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_gtaV.Enabled = false;
            this.btn_gtaV.Image = global::RPFTool.Properties.Resources.gtav;
            this.btn_gtaV.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btn_gtaV.Location = new System.Drawing.Point(0, 0);
            this.btn_gtaV.Name = "btn_gtaV";
            this.btn_gtaV.Size = new System.Drawing.Size(315, 108);
            this.btn_gtaV.TabIndex = 4;
            this.btn_gtaV.Click += new System.EventHandler(this.btn_gtaV_Click);
            // 
            // gameSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 486);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "gameSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select a Game...";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).EndInit();
            this.splitContainerControl3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl3;
        private DevExpress.XtraEditors.SimpleButton btn_mp3;
        private DevExpress.XtraEditors.SimpleButton btn_rdr;
        private DevExpress.XtraEditors.SimpleButton btn_mc;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.SimpleButton btn_gtaV;

    }
}