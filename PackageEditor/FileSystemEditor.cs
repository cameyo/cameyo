using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using VirtPackageAPI;
using PackageEditor.FilesEditing;
using PackageEditor.Utilities;
using Delay;
using Cameyo.OpenSrc.Common;

namespace PackageEditor
{
    public class FileData
    {
        public VirtFsNode virtFsNode;
        public String addedFrom;        // Only filled for new added files, contains the 'source' from the real system.
        public bool deleted;            // True if file deleted from virtual package
    }

    public class FolderTreeNode : TreeNode
    {
        public VirtFsNode virtFsNode;
        public List<FileData> childFiles;
        public UInt32 sandboxFlags;
        public bool deleted;
        public bool addedEmpty;         // User added this as a new empty folder
    }

    public class FileSystemEditor
    {
        private VirtPackage virtPackage;
        private TreeView fsFolderTree;
        private ListView fsFilesList;
        private Label fsFolderInfoFullName;
        private ComboBox fsFolderInfoIsolationCombo;
        private ToolStripButton fsAddBtn;
        private ToolStripButton fsAddDirBtn;
        private ToolStripButton fsRemoveBtn;
        private ToolStripButton fsAddEmptyDirBtn;
        private ToolStripButton fsSaveFileAsBtn;
        private TreeHelper treeHelper;
        private String fileSaveTargetDir;
        private String DnDtempFolder;// for Drag and Drop from the PackageEditor to Explorer, folder used for temporarely extracting files
        public bool dirty;

        // creation of delegate for AddFileOrFolderRecursive
        public delegate bool DelegateAddFileOrFolderRecursive(FolderTreeNode parentNode, String path);
        public DelegateAddFileOrFolderRecursive Del_AddFOrFR;

        public FolderTreeNode getFileTree()
        {
            return (FolderTreeNode)fsFolderTree.Nodes[0];
        }

        public FileSystemEditor(VirtPackage virtPackage, TreeView fsFolderTree, ListView fsFilesList,
            Label fsFolderInfoFullName, ComboBox fsFolderInfoIsolationCombo,
            ToolStripButton fsAddBtn, ToolStripButton fsRemoveBtn, ToolStripButton fsAddEmptyDirBtn,
            ToolStripButton fsSaveFileAsBtn, ToolStripButton fsAddDirBtn)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

            this.virtPackage = virtPackage;
            this.fsFolderTree = fsFolderTree;
            this.fsFilesList = fsFilesList;
            this.fsFolderInfoFullName = fsFolderInfoFullName;
            this.fsFolderInfoIsolationCombo = fsFolderInfoIsolationCombo;
            this.fsAddBtn = fsAddBtn;
            this.fsRemoveBtn = fsRemoveBtn;
            this.fsAddEmptyDirBtn = fsAddEmptyDirBtn;
            this.fsSaveFileAsBtn = fsSaveFileAsBtn;
            this.fsAddDirBtn = fsAddDirBtn;

            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.Text = "";
            fsFolderInfoIsolationCombo.Items.Add(PackageEditor.Messages.Messages.fullAccess);
            fsFolderInfoIsolationCombo.Items.Add(PackageEditor.Messages.Messages.isolated);
            fsFolderInfoIsolationCombo.Items.Add(PackageEditor.Messages.Messages.strictlyIsolated);
            fsFolderTree.AfterSelect += OnFolderTreeSelect;
            fsFolderInfoIsolationCombo.SelectionChangeCommitted += OnFolderSandboxChange;
            fsAddBtn.Click += OnAddBtnClick;
            fsAddDirBtn.Click += OnAddDirBtnClick;
            fsRemoveBtn.Click += OnRemoveBtnClick;
            fsAddEmptyDirBtn.Click += OnAddEmptyDirBtnClick;
            fsSaveFileAsBtn.Click += OnSaveFileAsBtnClick;
            fsFilesList.KeyDown += Vfs_KeyDown;
            fsFolderTree.KeyDown += Vfs_KeyDown;
            dirty = false;
            treeHelper = new TreeHelper(virtPackage);

            // delegate for AddFileOrFolderRecursive init
            Del_AddFOrFR = new DelegateAddFileOrFolderRecursive(this.AddFileOrFolderRecursive);
        }

