using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VirtPackageAPI;
using Cameyo.OpenSrc.Common;

namespace PackageEditor.FilesEditing
{
    public partial class FileProperties : Form
    {
        VirtPackage virtPackage;
        List<FileData> files;

        public FileProperties(VirtPackage virtPackage)
        {
            InitializeComponent();
            this.virtPackage = virtPackage;
        }

        internal bool Open(List<FileData> files)
        {
            this.files = files;
            bool result = false;
            String fileNames = null;
            ulong totalSize = 0;
            String dirStr = "", itemsStr;
            VIRT_FILE_FLAGS min = VIRT_FILE_FLAGS.ALL_FLAGS;
            VIRT_FILE_FLAGS max = VIRT_FILE_FLAGS.NO_FLAGS;

            foreach (FileData fd in files)
            {
                totalSize += fd.virtFsNode.EndOfFile;
                min &= (VIRT_FILE_FLAGS)fd.virtFsNode.FileFlags;
                max |= (VIRT_FILE_FLAGS)fd.virtFsNode.FileFlags;
                fileNames += (fileNames == null ? "" : ", ") + Path.GetFileName(fd.virtFsNode.FileName);
            }
            String sizeStr = Win32Function.StrFormatByteSize64(totalSize);
            this.Text = fileNames;
            tbFullPath.Text = files[0].virtFsNode.FileName;

            if (((VIRT_FILE_FLAGS)files[0].virtFsNode.FileFlags & VIRT_FILE_FLAGS.ISFILE) == 0)
            {
                dirStr = " (directory)";
                itemsStr = "directories";
            }
            else
                itemsStr = "files";
            if (files.Count == 1)
                groupBox.Text = Path.GetFileName(files[0].virtFsNode.FileName) + dirStr + ": " + sizeStr;
            else
                groupBox.Text = files.Count + " " + itemsStr + ": " + sizeStr;

            chkFileFlagDEPLOYED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DEPLOY_UPON_PRELOAD, min, max);
            chkFileFlagDEPLOYED_RAM.CheckState = getCheckedState(VIRT_FILE_FLAGS.DEPLOY_RAM_MODE, min, max);
            chkFileFlagDELETED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DELETED, min, max);
            chkFileFlagISFILE.CheckState = getCheckedState(VIRT_FILE_FLAGS.ISFILE, min, max);
            chkFileFlagPKG_FILE.CheckState = getCheckedState(VIRT_FILE_FLAGS.PKG_FILE, min, max);
            chkFileFlagDISCONNECTED.CheckState = getCheckedState(VIRT_FILE_FLAGS.DISCONNECTED, min, max);
            if (ShowDialog() == DialogResult.OK)
            {
                min = VIRT_FILE_FLAGS.NO_FLAGS;
                max = VIRT_FILE_FLAGS.ALL_FLAGS;
                if (chkFileFlagDEPLOYED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DEPLOY_UPON_PRELOAD;
                if (chkFileFlagDEPLOYED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DEPLOY_UPON_PRELOAD;
                if (chkFileFlagDEPLOYED_RAM.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DEPLOY_RAM_MODE;
                if (chkFileFlagDEPLOYED_RAM.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DEPLOY_RAM_MODE;
                if (chkFileFlagDELETED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DELETED;
                if (chkFileFlagDELETED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DELETED;
                if (chkFileFlagISFILE.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.ISFILE;
                if (chkFileFlagISFILE.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.ISFILE;
                if (chkFileFlagPKG_FILE.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.PKG_FILE;
                if (chkFileFlagPKG_FILE.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.PKG_FILE;
                if (chkFileFlagDISCONNECTED.CheckState == CheckState.Checked) min |= VIRT_FILE_FLAGS.DISCONNECTED;
                if (chkFileFlagDISCONNECTED.CheckState == CheckState.Unchecked) max &= ~VIRT_FILE_FLAGS.DISCONNECTED;

                foreach (FileData fd in files)
                {
                    VIRT_FILE_FLAGS flags = fd.virtFsNode.FileFlags;
                    flags |= min;
                    flags &= max;
                    fd.virtFsNode.FileFlags = flags;
                }
                result = true;
            }
            return result;
        }

        private CheckState getCheckedState(VIRT_FILE_FLAGS flag, VIRT_FILE_FLAGS min, VIRT_FILE_FLAGS max)
        {
            CheckState state = CheckState.Indeterminate;
            if ((min & flag) == (max & flag))
            {
                state = (min & flag) == flag ? CheckState.Checked : CheckState.Unchecked;
            }
            return state;
        }
    }
}
