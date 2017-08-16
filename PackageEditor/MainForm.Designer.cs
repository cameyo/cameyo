namespace PackageEditor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fsFolderTree = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.fsFilesList = new System.Windows.Forms.ListView();
            this.columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fileContextMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.fileContextMenuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.fsFolderInfoIsolationCombo = new System.Windows.Forms.ComboBox();
            this.fsFolderInfoIsolationLbl = new System.Windows.Forms.Label();
            this.fsFolderInfoFullName = new System.Windows.Forms.Label();
            this.regSplitContainer = new System.Windows.Forms.SplitContainer();
            this.regFolderTree = new System.Windows.Forms.TreeView();
            this.ContextMenuStripRegistryFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbType = new System.Windows.Forms.TextBox();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.tbSize = new System.Windows.Forms.TextBox();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.regFilesList = new PackageEditor.ListViewEx();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.regFolderInfoIsolationCombo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.regFolderInfoFullName = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.langToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spanishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chineseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.itemHoverTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.dropboxLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.propertyFileVersion = new System.Windows.Forms.TextBox();
            this.lnkChangeIcon = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.propertyFriendlyName = new System.Windows.Forms.TextBox();
            this.propertyIcon = new System.Windows.Forms.PictureBox();
            this.propertyAppID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.lnkAutoLaunch = new System.Windows.Forms.LinkLabel();
            this.lnkChangeDataStorage = new System.Windows.Forms.LinkLabel();
            this.propertyAutoLaunch = new System.Windows.Forms.Label();
            this.propertyDataStorage = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.helpVirtMode = new System.Windows.Forms.Label();
            this.picRAM = new System.Windows.Forms.PictureBox();
            this.picDisk = new System.Windows.Forms.PictureBox();
            this.propertyVirtModeRam = new System.Windows.Forms.RadioButton();
            this.propertyVirtModeDisk = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAutoLaunch = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.picFullAccess = new System.Windows.Forms.PictureBox();
            this.picIsolatedMode = new System.Windows.Forms.PictureBox();
            this.helpIsolationMode = new System.Windows.Forms.Label();
            this.picDataMode = new System.Windows.Forms.PictureBox();
            this.propertyIsolationDataMode = new System.Windows.Forms.RadioButton();
            this.propertyIsolationIsolated = new System.Windows.Forms.RadioButton();
            this.propertyIsolationMerge = new System.Windows.Forms.RadioButton();
            this.dropboxButton = new System.Windows.Forms.Button();
            this.tabFileSystem = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.fileToolStrip = new System.Windows.Forms.ToolStrip();
            this.fsAddBtn = new System.Windows.Forms.ToolStripButton();
            this.fsAddEmptyDirBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.fsRemoveBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.fsAddDirBtn = new System.Windows.Forms.ToolStripButton();
            this.fsSaveFileAsBtn = new System.Windows.Forms.ToolStripButton();
            this.tabRegistry = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.regProgressBar = new System.Windows.Forms.ProgressBar();
            this.regToolStrip = new System.Windows.Forms.ToolStrip();
            this.regRemoveBtn = new System.Windows.Forms.ToolStripButton();
            this.regEditBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.regImportBtn = new System.Windows.Forms.ToolStripButton();
            this.regExportBtn = new System.Windows.Forms.ToolStripButton();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbVolatileRegistry = new System.Windows.Forms.CheckBox();
            this.propertyScmDirect = new System.Windows.Forms.CheckBox();
            this.propertyDisplayLogo = new System.Windows.Forms.CheckBox();
            this.cbDatFile = new System.Windows.Forms.CheckBox();
            this.lnkAutoUpdate = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lnkCustomEvents = new System.Windows.Forms.LinkLabel();
            this.propertyStopInheritance = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkCleanDoneDialog = new System.Windows.Forms.CheckBox();
            this.rdbCleanNone = new System.Windows.Forms.RadioButton();
            this.chkCleanAsk = new System.Windows.Forms.CheckBox();
            this.rdbCleanAll = new System.Windows.Forms.RadioButton();
            this.rdbCleanRegOnly = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbIntegrateVirtual = new System.Windows.Forms.RadioButton();
            this.rdbIntegrateStandard = new System.Windows.Forms.RadioButton();
            this.rdbIntegrateNone = new System.Windows.Forms.RadioButton();
            this.tabSecurity = new System.Windows.Forms.TabPage();
            this.panel10 = new System.Windows.Forms.Panel();
            this.groupDataEncrypt = new System.Windows.Forms.GroupBox();
            this.panel16 = new System.Windows.Forms.Panel();
            this.lnkExportPwdKey = new System.Windows.Forms.LinkLabel();
            this.lnkImportPwdKey = new System.Windows.Forms.LinkLabel();
            this.propertyEncryptUserCreatedPassword = new System.Windows.Forms.RadioButton();
            this.lnkEncryptionLearnMore = new System.Windows.Forms.LinkLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.tbEncryptionPwdConfirm = new System.Windows.Forms.TextBox();
            this.tbEncryptionPwd = new System.Windows.Forms.TextBox();
            this.lnkGenerateEncKey = new System.Windows.Forms.LinkLabel();
            this.tbEncryptionKey = new System.Windows.Forms.TextBox();
            this.propertyEncryptUsingPassword = new System.Windows.Forms.RadioButton();
            this.propertyEncryptUsingKey = new System.Windows.Forms.RadioButton();
            this.propertyEncryption = new System.Windows.Forms.CheckBox();
            this.groupUsageRights = new System.Windows.Forms.GroupBox();
            this.lnkActiveDirectory = new System.Windows.Forms.LinkLabel();
            this.groupExpiration = new System.Windows.Forms.GroupBox();
            this.propertyExpirationDatePicker = new System.Windows.Forms.DateTimePicker();
            this.propertyExpiration = new System.Windows.Forms.CheckBox();
            this.propertyTtlDays = new System.Windows.Forms.CheckBox();
            this.propertyTtlResistRemove = new System.Windows.Forms.CheckBox();
            this.propertyTtlDaysValue = new System.Windows.Forms.NumericUpDown();
            this.groupEditProtect = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPasswordConfirm = new System.Windows.Forms.TextBox();
            this.propertyProtPassword = new System.Windows.Forms.TextBox();
            this.propertyProt = new System.Windows.Forms.CheckBox();
            this.tabWelcome = new System.Windows.Forms.TabPage();
            this.panelWelcome = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.listViewMRU = new System.Windows.Forms.ListView();
            this.columnFileN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListMRU = new System.Windows.Forms.ImageList(this.components);
            this.lnkPackageEdit = new System.Windows.Forms.LinkLabel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.line = new System.Windows.Forms.GroupBox();
            this.bkPanel = new System.Windows.Forms.Panel();
            this.panelLicense = new System.Windows.Forms.Panel();
            this.lnkUpgrade = new System.Windows.Forms.LinkLabel();
            this.lblNotCommercial = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.fileContextMenu.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.regSplitContainer.Panel1.SuspendLayout();
            this.regSplitContainer.Panel2.SuspendLayout();
            this.regSplitContainer.SuspendLayout();
            this.ContextMenuStripRegistryFolder.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisk)).BeginInit();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFullAccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIsolatedMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDataMode)).BeginInit();
            this.tabFileSystem.SuspendLayout();
            this.panel5.SuspendLayout();
            this.fileToolStrip.SuspendLayout();
            this.tabRegistry.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.regToolStrip.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabSecurity.SuspendLayout();
            this.groupDataEncrypt.SuspendLayout();
            this.panel16.SuspendLayout();
            this.groupUsageRights.SuspendLayout();
            this.groupExpiration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyTtlDaysValue)).BeginInit();
            this.groupEditProtect.SuspendLayout();
            this.tabWelcome.SuspendLayout();
            this.panelWelcome.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel13.SuspendLayout();
            this.panel9.SuspendLayout();
            this.bkPanel.SuspendLayout();
            this.panelLicense.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fsFolderTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fsFilesList);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            // 
            // fsFolderTree
            // 
            this.fsFolderTree.AllowDrop = true;
            resources.ApplyResources(this.fsFolderTree, "fsFolderTree");
            this.fsFolderTree.ImageList = this.imageList;
            this.fsFolderTree.Name = "fsFolderTree";
            this.fsFolderTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.fsFolderTree_ItemDrag);
            this.fsFolderTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.fsFolderTree_BeforeSelect);
            this.fsFolderTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fsFolderTree_AfterSelect);
            this.fsFolderTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.Vfs_DragDrop);
            this.fsFolderTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.Vfs_DragEnter);
            this.fsFolderTree.DragOver += new System.Windows.Forms.DragEventHandler(this.fsFolderTree_DragOver);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Red;
            this.imageList.Images.SetKeyName(0, "folder_fullaccess.png");
            this.imageList.Images.SetKeyName(1, "folder");
            this.imageList.Images.SetKeyName(2, "folder_strictlyisolated.png");
            this.imageList.Images.SetKeyName(3, "remove");
            this.imageList.Images.SetKeyName(4, "058.png");
            this.imageList.Images.SetKeyName(5, "folder_opened");
            this.imageList.Images.SetKeyName(6, "new_document");
            this.imageList.Images.SetKeyName(7, "078b.png");
            this.imageList.Images.SetKeyName(8, "add");
            // 
            // fsFilesList
            // 
            this.fsFilesList.AllowDrop = true;
            this.fsFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileName,
            this.columnFileSize,
            this.columnFileType});
            this.fsFilesList.ContextMenuStrip = this.fileContextMenu;
            resources.ApplyResources(this.fsFilesList, "fsFilesList");
            this.fsFilesList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("fsFilesList.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("fsFilesList.Items1")))});
            this.fsFilesList.Name = "fsFilesList";
            this.fsFilesList.SmallImageList = this.imageList;
            this.fsFilesList.UseCompatibleStateImageBehavior = false;
            this.fsFilesList.View = System.Windows.Forms.View.Details;
            this.fsFilesList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.fsFilesList_ColumnClick);
            this.fsFilesList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.fsFilesList_ItemDrag);
            this.fsFilesList.DragDrop += new System.Windows.Forms.DragEventHandler(this.Vfs_DragDrop);
            this.fsFilesList.DragEnter += new System.Windows.Forms.DragEventHandler(this.Vfs_DragEnter);
            this.fsFilesList.DoubleClick += new System.EventHandler(this.fileContextMenuProperties_Click);
            // 
            // columnFileName
            // 
            resources.ApplyResources(this.columnFileName, "columnFileName");
            // 
            // columnFileSize
            // 
            resources.ApplyResources(this.columnFileSize, "columnFileSize");
            // 
            // columnFileType
            // 
            resources.ApplyResources(this.columnFileType, "columnFileType");
            // 
            // fileContextMenu
            // 
            this.fileContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.fileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileContextMenuDelete,
            this.fileContextMenuProperties});
            this.fileContextMenu.Name = "fileContextMenu";
            resources.ApplyResources(this.fileContextMenu, "fileContextMenu");
            // 
            // fileContextMenuDelete
            // 
            this.fileContextMenuDelete.Name = "fileContextMenuDelete";
            resources.ApplyResources(this.fileContextMenuDelete, "fileContextMenuDelete");
            this.fileContextMenuDelete.Click += new System.EventHandler(this.fileContextMenuDelete_Click);
            // 
            // fileContextMenuProperties
            // 
            this.fileContextMenuProperties.Name = "fileContextMenuProperties";
            resources.ApplyResources(this.fileContextMenuProperties, "fileContextMenuProperties");
            this.fileContextMenuProperties.Click += new System.EventHandler(this.fileContextMenuProperties_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.fsFolderInfoFullName);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.fsFolderInfoIsolationCombo);
            this.panel3.Controls.Add(this.fsFolderInfoIsolationLbl);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // fsFolderInfoIsolationCombo
            // 
            this.fsFolderInfoIsolationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fsFolderInfoIsolationCombo.FormattingEnabled = true;
            resources.ApplyResources(this.fsFolderInfoIsolationCombo, "fsFolderInfoIsolationCombo");
            this.fsFolderInfoIsolationCombo.Name = "fsFolderInfoIsolationCombo";
            // 
            // fsFolderInfoIsolationLbl
            // 
            resources.ApplyResources(this.fsFolderInfoIsolationLbl, "fsFolderInfoIsolationLbl");
            this.fsFolderInfoIsolationLbl.Name = "fsFolderInfoIsolationLbl";
            // 
            // fsFolderInfoFullName
            // 
            resources.ApplyResources(this.fsFolderInfoFullName, "fsFolderInfoFullName");
            this.fsFolderInfoFullName.Name = "fsFolderInfoFullName";
            // 
            // regSplitContainer
            // 
            resources.ApplyResources(this.regSplitContainer, "regSplitContainer");
            this.regSplitContainer.Name = "regSplitContainer";
            // 
            // regSplitContainer.Panel1
            // 
            this.regSplitContainer.Panel1.Controls.Add(this.regFolderTree);
            // 
            // regSplitContainer.Panel2
            // 
            this.regSplitContainer.Panel2.Controls.Add(this.tbType);
            this.regSplitContainer.Panel2.Controls.Add(this.tbValue);
            this.regSplitContainer.Panel2.Controls.Add(this.tbSize);
            this.regSplitContainer.Panel2.Controls.Add(this.tbFile);
            this.regSplitContainer.Panel2.Controls.Add(this.regFilesList);
            this.regSplitContainer.Panel2.Controls.Add(this.panel4);
            // 
            // regFolderTree
            // 
            this.regFolderTree.ContextMenuStrip = this.ContextMenuStripRegistryFolder;
            resources.ApplyResources(this.regFolderTree, "regFolderTree");
            this.regFolderTree.ImageList = this.imageList;
            this.regFolderTree.Name = "regFolderTree";
            // 
            // ContextMenuStripRegistryFolder
            // 
            this.ContextMenuStripRegistryFolder.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStripRegistryFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExport,
            this.deleteToolStripMenuItem});
            this.ContextMenuStripRegistryFolder.Name = "Export";
            resources.ApplyResources(this.ContextMenuStripRegistryFolder, "ContextMenuStripRegistryFolder");
            // 
            // toolStripMenuItemExport
            // 
            this.toolStripMenuItemExport.Name = "toolStripMenuItemExport";
            resources.ApplyResources(this.toolStripMenuItemExport, "toolStripMenuItemExport");
            this.toolStripMenuItemExport.Click += new System.EventHandler(this.toolStripMenuItemExport_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // tbType
            // 
            this.tbType.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbType, "tbType");
            this.tbType.Name = "tbType";
            this.tbType.ReadOnly = true;
            // 
            // tbValue
            // 
            resources.ApplyResources(this.tbValue, "tbValue");
            this.tbValue.Name = "tbValue";
            // 
            // tbSize
            // 
            this.tbSize.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbSize, "tbSize");
            this.tbSize.Name = "tbSize";
            this.tbSize.ReadOnly = true;
            // 
            // tbFile
            // 
            this.tbFile.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbFile, "tbFile");
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            // 
            // regFilesList
            // 
            this.regFilesList.AllowColumnReorder = true;
            this.regFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            resources.ApplyResources(this.regFilesList, "regFilesList");
            this.regFilesList.DoubleClickActivation = false;
            this.regFilesList.FullRowSelect = true;
            this.regFilesList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("regFilesList.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("regFilesList.Items1")))});
            this.regFilesList.Name = "regFilesList";
            this.regFilesList.UseCompatibleStateImageBehavior = false;
            this.regFilesList.View = System.Windows.Forms.View.Details;
            this.regFilesList.SubItemClicked += new PackageEditor.SubItemEventHandler(this.regFilesList_SubItemClicked);
            this.regFilesList.SubItemEndEditing += new PackageEditor.SubItemEndEditingEventHandler(this.regFilesList_SubItemEndEditing);
            this.regFilesList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.regFilesList_ColumnClick);
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.regFolderInfoFullName);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.regFolderInfoIsolationCombo);
            this.panel6.Controls.Add(this.label3);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // regFolderInfoIsolationCombo
            // 
            this.regFolderInfoIsolationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regFolderInfoIsolationCombo.FormattingEnabled = true;
            resources.ApplyResources(this.regFolderInfoIsolationCombo, "regFolderInfoIsolationCombo");
            this.regFolderInfoIsolationCombo.Name = "regFolderInfoIsolationCombo";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // regFolderInfoFullName
            // 
            resources.ApplyResources(this.regFolderInfoFullName, "regFolderInfoFullName");
            this.regFolderInfoFullName.Name = "regFolderInfoFullName";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.exportXmlToolStripMenuItem,
            this.langToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveasToolStripMenuItem
            // 
            this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
            resources.ApplyResources(this.saveasToolStripMenuItem, "saveasToolStripMenuItem");
            this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exportXmlToolStripMenuItem
            // 
            this.exportXmlToolStripMenuItem.Name = "exportXmlToolStripMenuItem";
            resources.ApplyResources(this.exportXmlToolStripMenuItem, "exportXmlToolStripMenuItem");
            this.exportXmlToolStripMenuItem.Click += new System.EventHandler(this.exportXmlToolStripMenuItem_Click);
            // 
            // langToolStripMenuItem
            // 
            this.langToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishMenuItem,
            this.frenchMenuItem,
            this.spanishMenuItem,
            this.chineseMenuItem});
            this.langToolStripMenuItem.Name = "langToolStripMenuItem";
            resources.ApplyResources(this.langToolStripMenuItem, "langToolStripMenuItem");
            // 
            // englishMenuItem
            // 
            this.englishMenuItem.Name = "englishMenuItem";
            resources.ApplyResources(this.englishMenuItem, "englishMenuItem");
            this.englishMenuItem.Click += new System.EventHandler(this.langMenuItem_Click);
            // 
            // frenchMenuItem
            // 
            this.frenchMenuItem.Name = "frenchMenuItem";
            resources.ApplyResources(this.frenchMenuItem, "frenchMenuItem");
            this.frenchMenuItem.Click += new System.EventHandler(this.langMenuItem_Click);
            // 
            // spanishMenuItem
            // 
            this.spanishMenuItem.Name = "spanishMenuItem";
            resources.ApplyResources(this.spanishMenuItem, "spanishMenuItem");
            this.spanishMenuItem.Click += new System.EventHandler(this.langMenuItem_Click);
            // 
            // chineseMenuItem
            // 
            this.chineseMenuItem.Name = "chineseMenuItem";
            resources.ApplyResources(this.chineseMenuItem, "chineseMenuItem");
            this.chineseMenuItem.Click += new System.EventHandler(this.langMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // regProgressTimer
            // 
            this.regProgressTimer.Tick += new System.EventHandler(this.regProgressTimer_Tick);
            // 
            // itemHoverTimer
            // 
            this.itemHoverTimer.Tick += new System.EventHandler(this.OnItemHover);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabFileSystem);
            this.tabControl.Controls.Add(this.tabRegistry);
            this.tabControl.Controls.Add(this.tabAdvanced);
            this.tabControl.Controls.Add(this.tabSecurity);
            this.tabControl.Controls.Add(this.tabWelcome);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.White;
            this.tabGeneral.Controls.Add(this.dropboxLabel);
            this.tabGeneral.Controls.Add(this.groupBox3);
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Controls.Add(this.dropboxButton);
            resources.ApplyResources(this.tabGeneral, "tabGeneral");
            this.tabGeneral.Name = "tabGeneral";
            // 
            // dropboxLabel
            // 
            resources.ApplyResources(this.dropboxLabel, "dropboxLabel");
            this.dropboxLabel.Name = "dropboxLabel";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.propertyFileVersion);
            this.groupBox3.Controls.Add(this.lnkChangeIcon);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.propertyFriendlyName);
            this.groupBox3.Controls.Add(this.propertyIcon);
            this.groupBox3.Controls.Add(this.propertyAppID);
            this.groupBox3.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // propertyFileVersion
            // 
            resources.ApplyResources(this.propertyFileVersion, "propertyFileVersion");
            this.propertyFileVersion.Name = "propertyFileVersion";
            this.propertyFileVersion.TextChanged += new System.EventHandler(this.PropertyChange);
            // 
            // lnkChangeIcon
            // 
            resources.ApplyResources(this.lnkChangeIcon, "lnkChangeIcon");
            this.lnkChangeIcon.Name = "lnkChangeIcon";
            this.lnkChangeIcon.TabStop = true;
            this.lnkChangeIcon.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeIcon_LinkClicked);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // propertyFriendlyName
            // 
            resources.ApplyResources(this.propertyFriendlyName, "propertyFriendlyName");
            this.propertyFriendlyName.Name = "propertyFriendlyName";
            this.propertyFriendlyName.TextChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyIcon
            // 
            resources.ApplyResources(this.propertyIcon, "propertyIcon");
            this.propertyIcon.Name = "propertyIcon";
            this.propertyIcon.TabStop = false;
            // 
            // propertyAppID
            // 
            resources.ApplyResources(this.propertyAppID, "propertyAppID");
            this.propertyAppID.Name = "propertyAppID";
            this.propertyAppID.TextChanged += new System.EventHandler(this.PropertyChange);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.groupBox11);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblAutoLaunch);
            this.groupBox1.Controls.Add(this.groupBox10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox11
            // 
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Controls.Add(this.lnkAutoLaunch);
            this.groupBox11.Controls.Add(this.lnkChangeDataStorage);
            this.groupBox11.Controls.Add(this.propertyAutoLaunch);
            this.groupBox11.Controls.Add(this.propertyDataStorage);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // lnkAutoLaunch
            // 
            resources.ApplyResources(this.lnkAutoLaunch, "lnkAutoLaunch");
            this.lnkAutoLaunch.Name = "lnkAutoLaunch";
            this.lnkAutoLaunch.TabStop = true;
            this.lnkAutoLaunch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAutoLaunch_LinkClicked);
            // 
            // lnkChangeDataStorage
            // 
            resources.ApplyResources(this.lnkChangeDataStorage, "lnkChangeDataStorage");
            this.lnkChangeDataStorage.Name = "lnkChangeDataStorage";
            this.lnkChangeDataStorage.TabStop = true;
            this.lnkChangeDataStorage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeDataStorage_LinkClicked);
            // 
            // propertyAutoLaunch
            // 
            resources.ApplyResources(this.propertyAutoLaunch, "propertyAutoLaunch");
            this.propertyAutoLaunch.Name = "propertyAutoLaunch";
            // 
            // propertyDataStorage
            // 
            resources.ApplyResources(this.propertyDataStorage, "propertyDataStorage");
            this.propertyDataStorage.Name = "propertyDataStorage";
            // 
            // groupBox9
            // 
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Controls.Add(this.helpVirtMode);
            this.groupBox9.Controls.Add(this.picRAM);
            this.groupBox9.Controls.Add(this.picDisk);
            this.groupBox9.Controls.Add(this.propertyVirtModeRam);
            this.groupBox9.Controls.Add(this.propertyVirtModeDisk);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // helpVirtMode
            // 
            resources.ApplyResources(this.helpVirtMode, "helpVirtMode");
            this.helpVirtMode.Name = "helpVirtMode";
            // 
            // picRAM
            // 
            resources.ApplyResources(this.picRAM, "picRAM");
            this.picRAM.Name = "picRAM";
            this.picRAM.TabStop = false;
            // 
            // picDisk
            // 
            resources.ApplyResources(this.picDisk, "picDisk");
            this.picDisk.Name = "picDisk";
            this.picDisk.TabStop = false;
            // 
            // propertyVirtModeRam
            // 
            resources.ApplyResources(this.propertyVirtModeRam, "propertyVirtModeRam");
            this.propertyVirtModeRam.Name = "propertyVirtModeRam";
            this.propertyVirtModeRam.TabStop = true;
            this.propertyVirtModeRam.UseVisualStyleBackColor = true;
            this.propertyVirtModeRam.CheckedChanged += new System.EventHandler(this.propertyVirtMode_CheckedChanged);
            this.propertyVirtModeRam.Click += new System.EventHandler(this.PropertyChange);
            // 
            // propertyVirtModeDisk
            // 
            resources.ApplyResources(this.propertyVirtModeDisk, "propertyVirtModeDisk");
            this.propertyVirtModeDisk.Name = "propertyVirtModeDisk";
            this.propertyVirtModeDisk.TabStop = true;
            this.propertyVirtModeDisk.UseVisualStyleBackColor = true;
            this.propertyVirtModeDisk.CheckedChanged += new System.EventHandler(this.propertyVirtMode_CheckedChanged);
            this.propertyVirtModeDisk.Click += new System.EventHandler(this.PropertyChange);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lblAutoLaunch
            // 
            resources.ApplyResources(this.lblAutoLaunch, "lblAutoLaunch");
            this.lblAutoLaunch.Name = "lblAutoLaunch";
            // 
            // groupBox10
            // 
            resources.ApplyResources(this.groupBox10, "groupBox10");
            this.groupBox10.Controls.Add(this.picFullAccess);
            this.groupBox10.Controls.Add(this.picIsolatedMode);
            this.groupBox10.Controls.Add(this.helpIsolationMode);
            this.groupBox10.Controls.Add(this.picDataMode);
            this.groupBox10.Controls.Add(this.propertyIsolationDataMode);
            this.groupBox10.Controls.Add(this.propertyIsolationIsolated);
            this.groupBox10.Controls.Add(this.propertyIsolationMerge);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.TabStop = false;
            // 
            // picFullAccess
            // 
            resources.ApplyResources(this.picFullAccess, "picFullAccess");
            this.picFullAccess.Name = "picFullAccess";
            this.picFullAccess.TabStop = false;
            // 
            // picIsolatedMode
            // 
            resources.ApplyResources(this.picIsolatedMode, "picIsolatedMode");
            this.picIsolatedMode.Name = "picIsolatedMode";
            this.picIsolatedMode.TabStop = false;
            // 
            // helpIsolationMode
            // 
            resources.ApplyResources(this.helpIsolationMode, "helpIsolationMode");
            this.helpIsolationMode.Name = "helpIsolationMode";
            // 
            // picDataMode
            // 
            resources.ApplyResources(this.picDataMode, "picDataMode");
            this.picDataMode.Name = "picDataMode";
            this.picDataMode.TabStop = false;
            // 
            // propertyIsolationDataMode
            // 
            resources.ApplyResources(this.propertyIsolationDataMode, "propertyIsolationDataMode");
            this.propertyIsolationDataMode.Name = "propertyIsolationDataMode";
            this.propertyIsolationDataMode.TabStop = true;
            this.propertyIsolationDataMode.UseVisualStyleBackColor = true;
            this.propertyIsolationDataMode.CheckedChanged += new System.EventHandler(this.propertyIsolationMode_CheckedChanged);
            this.propertyIsolationDataMode.Click += new System.EventHandler(this.IsolationChanged);
            // 
            // propertyIsolationIsolated
            // 
            resources.ApplyResources(this.propertyIsolationIsolated, "propertyIsolationIsolated");
            this.propertyIsolationIsolated.Name = "propertyIsolationIsolated";
            this.propertyIsolationIsolated.TabStop = true;
            this.propertyIsolationIsolated.UseVisualStyleBackColor = true;
            this.propertyIsolationIsolated.CheckedChanged += new System.EventHandler(this.propertyIsolationMode_CheckedChanged);
            this.propertyIsolationIsolated.Click += new System.EventHandler(this.IsolationChanged);
            // 
            // propertyIsolationMerge
            // 
            resources.ApplyResources(this.propertyIsolationMerge, "propertyIsolationMerge");
            this.propertyIsolationMerge.Name = "propertyIsolationMerge";
            this.propertyIsolationMerge.TabStop = true;
            this.propertyIsolationMerge.UseVisualStyleBackColor = true;
            this.propertyIsolationMerge.CheckedChanged += new System.EventHandler(this.propertyIsolationMode_CheckedChanged);
            this.propertyIsolationMerge.Click += new System.EventHandler(this.IsolationChanged);
            // 
            // dropboxButton
            // 
            resources.ApplyResources(this.dropboxButton, "dropboxButton");
            this.dropboxButton.Name = "dropboxButton";
            this.dropboxButton.UseVisualStyleBackColor = true;
            this.dropboxButton.Click += new System.EventHandler(this.dropboxButton_Click);
            // 
            // tabFileSystem
            // 
            this.tabFileSystem.BackColor = System.Drawing.Color.White;
            this.tabFileSystem.Controls.Add(this.panel5);
            this.tabFileSystem.Controls.Add(this.fileToolStrip);
            resources.ApplyResources(this.tabFileSystem, "tabFileSystem");
            this.tabFileSystem.Name = "tabFileSystem";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // fileToolStrip
            // 
            this.fileToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.fileToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fsAddBtn,
            this.fsAddEmptyDirBtn,
            this.toolStripSeparator3,
            this.fsRemoveBtn,
            this.toolStripSeparator4,
            this.fsAddDirBtn,
            this.fsSaveFileAsBtn});
            resources.ApplyResources(this.fileToolStrip, "fileToolStrip");
            this.fileToolStrip.Name = "fileToolStrip";
            // 
            // fsAddBtn
            // 
            this.fsAddBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fsAddBtn.Image = global::PackageEditor.Properties.Resources._078;
            resources.ApplyResources(this.fsAddBtn, "fsAddBtn");
            this.fsAddBtn.Name = "fsAddBtn";
            // 
            // fsAddEmptyDirBtn
            // 
            this.fsAddEmptyDirBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fsAddEmptyDirBtn.Image = global::PackageEditor.Properties.Resources._115;
            resources.ApplyResources(this.fsAddEmptyDirBtn, "fsAddEmptyDirBtn");
            this.fsAddEmptyDirBtn.Name = "fsAddEmptyDirBtn";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // fsRemoveBtn
            // 
            this.fsRemoveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fsRemoveBtn.Image = global::PackageEditor.Properties.Resources._058;
            resources.ApplyResources(this.fsRemoveBtn, "fsRemoveBtn");
            this.fsRemoveBtn.Name = "fsRemoveBtn";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // fsAddDirBtn
            // 
            this.fsAddDirBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fsAddDirBtn.Image = global::PackageEditor.Properties.Resources._019;
            resources.ApplyResources(this.fsAddDirBtn, "fsAddDirBtn");
            this.fsAddDirBtn.Name = "fsAddDirBtn";
            // 
            // fsSaveFileAsBtn
            // 
            this.fsSaveFileAsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.fsSaveFileAsBtn, "fsSaveFileAsBtn");
            this.fsSaveFileAsBtn.Name = "fsSaveFileAsBtn";
            // 
            // tabRegistry
            // 
            this.tabRegistry.AllowDrop = true;
            this.tabRegistry.Controls.Add(this.panel8);
            resources.ApplyResources(this.tabRegistry, "tabRegistry");
            this.tabRegistry.Name = "tabRegistry";
            this.tabRegistry.UseVisualStyleBackColor = true;
            this.tabRegistry.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabRegistry_DragDrop);
            this.tabRegistry.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabRegistry_DragEnter);
            this.tabRegistry.DragOver += new System.Windows.Forms.DragEventHandler(this.tabRegistry_DragOver);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.White;
            this.panel8.Controls.Add(this.panel1);
            this.panel8.Controls.Add(this.panel7);
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Name = "panel8";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.regSplitContainer);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel7
            // 
            resources.ApplyResources(this.panel7, "panel7");
            this.panel7.Controls.Add(this.regProgressBar);
            this.panel7.Controls.Add(this.regToolStrip);
            this.panel7.Name = "panel7";
            // 
            // regProgressBar
            // 
            resources.ApplyResources(this.regProgressBar, "regProgressBar");
            this.regProgressBar.Name = "regProgressBar";
            // 
            // regToolStrip
            // 
            this.regToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.regToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regRemoveBtn,
            this.regEditBtn,
            this.toolStripSeparator2,
            this.regImportBtn,
            this.regExportBtn});
            resources.ApplyResources(this.regToolStrip, "regToolStrip");
            this.regToolStrip.Name = "regToolStrip";
            // 
            // regRemoveBtn
            // 
            this.regRemoveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.regRemoveBtn, "regRemoveBtn");
            this.regRemoveBtn.Name = "regRemoveBtn";
            // 
            // regEditBtn
            // 
            this.regEditBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.regEditBtn, "regEditBtn");
            this.regEditBtn.Name = "regEditBtn";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // regImportBtn
            // 
            this.regImportBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.regImportBtn, "regImportBtn");
            this.regImportBtn.Name = "regImportBtn";
            this.regImportBtn.Click += new System.EventHandler(this.regImportBtn_Click);
            // 
            // regExportBtn
            // 
            this.regExportBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.regExportBtn, "regExportBtn");
            this.regExportBtn.Name = "regExportBtn";
            this.regExportBtn.Click += new System.EventHandler(this.regExportBtn_Click);
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.BackColor = System.Drawing.Color.White;
            this.tabAdvanced.Controls.Add(this.groupBox5);
            this.tabAdvanced.Controls.Add(this.groupBox7);
            this.tabAdvanced.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.tabAdvanced, "tabAdvanced");
            this.tabAdvanced.Name = "tabAdvanced";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbVolatileRegistry);
            this.groupBox5.Controls.Add(this.propertyScmDirect);
            this.groupBox5.Controls.Add(this.propertyDisplayLogo);
            this.groupBox5.Controls.Add(this.cbDatFile);
            this.groupBox5.Controls.Add(this.lnkAutoUpdate);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lnkCustomEvents);
            this.groupBox5.Controls.Add(this.propertyStopInheritance);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // cbVolatileRegistry
            // 
            resources.ApplyResources(this.cbVolatileRegistry, "cbVolatileRegistry");
            this.cbVolatileRegistry.Name = "cbVolatileRegistry";
            this.cbVolatileRegistry.UseVisualStyleBackColor = true;
            this.cbVolatileRegistry.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyScmDirect
            // 
            resources.ApplyResources(this.propertyScmDirect, "propertyScmDirect");
            this.propertyScmDirect.Name = "propertyScmDirect";
            this.propertyScmDirect.UseVisualStyleBackColor = true;
            this.propertyScmDirect.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyDisplayLogo
            // 
            resources.ApplyResources(this.propertyDisplayLogo, "propertyDisplayLogo");
            this.propertyDisplayLogo.Name = "propertyDisplayLogo";
            this.propertyDisplayLogo.UseVisualStyleBackColor = true;
            this.propertyDisplayLogo.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // cbDatFile
            // 
            resources.ApplyResources(this.cbDatFile, "cbDatFile");
            this.cbDatFile.Name = "cbDatFile";
            this.cbDatFile.UseVisualStyleBackColor = true;
            this.cbDatFile.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // lnkAutoUpdate
            // 
            resources.ApplyResources(this.lnkAutoUpdate, "lnkAutoUpdate");
            this.lnkAutoUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkAutoUpdate.Name = "lnkAutoUpdate";
            this.lnkAutoUpdate.TabStop = true;
            this.lnkAutoUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAutoUpdate_LinkClicked);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lnkCustomEvents
            // 
            resources.ApplyResources(this.lnkCustomEvents, "lnkCustomEvents");
            this.lnkCustomEvents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkCustomEvents.Name = "lnkCustomEvents";
            this.lnkCustomEvents.TabStop = true;
            this.lnkCustomEvents.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCustomEvents_LinkClicked);
            // 
            // propertyStopInheritance
            // 
            resources.ApplyResources(this.propertyStopInheritance, "propertyStopInheritance");
            this.propertyStopInheritance.Name = "propertyStopInheritance";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chkCleanDoneDialog);
            this.groupBox7.Controls.Add(this.rdbCleanNone);
            this.groupBox7.Controls.Add(this.chkCleanAsk);
            this.groupBox7.Controls.Add(this.rdbCleanAll);
            this.groupBox7.Controls.Add(this.rdbCleanRegOnly);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // chkCleanDoneDialog
            // 
            resources.ApplyResources(this.chkCleanDoneDialog, "chkCleanDoneDialog");
            this.chkCleanDoneDialog.Name = "chkCleanDoneDialog";
            this.chkCleanDoneDialog.UseVisualStyleBackColor = true;
            this.chkCleanDoneDialog.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // rdbCleanNone
            // 
            resources.ApplyResources(this.rdbCleanNone, "rdbCleanNone");
            this.rdbCleanNone.Name = "rdbCleanNone";
            this.rdbCleanNone.TabStop = true;
            this.rdbCleanNone.UseVisualStyleBackColor = true;
            this.rdbCleanNone.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // chkCleanAsk
            // 
            resources.ApplyResources(this.chkCleanAsk, "chkCleanAsk");
            this.chkCleanAsk.Name = "chkCleanAsk";
            this.chkCleanAsk.UseVisualStyleBackColor = true;
            this.chkCleanAsk.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // rdbCleanAll
            // 
            resources.ApplyResources(this.rdbCleanAll, "rdbCleanAll");
            this.rdbCleanAll.Name = "rdbCleanAll";
            this.rdbCleanAll.TabStop = true;
            this.rdbCleanAll.UseVisualStyleBackColor = true;
            this.rdbCleanAll.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdbCleanRegOnly
            // 
            resources.ApplyResources(this.rdbCleanRegOnly, "rdbCleanRegOnly");
            this.rdbCleanRegOnly.Name = "rdbCleanRegOnly";
            this.rdbCleanRegOnly.TabStop = true;
            this.rdbCleanRegOnly.UseVisualStyleBackColor = true;
            this.rdbCleanRegOnly.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbIntegrateVirtual);
            this.groupBox4.Controls.Add(this.rdbIntegrateStandard);
            this.groupBox4.Controls.Add(this.rdbIntegrateNone);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // rdbIntegrateVirtual
            // 
            resources.ApplyResources(this.rdbIntegrateVirtual, "rdbIntegrateVirtual");
            this.rdbIntegrateVirtual.Name = "rdbIntegrateVirtual";
            this.rdbIntegrateVirtual.TabStop = true;
            this.rdbIntegrateVirtual.UseVisualStyleBackColor = true;
            this.rdbIntegrateVirtual.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // rdbIntegrateStandard
            // 
            resources.ApplyResources(this.rdbIntegrateStandard, "rdbIntegrateStandard");
            this.rdbIntegrateStandard.Name = "rdbIntegrateStandard";
            this.rdbIntegrateStandard.TabStop = true;
            this.rdbIntegrateStandard.UseVisualStyleBackColor = true;
            this.rdbIntegrateStandard.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // rdbIntegrateNone
            // 
            resources.ApplyResources(this.rdbIntegrateNone, "rdbIntegrateNone");
            this.rdbIntegrateNone.Name = "rdbIntegrateNone";
            this.rdbIntegrateNone.TabStop = true;
            this.rdbIntegrateNone.UseVisualStyleBackColor = true;
            this.rdbIntegrateNone.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // tabSecurity
            // 
            this.tabSecurity.Controls.Add(this.panel10);
            this.tabSecurity.Controls.Add(this.groupDataEncrypt);
            this.tabSecurity.Controls.Add(this.groupUsageRights);
            this.tabSecurity.Controls.Add(this.groupExpiration);
            this.tabSecurity.Controls.Add(this.groupEditProtect);
            resources.ApplyResources(this.tabSecurity, "tabSecurity");
            this.tabSecurity.Name = "tabSecurity";
            this.tabSecurity.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            resources.ApplyResources(this.panel10, "panel10");
            this.panel10.Name = "panel10";
            // 
            // groupDataEncrypt
            // 
            this.groupDataEncrypt.Controls.Add(this.panel16);
            this.groupDataEncrypt.Controls.Add(this.propertyEncryptUserCreatedPassword);
            this.groupDataEncrypt.Controls.Add(this.lnkEncryptionLearnMore);
            this.groupDataEncrypt.Controls.Add(this.label12);
            this.groupDataEncrypt.Controls.Add(this.tbEncryptionPwdConfirm);
            this.groupDataEncrypt.Controls.Add(this.tbEncryptionPwd);
            this.groupDataEncrypt.Controls.Add(this.lnkGenerateEncKey);
            this.groupDataEncrypt.Controls.Add(this.tbEncryptionKey);
            this.groupDataEncrypt.Controls.Add(this.propertyEncryptUsingPassword);
            this.groupDataEncrypt.Controls.Add(this.propertyEncryptUsingKey);
            this.groupDataEncrypt.Controls.Add(this.propertyEncryption);
            resources.ApplyResources(this.groupDataEncrypt, "groupDataEncrypt");
            this.groupDataEncrypt.Name = "groupDataEncrypt";
            this.groupDataEncrypt.TabStop = false;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.lnkExportPwdKey);
            this.panel16.Controls.Add(this.lnkImportPwdKey);
            resources.ApplyResources(this.panel16, "panel16");
            this.panel16.Name = "panel16";
            // 
            // lnkExportPwdKey
            // 
            resources.ApplyResources(this.lnkExportPwdKey, "lnkExportPwdKey");
            this.lnkExportPwdKey.Name = "lnkExportPwdKey";
            this.lnkExportPwdKey.TabStop = true;
            this.lnkExportPwdKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExportPwdKey_LinkClicked);
            // 
            // lnkImportPwdKey
            // 
            resources.ApplyResources(this.lnkImportPwdKey, "lnkImportPwdKey");
            this.lnkImportPwdKey.Name = "lnkImportPwdKey";
            this.lnkImportPwdKey.TabStop = true;
            this.lnkImportPwdKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkImportPwdKey_LinkClicked);
            // 
            // propertyEncryptUserCreatedPassword
            // 
            resources.ApplyResources(this.propertyEncryptUserCreatedPassword, "propertyEncryptUserCreatedPassword");
            this.propertyEncryptUserCreatedPassword.Name = "propertyEncryptUserCreatedPassword";
            this.propertyEncryptUserCreatedPassword.TabStop = true;
            this.propertyEncryptUserCreatedPassword.UseVisualStyleBackColor = true;
            // 
            // lnkEncryptionLearnMore
            // 
            resources.ApplyResources(this.lnkEncryptionLearnMore, "lnkEncryptionLearnMore");
            this.lnkEncryptionLearnMore.Name = "lnkEncryptionLearnMore";
            this.lnkEncryptionLearnMore.TabStop = true;
            this.lnkEncryptionLearnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEncryptionLearnMore_LinkClicked);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbEncryptionPwdConfirm
            // 
            resources.ApplyResources(this.tbEncryptionPwdConfirm, "tbEncryptionPwdConfirm");
            this.tbEncryptionPwdConfirm.Name = "tbEncryptionPwdConfirm";
            // 
            // tbEncryptionPwd
            // 
            resources.ApplyResources(this.tbEncryptionPwd, "tbEncryptionPwd");
            this.tbEncryptionPwd.Name = "tbEncryptionPwd";
            this.tbEncryptionPwd.Enter += new System.EventHandler(this.tbEncryptionPwd_Enter);
            // 
            // lnkGenerateEncKey
            // 
            resources.ApplyResources(this.lnkGenerateEncKey, "lnkGenerateEncKey");
            this.lnkGenerateEncKey.Name = "lnkGenerateEncKey";
            this.lnkGenerateEncKey.TabStop = true;
            this.lnkGenerateEncKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGenerateEncKey_LinkClicked);
            // 
            // tbEncryptionKey
            // 
            resources.ApplyResources(this.tbEncryptionKey, "tbEncryptionKey");
            this.tbEncryptionKey.Name = "tbEncryptionKey";
            // 
            // propertyEncryptUsingPassword
            // 
            resources.ApplyResources(this.propertyEncryptUsingPassword, "propertyEncryptUsingPassword");
            this.propertyEncryptUsingPassword.Name = "propertyEncryptUsingPassword";
            this.propertyEncryptUsingPassword.TabStop = true;
            this.propertyEncryptUsingPassword.UseVisualStyleBackColor = true;
            this.propertyEncryptUsingPassword.CheckedChanged += new System.EventHandler(this.propertyEncryption_CheckedChanged);
            // 
            // propertyEncryptUsingKey
            // 
            resources.ApplyResources(this.propertyEncryptUsingKey, "propertyEncryptUsingKey");
            this.propertyEncryptUsingKey.Name = "propertyEncryptUsingKey";
            this.propertyEncryptUsingKey.TabStop = true;
            this.propertyEncryptUsingKey.UseVisualStyleBackColor = true;
            this.propertyEncryptUsingKey.CheckedChanged += new System.EventHandler(this.propertyEncryption_CheckedChanged);
            // 
            // propertyEncryption
            // 
            resources.ApplyResources(this.propertyEncryption, "propertyEncryption");
            this.propertyEncryption.Name = "propertyEncryption";
            this.propertyEncryption.UseVisualStyleBackColor = true;
            this.propertyEncryption.CheckedChanged += new System.EventHandler(this.propertyEncryption_CheckedChanged);
            // 
            // groupUsageRights
            // 
            this.groupUsageRights.Controls.Add(this.lnkActiveDirectory);
            resources.ApplyResources(this.groupUsageRights, "groupUsageRights");
            this.groupUsageRights.Name = "groupUsageRights";
            this.groupUsageRights.TabStop = false;
            // 
            // lnkActiveDirectory
            // 
            resources.ApplyResources(this.lnkActiveDirectory, "lnkActiveDirectory");
            this.lnkActiveDirectory.Name = "lnkActiveDirectory";
            this.lnkActiveDirectory.TabStop = true;
            this.lnkActiveDirectory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkActiveDirectory_LinkClicked);
            // 
            // groupExpiration
            // 
            this.groupExpiration.Controls.Add(this.propertyExpirationDatePicker);
            this.groupExpiration.Controls.Add(this.propertyExpiration);
            this.groupExpiration.Controls.Add(this.propertyTtlDays);
            this.groupExpiration.Controls.Add(this.propertyTtlResistRemove);
            this.groupExpiration.Controls.Add(this.propertyTtlDaysValue);
            resources.ApplyResources(this.groupExpiration, "groupExpiration");
            this.groupExpiration.Name = "groupExpiration";
            this.groupExpiration.TabStop = false;
            // 
            // propertyExpirationDatePicker
            // 
            resources.ApplyResources(this.propertyExpirationDatePicker, "propertyExpirationDatePicker");
            this.propertyExpirationDatePicker.Name = "propertyExpirationDatePicker";
            // 
            // propertyExpiration
            // 
            resources.ApplyResources(this.propertyExpiration, "propertyExpiration");
            this.propertyExpiration.Name = "propertyExpiration";
            this.propertyExpiration.UseVisualStyleBackColor = true;
            this.propertyExpiration.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyTtlDays
            // 
            resources.ApplyResources(this.propertyTtlDays, "propertyTtlDays");
            this.propertyTtlDays.Name = "propertyTtlDays";
            this.propertyTtlDays.UseVisualStyleBackColor = true;
            this.propertyTtlDays.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyTtlResistRemove
            // 
            resources.ApplyResources(this.propertyTtlResistRemove, "propertyTtlResistRemove");
            this.propertyTtlResistRemove.Name = "propertyTtlResistRemove";
            this.propertyTtlResistRemove.UseVisualStyleBackColor = true;
            this.propertyTtlResistRemove.CheckedChanged += new System.EventHandler(this.PropertyChange);
            // 
            // propertyTtlDaysValue
            // 
            resources.ApplyResources(this.propertyTtlDaysValue, "propertyTtlDaysValue");
            this.propertyTtlDaysValue.Name = "propertyTtlDaysValue";
            // 
            // groupEditProtect
            // 
            this.groupEditProtect.Controls.Add(this.label6);
            this.groupEditProtect.Controls.Add(this.tbPasswordConfirm);
            this.groupEditProtect.Controls.Add(this.propertyProtPassword);
            this.groupEditProtect.Controls.Add(this.propertyProt);
            resources.ApplyResources(this.groupEditProtect, "groupEditProtect");
            this.groupEditProtect.Name = "groupEditProtect";
            this.groupEditProtect.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbPasswordConfirm
            // 
            resources.ApplyResources(this.tbPasswordConfirm, "tbPasswordConfirm");
            this.tbPasswordConfirm.Name = "tbPasswordConfirm";
            // 
            // propertyProtPassword
            // 
            resources.ApplyResources(this.propertyProtPassword, "propertyProtPassword");
            this.propertyProtPassword.Name = "propertyProtPassword";
            this.propertyProtPassword.Enter += new System.EventHandler(this.propertyProtPassword_Enter);
            // 
            // propertyProt
            // 
            resources.ApplyResources(this.propertyProt, "propertyProt");
            this.propertyProt.Name = "propertyProt";
            this.propertyProt.UseVisualStyleBackColor = true;
            this.propertyProt.CheckedChanged += new System.EventHandler(this.propertyProt_CheckedChanged);
            // 
            // tabWelcome
            // 
            this.tabWelcome.Controls.Add(this.panelWelcome);
            resources.ApplyResources(this.tabWelcome, "tabWelcome");
            this.tabWelcome.Name = "tabWelcome";
            this.tabWelcome.UseVisualStyleBackColor = true;
            // 
            // panelWelcome
            // 
            resources.ApplyResources(this.panelWelcome, "panelWelcome");
            this.panelWelcome.BackColor = System.Drawing.Color.White;
            this.panelWelcome.Controls.Add(this.panel15);
            this.panelWelcome.Controls.Add(this.panel13);
            this.panelWelcome.Name = "panelWelcome";
            // 
            // panel15
            // 
            resources.ApplyResources(this.panel15, "panel15");
            this.panel15.BackColor = System.Drawing.Color.Transparent;
            this.panel15.Controls.Add(this.panel14);
            this.panel15.Name = "panel15";
            // 
            // panel14
            // 
            resources.ApplyResources(this.panel14, "panel14");
            this.panel14.BackColor = System.Drawing.Color.Transparent;
            this.panel14.Controls.Add(this.pictureBox2);
            this.panel14.Name = "panel14";
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // panel13
            // 
            resources.ApplyResources(this.panel13, "panel13");
            this.panel13.BackColor = System.Drawing.Color.White;
            this.panel13.Controls.Add(this.listViewMRU);
            this.panel13.Controls.Add(this.lnkPackageEdit);
            this.panel13.Controls.Add(this.panel9);
            this.panel13.Name = "panel13";
            // 
            // listViewMRU
            // 
            this.listViewMRU.Activation = System.Windows.Forms.ItemActivation.OneClick;
            resources.ApplyResources(this.listViewMRU, "listViewMRU");
            this.listViewMRU.BackColor = System.Drawing.Color.White;
            this.listViewMRU.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewMRU.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFileN});
            this.listViewMRU.ForeColor = System.Drawing.Color.Blue;
            this.listViewMRU.FullRowSelect = true;
            this.listViewMRU.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("listViewMRU.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("listViewMRU.Groups1")))});
            this.listViewMRU.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewMRU.LargeImageList = this.imageListMRU;
            this.listViewMRU.MultiSelect = false;
            this.listViewMRU.Name = "listViewMRU";
            this.listViewMRU.SmallImageList = this.imageListMRU;
            this.listViewMRU.UseCompatibleStateImageBehavior = false;
            this.listViewMRU.View = System.Windows.Forms.View.List;
            this.listViewMRU.Click += new System.EventHandler(this.listViewMRU_Click);
            // 
            // columnFileN
            // 
            resources.ApplyResources(this.columnFileN, "columnFileN");
            // 
            // imageListMRU
            // 
            this.imageListMRU.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageListMRU, "imageListMRU");
            this.imageListMRU.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lnkPackageEdit
            // 
            resources.ApplyResources(this.lnkPackageEdit, "lnkPackageEdit");
            this.lnkPackageEdit.Name = "lnkPackageEdit";
            this.lnkPackageEdit.TabStop = true;
            this.lnkPackageEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPackageEdit_LinkClicked);
            this.lnkPackageEdit.Click += new System.EventHandler(this.btnEditPackage_Click);
            // 
            // panel9
            // 
            resources.ApplyResources(this.panel9, "panel9");
            this.panel9.Controls.Add(this.label11);
            this.panel9.Controls.Add(this.line);
            this.panel9.Name = "panel9";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // line
            // 
            resources.ApplyResources(this.line, "line");
            this.line.Name = "line";
            this.line.TabStop = false;
            // 
            // bkPanel
            // 
            resources.ApplyResources(this.bkPanel, "bkPanel");
            this.bkPanel.Controls.Add(this.panelLicense);
            this.bkPanel.Name = "bkPanel";
            // 
            // panelLicense
            // 
            resources.ApplyResources(this.panelLicense, "panelLicense");
            this.panelLicense.BackColor = System.Drawing.SystemColors.Control;
            this.panelLicense.Controls.Add(this.lnkUpgrade);
            this.panelLicense.Controls.Add(this.lblNotCommercial);
            this.panelLicense.Name = "panelLicense";
            // 
            // lnkUpgrade
            // 
            this.lnkUpgrade.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lnkUpgrade, "lnkUpgrade");
            this.lnkUpgrade.Name = "lnkUpgrade";
            this.lnkUpgrade.TabStop = true;
            this.lnkUpgrade.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpgrade_LinkClicked);
            // 
            // lblNotCommercial
            // 
            resources.ApplyResources(this.lblNotCommercial, "lblNotCommercial");
            this.lblNotCommercial.Name = "lblNotCommercial";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.bkPanel);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.fileContextMenu.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.regSplitContainer.Panel1.ResumeLayout(false);
            this.regSplitContainer.Panel2.ResumeLayout(false);
            this.regSplitContainer.Panel2.PerformLayout();
            this.regSplitContainer.ResumeLayout(false);
            this.ContextMenuStripRegistryFolder.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDisk)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFullAccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picIsolatedMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDataMode)).EndInit();
            this.tabFileSystem.ResumeLayout(false);
            this.tabFileSystem.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.fileToolStrip.ResumeLayout(false);
            this.fileToolStrip.PerformLayout();
            this.tabRegistry.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.regToolStrip.ResumeLayout(false);
            this.regToolStrip.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabSecurity.ResumeLayout(false);
            this.groupDataEncrypt.ResumeLayout(false);
            this.groupDataEncrypt.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.groupUsageRights.ResumeLayout(false);
            this.groupUsageRights.PerformLayout();
            this.groupExpiration.ResumeLayout(false);
            this.groupExpiration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyTtlDaysValue)).EndInit();
            this.groupEditProtect.ResumeLayout(false);
            this.groupEditProtect.PerformLayout();
            this.tabWelcome.ResumeLayout(false);
            this.panelWelcome.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.bkPanel.ResumeLayout(false);
            this.panelLicense.ResumeLayout(false);
            this.panelLicense.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer regProgressTimer;
        private System.Windows.Forms.Timer itemHoverTimer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Label dropboxLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox propertyFriendlyName;
        private System.Windows.Forms.TextBox propertyAppID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lnkAutoLaunch;
        private System.Windows.Forms.Label propertyAutoLaunch;
        private System.Windows.Forms.LinkLabel lnkChangeIcon;
        private System.Windows.Forms.LinkLabel lnkChangeDataStorage;
        private System.Windows.Forms.Label propertyDataStorage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton propertyIsolationDataMode;
        private System.Windows.Forms.RadioButton propertyIsolationIsolated;
        private System.Windows.Forms.RadioButton propertyIsolationMerge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox propertyIcon;
        private System.Windows.Forms.Label lblAutoLaunch;
        private System.Windows.Forms.Button dropboxButton;
        private System.Windows.Forms.TabPage tabFileSystem;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView fsFolderTree;
        private System.Windows.Forms.ListView fsFilesList;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnFileSize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox fsFolderInfoIsolationCombo;
        private System.Windows.Forms.Label fsFolderInfoIsolationLbl;
        private System.Windows.Forms.Label fsFolderInfoFullName;
        private System.Windows.Forms.ToolStrip fileToolStrip;
        private System.Windows.Forms.ToolStripButton fsAddBtn;
        private System.Windows.Forms.ToolStripButton fsAddDirBtn;
        private System.Windows.Forms.ToolStripButton fsRemoveBtn;
        private System.Windows.Forms.ToolStripButton fsAddEmptyDirBtn;
        private System.Windows.Forms.ToolStripButton fsSaveFileAsBtn;
        private System.Windows.Forms.TabPage tabRegistry;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer regSplitContainer;
        private System.Windows.Forms.TreeView regFolderTree;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox regFolderInfoIsolationCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label regFolderInfoFullName;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ProgressBar regProgressBar;
        private System.Windows.Forms.ToolStrip regToolStrip;
        private System.Windows.Forms.ToolStripButton regRemoveBtn;
        private System.Windows.Forms.ToolStripButton regEditBtn;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel lnkCustomEvents;
        private System.Windows.Forms.TextBox propertyStopInheritance;
        private System.Windows.Forms.Panel bkPanel;
        private ListViewEx regFilesList;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.TextBox tbSize;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox chkCleanAsk;
        private System.Windows.Forms.RadioButton rdbCleanAll;
        private System.Windows.Forms.RadioButton rdbCleanRegOnly;
        private System.Windows.Forms.RadioButton rdbCleanNone;
        private System.Windows.Forms.CheckBox chkCleanDoneDialog;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripRegistryFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExport;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportXmlToolStripMenuItem;
        private System.Windows.Forms.Panel panelWelcome;
        private System.Windows.Forms.ImageList imageListMRU;
        private System.Windows.Forms.TabPage tabWelcome;
        private System.Windows.Forms.ToolStripButton regImportBtn;
        private System.Windows.Forms.ToolStripButton regExportBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip fileContextMenu;
        private System.Windows.Forms.ToolStripMenuItem fileContextMenuDelete;
        private System.Windows.Forms.ToolStripMenuItem fileContextMenuProperties;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox propertyFileVersion;
        private System.Windows.Forms.ColumnHeader columnFileType;
        private System.Windows.Forms.LinkLabel lnkPackageEdit;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListView listViewMRU;
        private System.Windows.Forms.ColumnHeader columnFileN;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem langToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frenchMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spanishMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdbIntegrateVirtual;
        private System.Windows.Forms.RadioButton rdbIntegrateStandard;
        private System.Windows.Forms.RadioButton rdbIntegrateNone;
        private System.Windows.Forms.ToolStripMenuItem chineseMenuItem;
        private System.Windows.Forms.RadioButton propertyVirtModeDisk;
        private System.Windows.Forms.RadioButton propertyVirtModeRam;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel lnkAutoUpdate;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.PictureBox picDisk;
        private System.Windows.Forms.PictureBox picRAM;
        private System.Windows.Forms.Label helpVirtMode;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label helpIsolationMode;
        private System.Windows.Forms.PictureBox picDataMode;
        private System.Windows.Forms.PictureBox picIsolatedMode;
        private System.Windows.Forms.PictureBox picFullAccess;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.CheckBox cbDatFile;
        private System.Windows.Forms.GroupBox line;
        private System.Windows.Forms.CheckBox propertyDisplayLogo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panelLicense;
        private System.Windows.Forms.Label lblNotCommercial;
        private System.Windows.Forms.LinkLabel lnkUpgrade;
        private System.Windows.Forms.CheckBox propertyScmDirect;
        private System.Windows.Forms.CheckBox cbVolatileRegistry;
        private System.Windows.Forms.TabPage tabSecurity;
        private System.Windows.Forms.GroupBox groupUsageRights;
        private System.Windows.Forms.LinkLabel lnkActiveDirectory;
        private System.Windows.Forms.GroupBox groupExpiration;
        private System.Windows.Forms.DateTimePicker propertyExpirationDatePicker;
        private System.Windows.Forms.CheckBox propertyExpiration;
        private System.Windows.Forms.CheckBox propertyTtlDays;
        private System.Windows.Forms.CheckBox propertyTtlResistRemove;
        private System.Windows.Forms.NumericUpDown propertyTtlDaysValue;
        private System.Windows.Forms.GroupBox groupEditProtect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPasswordConfirm;
        private System.Windows.Forms.TextBox propertyProtPassword;
        private System.Windows.Forms.CheckBox propertyProt;
        private System.Windows.Forms.GroupBox groupDataEncrypt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbEncryptionPwdConfirm;
        private System.Windows.Forms.TextBox tbEncryptionPwd;
        private System.Windows.Forms.LinkLabel lnkGenerateEncKey;
        private System.Windows.Forms.TextBox tbEncryptionKey;
        private System.Windows.Forms.RadioButton propertyEncryptUsingPassword;
        private System.Windows.Forms.RadioButton propertyEncryptUsingKey;
        private System.Windows.Forms.CheckBox propertyEncryption;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.LinkLabel lnkEncryptionLearnMore;
        private System.Windows.Forms.RadioButton propertyEncryptUserCreatedPassword;
        private System.Windows.Forms.LinkLabel lnkExportPwdKey;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.LinkLabel lnkImportPwdKey;

    }
}

