#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using RPFLib;
using RPFLib.Common;
using System.Diagnostics;
using RPFLib.Resources;
#endregion


namespace RPFTool
{
    public partial class mainForm : XtraForm
    {

        #region Vars
        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();
        private string currentGame;
        gameSelection gameSelectForm;
        string _lastOpenPath;
        string currentFileName;
        BackgroundWorker bgwListBuilder = new BackgroundWorker();
        //private Version6 ver6RPF;
        //private Version4 ver4RPF;
        //private Version3 ver3RPF;
        private Archive archiveFile;
        List<fileSystemObject> masterlist = new List<fileSystemObject>();
        RPFLib.Common.Directory currentDir = null;
        bool searching = false;

        Color highlightCellColor = Color.FromArgb(255, 250, 250, 250);
        Color primaryCellColor = Color.FromArgb(0, 0, 0, 0);
        Color otherCellColor = Color.FromArgb(0, 0, 0, 0);
        Color headerBackColor = Color.FromArgb(255, 140, 140, 140);
        #endregion

        #region Constructor

        public mainForm(string gameSelected, gameSelection gsF)
        {
            currentGame = gameSelected;
            gameSelectForm = gsF;
            bgwListBuilder.WorkerReportsProgress = true;
            bgwListBuilder.DoWork += new DoWorkEventHandler(bgwListBuilder_DoWork);
            bgwListBuilder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwListBuilder_RunWorkerCompleted);
            InitializeComponent();
            tb_search.Edit.KeyDown += new KeyEventHandler(tb_search_KeyDown);
            setImage();
            // Initialize hot item style
            this.hotItemStyle1.ForeColor = highlightCellColor;
            RowBorderDecoration rbd = new RowBorderDecoration();
            rbd.BorderPen = new Pen(primaryCellColor, 0.5f);
            rbd.FillBrush = new SolidBrush(Color.FromArgb(32, Color.White));
            rbd.CornerRounding = 0;
            rbd.BoundsPadding = new Size(0, 0);
            rbd.LeftColumn = 1;
            this.hotItemStyle1.Decoration = rbd;
            HeaderFormatStyle headerstyle = new HeaderFormatStyle();
            headerstyle.SetBackColor(headerBackColor);
            filelistview.HeaderFormatStyle = headerstyle;
        }

        #endregion

        #region iMenuButtons

