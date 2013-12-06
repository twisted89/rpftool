using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Shell;

namespace RPFTool
{
    internal partial class Saving : DevExpress.XtraEditors.XtraForm
    {
        private FileStream newRPFStream;
        RPFLib.RPF6.File rpfFile;
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        TaskbarItemInfo TaskBar = new TaskbarItemInfo();

        public Saving(FileStream newRPF, RPFLib.RPF6.File file)
        {
            InitializeComponent();
            newRPFStream = newRPF;
            rpfFile = file;
            progressBar1.Properties.PercentView = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        private void Saving_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate { this.TaskBar.ProgressState = TaskbarItemProgressState.Normal; }));        
            rpfFile.save(newRPFStream, backgroundWorker1, e);
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Position = e.ProgressPercentage;
            if (Environment.OSVersion.Version.Major >= 6)
                TaskBar.ProgressValue = e.ProgressPercentage;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Failed to save new archive :" + Environment.NewLine + e.Error.ToString());
            }
            else
            {
                this.Hide();
                MessageBox.Show("New archive saved successfully.", "Success", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            if (Environment.OSVersion.Version.Major >= 6)
                this.Invoke(new MethodInvoker(delegate { this.TaskBar.ProgressState = TaskbarItemProgressState.None; }));           
            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}