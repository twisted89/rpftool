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
    public partial class StringsView : DevExpress.XtraEditors.XtraForm
    {
        public StringsView(byte[] data)
        {
            InitializeComponent();
        }
    }
}