        public void OnPackageOpen()
        {
            List<VirtFsNode> virtFsNodes = new List<VirtFsNode>();
            virtPackage.EnumFiles(ref virtFsNodes);
            fsFolderTree.Nodes.Clear();

            // Add first "FileSystem" root node"
            FolderTreeNode newNode = new FolderTreeNode();
            newNode.Text = "FileSystem";
            newNode.virtFsNode = new VirtFsNode();
            treeHelper.SetFolderNodeImage(newNode,
                false, false, virtPackage.GetFileSandbox("", false));
            fsFolderTree.Nodes.Add(newNode);

            foreach (VirtFsNode virtFsNode in virtFsNodes)
            {
                AddFileOrFolder(virtFsNode, "");
            }

            // %Temp Internet% has predefined "WriteCopy" attribute, set by Packager
            // Add it here just so that user can edit its Sandbox flags.
            /*VirtFsNode tempVirtFsNode = new VirtFsNode();
            tempVirtFsNode.FileName = "%Temp Internet%";
            tempVirtFsNode.FileFlags = 0;               // Folder, not file
            AddFileOrFolder(tempVirtFsNode, "");*/

            fsFolderTree.Nodes[0].Expand();             // Expand the "FileSystem" node
            fsFolderTree.SelectedNode = fsFolderTree.Nodes[0];
            dirty = false;
        }

        public void OnTabActivate()
        {
            // Force Isolation combo to refresh. This is in case the General Properties'
            // Isolation radio was changed, we need to refresh the Isolation combo here.
            if (fsFolderTree.Nodes.Count > 0)
            {
                fsFolderTree.SelectedNode = null;
                fsFolderTree.SelectedNode = fsFolderTree.Nodes[0];
            }
        }

        public void OnPackageClose()
        {
            fsFolderTree.Nodes.Clear();
            fsFilesList.Items.Clear();
        }

        private bool OnPackageSaveRecursive(TreeNodeCollection curNodes)
        {
            foreach (FolderTreeNode curFolder in curNodes)
            {
                if (curFolder.deleted)                      // Deleted Folder
                {
                    virtPackage.DeleteFile(treeHelper.GetFullNodeName((TreeNode)curFolder));
                    // Stop recursing inside this node
                }
                else
                {
                    if (curFolder.childFiles != null)
                    {
                        foreach (FileData child in curFolder.childFiles)
                        {
                            if (child.deleted)                  // Deleted File
                            {
                                String findFile = child.virtFsNode.FileName;
                                Boolean findDeleted = false;
                                if (!curFolder.childFiles.Exists(
                                    delegate(FileData data)
                                    {
                                        return data.virtFsNode.FileName.Equals(findFile, StringComparison.CurrentCultureIgnoreCase) && data.deleted == findDeleted;
                                    }))
                                {
                                    // only delete if no new file is added with same name.
                                    // the virtPackage will automaticaly replace it when AddFile is called.

                                    virtPackage.DeleteFile(child.virtFsNode.FileName);
                                }
                            }
                            else
                            {
                                if (child.addedFrom != "")
                                {
                                  // Added File                                  
                                  virtPackage.AddFileEx(child.addedFrom, child.virtFsNode.FileName, false, (VIRT_FILE_FLAGS)child.virtFsNode.FileFlags);
                                }
                                virtPackage.SetFileFlags(child.virtFsNode.FileName, child.virtFsNode.FileFlags);
                            }
                        }
                    }
                    if (curFolder.addedEmpty)
                        virtPackage.AddEmptyDir(treeHelper.GetFullNodeName((TreeNode)curFolder), false);
                    OnPackageSaveRecursive(curFolder.Nodes);
                }
            }
            return true;
        }

        public bool OnPackageSave()
        {
            bool Ret = true;
            if (fsFolderTree.Nodes.Count == 0 ||        // No node other than "FileSystem"
                fsFolderTree.Nodes[0].Nodes.Count == 0) // No node other than "FileSystem"
                return true;                            // Empty tree..
            OnPackageSaveRecursive(fsFolderTree.Nodes[0].Nodes);
            //for (int i = 1; i < fsFolderTree.Nodes.Count; i++)
            //FolderTreeNode curNode = (FolderTreeNode)fsFolderTree.Nodes[0].Nodes[0];
            return (Ret);
        }

