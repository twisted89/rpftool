using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.IO;
using System.Threading;
using System.Security;

namespace RPFTool
{
    public partial class DownloadForm : DevExpress.XtraEditors.XtraForm
    {
        public string URL;
        private WebClient wc = new WebClient();

        public DownloadForm()
        {
            InitializeComponent();
        }

        private void DownloadForm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (URL == null || URL == "")
                {
                    MessageBox.Show("Download Failed, URL empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string downloadPath = Path.GetTempPath() + @"latest.zip";
                wc.DownloadProgressChanged += (s, f) =>
                {
                    downloadProgressBar.Position = f.ProgressPercentage;
                };
                wc.DownloadFileCompleted += (s, g) =>
                {
                    if (g.Cancelled)
                        return;
                    System.Diagnostics.Process.Start(downloadPath);
                    Application.Exit();
                };
                wc.DownloadFileAsync(new Uri(URL), downloadPath);
            }
            catch
            {
                MessageBox.Show("Download Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                wc.CancelAsync();
                this.Close();
            }
            catch
            {
                this.Close();
            }
        }



    }
}