using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using RPFLib.Common;

namespace RPFTool
{
    public partial class gameSelection : DevExpress.XtraEditors.XtraForm
    {
        mainForm mainF;

        public gameSelection(bool rdrEnabled, bool mp3Enabled, bool mcEnabled, bool gtavEnabled)
        {
            InitializeComponent();
            btn_mc.Enabled = mcEnabled;
            btn_rdr.Enabled = rdrEnabled;
            btn_mp3.Enabled = mp3Enabled;
            btn_gtaV.Enabled = gtavEnabled;
        }

        private void btn_mp3_xbox_Click(object sender, EventArgs e)
        {
            DataUtil.setKey(keyHolder.mp3Key_xbox);
            loadMainForm("mp3Xbox");
        }

        private void btn_rdr_xbox_Click(object sender, EventArgs e)
        {
            DataUtil.setKey(keyHolder.rdrKey);
            loadMainForm("rdrXbox");
        }

        private void btn_mc_xbox_Click(object sender, EventArgs e)
        {
            DataUtil.setKey(keyHolder.mcKey);
            loadMainForm("mcXbox");
        }

        private void loadMainForm(String game)
        {
            this.Hide();
            mainF = new mainForm(game, this);
            mainF.Closed += (sender2, args) => this.Close();
            mainF.Show();
        }

        private void btn_gtaV_Click(object sender, EventArgs e)
        {
            DataUtil.setKey(keyHolder.gtaVKey);
            loadMainForm("gtaVXbox");
        }
    }
}