        private FolderTreeNode AddFileOrFolder(VirtFsNode virtFsNode, String SourceFileName)
        {
            FolderTreeNode newNode = null;
            FolderTreeNode curParent;
            TreeNode node;
            bool bFound;

            curParent = null;
            String fileName = "FileSystem\\" + virtFsNode.FileName;
            String[] tokens = fileName.Split('\\');
            foreach (String curToken in tokens)
            {
                if (curParent == null)
                    node = fsFolderTree.Nodes[0]; // There's always a top-node, since we've added "FileSystem" node
                //(fsFolderTree.Nodes.Count > 0 ? fsFolderTree.Nodes[0] : null);   // Top-most node
                else
                    node = curParent.FirstNode;

                bFound = false;
                while (node != null)
                {
                    if (node.Text == curToken)
                    {
                        curParent = (FolderTreeNode)node;
                        bFound = true;
                        break;
                    }
                    node = node.NextNode;
                }
                if (bFound == false)
                {
                    if ((virtFsNode.FileFlags & VIRT_FILE_FLAGS.ISFILE) == 0)
                    {
                        // Adding Folder
                        newNode = new FolderTreeNode();
                        newNode.Text = Path.GetFileName(virtFsNode.FileName);
                        newNode.virtFsNode = virtFsNode;
                        newNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                        newNode.deleted = false;
                        newNode.addedEmpty = false;
                        treeHelper.SetFolderNodeImage(newNode, newNode.deleted, (virtFsNode.FileFlags & VIRT_FILE_FLAGS.DELETED) != 0, newNode.sandboxFlags);
                        //if (newNode.sandboxFlags == SANDBOXFLAGS_COPY_ON_WRITE) newNode.ImageIndex = 3;
                        if (curParent != null)
                            curParent.Nodes.Add(newNode);
                        else
                            fsFolderTree.Nodes.Add(newNode);
                        curParent = newNode;
                    }
                    else
                    {
                        // Adding File
                        if (curParent != null)
                        {
                            FileData childFile = new FileData();
                            childFile.virtFsNode = virtFsNode;
                            if (curParent.childFiles == null)
                                curParent.childFiles = new List<FileData>();
                            childFile.addedFrom = SourceFileName;
                            childFile.deleted = false;
                            curParent.childFiles.Add(childFile);
                        }
                    }
                    if (curParent != null)
                    {
                        FolderTreeNode upperParent = curParent;
                        while (upperParent != null)
                        {
#pragma warning disable 1690
                            upperParent.virtFsNode.EndOfFile += virtFsNode.EndOfFile;   // CS1690 is OK
#pragma warning restore 1690
                            upperParent = (FolderTreeNode)upperParent.Parent;
                        }
                    }
                }
            }
            dirty = true;
            return newNode;
        }

        public void OnFolderTreeSelect(object sender, TreeViewEventArgs e)
        {
            FolderTreeNode folderNode = (FolderTreeNode)e.Node;
            fsFolderInfoFullName.Text = "";
            fsFolderInfoIsolationCombo.SelectedIndex = -1;
            if (folderNode == null)
                return;
            fsFolderInfoIsolationCombo.Enabled = true; //(folderNode != fsFolderTree.Nodes[0]);
            VirtFsNode virtFsNode = folderNode.virtFsNode;    // Avoids CS1690

            // Fill info panel
            String fullName = treeHelper.GetFullNodeName(folderNode);
            fsFolderInfoFullName.Text = "[" + Win32Function.StrFormatByteSize64(virtFsNode.EndOfFile) + "] " + fullName;
            fsFolderInfoIsolationCombo.SelectedIndex = treeHelper.SandboxFlagsToComboIndex(
                virtPackage.GetFileSandbox(fullName, false));

            // Fill fsFilesList
            fsFilesList.Items.Clear();
            if (folderNode.childFiles != null)
            {
                for (int i = folderNode.childFiles.Count - 1; i >= 0; i--)
                {
                    FileData childFile = folderNode.childFiles[i];
                    FileListViewItem newItem = new FileListViewItem();
                    newItem.Text = Path.GetFileName(childFile.virtFsNode.FileName);
                    newItem.SubItems.Add(Win32Function.StrFormatByteSize64(childFile.virtFsNode.EndOfFile));

                    newItem.flags = (VIRT_FILE_FLAGS)childFile.virtFsNode.FileFlags;

                    if ((newItem.flags & VirtPackageAPI.VIRT_FILE_FLAGS.DEPLOY_UPON_PRELOAD) != 0)
                        newItem.ImageIndex = 7;
                    else
                        newItem.ImageIndex = 6;
                    //ListViewItem.ListViewSubItem x = new ListViewItem.ListViewSubItem();
                    //x.Text = ((VIRT_FILE_FLAGS)childFile.virtFsNode.FileFlags).ToString();
                    //newItem.SubItems.Add(x);
                    newItem.SubItems.Add(Path.GetExtension(newItem.Text));
                  
                    newItem.fileSize = childFile.virtFsNode.EndOfFile;
                    if (childFile.addedFrom != "")
                    {
                        if (folderNode.deleted)
                        {
                            folderNode.childFiles.Remove(childFile);       // Added file in a Removed folder. Forbidden!!
                            continue;
                        }
                        else
                            newItem.ForeColor = Color.Green;               // Newly-added
                    }
                    else if (folderNode.ImageIndex == 5)                   // deleted
                        newItem.ForeColor = Color.Red;
                    else if (childFile.deleted)
                        newItem.ForeColor = Color.Red;
                    fsFilesList.Items.Add(newItem);
                }
            }
        }

