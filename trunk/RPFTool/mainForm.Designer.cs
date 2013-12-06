namespace RPFTool
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.filelistview = new BrightIdeasSoftware.ObjectListView();
            this.columnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnAttributes = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btn_extract = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_replace = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_goto = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.hotItemStyle1 = new BrightIdeasSoftware.HotItemStyle();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.mFile = new DevExpress.XtraBars.BarSubItem();
            this.iOpen = new DevExpress.XtraBars.BarButtonItem();
            this.menu_changeGame = new DevExpress.XtraBars.BarButtonItem();
            this.iSave = new DevExpress.XtraBars.BarButtonItem();
            this.iExit = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.iExtractAll = new DevExpress.XtraBars.BarButtonItem();
            this.btn_exportFileList = new DevExpress.XtraBars.BarButtonItem();
            this.btn_UnpackRSC = new DevExpress.XtraBars.BarButtonItem();
            this.mHelp = new DevExpress.XtraBars.BarSubItem();
            this.iAbout = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.tb_search = new DevExpress.XtraBars.BarEditItem();
            this.tb_searchEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.iNew = new DevExpress.XtraBars.BarButtonItem();
            this.iSaveAs = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.popupExtract = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.popupReplace = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.barDockControl5 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl6 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl7 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl8 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl9 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl10 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl11 = new DevExpress.XtraBars.BarDockControl();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.mainStatusbar = new DevExpress.XtraBars.Bar();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barDockControl15 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl16 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl17 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl18 = new DevExpress.XtraBars.BarDockControl();
            this.barManager2 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar4 = new DevExpress.XtraBars.Bar();
            this.barRenderer1 = new BrightIdeasSoftware.BarRenderer();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.filelistview)).BeginInit();
            this.popupMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_searchEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).BeginInit();
            this.SuspendLayout();
            // 
            // filelistview
            // 
            this.filelistview.AllColumns.Add(this.columnName);
            this.filelistview.AllColumns.Add(this.columnAttributes);
            this.filelistview.AllColumns.Add(this.columnSize);
            this.filelistview.AlternateRowBackColor = System.Drawing.Color.Black;
            this.filelistview.BackColor = System.Drawing.Color.Silver;
            this.filelistview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filelistview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnAttributes,
            this.columnSize});
            this.filelistview.ContextMenuStrip = this.popupMenu;
            this.filelistview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filelistview.FullRowSelect = true;
            this.filelistview.HeaderUsesThemes = false;
            this.filelistview.HighlightBackgroundColor = System.Drawing.Color.Black;
            this.filelistview.HighlightForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.filelistview.HotItemStyle = this.hotItemStyle1;
            this.filelistview.Location = new System.Drawing.Point(0, 22);
            this.filelistview.Name = "filelistview";
            this.filelistview.ShowGroups = false;
            this.filelistview.Size = new System.Drawing.Size(867, 545);
            this.filelistview.TabIndex = 22;
            this.filelistview.UseCellFormatEvents = true;
            this.filelistview.UseCompatibleStateImageBehavior = false;
            this.filelistview.View = System.Windows.Forms.View.Details;
            this.filelistview.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.fileViewObject_FormatCell);
            this.filelistview.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.filelistview_FormatRow);
            this.filelistview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fileViewObject_MouseDoubleClick);
            // 
            // columnName
            // 
            this.columnName.AspectName = "Name";
            this.columnName.Text = "Name";
            this.columnName.Width = 320;
            // 
            // columnAttributes
            // 
            this.columnAttributes.AspectName = "Attributes";
            this.columnAttributes.Text = "Attributes";
            this.columnAttributes.Width = 300;
            // 
            // columnSize
            // 
            this.columnSize.AspectName = "SizeS";
            this.columnSize.FillsFreeSpace = true;
            this.columnSize.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSize.Text = "Size";
            this.columnSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnSize.Width = 136;
            // 
            // popupMenu
            // 
            this.popupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_extract,
            this.btn_replace,
            this.btn_goto,
            this.btn_delete});
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.Size = new System.Drawing.Size(153, 92);
            this.popupMenu.Opening += new System.ComponentModel.CancelEventHandler(this.popupMenu_Opening);
            // 
            // btn_extract
            // 
            this.btn_extract.Name = "btn_extract";
            this.btn_extract.Size = new System.Drawing.Size(152, 22);
            this.btn_extract.Text = "Extract";
            this.btn_extract.Click += new System.EventHandler(this.btn_extract_Click);
            // 
            // btn_replace
            // 
            this.btn_replace.Name = "btn_replace";
            this.btn_replace.Size = new System.Drawing.Size(152, 22);
            this.btn_replace.Text = "Replace";
            this.btn_replace.Click += new System.EventHandler(this.btn_replace_Click);
            // 
            // btn_goto
            // 
            this.btn_goto.Name = "btn_goto";
            this.btn_goto.Size = new System.Drawing.Size(152, 22);
            this.btn_goto.Text = "Go to Location";
            this.btn_goto.Click += new System.EventHandler(this.btn_goto_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(152, 22);
            this.btn_delete.Text = "Delete";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.mFile,
            this.barButtonItem2,
            this.iOpen,
            this.menu_changeGame,
            this.iNew,
            this.iSave,
            this.iSaveAs,
            this.iExit,
            this.mHelp,
            this.iAbout,
            this.barSubItem1,
            this.popupExtract,
            this.barButtonItem3,
            this.popupReplace,
            this.barSubItem2,
            this.iExtractAll,
            this.barSubItem3,
            this.tb_search,
            this.barStaticItem1,
            this.btn_exportFileList,
            this.btn_UnpackRSC});
            this.barManager.MainMenu = this.bar2;
            this.barManager.MaxItemId = 37;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.tb_searchEdit});
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.FloatLocation = new System.Drawing.Point(87, 153);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.mFile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.mHelp),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.tb_search)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // mFile
            // 
            this.mFile.Caption = "&File";
            this.mFile.Id = 0;
            this.mFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.menu_changeGame),
            new DevExpress.XtraBars.LinkPersistInfo(this.iSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.iExit)});
            this.mFile.Name = "mFile";
            // 
            // iOpen
            // 
            this.iOpen.Caption = "&Open";
            this.iOpen.Id = 4;
            this.iOpen.Name = "iOpen";
            this.iOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iOpen_ItemClick);
            // 
            // menu_changeGame
            // 
            this.menu_changeGame.Caption = "&Change Game";
            this.menu_changeGame.Id = 5;
            this.menu_changeGame.Name = "menu_changeGame";
            this.menu_changeGame.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.menu_changeGame_ItemClick);
            // 
            // iSave
            // 
            this.iSave.Caption = "&Save";
            this.iSave.Id = 7;
            this.iSave.Name = "iSave";
            this.iSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iSave_ItemClick);
            // 
            // iExit
            // 
            this.iExit.Caption = "E&xit";
            this.iExit.Id = 9;
            this.iExit.Name = "iExit";
            this.iExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iExit_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "Tools";
            this.barSubItem2.Id = 17;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iExtractAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_exportFileList),
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_UnpackRSC)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // iExtractAll
            // 
            this.iExtractAll.Caption = "Extract All";
            this.iExtractAll.Id = 18;
            this.iExtractAll.Name = "iExtractAll";
            this.iExtractAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iExtractAll_ItemClick);
            // 
            // btn_exportFileList
            // 
            this.btn_exportFileList.Caption = "Export Filelist";
            this.btn_exportFileList.Id = 35;
            this.btn_exportFileList.Name = "btn_exportFileList";
            this.btn_exportFileList.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btn_exportFileList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_exportFileList_ItemClick);
            // 
            // btn_UnpackRSC
            // 
            this.btn_UnpackRSC.Caption = "Unpack Resource";
            this.btn_UnpackRSC.Id = 36;
            this.btn_UnpackRSC.Name = "btn_UnpackRSC";
            this.btn_UnpackRSC.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_UnpackRSC_ItemClick);
            // 
            // mHelp
            // 
            this.mHelp.Caption = "&Help";
            this.mHelp.Id = 10;
            this.mHelp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iAbout)});
            this.mHelp.Name = "mHelp";
            // 
            // iAbout
            // 
            this.iAbout.Caption = "&About";
            this.iAbout.Id = 11;
            this.iAbout.Name = "iAbout";
            this.iAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iAbout_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem1.Caption = "Search";
            this.barStaticItem1.Id = 22;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // tb_search
            // 
            this.tb_search.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.tb_search.Edit = this.tb_searchEdit;
            this.tb_search.Id = 21;
            this.tb_search.Name = "tb_search";
            this.tb_search.Width = 171;
            // 
            // tb_searchEdit
            // 
            this.tb_searchEdit.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.tb_searchEdit.AutoHeight = false;
            this.tb_searchEdit.Name = "tb_searchEdit";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(867, 22);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 567);
            this.barDockControlBottom.Size = new System.Drawing.Size(867, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 545);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(867, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 545);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Open";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // iNew
            // 
            this.iNew.Caption = "&New";
            this.iNew.Id = 6;
            this.iNew.Name = "iNew";
            // 
            // iSaveAs
            // 
            this.iSaveAs.Caption = "Save &As";
            this.iSaveAs.Id = 8;
            this.iSaveAs.Name = "iSaveAs";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Extract";
            this.barSubItem1.Id = 12;
            this.barSubItem1.Name = "barSubItem1";
            // 
            // popupExtract
            // 
            this.popupExtract.Id = 27;
            this.popupExtract.Name = "popupExtract";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Id = 15;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // popupReplace
            // 
            this.popupReplace.Id = 28;
            this.popupReplace.Name = "popupReplace";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "Options";
            this.barSubItem3.Id = 19;
            this.barSubItem3.Name = "barSubItem3";
            // 
            // barDockControl5
            // 
            this.barDockControl5.CausesValidation = false;
            this.barDockControl5.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl5.Location = new System.Drawing.Point(0, 0);
            this.barDockControl5.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl4.Location = new System.Drawing.Point(0, 0);
            this.barDockControl4.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl6
            // 
            this.barDockControl6.CausesValidation = false;
            this.barDockControl6.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl6.Location = new System.Drawing.Point(0, 0);
            this.barDockControl6.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl3.Location = new System.Drawing.Point(0, 0);
            this.barDockControl3.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl7
            // 
            this.barDockControl7.CausesValidation = false;
            this.barDockControl7.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl7.Location = new System.Drawing.Point(0, 0);
            this.barDockControl7.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl8
            // 
            this.barDockControl8.CausesValidation = false;
            this.barDockControl8.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl8.Location = new System.Drawing.Point(0, 0);
            this.barDockControl8.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl2.Location = new System.Drawing.Point(0, 0);
            this.barDockControl2.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl9
            // 
            this.barDockControl9.CausesValidation = false;
            this.barDockControl9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl9.Location = new System.Drawing.Point(0, 0);
            this.barDockControl9.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl10
            // 
            this.barDockControl10.CausesValidation = false;
            this.barDockControl10.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl10.Location = new System.Drawing.Point(0, 0);
            this.barDockControl10.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControl11
            // 
            this.barDockControl11.CausesValidation = false;
            this.barDockControl11.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl11.Location = new System.Drawing.Point(0, 0);
            this.barDockControl11.Size = new System.Drawing.Size(0, 0);
            // 
            // bar1
            // 
            this.bar1.BarName = "Main menu";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.MultiLine = true;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Main menu";
            // 
            // bar3
            // 
            this.bar3.BarName = "Main menu";
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.MultiLine = true;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Main menu";
            // 
            // mainStatusbar
            // 
            this.mainStatusbar.BarName = "Statusbar";
            this.mainStatusbar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.mainStatusbar.DockCol = 0;
            this.mainStatusbar.DockRow = 0;
            this.mainStatusbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.mainStatusbar.OptionsBar.AllowQuickCustomization = false;
            this.mainStatusbar.OptionsBar.DrawDragBorder = false;
            this.mainStatusbar.OptionsBar.UseWholeRow = true;
            this.mainStatusbar.Text = "Status bar";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // barDockControl15
            // 
            this.barDockControl15.CausesValidation = false;
            this.barDockControl15.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl15.Location = new System.Drawing.Point(0, 0);
            this.barDockControl15.Size = new System.Drawing.Size(867, 0);
            // 
            // barDockControl16
            // 
            this.barDockControl16.CausesValidation = false;
            this.barDockControl16.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl16.Location = new System.Drawing.Point(0, 567);
            this.barDockControl16.Size = new System.Drawing.Size(867, 23);
            // 
            // barDockControl17
            // 
            this.barDockControl17.CausesValidation = false;
            this.barDockControl17.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl17.Location = new System.Drawing.Point(0, 0);
            this.barDockControl17.Size = new System.Drawing.Size(0, 567);
            // 
            // barDockControl18
            // 
            this.barDockControl18.CausesValidation = false;
            this.barDockControl18.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl18.Location = new System.Drawing.Point(867, 0);
            this.barDockControl18.Size = new System.Drawing.Size(0, 567);
            // 
            // barManager2
            // 
            this.barManager2.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.mainStatusbar});
            this.barManager2.DockControls.Add(this.barDockControl15);
            this.barManager2.DockControls.Add(this.barDockControl16);
            this.barManager2.DockControls.Add(this.barDockControl17);
            this.barManager2.DockControls.Add(this.barDockControl18);
            this.barManager2.Form = this;
            this.barManager2.MaxItemId = 8;
            this.barManager2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.barManager2.StatusBar = this.mainStatusbar;
            // 
            // bar4
            // 
            this.bar4.BarName = "Status bar";
            this.bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar4.DockCol = 0;
            this.bar4.DockRow = 0;
            this.bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar4.OptionsBar.AllowQuickCustomization = false;
            this.bar4.OptionsBar.DrawDragBorder = false;
            this.bar4.OptionsBar.UseWholeRow = true;
            this.bar4.Text = "Status bar";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = -1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 590);
            this.Controls.Add(this.filelistview);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.barDockControl17);
            this.Controls.Add(this.barDockControl18);
            this.Controls.Add(this.barDockControl16);
            this.Controls.Add(this.barDockControl15);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Sharp";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RPF Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.filelistview)).EndInit();
            this.popupMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_searchEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarDockControl barDockControl5;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarDockControl barDockControl6;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl7;
        private DevExpress.XtraBars.BarDockControl barDockControl8;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl9;
        private DevExpress.XtraBars.BarDockControl barDockControl10;
        private DevExpress.XtraBars.BarDockControl barDockControl11;
        private BrightIdeasSoftware.ObjectListView filelistview;
        private BrightIdeasSoftware.OLVColumn columnName;
        private BrightIdeasSoftware.OLVColumn columnSize;
        private BrightIdeasSoftware.HotItemStyle hotItemStyle1;
        private DevExpress.XtraBars.BarDockControl barDockControl17;
        private DevExpress.XtraBars.BarDockControl barDockControl18;
        private DevExpress.XtraBars.BarDockControl barDockControl16;
        private DevExpress.XtraBars.BarDockControl barDockControl15;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.Bar mainStatusbar;
        private DevExpress.XtraBars.BarManager barManager2;
        private DevExpress.XtraBars.Bar bar4;
        private BrightIdeasSoftware.BarRenderer barRenderer1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private BrightIdeasSoftware.OLVColumn columnAttributes;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarSubItem mFile;
        private DevExpress.XtraBars.BarButtonItem iOpen;
        private DevExpress.XtraBars.BarButtonItem menu_changeGame;
        private DevExpress.XtraBars.BarButtonItem iSave;
        private DevExpress.XtraBars.BarButtonItem iExit;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarButtonItem iExtractAll;
        private DevExpress.XtraBars.BarSubItem mHelp;
        private DevExpress.XtraBars.BarButtonItem iAbout;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarEditItem tb_search;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit tb_searchEdit;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem iNew;
        private DevExpress.XtraBars.BarButtonItem iSaveAs;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem popupExtract;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem popupReplace;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private System.Windows.Forms.ContextMenuStrip popupMenu;
        private System.Windows.Forms.ToolStripMenuItem btn_extract;
        private System.Windows.Forms.ToolStripMenuItem btn_replace;
        private System.Windows.Forms.ToolStripMenuItem btn_goto;
        private System.Windows.Forms.ToolStripMenuItem btn_delete;
        private DevExpress.XtraBars.BarButtonItem btn_exportFileList;
        private DevExpress.XtraBars.BarButtonItem btn_UnpackRSC;

    }
}
