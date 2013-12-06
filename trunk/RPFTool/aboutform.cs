using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace RPFTool
{
    public partial class aboutform : DevExpress.XtraEditors.XtraForm
    {
        public aboutform()
        {
            InitializeComponent();
            WebLink.Links.Add(0, WebLink.Text.Length, "http://tmacdev.com");
        }

        private void WebLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
                Process.Start(sInfo);
            }
            catch { }
        }
    }
}