using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RPFTool.Viewers
{
    public partial class TextView : XtraForm
    {
        RPFLib.Common.File fileEntry;

        public TextView(byte[] data, RPFLib.Common.File entry)
        {
            InitializeComponent();
            fileEntry = entry;
            textBox.Text = System.Text.Encoding.UTF8.GetString(data);
            textBox.Select(0, 0);
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (fileEntry != null)
            {
                fileEntry.SetData(System.Text.Encoding.UTF8.GetBytes(textBox.Text));
                if (this.Text.Contains("*"))
                {
                    this.Text = this.Text.Replace("*", "");
                }
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (!this.Text.Contains("*"))
            {
                this.Text += "*";
            }
        }
    }
}