using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtPackageAPI;

namespace PackageEditor
{
    public partial class AutoLaunchForm : Form
    {
        private VirtPackage virtPackage;
        private FileSystemEditor fileSystemEditor;
        String autoLaunchCmd;
        String autoLaunchMenu;

        private void PropertyToLv(String property)
        {
            // propertyAutoLaunchMenuLV (from autoLaunchMenu)
            propertyMenuLV.Items.Clear();
            String[] items = autoLaunchMenu.Split(';');
            for (int i = 0; i < items.Length; i++)
            {
                String[] values = items[i].Split('>');
                if (values.Length < 2) continue;     // Not allowed
                ListViewItem lvItem = new ListViewItem();

                // Col 0: name
                if (values.Length >= 3)
                    lvItem.Text = VirtPackage.FriendlyShortcutName(values[2]);

                // Col 1: target
                if (values.Length >= 1)
                    lvItem.SubItems.Add(values[0]);
                else
                    lvItem.SubItems.Add("");

                // Col 2: description
                if (values.Length >= 4)
                    lvItem.SubItems.Add(values[3]);
                else
                    lvItem.SubItems.Add("");

                // Col 3: args
                if (values.Length >= 2)
                    lvItem.SubItems.Add(values[1]);
                else
                    lvItem.SubItems.Add("");

                // Add to list
                propertyMenuLV.Items.Add(lvItem);
            }
        }

        private String LvToProperty()
        {
            String property = "";
            for (int i = 0; i < propertyMenuLV.Items.Count; i++)
            {
                if (property != "")
                    property += ";";
                // Note: SubItems are 1-indexed!
                property += 
                    propertyMenuLV.Items[i].SubItems[1].Text + ">" +          // target
                    propertyMenuLV.Items[i].SubItems[3].Text + ">" +          // args
                    propertyMenuLV.Items[i].Text + ">" +                      // name
                    propertyMenuLV.Items[i].SubItems[2].Text;                 // description
            }
            return property;
        }

        public AutoLaunchForm(VirtPackage virtPackage, FileSystemEditor fileSystemEditor)
        {
            this.virtPackage = virtPackage;
            this.fileSystemEditor = fileSystemEditor;
            InitializeComponent();
        }

        private void FillAutoLaunchCombo(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            String[] shortcutDetails = virtPackage.GetProperty("Shortcuts").Split(';');
            foreach (String shortcutDetail in shortcutDetails)
            {
                if (shortcutDetail != "")
                {
                    // [0] = Target
                    // [1] = Args
                    // [2] = Name
                    // [3] = Description
                    String[] items = shortcutDetail.Split('>');
                    if (items.Length >= 2)
                    {
                        comboBox.Items.Add(items[0]);
                        //System.IO.Path.GetFileNameWithoutExtension(items[0]) + 
                        //SHORTCUT_DESCRIPTION_FRIENDLY_SEPARATOR + 
                    }
                }
            }
        }

