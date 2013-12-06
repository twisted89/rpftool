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
    public partial class TextureView : XtraForm
    {

        public TextureView(byte[] data)
        {
            InitializeComponent();
        }

        /*
        RPFLib.Resouces.xtd xtd;

        public TextureView(ListViewItem[] items, RPFLib.Resouces.xtd xtdObject)
        {
            textureList.Items.Clear();
            textureList.Items.AddRange(items);
            xtd = xtdObject;
            InitializeComponent();
        }

        private void textureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            texturebox.Image = xtd.Textures[textureList.SelectedIndex].GeneratePreview(xtd.data);      
        }
         */
    }
}