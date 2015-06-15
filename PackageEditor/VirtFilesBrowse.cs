using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;
using System.IO;

namespace PackageEditor
{
    public partial class VirtFilesBrowse : Form
    {
        private VirtPackage virtPackage;
        private FileSystemEditor fileSystemEditor;
        private bool allowSelectFolder;
        const int IMGINDEX_FOLDER = 1;
        const int IMGINDEX_FILE = 6;

        public VirtFilesBrowse(VirtPackage virtPackage, FileSystemEditor fileSystemEditor)
        {
            InitializeComponent();
            this.virtPackage = virtPackage;
            this.fileSystemEditor = fileSystemEditor;
        }

        private void ShowFiles(TreeNodeCollection treeNodeCollection, FolderTreeNode folder)
        {
          if (folder.childFiles != null)
          {
            foreach (FileData fd in folder.childFiles)
            {
              if (!fd.deleted)
              {
                TreeNode filenode = treeNodeCollection.Add(Path.GetFileName(fd.virtFsNode.FileName));
                filenode.ImageIndex = 6;
                filenode.SelectedImageIndex = 6;
              }
            }
          }
        }
      
        private void AddItems(TreeNodeCollection fsFolderTree, FolderTreeNode folderTree)
        {
          foreach(FolderTreeNode folder in folderTree.Nodes)
          {
            if (!folder.deleted)
            {
              TreeNode folderNode  = fsFolderTree.Add(folder.Text);
              folderNode.ImageIndex = 0;
              folderNode.SelectedImageIndex = 0;
              AddItems(folderNode.Nodes, folder);
              ShowFiles(folderNode.Nodes, folder);
            }
          }
        }

        private void Populate()
        {
            fsFolderTree.Nodes.Clear();//
            FolderTreeNode folderTree = fileSystemEditor.getFileTree();
            AddItems(fsFolderTree.Nodes, folderTree);  
        }

        private void AddFileOrFolder(VirtFsNode virtFsNode, String SourceFileName)
        {
            FolderTreeNode newNode;
            FolderTreeNode curParent;
            TreeNode node;
            bool bFound;

            curParent = null;
            String fileName = virtFsNode.FileName;
            String[] tokens = fileName.Split('\\');
            foreach (String curToken in tokens)
            {
                if (curParent == null)
                    node = (FolderTreeNode)(fsFolderTree.Nodes.Count > 0 ? fsFolderTree.Nodes[0] : null);
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
                    // Adding Folder
                    newNode = new FolderTreeNode();
                    newNode.Text = System.IO.Path.GetFileName(virtFsNode.FileName);
                    newNode.virtFsNode = virtFsNode;
                    newNode.ImageIndex = newNode.SelectedImageIndex =
                        (virtFsNode.FileFlags & VIRT_FILE_FLAGS.ISFILE) > 0 ? IMGINDEX_FILE : IMGINDEX_FOLDER;
                    if (curParent != null)
                        curParent.Nodes.Add(newNode);
                    else
                        fsFolderTree.Nodes.Add(newNode);
                    curParent = newNode;
                    /*if ((virtFsNode.FileFlags & VirtPackage.VIRT_FILE_FLAGS_ISFILE) == 0)
                    {
                        // Adding Folder
                        newNode = new FolderTreeNode();
                        newNode.Text = System.IO.Path.GetFileName(virtFsNode.FileName);
                        newNode.virtFsNode = virtFsNode;
                        newNode.ImageIndex = newNode.SelectedImageIndex = 0;
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
                            curParent.childFiles.Add(childFile);
                        }
                    }*/
                }
            }
        }

        private String GetFullNodeName(TreeNode node)
        {
            String FullName = "";
            while (node != null)     // node.Parent != null avoids the first node ("FileSystem" or "Registry")
            {
                FullName = node.Text + "\\" + FullName;
                node = node.Parent;
            }
            FullName = FullName.Trim('\\');
            return (FullName);
        }

        public bool Do(ref String result, bool allowSelectFolder)
        {
            this.allowSelectFolder = allowSelectFolder;
            Populate();
            if (ShowDialog() == DialogResult.OK)
            {
                result = GetFullNodeName((TreeNode)fsFolderTree.SelectedNode);
                return true;
            }
            else
                return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (fsFolderTree.SelectedNode != null)
            {
                if (allowSelectFolder || fsFolderTree.SelectedNode.ImageIndex == IMGINDEX_FILE)
                    DialogResult = DialogResult.OK;
            }
        }
    }
}
