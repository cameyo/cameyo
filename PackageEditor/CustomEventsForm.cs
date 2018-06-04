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
    public partial class CustomEventsForm : Form
    {
        private VirtPackage virtPackage;
        List<CustomEvent> onStartVirtualized;
        List<CustomEvent> onStartUnvirtualized;
        List<CustomEvent> onStopUnvirtualized;
        List<CustomEvent> onStopVirtualized;
        List<CustomEvent> onProcessStartVirtualized;
        List<CustomEvent> onProcessStartUnvirtualized;
        List<CustomEvent> onProcessStopVirtualized;
        List<CustomEvent> onProcessStopUnvirtualized;
        List<CustomEvent> curCustomEvents;
        public String oldPropertiesChecksum;
        public bool dirty;

        public CustomEventsForm(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
            onStartUnvirtualized = new List<CustomEvent>();
            onStartVirtualized = new List<CustomEvent>();
            onStopVirtualized = new List<CustomEvent>();
            onStopUnvirtualized = new List<CustomEvent>();
            onProcessStartVirtualized = new List<CustomEvent>();
            onProcessStartUnvirtualized = new List<CustomEvent>();
            onProcessStopVirtualized = new List<CustomEvent>();
            onProcessStopUnvirtualized = new List<CustomEvent>();
            InitializeComponent();
        }

        private void CustomEventsForm_Load(object sender, EventArgs e)
        {
            dirty = false;
            ActiveControl = comboBox;
            oldPropertiesChecksum = GetPropertiesChecksum();
            PropertyToCustomEvents("OnStartUnVirtualized", onStartUnvirtualized);
            PropertyToCustomEvents("OnStartVirtualized", onStartVirtualized);
            PropertyToCustomEvents("OnStopVirtualized", onStopVirtualized);
            PropertyToCustomEvents("OnStopUnvirtualized", onStopUnvirtualized);
            PropertyToCustomEvents("OnProcessStartVirtualized", onProcessStartVirtualized);
            PropertyToCustomEvents("OnProcessStartUnvirtualized", onProcessStartUnvirtualized);
            PropertyToCustomEvents("OnProcessStopVirtualized", onProcessStopVirtualized);
            PropertyToCustomEvents("OnProcessStopUnvirtualized", onProcessStopUnvirtualized);
            curCustomEvents = onStartVirtualized;
            comboBox.SelectedIndex = 0;
            RefreshDisplay();
            listBox.SelectedIndex = -1;   // Otherwise it's impossible to deselect listbox items (and hence the "Add" button can never be accessed)
        }

        private String GetPropertiesChecksum()
        {
            String checksum = "";
            String value;
            value = ""; virtPackage.GetProperty("OnStartUnvirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnStartVirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnStopVirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnStopUnvirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnProcessStartVirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnProcessStartUnvirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnProcessStopVirtualized", ref value);
            checksum += value + ";";
            value = ""; virtPackage.GetProperty("OnProcessStopUnvirtualized", ref value);
            checksum += value + ";";
            return (checksum);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CustomEventsToProperty(onStartUnvirtualized, "OnStartUnvirtualized");
            CustomEventsToProperty(onStartVirtualized, "OnStartVirtualized");
            CustomEventsToProperty(onStopVirtualized, "OnStopVirtualized");
            CustomEventsToProperty(onStopUnvirtualized, "OnStopUnvirtualized");
            CustomEventsToProperty(onProcessStartVirtualized, "OnProcessStartVirtualized");
            CustomEventsToProperty(onProcessStartUnvirtualized, "OnProcessStartUnvirtualized");
            CustomEventsToProperty(onProcessStopVirtualized, "OnProcessStopVirtualized");
            CustomEventsToProperty(onProcessStopUnvirtualized, "OnProcessStopUnvirtualized");
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
            for (int i = 0; i < curCustomEvents.Count; i++)
            {
                String cmdDisplay = curCustomEvents[i].cmd;
                if (curCustomEvents[i].args != "")
                    cmdDisplay += " [" + curCustomEvents[i].args + "]";
                listBox.Items.Add(cmdDisplay);
            }
            listBox_SelectedIndexChanged(null, null);

            // lblTotalEvents
            int totalEvents = onStartVirtualized.Count + onStartUnvirtualized.Count +
                onStopVirtualized.Count + onStopUnvirtualized.Count +
                onProcessStartVirtualized.Count + onProcessStartUnvirtualized.Count +
                onProcessStopVirtualized.Count + onProcessStopUnvirtualized.Count;
            if (totalEvents == 0)
                lblTotalEvents.Text = Messages.Messages.customEventsNone;
            else
                lblTotalEvents.Text = string.Format(Messages.Messages.customEventsCount, totalEvents);
        }

        private void btnAddSave_Click(object sender, EventArgs e)
        {
            int saveSelectedIndex = listBox.SelectedIndex;
            System.ComponentModel.ComponentResourceManager localResources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEventsForm));
            if (txtCmd.Text == "")
            {
                MessageBox.Show("Please enter a command.");
                return;
            }
            int selectedIndex = listBox.SelectedIndex;
            if (listBox.SelectedIndex == -1)
            {
                CustomEvent customEvent = new CustomEvent(
                    txtCmd.Text, txtArgs.Text, boxWait.Checked);
                curCustomEvents.Add(customEvent);
            }
            else
            {
                CustomEvent customEvent = curCustomEvents[listBox.SelectedIndex];
                customEvent.cmd = txtCmd.Text;
                customEvent.args = txtArgs.Text;
                customEvent.execWait = boxWait.Checked;
            }
            RefreshDisplay();
            listBox.SelectedIndex = -1;   // Force refresh
            listBox.SelectedIndex = saveSelectedIndex;   // Re-select edited item
            ActiveControl = comboBox;
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            if (listBox.SelectedIndex == -1)
            {
                System.ComponentModel.ComponentResourceManager localResources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEventsForm));
                btnAddSave.Text = localResources.GetString("btnAddSave.Text");
                txtCmd.Text = "";
                txtArgs.Text = "";
                boxWait.Checked = false;
                return;
            }
            btnAddSave.Text = PackageEditor.Messages.Messages.btnApply;
            CustomEvent customEvent = curCustomEvents[listBox.SelectedIndex];
            txtCmd.Text = customEvent.cmd;
            txtArgs.Text = customEvent.args;
            boxWait.Checked = customEvent.execWait;
        }

        private void btnErase_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;
            curCustomEvents.RemoveAt(listBox.SelectedIndex);
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.SelectedIndex = -1;
            RefreshDisplay();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == 0)
                return;
            curCustomEvents.Reverse(listBox.SelectedIndex - 1, 2);
            listBox.Items.Insert(listBox.SelectedIndex + 1, listBox.Items[listBox.SelectedIndex - 1]);
            listBox.Items.Remove(listBox.SelectedIndex - 1);
            int selectedIndex = listBox.SelectedIndex;
            RefreshDisplay();
            listBox.SelectedIndex = selectedIndex - 1;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1 || listBox.SelectedIndex == listBox.Items.Count - 1)
                return;
            int selectedIndex = listBox.SelectedIndex;
            curCustomEvents.Reverse(listBox.SelectedIndex, 2);
            listBox.Items.Insert(listBox.SelectedIndex, listBox.Items[listBox.SelectedIndex + 1]);
            listBox.Items.Remove(selectedIndex + 2);
            RefreshDisplay();
            listBox.SelectedIndex = selectedIndex + 1;
        }

        private void CustomEventsToProperty(List<CustomEvent> inCustomEvents, String outPropertyName)
        {
            String outPropertyValue = "";
            for (int i = 0; i < inCustomEvents.Count; i++)
            {
                if (outPropertyValue != "")
                    outPropertyValue += ";";
                outPropertyValue += inCustomEvents[i].cmd;
                outPropertyValue += ">" + inCustomEvents[i].args;
                if (inCustomEvents[i].execWait)
                    outPropertyValue += ">wait";
            }
            virtPackage.SetProperty(outPropertyName, outPropertyValue);
        }

        private void PropertyToCustomEvents(String inPropertyName, List<CustomEvent> outCustomEvents)
        {
            String inPropertyValue = "";
            virtPackage.GetProperty(inPropertyName, ref inPropertyValue);

            String[] values = inPropertyValue.Split(';');
            foreach (String value in values)
            {
                if (value == "") continue;
                CustomEvent newCustomEvent;
                newCustomEvent = new CustomEvent("", "", false);
                String[] eventElements = value.Split('>');

                // cmd
                if (eventElements.Length > 0)
                    newCustomEvent.cmd = eventElements[0];
                // args
                if (eventElements.Length > 1)
                    newCustomEvent.args = eventElements[1];
                // flags
                if (eventElements.Length > 2)
                {
                    String[] flags = eventElements[2].Split(',');
                    foreach (String flag in flags)
                    {
                        if (flag.ToLower() == "wait")
                            newCustomEvent.execWait = true;
                    }
                }
                outCustomEvents.Add(newCustomEvent);
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // On app start (unvirtualized)
            // On app start (virtualized)
            // On app stop (virtualized)
            // On app stop (unvirtualized)
            // On process execution (virtualized)
            // On process execution (unvirtualized)
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    curCustomEvents = onStartUnvirtualized;
                    break;
                case 1:
                    curCustomEvents = onStartVirtualized;
                    break;
                case 2:
                    curCustomEvents = onStopVirtualized;
                    break;
                case 3:
                    curCustomEvents = onStopUnvirtualized;
                    break;
                case 4:
                    curCustomEvents = onProcessStartVirtualized;
                    break;
                case 5:
                    curCustomEvents = onProcessStartUnvirtualized;
                    break;
                case 6:
                    curCustomEvents = onProcessStopVirtualized;
                    break;
                case 7:
                    curCustomEvents = onProcessStopUnvirtualized;
                    break;
            }
            RefreshDisplay();
            listBox.SelectedIndex = -1;   // Otherwise it's impossible to deselect listbox items (and hence the "Add" button can never be accessed)
        }
    }

    public class CustomEvent
    {
        public CustomEvent(String cmd, String args, bool execWait)
        { 
            this.cmd = cmd;
            this.args = args;
            this.execWait = execWait;
        }
        public String cmd;
        public String args;
        public bool execWait;
    }
}
