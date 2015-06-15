using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Cameyo.OpenSrc.Common;
using VirtPackageAPI;

namespace PackageEditor
{
    public partial class AutoUpdateForm : Form
    {
        private VirtPackage virtPackage;
        string txtFinish, txtNext;
        bool didXmlGeneration = false, didTesting = false, autoUpdateAlreadyConfigured = false;

        public AutoUpdateForm(VirtPackage virtPackage)
        {
            this.virtPackage = virtPackage;
            InitializeComponent();
            txtFinish = literalFinish.Text;
            txtNext = btnNext.Text;
            tbVersion.Text = virtPackage.GetProperty("Version");
            tbLocation.Text = virtPackage.GetProperty("AutoUpdate");
            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                radioDisableFeature.Checked = true;
                tbLocation.Text = @"\\server\apps\" + Path.ChangeExtension(Path.GetFileName(virtPackage.openedFile), ".xml");
            }
            else
            {
                autoUpdateAlreadyConfigured = true;
                radioEnableFeature.Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Back
            btnNext.Text = txtNext;
            tabWizard.SelectedIndex--;
            if (tabWizard.SelectedIndex == 0)
                btnBack.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Validate
            if (tabWizard.SelectedTab == tabLocation)
            {
                tbLocation.Text = tbLocation.Text.Trim();
                if (string.IsNullOrEmpty(tbLocation.Text))
                {
                    MessageBox.Show("Please enter a location.");
                    return;
                }
            }
            if (tabWizard.SelectedTab == tabGenerateXml)
            {
                tbVersionTest.Text = tbVersion.Text;
                if (!didXmlGeneration)
                {
                    if (!autoUpdateAlreadyConfigured)   // Probably already been through this wizard
                    {
                        if (MessageBox.Show("You did not generate the XML file. It needs to be uploaded along with your executable package. Would you still like to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                }
            }
            if (tabWizard.SelectedTab == tabTest)
            {
                if (!didTesting)
                {
                    if (MessageBox.Show("You did not test the settings. Would you still like to continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
            }
            
            // Act
            if (tabWizard.SelectedTab == tabEnableDisable && radioDisableFeature.Checked)
            {
                DialogResult = DialogResult.OK;   // Auto update disabled
                virtPackage.SetProperty("AutoUpdate", "");
            }
            if (tabWizard.SelectedIndex == tabWizard.TabPages.Count - 1)
            {
                // Done
                DialogResult = DialogResult.OK;
                virtPackage.SetProperty("Version", tbVersion.Text);
                virtPackage.SetProperty("AutoUpdate", tbLocation.Text);
            }

            // Next
            btnBack.Enabled = true;
            tabWizard.SelectedIndex++;
            if (tabWizard.SelectedIndex == tabWizard.TabPages.Count - 1)
                btnNext.Text = txtFinish;
        }

        private void btnGenerateXml_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Cameyo auto update XML (*.xml)|*.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FileName = Path.ChangeExtension(virtPackage.openedFile, ".xml");
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try   // catch
                {
                    XmlTextWriter xmlOut = new XmlTextWriter(saveFileDialog.FileName, Encoding.Default);
                    try   // finally
                    {
                        xmlOut.Formatting = Formatting.Indented;
                        xmlOut.WriteStartDocument();
                        xmlOut.WriteStartElement("AutoUpdate");
                        {
                            xmlOut.WriteStartElement("Pkg");
                            {
                                xmlOut.WriteAttributeString("updateCondition", "IfHigher");
                                xmlOut.WriteAttributeString("version", tbVersion.Text);
                                xmlOut.WriteAttributeString("buildUID", virtPackage.GetProperty("BuildUID"));
                            }
                            xmlOut.WriteEndElement();
                        }
                        xmlOut.WriteEndElement();
                        xmlOut.WriteEndDocument();
                        xmlOut.Flush();
                        didXmlGeneration = true;
                    }
                    finally
                    {
                        xmlOut.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating XML file: \n" + ex.Message);
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            didTesting = false;
            int exitCode = 0;
            string args = string.Format("-TestAutoUpdate " +
                "\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"",
                tbLocation.Text, tbVersionTest.Text, virtPackage.GetProperty("AppID"), virtPackage.GetProperty("BuildUID"), virtPackage.GetProperty("CloudPkgId"));
            if (Cameyo.OpenSrc.Common.Utils.ExecProg(Path.Combine(Utils.MyPath(), "Loader.exe"), args, false, ref exitCode))
            {
                if (exitCode == (int)VirtPackageAPI.VirtPackage.APIRET.SUCCESS)
                    didTesting = true;
            }
            else
                MessageBox.Show("Can't start Loader.exe");
        }
    }
}
