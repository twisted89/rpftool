using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RPFLib.Common;
using System.IO;
using DevExpress.XtraEditors;

namespace RPFTool
{
    public partial class form_extract : XtraForm
    {
        public form_extract(List<fileSystemObject> objectList, string pth)
        {
            InitializeComponent();

            filesystem = objectList;
            path = pth;
            fileprogress = 0;
            progressBar1.Properties.PercentView = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        private static List<fileSystemObject> filesystem;
        private static string path;
        private static int fileprogress = 0;
        private static int filecount = 0;

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            initialExtract(filesystem, path, e);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Position = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
            }

            else if (!(e.Error == null))
            {
                MessageBox.Show("Failed to extract all files :" + Environment.NewLine + e.Error.ToString());
            }

            else
            {
                MessageBox.Show("All files in archive exported.", "Export All", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            this.Invoke(new MethodInvoker(delegate { this.Close(); }));
        }

        private void form_extract_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void initialExtract(List<fileSystemObject> fileList, string path, DoWorkEventArgs e)
        {
            filecount = getFilecount(fileList);

            foreach (fileSystemObject item in fileList)
            {
                if (item.IsReturnDirectory)
                    continue;
                else if (item.IsDirectory)
                {
                    try
                    {
                        //var dir = item as RPFLib.Common.Directory;
                        if (item.Name == "Root")
                        {
                            ExtractToPath(item as RPFLib.Common.Directory, path + "\\", e);
                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(Path.Combine(path, item.Name));
                            ExtractToPath(item as RPFLib.Common.Directory, Path.Combine(path, item.Name) + "\\", e);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        int test  =  Convert.ToInt32(((double)fileprogress / filecount)*100.0);
                        backgroundWorker1.ReportProgress(test);
                        
                        var file = item as RPFLib.Common.File;
                        this.Invoke(new MethodInvoker(delegate { label_filename.Text = "Extracting: " + file.Name; }));
                        byte[] data = file.GetData(false);
                        System.IO.File.WriteAllBytes(Path.Combine(path, file.Name), data);
                        fileprogress++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
                if ((backgroundWorker1.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private int getFilecount(List<fileSystemObject> fileList)
        {
            int filecount = 0;
            foreach (fileSystemObject item in fileList)
            {
                if (item.IsReturnDirectory)
                    continue;
                else if (item.IsDirectory)
                {
                    filecount = dirCount(item as RPFLib.Common.Directory, filecount);
                }
                else
                {
                    filecount++;
                }
            }
            return filecount;
        }

        private int dirCount(RPFLib.Common.Directory dir, int filecount)
        {
            foreach (fileSystemObject item in dir)
            {
                if (item.IsReturnDirectory)
                    continue;
                else if (item.IsDirectory)
                {
                    filecount = dirCount(item as RPFLib.Common.Directory, filecount);
                }
                else
                {
                    filecount++;
                }
            }
            return filecount;
        }

        private void ExtractToPath(RPFLib.Common.Directory dir, string path, DoWorkEventArgs e)
        {
            foreach (fileSystemObject item in dir)
            {
                if ((backgroundWorker1.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                if (item.IsReturnDirectory)
                    continue;
                else if (item.IsDirectory)
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path + item.Name);
                        ExtractToPath(item as RPFLib.Common.Directory, Path.Combine(path, item.Name) + "\\", e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        int test = Convert.ToInt32(((double)fileprogress / filecount) * 100.0);
                        backgroundWorker1.ReportProgress(test);
                        var file = item as RPFLib.Common.File;
                        this.Invoke(new MethodInvoker(delegate { label_filename.Text = "Extracting: " + file.Name; }));
                        byte[] data = file.GetData(false);
                        System.IO.File.WriteAllBytes(Path.Combine(path, file.Name), data);
                        fileprogress++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_cancel_Click_1(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}