        private void OnFolderSandboxChange(object sender, EventArgs e)
        {
            FolderTreeNode node = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (node == null)
                return;
            String fullName = treeHelper.GetFullNodeName(node);
            virtPackage.SetFileSandbox(
                node != fsFolderTree.Nodes[0] ? fullName : "",   // Root = ""
                treeHelper.ComboIndexToSandboxFlags(fsFolderInfoIsolationCombo.SelectedIndex), false);
            node.sandboxFlags = virtPackage.GetFileSandbox(fullName, false);
            RefreshFolderNodeRecursively(node, 0);
            dirty = true;
        }


        // handler for the key down in the virtual filesystem view
        // it now handles CTRL+V and CANC
        private void Vfs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V && Clipboard.ContainsFileDropList())
            {
                FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
                if (parentNode == null)
                {
                    MessageBox.Show("Please select a folder to add files into");
                    return;
                }
                if (parentNode.deleted)
                {
                    MessageBox.Show("Folder was deleted");
                    return;
                }

                StringCollection paths = Clipboard.GetFileDropList();
                foreach (String path in paths)
                {
                    AddFileOrFolderRecursive(parentNode, path);
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                OnRemoveBtnClick(fsRemoveBtn, null);
            }
        }

        private void OnAddBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (parentNode == null)
            {
                MessageBox.Show("Please select a folder to add files into");
                return;
            }
            if (parentNode.deleted)
            {
                MessageBox.Show("Folder was deleted");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All file types|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String srcFileName in openFileDialog.FileNames)
                {
                    AddFileOrFolderRecursive(parentNode, srcFileName);
                }
            }
        }

        // handler for the add dir button
        private void OnAddDirBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (parentNode == null)
            {
                MessageBox.Show("Please select a folder to add to");
                return;
            }
            if (parentNode.deleted)
            {
                MessageBox.Show("Folder was deleted");
                return;
            }

            String selectedFolder = "";
            FolderBrowserDialog selectFolder = new FolderBrowserDialog();
            selectFolder.ShowNewFolderButton = false;

            if (selectFolder.ShowDialog() != DialogResult.OK)
                return;

            selectedFolder = selectFolder.SelectedPath;

            AddFileOrFolderRecursive(parentNode, selectedFolder);
        }

        // function to add files and folders recursively
        private bool AddFileOrFolderRecursive(FolderTreeNode parentNode, String path)
        {
            // if path is a file
            if (File.Exists(path))
            {
                VirtFsNode virtFsFileNode;
                FileData fileOverwrite = null;
                if (parentNode.childFiles != null)
                {

                    foreach (FileData file in parentNode.childFiles)
                    {
                        if (!file.deleted && Path.GetFileName(file.virtFsNode.FileName).Equals(Path.GetFileName(path), StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (MessageBox.Show(String.Format("File \"{0}\" already exists, overwrite?", file.virtFsNode.FileName), "Overwrite?", MessageBoxButtons.YesNo) == DialogResult.No)
                                return false;
                            else
                            {
                                if ((file.addedFrom != ""))
                                    fileOverwrite = file;
                                else
                                    file.deleted = true;
                                break;
                            }
                        }
                    }
                }


                virtFsFileNode = new VirtFsNode();
#pragma warning disable 1690
                virtFsFileNode.FileName = TreeHelper.FullPath(parentNode.virtFsNode.FileName, Path.GetFileName(path));
#pragma warning restore 1690
                virtFsFileNode.FileFlags = VIRT_FILE_FLAGS.ISFILE;      //it's a file
                if (Path.GetExtension(virtFsFileNode.FileName).Equals(".dll"))
                {
                    virtFsFileNode.FileFlags |= VIRT_FILE_FLAGS.DEPLOY_UPON_PRELOAD;
                }


                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                virtFsFileNode.EndOfFile = (ulong)fi.Length;

                if (fileOverwrite != null)
                    fileOverwrite.virtFsNode = virtFsFileNode;
                else
                    AddFileOrFolder(virtFsFileNode, path);     // Also sets dirty = true

                if (parentNode == fsFolderTree.SelectedNode)
                {
                    RefreshFolderNodeRecursively(parentNode, 0);
                    TreeViewEventArgs ev = new TreeViewEventArgs(parentNode);
                    OnFolderTreeSelect(null, ev);
                }
                return true;
            }

            if (!Directory.Exists(path))
                return false;

            FolderTreeNode folderOverwrite = null;
            //foreach (String subdir in subdirs)
            {
                foreach (FolderTreeNode childNode in parentNode.Nodes)
                {
                    if (childNode.Text.Equals(Path.GetFileName(path), StringComparison.CurrentCultureIgnoreCase))
                    {

#pragma warning disable 1690
                        if (!childNode.deleted && MessageBox.Show(String.Format("Folder \"{0}\" already exists, overwrite?", childNode.virtFsNode.FileName), "Overwrite?", MessageBoxButtons.YesNo) == DialogResult.No)
#pragma warning restore 1690
                            return false;
                        else
                        {
                            folderOverwrite = childNode;
                            childNode.deleted = false;
                            if (childNode.childFiles != null)
                                foreach (FileData file in childNode.childFiles)
                                    file.deleted = true;// make sure files from a previously deleted folder dont come back..
                            break;
                        }
                    }
                }
            }

            String[] lsFiles = Directory.GetFiles(path);
            String[] lsDirs = Directory.GetDirectories(path);
            FolderTreeNode subdirNode;
            if (folderOverwrite == null)
            {
                // if path is a folder
                VirtFsNode virtFsDirNode = new VirtFsNode();
#pragma warning disable 1690
                virtFsDirNode.FileName = TreeHelper.FullPath(parentNode.virtFsNode.FileName, Path.GetFileName(path));
#pragma warning restore 1690
                virtFsDirNode.FileFlags = 0;                                       //it's a dir
                subdirNode = AddFileOrFolder(virtFsDirNode, path);     // Also sets dirty = true
            }
            else
                subdirNode = folderOverwrite;


            foreach (String file in lsFiles)
            {
                if (!AddFileOrFolderRecursive(subdirNode, file))
                    return false;
            }
            foreach (String dir in lsDirs)
            {
                if (!AddFileOrFolderRecursive(subdirNode, dir))
                    return false;
            }
            if (parentNode == fsFolderTree.SelectedNode)
            {
                RefreshFolderNodeRecursively(parentNode, 0);
                TreeViewEventArgs ev = new TreeViewEventArgs(parentNode);
                OnFolderTreeSelect(null, ev);
            }
            return true;
        }

        private void OnRemoveBtnClick(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection fileItems = fsFilesList.SelectedItems;
            FolderTreeNode folderNode;
            if (sender == fsRemoveBtn)      // First recursion iteration
                folderNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            else
                folderNode = (FolderTreeNode)sender;

            if (fileItems.Count > 0)    // In this case, folderNode is always selected too
            {
                // Removing a File
                foreach (ListViewItem item in fileItems)
                {
                    if (folderNode.childFiles.Count == 0) continue;     // Should never happen
                    FileData fileData;
                    for (int i = folderNode.childFiles.Count - 1; i >= 0; i--)
                    {
                        fileData = folderNode.childFiles[i];
                        if (Path.GetFileName(fileData.virtFsNode.FileName) == item.Text)
                        {
                            if (fileData.addedFrom != "")   // Just added
                            {
                                item.Remove();
                                folderNode.childFiles.Remove(fileData);
                            }
                            else
                                folderNode.childFiles[i].deleted = true;
                            break;
                        }
                    }
                }
                TreeViewEventArgs ev = new TreeViewEventArgs(folderNode);
                OnFolderTreeSelect(sender, ev);
                dirty = true;
            }
            else if (folderNode != null)
            {
                // Removing a Folder: recurse!
                FolderTreeNode curNode;
                if (sender == fsRemoveBtn)      // First recursion iteration
                {
                    curNode = folderNode;
                    curNode.deleted = true; //curNode.ImageIndex = curNode.SelectedImageIndex = 5;      // deleted
                    if (curNode.Nodes.Count > 0)
                        OnRemoveBtnClick(curNode.Nodes[0], e);
                    RefreshFolderNodeRecursively(curNode, 0);
                    TreeViewEventArgs ev = new TreeViewEventArgs(curNode);
                    OnFolderTreeSelect(sender, ev);

                }
                else
                {
                    for (curNode = folderNode; curNode != null; curNode = (FolderTreeNode)curNode.NextNode)
                    {
                        curNode.deleted = true; //curNode.ImageIndex = curNode.SelectedImageIndex = 5;      // deleted
                        if (curNode.Nodes.Count > 0)
                            OnRemoveBtnClick(curNode.Nodes[0], e);
                    }
                }
                dirty = true;
            }
            else
                MessageBox.Show("Please select a folder/file to remove");

            /* ToDo: it seems some file remain in the folder after it's been deleted (only on delete folder)
             * Check and correct.
             *
            if (folderNode == fsFolderTree.SelectedNode && folderNode.childFiles.Count == 0)
            {
                foreach (ListViewItem item in fsFilesList.Items)
                {
                    item.Remove();
                }
            }
             * */
        }

        private void OnAddEmptyDirBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode parentNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (parentNode == null)
            {
                MessageBox.Show("Please select a folder to add to");
                return;
            }
            if (parentNode.deleted)
            {
                MessageBox.Show("Folder was deleted");
                return;
            }
retry_input:
            String newFolderName = "";
            if (TreeHelper.InputBox("Add empty folder", "Folder name:", ref newFolderName) != DialogResult.OK ||
                string.IsNullOrEmpty(newFolderName))
            {
                return;
            }

            // Help user: don't let them allow root dirs such as "SomeDir". Instead, force "C_" or "%Program Files%" for root nodes.
            if (parentNode.Parent == null)   // Adding to root dir
            {
                if ((newFolderName.Length < 2) || 
                    (newFolderName.Length == 2 && newFolderName[1] != '_') || 
                    (newFolderName.Length > 2 && newFolderName[0] != '%'))
                {
                    if (MessageBox.Show(Messages.Messages.incorrectRootDir, "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        goto retry_input;
                }
            }

            if (newFolderName.Contains("\\"))
            {
                MessageBox.Show("Folder must not contain '\\'. Please specify one folder at a time.");
                return;
            }

            VirtFsNode virtFsNode = new VirtFsNode();
#pragma warning disable 1690
            virtFsNode.FileName = TreeHelper.FullPath(parentNode.virtFsNode.FileName, newFolderName);
#pragma warning restore 1690
            virtFsNode.FileFlags = 0;                       // Folder, not file

            //String[] subdirs = newFolderName.Split('\\');
            FolderTreeNode curParentNode = parentNode;
            FolderTreeNode folderOverwrite = null;
            //foreach (String subdir in subdirs)
            {
                foreach (FolderTreeNode childNode in curParentNode.Nodes)
                {
                    if (childNode.Text.Equals(newFolderName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!childNode.deleted)
                        {
                            MessageBox.Show("Folder already exists");
                            return;
                        }
                        else
                        {
                            folderOverwrite = childNode;
                            childNode.deleted = false;
                            if (childNode.childFiles != null)
                                foreach (FileData file in childNode.childFiles)
                                    file.deleted = true;// make sure files from a previously deleted folder dont come back..
                        }
                    }
                }
            }

            FolderTreeNode newNode;
            if (folderOverwrite != null)
                newNode = folderOverwrite;
            else
            {
                newNode = AddFileOrFolder(virtFsNode, newFolderName);     // Also sets dirty = true
                if (newNode != null)
                    newNode.addedEmpty = true;
            }
            RefreshFolderNodeRecursively(parentNode, 0);
            TreeViewEventArgs ev = new TreeViewEventArgs(parentNode);
            OnFolderTreeSelect(sender, ev);
        }

        private void OnSaveFileAsBtnClick(object sender, EventArgs e)
        {
            FolderTreeNode folderNode = (FolderTreeNode)fsFolderTree.SelectedNode;
            if (folderNode == null)
            {
                MessageBox.Show("Please select a file to save");
                return;
            }

            if (fileSaveTargetDir == null || !Directory.Exists(fileSaveTargetDir))
            {
                if (!string.IsNullOrEmpty(virtPackage.openedFile))
                    fileSaveTargetDir = Path.GetDirectoryName(virtPackage.openedFile);
                else
                    fileSaveTargetDir = "";
            }

            ListView.SelectedListViewItemCollection fileItems = fsFilesList.SelectedItems;
            if (fileItems.Count == 0)    // In this case, folderNode is always selected too
            {
                if (TreeHelper.InputFolderBrowserDialog("Select the destination path on your hard disk to save the files.", ref fileSaveTargetDir) != DialogResult.OK)
                    return;
                SaveFolderContent(folderNode, fileSaveTargetDir);
                //MessageBox.Show("Please select a file, not a folder");
                return;
            }

            if (TreeHelper.InputFolderBrowserDialog("Select the destination path on your hard disk to save the file.", ref fileSaveTargetDir) != DialogResult.OK)
                return;

            // Save files
            if (folderNode.childFiles.Count == 0)
                return;     // Should never happen
            List<FileData> files = getSelectedFiles();
            foreach  (FileData item in files)
            {
              SaveFile(item, fileSaveTargetDir);
            }
        }

        private void SaveFile(FileData fileData, string fileSaveTargetDir)
        {
            if (fileData.addedFrom != "")   // Just added
                MessageBox.Show("Cannot save a file that was just added: " + fileData.virtFsNode.FileName);
            else
            {
                if (!virtPackage.ExtractFile(fileData.virtFsNode.FileName, fileSaveTargetDir))
                    MessageBox.Show("Cannot save file: " + fileData.virtFsNode.FileName + " to " + fileSaveTargetDir);
            }
        }

        private void SaveFolderContent(FolderTreeNode node, string fileSaveTargetDir)
        {
            if (node.childFiles != null)
            {
                foreach (FileData f in node.childFiles)
                {
                    SaveFile(f, fileSaveTargetDir);
                }
            }
            foreach (FolderTreeNode f in node.Nodes)
            {
                String subFolder = fileSaveTargetDir + '\\' + f.Text;
                Directory.CreateDirectory(subFolder);
                SaveFolderContent(f, subFolder);
            }
        }

        public void RefreshFolderNodeRecursively(FolderTreeNode node, int iteration)
        {
            FolderTreeNode curNode = node;

            if (iteration == 0)
            {
                VirtFsNode virtFsNode = curNode.virtFsNode;         // Avoids CS1690
                curNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                treeHelper.SetFolderNodeImage(curNode, curNode.deleted, (virtFsNode.FileFlags & VIRT_FILE_FLAGS.DELETED) != 0, curNode.sandboxFlags);
                if (curNode.Nodes.Count > 0)
                    RefreshFolderNodeRecursively((FolderTreeNode)curNode.Nodes[0], iteration + 1);
            }
            else
            {
                while (curNode != null)
                {
                    VirtFsNode virtFsNode = curNode.virtFsNode;     // Avoids CS1690
                    curNode.sandboxFlags = virtPackage.GetFileSandbox(virtFsNode.FileName, false);
                    treeHelper.SetFolderNodeImage(curNode, curNode.deleted, (virtFsNode.FileFlags & VIRT_FILE_FLAGS.DELETED) != 0, curNode.sandboxFlags);
                    if (curNode.Nodes.Count > 0)
                        RefreshFolderNodeRecursively((FolderTreeNode)curNode.Nodes[0], iteration + 1);
                    curNode = (FolderTreeNode)curNode.NextNode;
                }
            }
        }

        /*[DllImport("kernel32.dll")]
        static extern bool FileTimeToLocalFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime, out System.Runtime.InteropServices.ComTypes.FILETIME lpLocalFileTime);
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern long FileTimeToSystemTime(ref System.Runtime.InteropServices.ComTypes.FILETIME FileTime, ref SYSTEMTIME SystemTime);*/

        private List<FileData> getSelectedFiles()
        {
          FolderTreeNode currentfolder = (FolderTreeNode)fsFolderTree.SelectedNode;          

          List<FileData> result = new List<FileData>();
          foreach (FileListViewItem file in fsFilesList.SelectedItems)
          {
            foreach (FileData fd in currentfolder.childFiles)
            {
              if (Path.GetFileName(fd.virtFsNode.FileName).Equals(file.Text, StringComparison.CurrentCultureIgnoreCase))
              {
                result.Add(fd);
              }
            }
          }
          return result;
        }

        public void GetFileData(Delay.VirtualFileDataObject.Tuple<Stream, string> tuple)
        {
          String tempFile = Path.Combine(DnDtempFolder, tuple.Item2);
          Directory.CreateDirectory(Path.GetDirectoryName(tempFile));
          virtPackage.ExtractFile(tuple.Item2, Path.GetDirectoryName(tempFile));

          using (FileStream sourceStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
          {
            byte[] buffer = new byte[1024 * 1024];
            while (true)
            {
              int read = sourceStream.Read(buffer, 0, buffer.Length);
              if (read <= 0)
                break;
              tuple.Item1.Write(buffer, 0, read);
            }
          }
          File.Delete(tempFile);
        }

        List<FileData> getAllFilesUnderNode(FolderTreeNode node)
        {
          List<FileData> files = new List<FileData>();

          FileData fileData = new FileData();
          fileData.virtFsNode = new VirtFsNode();
          fileData.virtFsNode.FileName = treeHelper.GetFullNodeName(node) + "\\";
          if (fileData.virtFsNode.FileName != "\\")
          {
            files.Add(fileData);
          }

          if (node.childFiles != null)
          {
            files.AddRange(node.childFiles);
          }
          foreach (FolderTreeNode subnode in node.Nodes)
          {
            files.AddRange(getAllFilesUnderNode(subnode));
          }
          return files;
        }

        internal bool DragDropFiles(object dragitem)
        {
          DnDtempFolder = Common.CreateTempFolder("PackageEditorDnD_");
          List<FileData> files = null;
          FolderTreeNode node;
          int fullNodeNameLen = 0;     
          if (dragitem is FolderTreeNode)
          {
            node = (FolderTreeNode)dragitem;
            files = getAllFilesUnderNode(node);

            VirtFsNode fsnode = node.virtFsNode;// Avoids CS1690
            if (fsnode.FileName != null)
              fullNodeNameLen = (treeHelper.GetFullNodeName(node)).Length - node.Text.Length;
          }
          else
          {
            node = (FolderTreeNode)fsFolderTree.SelectedNode;
            files = getSelectedFiles();

            VirtFsNode fsnode = node.virtFsNode;// Avoids CS1690
            if (fsnode.FileName != null)
              fullNodeNameLen = (treeHelper.GetFullNodeName(node)).Length;
          }


          VirtualFileDataObject vfdo = new VirtualFileDataObject();
          vfdo.IsAsynchronous = true;
          vfdo.PreferredDropEffect = DragDropEffects.Copy;
          
          List<VirtualFileDataObject.FileDescriptor> fileDescriptors = new List<VirtualFileDataObject.FileDescriptor>();
          foreach (FileData item in files)
          {
            VirtualFileDataObject.FileDescriptor fileDecriptor = new VirtualFileDataObject.FileDescriptor();
            fileDecriptor.Name = item.virtFsNode.FileName.Remove(0, fullNodeNameLen);
            fileDecriptor.Length = (long)item.virtFsNode.EndOfFile;
            fileDecriptor.ChangeTimeUtc = DateTime.FromFileTime((long)item.virtFsNode.ChangeTime).ToUniversalTime();
            fileDecriptor.ExtraInfo = item.virtFsNode.FileName;
            fileDecriptor.StreamContents = new VirtualFileDataObject.MyAction<Delay.VirtualFileDataObject.Tuple<Stream, string>>(GetFileData);
            fileDescriptors.Add(fileDecriptor);
          }

          vfdo.SetData(fileDescriptors);
          vfdo.PreferredDropEffect = DragDropEffects.Copy;
          fsFilesList.DoDragDrop(vfdo, DragDropEffects.Copy);
          return true;
        }

        internal void ShowProperties()
        {
          FileProperties fp = new FileProperties(virtPackage);
          if (fp.Open(getSelectedFiles()))
          {
            FolderTreeNode node = (FolderTreeNode)fsFolderTree.SelectedNode;
            TreeViewEventArgs ev = new TreeViewEventArgs(node);
            OnFolderTreeSelect(null, ev);
          }
        }
    }

    public class TreeHelper
    {
        private VirtPackage virtPackage;

        public TreeHelper(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
        }

        public int SandboxFlagsToComboIndex(UInt32 SandboxFlags)
        {
            switch (SandboxFlags)
            {
                case VirtPackage.SANDBOXFLAGS_PASSTHROUGH:
                    return 0;
                case VirtPackage.SANDBOXFLAGS_COPY_ON_WRITE:
                    return 1;
                case VirtPackage.SANDBOXFLAGS_STRICTLY_ISOLATED:
                    return 2;
                default:
                    return 0;
            }
        }

        public UInt32 ComboIndexToSandboxFlags(int ComboIndex)
        {
            switch (ComboIndex)
            {
                case 0:
                    return VirtPackage.SANDBOXFLAGS_PASSTHROUGH;
                case 1:
                    return VirtPackage.SANDBOXFLAGS_COPY_ON_WRITE;
                case 2:
                    return VirtPackage.SANDBOXFLAGS_STRICTLY_ISOLATED;
                default:
                    return 0;
            }
        }

        public void SetFolderNodeImage(TreeNode node, bool toRemove, bool virtFsDeleted, UInt32 sandboxFlags)
        {
            if (toRemove)
                node.ImageIndex = node.SelectedImageIndex = 4;
            else if (virtFsDeleted)
                node.ImageIndex = node.SelectedImageIndex = 3;
            else
            {
                switch (sandboxFlags)
                {
                    case VirtPackage.SANDBOXFLAGS_PASSTHROUGH:
                        node.ImageIndex = node.SelectedImageIndex = 0;
                        break;
                    case VirtPackage.SANDBOXFLAGS_COPY_ON_WRITE:
                        node.ImageIndex = node.SelectedImageIndex = 1;
                        break;
                    case VirtPackage.SANDBOXFLAGS_STRICTLY_ISOLATED:
                        node.ImageIndex = node.SelectedImageIndex = 2;
                        break;
                    default:
                        node.ImageIndex = node.SelectedImageIndex = 0;
                        break;
                }
            }
        }

        public String GetFullNodeName(TreeNode node)
        {
            String FullName = "";
            while (node != null && node.Parent != null)     // node.Parent != null avoids the first node ("FileSystem" or "Registry")
            {
                FullName = TreeHelper.FullPath(node.Text, FullName);
                node = node.Parent;
            }
            FullName = FullName.Trim('\\');
            return (FullName);
        }

        public static String FullPath(String dir, String file)
        {
            if (dir == null || dir.EndsWith("\\") || file.StartsWith("\\"))
                return dir + file;
            else
                return dir + "\\" + file;
        }


        public static DialogResult InputFolderBrowserDialog(string promptText, ref string value)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = promptText;
            fbd.SelectedPath = value;
            DialogResult dialogResult = fbd.ShowDialog();
            value = fbd.SelectedPath;
            return dialogResult;
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
}
