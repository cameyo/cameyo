using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PackageEditor
{
    public partial class PackageBuiltNotify : Form
    {
        String realPath;
        String friendlyPath;
        String regAlwaysValueName;

        public PackageBuiltNotify()
        {
            InitializeComponent();
        }

        public void Do(String txt, String realPath, String friendlyPath, String regAlwaysValueName)
        {
            // Prepare
            RegistryKey regKey = null;
            try
            {
                label1.Text = txt;
                this.realPath = realPath;
                this.friendlyPath = friendlyPath;
                this.regAlwaysValueName = regAlwaysValueName;

                if (friendlyPath != "")
                    lblLink.Text = System.IO.Path.GetDirectoryName(friendlyPath);
                else
                {
                    lblLink.Text = "";
                    lblLink.Visible = false;
                }

                int dw = 0;
                try
                {
                    regKey = Registry.CurrentUser.OpenSubKey("Software\\Cameyo Package Editor", true);
                    dw = (int)regKey.GetValue(regAlwaysValueName, 1);
                }
                catch
                {
                }
                if (dw == 0)
                    return;
            }
            catch
            {
            }

            // Show
            ShowDialog();

            // Interpet
            if (regKey != null)
            {
                if (cbDontShowAgain.Checked)
                    regKey.SetValue(regAlwaysValueName, 0, RegistryValueKind.DWord);
                regKey.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo(System.IO.Path.GetDirectoryName(realPath));
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo = procStartInfo;
            proc.Start();
        }
    }
}