        private void AutoLaunchForm_Load(object sender, EventArgs e)
        {
            String property = virtPackage.GetProperty("AutoLaunch");

            // Select Menu
            if (property.Contains(";"))
            {
                propertyMenuRadio.Checked = true;
                autoLaunchCmd = virtPackage.GetProperty("SavedAutoLaunchCmd");
                autoLaunchMenu = property;
            }
            // Select Cmd
            else
            {
                propertyCmdRadio.Checked = true;
                autoLaunchCmd = property;
                autoLaunchMenu = virtPackage.GetProperty("SavedAutoLaunchMenu");
            }
            PropertyToLv(autoLaunchMenu);
            String[] cmd = autoLaunchCmd.Split('>');
            propertyCmdText.Text = cmd[0];
            if (cmd.Length > 1)
                propertyCmdArgs.Text = cmd[1];
            //propertyCmdText.Text = autoLaunchCmd;
            FillAutoLaunchCombo(propertyCmdText);
            //if (propertyMenuLV.Items.Count > 0)
            //    propertyMenuLV.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
          AutoLaunchEditItemForm autoLaunchEditItemForm = new AutoLaunchEditItemForm(virtPackage, fileSystemEditor);
            autoLaunchEditItemForm.Text = btnAdd.Text.Replace("&", "");
            autoLaunchEditItemForm.SetValues("", "", "", "");
            if (autoLaunchEditItemForm.ShowDialog() == DialogResult.OK)
            {
                String name = "", target = "", args = "", description = "";
                autoLaunchEditItemForm.GetValues(
                    ref name,
                    ref target,
                    ref args,
                    ref description);

                // Add to list
                ListViewItem lvItem = new ListViewItem();
                lvItem.Text = name;
                lvItem.SubItems.Add(target);
                lvItem.SubItems.Add(description);
                lvItem.SubItems.Add(args);
                propertyMenuLV.Items.Add(lvItem);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (propertyMenuLV.SelectedItems.Count < 1)
                return;
            propertyMenuLV.SelectedItems[0].Remove();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (propertyMenuLV.SelectedItems.Count != 1)
                return;
            String name = propertyMenuLV.SelectedItems[0].Text;
            String target = propertyMenuLV.SelectedItems[0].SubItems[1].Text;       // Note: SubItems are 1-indexed!
            String args = propertyMenuLV.SelectedItems[0].SubItems[3].Text;         // Note: SubItems are 1-indexed!
            String description = propertyMenuLV.SelectedItems[0].SubItems[2].Text;  // Note: SubItems are 1-indexed!
            AutoLaunchEditItemForm autoLaunchEditItemForm = new AutoLaunchEditItemForm(virtPackage, fileSystemEditor);
            autoLaunchEditItemForm.Text = btnModify.Text.Replace("&", ""); ;
            autoLaunchEditItemForm.SetValues(name, target, args, description);
            if (autoLaunchEditItemForm.ShowDialog() == DialogResult.OK)
            {
                autoLaunchEditItemForm.GetValues(ref name, ref target, ref args, ref description);
                propertyMenuLV.SelectedItems[0].SubItems.Clear();
                propertyMenuLV.SelectedItems[0].Text = name;
                propertyMenuLV.SelectedItems[0].SubItems.Add(target);
                propertyMenuLV.SelectedItems[0].SubItems.Add(description);
                propertyMenuLV.SelectedItems[0].SubItems.Add(args);
            }
        }
        //btnDefault_Click:
        //if (MessageBox.Show("Restore package's defaults?", "Confirm", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
        //{
        //    PropertyToLv(virtPackage.GetProperty("Shortcuts"));
        //    ToDo: autoLaunchCmd
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Validate menu (only if menu is selected)
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            if (propertyMenuRadio.Checked && propertyMenuLV.Items.Count == 0)
            {
                MessageBox.Show(PackageEditor.Messages.Messages.menuRequiresAtLeastTwo);
                return;
            }
            if (propertyMenuRadio.Checked && propertyMenuLV.Items.Count == 1)
            {
                if (MessageBox.Show(PackageEditor.Messages.Messages.menuRequiresAtLeastTwoTransform, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            // Menu chosen
            if (propertyMenuRadio.Checked)
            {
                virtPackage.SetProperty("AutoLaunch", LvToProperty());
                virtPackage.SetProperty("SavedAutoLaunchCmd", propertyCmdText.Text + ">" + propertyCmdArgs.Text);
            }
            // Cmd chosen
            else if (propertyCmdRadio.Checked)
            {
                virtPackage.SetProperty("AutoLaunch", propertyCmdText.Text + ">" + propertyCmdArgs.Text);
                virtPackage.SetProperty("SavedAutoLaunchMenu", LvToProperty());
            }

            DialogResult = DialogResult.OK;
        }

        private void btnVirtFilesBrowse_Click(object sender, EventArgs e)
        {
          VirtFilesBrowse virtFilesBrowse = new VirtFilesBrowse(virtPackage, fileSystemEditor);
            String path = "";
            if (virtFilesBrowse.Do(ref path, false))
            {
                propertyCmdText.Text = path;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
          if (propertyMenuLV.SelectedItems.Count == 1)
          {
            int selecteditem = propertyMenuLV.SelectedItems[0].Index;
            if (selecteditem > 0)
            {
              ListViewItem lvi = propertyMenuLV.Items[selecteditem];
              propertyMenuLV.Items.RemoveAt(selecteditem);
              propertyMenuLV.Items.Insert(selecteditem - 1, lvi);
              lvi.Selected = true;
              //lvi.Focused = true;
              //propertyMenuLV.Focus();
            }
          }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
          if (propertyMenuLV.SelectedItems.Count == 1)
          {
            int selecteditem = propertyMenuLV.SelectedItems[0].Index;
            if (selecteditem < propertyMenuLV.Items.Count - 1)
            {
              ListViewItem lvi = propertyMenuLV.Items[selecteditem];
              propertyMenuLV.Items.RemoveAt(selecteditem);
              propertyMenuLV.Items.Insert(selecteditem + 1, lvi);
              lvi.Selected = true;
            }
          }
        }
    }
}
