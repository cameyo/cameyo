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
    public partial class ADPermissionsForm : Form
    {
        private VirtPackage virtPackage;
        List<ADEntity> allowedEntities;
        List<ADEntity> deniedEntities;
        List<ADEntity> curEntities = null;
        public String oldPropertiesChecksum;
        public bool dirty;

        public ADPermissionsForm(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
            allowedEntities = new List<ADEntity>();
            deniedEntities = new List<ADEntity>();
            InitializeComponent();
        }

        private void ADPermissionsForm_Load(object sender, EventArgs e)
        {
            dirty = false;
            ActiveControl = comboBox;
            oldPropertiesChecksum = GetPropertiesChecksum();

            // AuthMaxOfflineDays
            int authMaxOfflineDays = 0;
            Int32.TryParse(virtPackage.GetProperty("AuthMaxOfflineDays"), out authMaxOfflineDays);
            numOfflineUsage.Value = authMaxOfflineDays;

            // AuthDenyMsg
            cbAuthDenyMsg.Checked = (virtPackage.GetProperty("AuthDenyMsgMode") == "1");
            tbAuthDenyMsg.Text = virtPackage.GetProperty("AuthDenyMsg");
            if (string.IsNullOrEmpty(tbAuthDenyMsg.Text))
                tbAuthDenyMsg.Text = "The company's security policy does not allow you to run this application.";

            // ActiveDirectory
            PropertyToADEntities("ADAllowedGroups", allowedEntities);
            PropertyToADEntities("ADDeniedGroups", deniedEntities);
            cbRequireDomainConnection.Checked = (virtPackage.GetProperty("ADAllowedDomainsMode") == "1");
            tbRequireDomainConnection.Text = virtPackage.GetProperty("ADAllowedDomains");
            cbNestedCheck.Checked = (virtPackage.GetProperty("ADNestedCheck") == "1");

            curEntities = allowedEntities;
            comboBox.SelectedIndex = 0;
            RefreshDisplay();
        }

        private String GetPropertiesChecksum()
        {
            String checksum = "";
            String value;
            value = ""; virtPackage.GetProperty("AuthMaxOfflineDays", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("AuthDenyMsgMode", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("AuthDenyMsg", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("ADAllowedGroups", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("ADDeniedGroups", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("ADNestedCheck", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("ADAllowedDomainsMode", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("ADAllowedDomains", ref value);
            checksum += value + ";";
            return (checksum);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            virtPackage.SetProperty("AuthMaxOfflineDays", numOfflineUsage.Value.ToString());
            virtPackage.SetProperty("AuthDenyMsgMode", cbAuthDenyMsg.Checked ? "1" : "0");
            virtPackage.SetProperty("AuthDenyMsg", tbAuthDenyMsg.Text.Trim());
            ADEntitiesToProperty(allowedEntities, "ADAllowedGroups");
            ADEntitiesToProperty(deniedEntities, "ADDeniedGroups");
            virtPackage.SetProperty("ADAllowedDomainsMode", cbRequireDomainConnection.Checked ? "1" : "0");
            virtPackage.SetProperty("ADAllowedDomains", tbRequireDomainConnection.Text.Trim());
            virtPackage.SetProperty("ADNestedCheck", cbNestedCheck.Checked ? "1" : "0");

            dirty = (GetPropertiesChecksum() != oldPropertiesChecksum);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void RefreshDisplay()
        {
            listBox.Items.Clear();
            if (curEntities != null)
            {
                for (int i = 0; i < curEntities.Count; i++)
                    listBox.Items.Add(curEntities[i].name);
                listBox_SelectedIndexChanged(null, null);
            }

            // lblTotalEvents
            int totalEntities = allowedEntities.Count + deniedEntities.Count;
            if (totalEntities == 0)
                lblTotalEvents.Text = "None";
            else
                lblTotalEvents.Text = string.Format("{0} groups", totalEntities);

            bool enabled = (totalEntities > 0 || cbRequireDomainConnection.Checked);
            numOfflineUsage.Enabled = cbAuthDenyMsg.Enabled = tbAuthDenyMsg.Enabled = enabled;
        }

        private void btnAddSave_Click(object sender, EventArgs e)
        {
            int saveSelectedIndex = listBox.SelectedIndex;
            System.ComponentModel.ComponentResourceManager localResources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEventsForm));
            if (txtCmd.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a valid name");
                return;
            }
            int selectedIndex = listBox.SelectedIndex;
            if (listBox.SelectedIndex == -1)
            {
                curEntities.Add(new ADEntity(txtCmd.Text));
            }
            else
            {
                ADEntity entity = curEntities[listBox.SelectedIndex];
                entity.name = txtCmd.Text;
            }
            RefreshDisplay();
            //listBox.SelectedIndex = -1;   // Force refresh
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;
            curEntities.RemoveAt(listBox.SelectedIndex);
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.SelectedIndex = -1;
            RefreshDisplay();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            if (listBox.SelectedIndex == -1)
            {
                System.ComponentModel.ComponentResourceManager localResources = 
                    new System.ComponentModel.ComponentResourceManager(typeof(ADPermissionsForm));
                btnAddSave.Text = localResources.GetString("btnAddSave.Text");
                txtCmd.Text = "";
                return;
            }
            btnAddSave.Text = PackageEditor.Messages.Messages.btnApply;
            ADEntity entity = curEntities[listBox.SelectedIndex];
            txtCmd.Text = entity.name;
        }

        private void ADEntitiesToProperty(List<ADEntity> inEntities, String outPropertyName)
        {
            String outPropertyValue = "";
            for (int i = 0; i < inEntities.Count; i++)
            {
                if (outPropertyValue != "")
                    outPropertyValue += ";";
                outPropertyValue += inEntities[i].name;
            }
            virtPackage.SetProperty(outPropertyName, outPropertyValue);
        }

        private void PropertyToADEntities(String inPropertyName, List<ADEntity> outEntities)
        {
            String inPropertyValue = "";
            virtPackage.GetProperty(inPropertyName, ref inPropertyValue);

            String[] values = inPropertyValue.Split(';');
            foreach (String value in values)
            {
                if (value == "") continue;
                var newEntity = new ADEntity(value);
                outEntities.Add(newEntity);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    curEntities = allowedEntities;
                    break;
                case 1:
                    curEntities = deniedEntities;
                    break;
            }
            RefreshDisplay();
        }

        private void cbRequireDomainConnection_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDisplay();
        }
    }

    public class ADEntity
    {
        public ADEntity(string name)
        {
            this.name = name;
        }
        public string name;
    }
}