        private void menu_changeGame_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (archiveFile != null)
                {
                    archiveFile.Close();
                }
            }
            catch
            {
                MessageBox.Show("Failed to close the archive, please restart the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Hide();
            gameSelectForm.Show();
        }

        private void iOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "RPFTool Archive";
            ofd.Filter = "RPF Files (*.rpf)|*.rpf";
            ofd.FileName = _lastOpenPath;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                currentFileName = ofd.FileName;
                filelistview.ClearObjects();
                removeBreadCrumb(0);
                searching = false;
                currentDir = null;
                try
                {
                    if (archiveFile != null)
                    {
                        archiveFile.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Failed to close the archive, please restart the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                bgwListBuilder.RunWorkerAsync();
            }
        }

        private void iSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (archiveFile != null)
            {
                try
                {
                    archiveFile.Save();
                    filelistview.RefreshObjects(masterlist);
                    if (this.Text.Contains("*"))
                        this.Text = this.Text.Replace("*", "");
                }
                catch
                {
                    MessageBox.Show("Failed to replace file, archive may be corrupt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void iExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void iAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            aboutform About = new aboutform();
            About.ShowDialog();
        }

        private void iExtractAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (filelistview.Items.Count > 0)
            {
                List<fileSystemObject> objectList = new List<fileSystemObject>();
                using (var frm = new OpenFolderDialog())
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            using (Cursors.WaitCursor)
                            {
                                objectList.Add(archiveFile.RootDirectory);
                                if (objectList.Count > 0)
                                {
                                    form_extract extract_form = new form_extract(objectList, frm.Folder);
                                    extract_form.ShowDialog();
                                    extract_form.Dispose();
                                }
                                else
                                    MessageBox.Show("Failed to find root Dir", "Extract Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                        catch
                        {
                            MessageBox.Show("Failed to extract files", "Extract Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Return)
                {
                    var edititem = sender as DevExpress.XtraEditors.TextEdit;
                    if (archiveFile != null)
                    {
                        displaySearchList(archiveFile.search(archiveFile.RootDirectory, edititem.Text));
                        searching = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_exportFileList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btn_UnpackRSC_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (var oFile = new OpenFileDialog())
            {
                if (oFile.ShowDialog(this) == DialogResult.OK)
                {
                    RSCFile rsc = new RSCFile(System.IO.File.ReadAllBytes(oFile.FileName));
                    if (rsc.fileData != null)
                    {
                        using (var sFile = new SaveFileDialog())
                        {
                            sFile.FileName = Path.GetFileName(oFile.FileName) + "_unpacked";
                            if (sFile.ShowDialog(this) == DialogResult.OK)
                            {
                                System.IO.File.WriteAllBytes(sFile.FileName, rsc.fileData);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region BackgroundWorker

        private void bgwListBuilder_DoWork(object sender, DoWorkEventArgs e)
        {
            _lastOpenPath = currentFileName;
            try
            {
                using (Cursors.WaitCursor)
                {
                    using (BinaryReader s = new BinaryReader(new FileStream(currentFileName, FileMode.Open, FileAccess.Read)))
                    {
                        char[] Magic = new char[4];
                        s.Read(Magic, 0, 4);
                        string magicStr = new string(Magic);
                        switch (magicStr)
                        {
                            case "RPF6":
                                {
                                    archiveFile = new Version6();
                                    break;
                                }
                            case "RPF3":
                                {
                                    archiveFile = new Version3();
                                    break;
                                }
                            case "RPF4":
                                {
                                    archiveFile = new Version4();
                                    break;
                                }
                            case "RPF7":
                                {
                                    archiveFile = new Version7();
                                    break;
                                }
                            default:
                                MessageBox.Show("Invalid archive selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                        }
                    }
                    archiveFile.Open(currentFileName);
                    buildlist(archiveFile.RootDirectory);
                    startBreadCrumb(archiveFile.RootDirectory);
                }
                this.Invoke((MethodInvoker)delegate
                {
                    Text = Application.ProductName + " - " + new FileInfo(currentFileName).Name;
                });
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    filelistview.ClearObjects();
                    mainStatusbar.ItemLinks.Clear();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        private void bgwListBuilder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #endregion

        #region ViewMethods

        private void buildlist(RPFLib.Common.Directory dir)
        {
            masterlist.Clear();
            currentDir = dir;

            //Setup return dir
            if (dir.ParentDirectory != null)
            {
                RPFLib.Common.ReturnDir returnDir = new ReturnDir();
                returnDir.Tag = dir.ParentDirectory;
                masterlist.Add(returnDir);
            }
            foreach (fileSystemObject item in dir)
            {
                if (item.IsDirectory)
                {
                    var subdir = item as RPFLib.Common.Directory;
                    masterlist.Add(item);
                }
                else
                {
                    var subFile = item as RPFLib.Common.File;
                    masterlist.Add(item);
                }
            }
            setViewObjects(masterlist);
        }

        private void displaySearchList(List<fileSystemObject> searchList)
        {
            masterlist.Clear();
            masterlist = searchList;
            setViewObjects(masterlist);
        }

        delegate void setViewObjectsDelegate(List<fileSystemObject> setlist);
        private void setViewObjects(List<fileSystemObject> setlist)
        {
            if (InvokeRequired)
            {
                Invoke(new setViewObjectsDelegate(setViewObjects), setlist);
                return;
            }
            else
            {
                filelistview.SetObjects(setlist);
            }
        }

        private void setImage()
        {

            ImageList imageListSmall = new ImageList();

            Image documenticon = RPFTool.Properties.Resources.file;

            Image foldericon = RPFTool.Properties.Resources.folder;

            Bitmap folder = new Bitmap(foldericon);
            Bitmap document = new Bitmap(documenticon);

            // Initialize the ImageList objects with bitmaps.
            imageListSmall.Images.Add(document);
            imageListSmall.Images.Add(folder);

            filelistview.SmallImageList = imageListSmall;

            BrightIdeasSoftware.OLVColumn namecolumn = filelistview.GetColumn(0);
            namecolumn.ImageGetter = delegate(object rowObject)
            {
                fileSystemObject p = (fileSystemObject)rowObject;
                if (p is RPFLib.Common.File)
                    return 0; // document
                else
                    return 1; // folder
            };
        }

        private void fileViewObject_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
                e.SubItem.ForeColor = primaryCellColor;
            else
                e.SubItem.ForeColor = otherCellColor;
        }

        private void fileViewObject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (filelistview.SelectedObjects.Count == 1)
            {
                if (filelistview.SelectedObject is RPFLib.Common.ReturnDir)
                {
                    var returndirectory = filelistview.SelectedObject as RPFLib.Common.ReturnDir;
                    filelistview.ClearObjects();
                    buildlist(returndirectory.Tag);
                    removeBreadCrumb();
                }
                else if (filelistview.SelectedObject is RPFLib.Common.Directory)
                {
                    var directory = filelistview.SelectedObject as RPFLib.Common.Directory;
                    filelistview.ClearObjects();
                    buildlist(directory);
                    addBreadCrumb(directory);
                }
                else if (filelistview.SelectedObject is RPFLib.Common.File)
                {
                    if (currentGame == "rdrXbox")
                    {
                        var fileEntry = filelistview.SelectedObject as RPFLib.Common.File;
                        switch (fileEntry.resourcetype)
                        {
                            case 0:
                                {
                                    Viewers.TextView TextViewer = new Viewers.TextView(fileEntry.GetData(true), fileEntry);
                                    TextViewer.ShowDialog();
                                    filelistview.RefreshObjects(masterlist);
                                }
                                break;
                            case 1:
                                {
                                    //Viewers.StringsView StringViewer = new Viewers.StringsView(fileEntry.GetData());
                                    //StringViewer.ShowDialog();
                                }
                                break;
                            case 2:
                                {
                                    Viewers.xscView xscViewer = new Viewers.xscView(fileEntry.GetData(true));
                                    xscViewer.ShowDialog();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (currentGame == "gtaVXbox")
                    {
                        var fileEntry = filelistview.SelectedObject as RPFLib.Common.File;
                        switch (Path.GetExtension(fileEntry.Name))
                        {
                            case "xsc":
                                {
                                    Viewers.xscViewV7 xscViewer = new Viewers.xscViewV7(fileEntry.GetData(true));
                                    xscViewer.ShowDialog();
                                }
                                break;
                            default:
                                Viewers.TextView TextViewer = new Viewers.TextView(fileEntry.GetData(true), fileEntry);
                                TextViewer.ShowDialog();
                                filelistview.RefreshObjects(masterlist);
                                break;
                        }
                    }
                }
            }
        }

        private void mainForm_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Back)
                {
                    IntPtr wndHandle = GetFocus();
                    Control focusedControl = FromChildHandle(wndHandle);
                    if (focusedControl is DevExpress.XtraEditors.TextBoxMaskBox)
                        return;

                    if (currentDir != null && currentDir.ParentDirectory != null)
                    {
                        buildlist(currentDir.ParentDirectory);
                        removeBreadCrumb();
                    }
                }
            }
            catch { }
        }

        private void reset()
        {
            searching = false;
            if (archiveFile.RootDirectory != null)
            {
                buildlist(archiveFile.RootDirectory);
            }
            else
            {
                MessageBox.Show("Fatal error, returning to game selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void filelistview_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (e.Model is RPFLib.Common.File)
            {
                var file = e.Model as RPFLib.Common.File;
                if (file.IsCustomData)
                    e.Item.BackColor = Color.Orange;
                else
                    e.Item.BackColor = Color.Silver;
            }
        }

        #endregion

        #region BreakCrumbControl

        private void removeBreadCrumb(int ID = -1)
        {
            try
            {
                if (ID == -1)
                {
                    mainStatusbar.ItemLinks[mainStatusbar.ItemLinks.Count - 1].Dispose();
                    mainStatusbar.ItemLinks[mainStatusbar.ItemLinks.Count - 1].Dispose();
                }
                else
                {
                    while (ID < mainStatusbar.ItemLinks.Count)
                    {
                        mainStatusbar.ItemLinks[mainStatusbar.ItemLinks.Count - 1].Dispose();
                    }
                }
            }
            catch
            {
                mainStatusbar.ItemLinks.Clear();
            }
        }

        private void addBreadCrumb(RPFLib.Common.Directory directory)
        {
            try
            {
                BarStaticItem bartextItem = new BarStaticItem();
                bartextItem.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
                bartextItem.Caption = ">";
                bartextItem.Alignment = BarItemLinkAlignment.Left;
                mainStatusbar.AddItem(bartextItem);

                BarButtonItem barItem = new BarButtonItem();
                barItem.Id = mainStatusbar.ItemLinks.Count + 1;
                barItem.Tag = directory;
                barItem.Caption = directory.Name;
                barItem.ItemClick += new ItemClickEventHandler(barButtonItem_ItemClick);
                barItem.Alignment = BarItemLinkAlignment.Left;
                mainStatusbar.AddItem(barItem);
            }
            catch
            {
                mainStatusbar.ItemLinks.Clear();
            }
        }

        private void startBreadCrumb(RPFLib.Common.Directory directory)
        {
            try
            {
                BarButtonItem barItem = new BarButtonItem();
                barItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
                barItem.Glyph = RPFTool.Properties.Resources.Home;
                barItem.Id = mainStatusbar.ItemLinks.Count + 1;
                barItem.Tag = directory;
                barItem.Caption = "Root";
                this.barManager.Items.Add(barItem);
                barItem.ItemClick += new ItemClickEventHandler(barButtonItem_ItemClick);
                mainStatusbar.AddItem(barItem);
            }
            catch
            {
                mainStatusbar.ItemLinks.Clear();
            }
        }

        private void barButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Id < mainStatusbar.ItemLinks.Count || e.Item.Id == 2)
            {
                searching = false;
                buildlist(e.Item.Tag as RPFLib.Common.Directory);
                removeBreadCrumb(e.Item.Id);
            }
        }

        #endregion

        #region ContextMenu

        private void btn_replace_Click(object sender, EventArgs e)
        {
            if (filelistview.SelectedObjects.Count < 1 || filelistview.SelectedObjects.Count > 1)
            {
                MessageBox.Show("Only 1 file can be selected for replacing", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (filelistview.SelectedObject is RPFLib.Common.Directory)
            {
                MessageBox.Show("Cannot replace directories", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                var file = filelistview.SelectedObject as RPFLib.Common.File;
                using (var ofrm = new OpenFileDialog())
                {
                    if (ofrm.ShowDialog(this) == DialogResult.OK)
                    {
                        byte[] filebytes = System.IO.File.ReadAllBytes(ofrm.FileName);
                        file.SetData(filebytes);
                    }
                }
                filelistview.RefreshSelectedObjects();
                if (!this.Text.Contains("*"))
                    this.Text += this.Text + "*";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void btn_extract_Click(object sender, EventArgs e)
        {
            if (filelistview.SelectedObjects.Count < 1)
                return;

            List<fileSystemObject> objectList = new List<fileSystemObject>();
            if (filelistview.SelectedObjects.Count == 1)
            {
                // Handle single Dir
                if (filelistview.SelectedObject is RPFLib.Common.Directory)
                {
                    using (var sfrm = new OpenFolderDialog())
                    {
                        if (sfrm.ShowDialog(this) == DialogResult.OK)
                        {
                            try
                            {
                                using (Cursors.WaitCursor)
                                {
                                    objectList.Add(filelistview.SelectedObject as fileSystemObject);
                                    form_extract extract_form = new form_extract(objectList, Path.GetFullPath(sfrm.Folder));
                                    extract_form.ShowDialog();
                                    extract_form.Dispose();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Failed to extract files", "Extract Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    // Handle single file
                    using (var sfrm = new SaveFileDialog())
                    {
                        var file = filelistview.SelectedObject as RPFLib.Common.File;
                        sfrm.FileName = file.Name;
                        if (sfrm.ShowDialog(this) == DialogResult.OK)
                        {
                            byte[] data = file.GetData(false);
                            System.IO.File.WriteAllBytes(sfrm.FileName, data);
                        }
                    }
                }
            }
            //Handle multiple files/folders
            else if (filelistview.SelectedObjects.Count > 1)
            {
                using (var frm = new OpenFolderDialog())
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            using (Cursors.WaitCursor)
                            {
                                foreach (object item in filelistview.SelectedObjects)
                                {
                                    objectList.Add(item as fileSystemObject);
                                }
                                form_extract extract_form = new form_extract(objectList, frm.Folder);
                                extract_form.ShowDialog();
                                extract_form.Dispose();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Failed to extract files", "Extract Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
                return;
        }

        private void btn_goto_Click(object sender, EventArgs e)
        {
            try
            {
                if (filelistview.SelectedObjects.Count == 1)
                {
                    using (Cursors.WaitCursor)
                    {
                        List<RPFLib.Common.Directory> pathList = new List<RPFLib.Common.Directory>();
                        var currentfile = filelistview.SelectedObject as fileSystemObject;
                        string filename = currentfile.Name;
                        var currentDir = currentfile.ParentDirectory;
                        while (currentDir != null)
                        {
                            pathList.Add(currentDir);
                            currentDir = currentDir.ParentDirectory;
                        }
                        pathList.Reverse();
                        for (int i = 1; i < pathList.Count; i++)
                        {
                            var dir = pathList[i] as RPFLib.Common.Directory;
                            addBreadCrumb(dir);
                        }
                        buildlist(pathList[pathList.Count - 1]);
                        filelistview.SelectedIndex = filelistview.FindMatchingRow(filename, 0, SearchDirectionHint.Down);
                        filelistview.EnsureModelVisible(filelistview.SelectedObject);
                        searching = false;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to find file folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                reset();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (filelistview.SelectedObjects.Count > 0)
                {
                    foreach (fileSystemObject item in filelistview.SelectedObjects)
                    {
                        if (item is RPFLib.Common.File)
                        {
                            var fileitem = item as RPFLib.Common.File;
                            fileitem.DeleteEntry();
                            fileitem.ParentDirectory.DeleteObject(item);
                            if (!this.Text.Contains("*"))
                                this.Text += this.Text + "*";
                        }
                        else if (item is RPFLib.Common.Directory)
                        {
                            MessageBox.Show("Deletion of Folders is not yet supported", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    buildlist(currentDir);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Failed to delete files from archive, please reload the archive and try again." + Environment.NewLine + "Details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void popupMenu_Opening(object sender, CancelEventArgs e)
        {
            if (archiveFile == null)
            {
                e.Cancel = true;
                return;
            }
            if (searching)
                btn_goto.Visible = true;
            else
                btn_goto.Visible = false;
            if (archiveFile is Version6)
                btn_delete.Visible = false; // making delete invisible again
            else
                btn_delete.Visible = false;
        }

        #endregion

        #region onClosing
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (archiveFile != null)
                {
                    archiveFile.Close();
                }
            }
            catch
            {
                MessageBox.Show("Failed to close the archive, please restart the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}