using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace RPFTool
{
    public partial class Loader : DevExpress.XtraEditors.XtraForm
    {
        double Version = 1.3;
        bool rdrEnabled = false;
        bool mp3_xboxEnabled = false;
        bool mcEnabled = false;
        bool gtavEnabled = false;
        BackgroundWorker bw = new BackgroundWorker();

        public Loader()
        {
            InitializeComponent();
            label_Loading.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                try
                {

                    BackgroundWorker worker = sender as BackgroundWorker;

                    this.Invoke((MethodInvoker)delegate
                    {
                        label_Loading.Text = "Checking for updates...";
                    });


                    XDocument updateXML = XDocument.Load(@"http://tmacdev.com/updates/update.xml");

                    var latestVersion = updateXML.Element("application").Element("version");
                    var updateURL = updateXML.Element("application").Element("url");

                    if (Version < Convert.ToDouble(latestVersion.Value))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (MessageBox.Show("There is an updated version of RPF Tool available, download now?", "Update?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    DownloadForm dlForm = new DownloadForm();
                                    dlForm.URL = updateURL.Value;
                                    this.Hide();
                                    //dlForm.Closed += (sender2, args) => 
                                    dlForm.ShowDialog();
                                    this.Show();
                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.Show("Failed to download the latest version:" + ex.Message + Environment.NewLine + updateURL.Value.ToString(), "Error!", MessageBoxButtons.OK);
                                }
                            }
                        });
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Failed to download the latest version:" + ex.Message, "Error!", MessageBoxButtons.OK);
                }
                this.Invoke((MethodInvoker)delegate
                {
                    label_Loading.Text = "Getting Keys...";
                });

                try
                {
                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Keys.ini";
                    iniReader iniFile = new iniReader(path);
                    if (!File.Exists(path))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (MessageBox.Show("Keys.ini file not found, would you like to generate a blank config file?", "File not found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    // Create a new file 
                                    using (FileStream fs = File.Create(path))
                                    {
                                        // Add some text to file
                                        Byte[] title = new UTF8Encoding(true).GetBytes(
                                        @"[Keys]" + Environment.NewLine +
                                        "RDR=" + Environment.NewLine +
                                        "MP3=" + Environment.NewLine +
                                        "MC=" + Environment.NewLine +
                                        "GTAV=");
                                        fs.Write(title, 0, title.Length);
                                    }
                                }
                                catch
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        MessageBox.Show("Failed to create new config file, make sure you have permissions to write to the current directory or try running RPF Tool as Administrator.", "Error!", MessageBoxButtons.OK);
                                    });                                   
                                }
                            }
                        });
                    }
                    string key = iniFile.IniReadValue("Keys", "RDR").ToUpper();

                    if (CreateMD5Hash(key) == "D24B88DD3D21F81AA2831DC7C10F3065")
                    {
                        rdrEnabled = true;
                        keyHolder.rdrKey = StringToByteArray(key);
                    }
                    key = iniFile.IniReadValue("Keys", "MP3").ToUpper();
                    if (CreateMD5Hash(key) == "C07777FD1730F0547D13F1B64CBBE59F")
                    {
                        mp3_xboxEnabled = true;
                        keyHolder.mp3Key_xbox = StringToByteArray(key);
                    }
                    key = iniFile.IniReadValue("Keys", "MC").ToUpper();
                    if (CreateMD5Hash(key) == "292616021EB70171EF8360821A386B6D")
                    {
                        mcEnabled = true;
                        keyHolder.mcKey = StringToByteArray(key);
                    }
                    key = iniFile.IniReadValue("Keys", "GTAV").ToUpper();
                    //if (CreateMD5Hash(key) == "2C37D9BD5602F87D27CE3D1791381E34")
                    //{
                        gtavEnabled = true;
                        keyHolder.gtaVKey = StringToByteArray(key);
                   // }
                }
                catch (System.Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Failed to read Keys.ini:" + ex.Message, "Error!", MessageBoxButtons.OK);
                    });
                }


                this.Invoke((MethodInvoker)delegate
                {
                    label_Loading.Text = "Loading RPFTool...";
                });
            }
            catch (System.Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    throw new Exception(ex.Message, ex);
                });               
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
                var gameSelectForm = new gameSelection(rdrEnabled, mp3_xboxEnabled, mcEnabled, gtavEnabled);
                gameSelectForm.Closed += (sender2, args) => this.Close();
                gameSelectForm.Show();
            });
        }

        private void Loader_Shown(object sender, EventArgs e)
        {
            bw.RunWorkerAsync();
        }

        public string CreateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public byte[] StringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : 55);
        }

    }
}