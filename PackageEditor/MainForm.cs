using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Resources;
using System.Reflection;
using Microsoft.Win32;
using VirtPackageAPI;
using PackageEditor.FilesEditing;
using Delay;
using Cameyo.OpenSrc.Common;

namespace PackageEditor
{
	public partial class MainForm : Form
	{
		private VirtPackage virtPackage = null;
		private FileSystemEditor fsEditor;
		private RegistryEditor regEditor;
		private bool regLoaded;
		private Thread regLoadThread = null;
		private MRU mru;
		public bool dirty, dirtyIcon;
		private bool dragging;
		private Control[] Editors;
		private string memorizedPassword;
		string helpVirtModeDisk, helpVirtModeRam;
		string helpIsolationModeData, helpIsolationModeIsolated, helpIsolationModeFull;
		private bool isElevatedProcess = Cameyo.OpenSrc.Common.Utils.IsElevatedProcess();
		private bool generateEncKeyAlreadyWarned = false, changePwdKeyAlreadyWarned = false;

		// Creation of delegate for PackageOpen
		private delegate bool DelegatePackageOpen(String path);
		DelegatePackageOpen Del_Open;

		// Encryption constants
		const int SaltSize = 20;
		const int Pbkdf2Iterations = 1000;
		const int KeySizeBytes = 128 / 8;
		const int KeySizeBits = 128;

		void SplitTextHelp(RadioButton radio, out string helpText)
		{
			helpText = radio.Text;

			int index = helpText.IndexOf(':');
			if (index == -1)
				index = helpText.IndexOf((char)0xff1a);   // Chinese ':'
			if (index == -1)
			{
				helpText = "";
				return;
			}

			helpText = helpText.Substring(index);
			radio.Text = radio.Text.Substring(0, radio.Text.Length - helpText.Length);
			helpText = helpText.Trim(':', ' ', (char)0xff1a);
			helpText = char.ToUpper(helpText[0]) + helpText.Substring(1) + ".";
		}

		public MainForm(string packageExeFile, bool notifyPackageBuilt)
		{
			InitializeComponent();
			dragging = false;

			// helpVirtMode
			SplitTextHelp(propertyVirtModeRam, out helpVirtModeRam);
			SplitTextHelp(propertyVirtModeDisk, out helpVirtModeDisk);

			// helpIsolationMode
			SplitTextHelp(propertyIsolationDataMode, out helpIsolationModeData);
			SplitTextHelp(propertyIsolationIsolated, out helpIsolationModeIsolated);
			SplitTextHelp(propertyIsolationMerge, out helpIsolationModeFull);

			// panelWelcome
			panelWelcome.Parent = this;
			panelWelcome.BringToFront();
			panelWelcome.Dock = DockStyle.None;
			panelWelcome.Dock = DockStyle.Fill;
			tabControl.TabPages.Remove(tabWelcome);
			this.Text = CaptionText();

			// delegate for PackageOpen init
			Del_Open = new DelegatePackageOpen(this.PackageOpen);

			tabControl.Visible = false;
			panelWelcome.Visible = !tabControl.Visible;
			regLoaded = false;
			dirty = dirtyIcon = false;
			virtPackage = new VirtPackage();
			mru = new MRU("Software\\Cameyo\\Packager\\MRU");

			fsEditor = new FileSystemEditor(virtPackage, fsFolderTree, fsFilesList,
				fsFolderInfoFullName, fsFolderInfoIsolationCombo, fsAddBtn, fsRemoveBtn, fsAddEmptyDirBtn, fsSaveFileAsBtn, fsAddDirBtn);
			regEditor = new RegistryEditor(virtPackage, new RegistryEditor.DelegateRequireElevation(RequireElevation),
				regFolderTree, regFilesList, regFolderInfoFullName, regFolderInfoIsolationCombo, regRemoveBtn, regEditBtn);

			regFilesList.DoubleClickActivation = true;
			Editors = new Control[] { tbFile, tbValue, tbType, tbSize };

			EnableDisablePackageControls(false);       // No package opened yet; disable Save menu etc
			if (packageExeFile != "" && !packageExeFile.Equals("/OPEN", StringComparison.InvariantCultureIgnoreCase))
			{
				if (PackageOpen(packageExeFile) && notifyPackageBuilt)
				{
					PackageBuiltNotify packageBuiltNotify = new PackageBuiltNotify();
					String friendlyPath = packageExeFile;
					int pos = friendlyPath.ToUpper().IndexOf("\\DOCUMENTS\\");
					if (pos == -1) pos = friendlyPath.ToUpper().IndexOf("\\MY DOCUMENTS\\");
					if (pos != -1) friendlyPath = friendlyPath.Substring(pos + 1);

					System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
					packageBuiltNotify.Do(PackageEditor.Messages.Messages.packageBuiltOk, packageExeFile, friendlyPath, "PackageBuiltNotify");
				}
			}

#if DropBox
			if (!System.IO.File.Exists(Application.StartupPath + "\\AppLimit.CloudComputing.oAuth.dll")
				|| !System.IO.File.Exists(Application.StartupPath + "\\AppLimit.CloudComputing.SharpBox.dll")
				|| !System.IO.File.Exists(Application.StartupPath + "\\Newtonsoft.Json.Net20.dll"))
			{
				dropboxLabel.Hide();
				dropboxButton.Hide();
				resetCredLink.Hide();
				MessageBox.Show("This version is compiled with DropBox funtionality, but one or more of the dlls needed are missing:\nAppLimit.CloudComputing.oAuth.dll\nAppLimit.CloudComputing.SharpBox.dll\nNewtonsoft.Json.Net20.dll\n\nThe button will be hidden", "Missing DLL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
#else
			dropboxLabel.Hide();
			dropboxButton.Hide();
			//resetCredLink.Hide();
#endif
			// Display logo
			/*string branding = "";
			try { Environment.GetEnvironmentVariable("CAMEYO_RO_PROPERTY_BRANDING"); } catch { }
			propertyDisplayLogo.Visible = !string.IsNullOrEmpty(branding);   // Freeware Cameyo does not have this feature*/

			// Culture
			langToolStripMenuItem.DropDownItems.Clear();
			ToolStripMenuItem item;
			foreach (Cameyo.OpenSrc.Common.LangItem lang in Cameyo.OpenSrc.Common.LangItem.SupportedLangs())
			{
				item = new ToolStripMenuItem(lang.DisplayName) { Tag = lang.Culture };
				item.Click += langMenuItem_Click;
				langToolStripMenuItem.DropDownItems.Add(item);
			}
			CurLanguageMenuItem().Checked = true;
		}

		private void ThreadedRegLoad()
		{
			regEditor.OnPackageOpenBeforeUI();
			regLoaded = true;
			regLoadThread = null;
		}

		private void ThreadedRegLoadStop(int timeout)
		{
			regEditor.threadedRegLoadStop();
			if (regLoadThread != null)
			{
				if (timeout == -1)
					regLoadThread.Join();
				else
				{
					if (regLoadThread.IsAlive)
					{
						regLoadThread.Join(timeout);
						regLoadThread.Abort();
					}
				}
				regLoadThread = null;
			}
		}

		private void regProgressTimer_Tick(object sender, EventArgs e)
		{
			if (regLoaded)
			{
				regEditor.OnPackageOpenUI();
				regProgressBar.Visible = false;
				regToolStrip.Visible = true;
				regSplitContainer.Visible = true;
				regProgressTimer.Enabled = false;
				return;
			}
			regProgressBar.Value += 5;
			if (regProgressBar.Value >= 100)
				regProgressBar.Value = 0;
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedTab == tabGeneral)
				this.OnTabActivate();
			else if (tabControl.SelectedTab == tabFileSystem)
				fsEditor.OnTabActivate();
			else if (tabControl.SelectedTab == tabRegistry)
			{
				if (regEditor.workKey == null && (regLoadThread == null || !regLoadThread.IsAlive))
				{
					regLoadThread = new Thread(ThreadedRegLoad);   // 2:5 moved to here
					regLoadThread.Start();   // 2:5 moved to here
				}
				regEditor.OnTabActivate();
			}
		}

		static public void DisableControl(Control control)
		{
			control.Enabled = false;
			control.ForeColor = System.Drawing.SystemColors.GrayText;
			if (!control.Text.Contains("(Pro version)"))
				control.Text += " (Pro version)";
		}

		private void EnableDisablePackageControls(bool enable)
		{
			tabControl.Visible = enable;
			panelWelcome.Visible = !enable;
			exportXmlToolStripMenuItem.Enabled = enable;
			saveToolStripMenuItem.Enabled = enable;
			saveasToolStripMenuItem.Enabled = enable;
			closeToolStripMenuItem.Enabled = enable;

			// Developers, please respect copyright restrictions!
			int licenseType = VirtPackage.LicDataLoadFromFile(null);
			if (licenseType < VirtPackage.LICENSETYPE_PRO)
			{
				DisableControl(lnkAutoUpdate);
				DisableControl(propertyDisplayLogo);
			}
			if (licenseType < VirtPackage.LICENSETYPE_PRO)
			{
				DisableControl(groupEditProtect);
				//DisableControl(groupDataEncrypt);
				DisableControl(groupExpiration);
				DisableControl(groupUsageRights);
				DisableControl(lnkCustomEvents);
			}
			lblNotCommercial.Visible = (licenseType < VirtPackage.LICENSETYPE_DEV);   // "Not for commercial use"
			lnkUpgrade.Visible = (licenseType < VirtPackage.LICENSETYPE_PRO);         // "Upgrade"
		}

		private bool PackageOpen(String packageExeFile)
		{
			VirtPackage.APIRET apiRet;
			bool ret = PackageOpen(packageExeFile, true, out apiRet);
			return (ret);
		}

		private bool PackageOpen(String packageExeFile, bool displayWaitMsg, out VirtPackage.APIRET apiRet)
		{
			if (File.Exists(Path.ChangeExtension(packageExeFile, ".dat")))
				packageExeFile = Path.ChangeExtension(packageExeFile, ".dat");

		  retry:
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			bool ret = false;
			apiRet = 0;
			if (virtPackage.opened && !PackageClose())      // User doesn't want to discard changes
				return false;
			if (displayWaitMsg)
				PleaseWait.PleaseWaitBegin(PackageEditor.Messages.Messages.openingPackage, PackageEditor.Messages.Messages.opening + " " + System.IO.Path.GetFileName(packageExeFile) + "...", packageExeFile);

			System.Drawing.Icon exeIcon = null;
			try
			{
				exeIcon = Win32Function.getIconFromFile(packageExeFile);   // Must be done before PackageOpen is called
			}
			catch { }

			// virtPackage.Open
			if (!string.IsNullOrEmpty(memorizedPassword))
				ret = virtPackage.Open(packageExeFile + "|" + memorizedPassword, out apiRet);
			else   // 
				ret = virtPackage.Open(packageExeFile, out apiRet);
			if (apiRet == VirtPackage.APIRET.PASSWORD_REQUIRED || apiRet == VirtPackage.APIRET.PASSWORD_MISMATCH)
			{
				string password = "";
				while (string.IsNullOrEmpty(password))
				{
					if (displayWaitMsg)   // Hide progress window
						PleaseWait.PleaseWaitEnd();   // Otherwise it'll hide our below MessageBox
					var passwordInput = new PasswordInput();
					if (passwordInput.ShowDialog() != System.Windows.Forms.DialogResult.OK)
						return false;
					password = passwordInput.tbPassword.Text;
					ret = virtPackage.Open(packageExeFile + "|" + password, out apiRet);
					if (apiRet == VirtPackage.APIRET.SUCCESS)
						memorizedPassword = password;
					if (apiRet == VirtPackage.APIRET.PASSWORD_MISMATCH)
					{
						MessageBox.Show("Incorrect password");
						password = "";
					}
					if (displayWaitMsg)   // Restore progress window
						PleaseWait.PleaseWaitBegin(PackageEditor.Messages.Messages.openingPackage, PackageEditor.Messages.Messages.opening + " " + System.IO.Path.GetFileName(packageExeFile) + "...", packageExeFile);
				}
			}
			else if (apiRet != VirtPackage.APIRET.SUCCESS)
			{

			}

			// OLD_VERSION conversion
			if (!ret && apiRet == VirtPackage.APIRET.OLD_VERSION)
			{
				if (displayWaitMsg)   // Hide progress window
					PleaseWait.PleaseWaitEnd();     // Otherwise it'll hide our below MessageBox
				if (MessageBox.Show("This package was built with an older version and needs to be converted.\nConvert now?",
					"Conversion required", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
				{
					int exitCode = 0;
					if (displayWaitMsg)   // Restore progress window
						PleaseWait.PleaseWaitBegin(PackageEditor.Messages.Messages.openingPackage, "Converting " + System.IO.Path.GetFileName(packageExeFile) + "...", packageExeFile);

					// Init file names: old, new
					string newPkgFile = packageExeFile, oldPkgFile = packageExeFile;
					int pos = newPkgFile.LastIndexOf('.');
					newPkgFile = newPkgFile.Insert(pos, ".new");
					oldPkgFile = oldPkgFile.Insert(pos, ".old");

					// Check whether target file already exists
					bool proceed = true;
					if (File.Exists(newPkgFile) && MessageBox.Show("Warning: output file will be overwritten:\n" + newPkgFile + 
						"\n" + "Proceed?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
						proceed = false;

					if (proceed)
					{
						bool convertedOk = (ExecProg(PackagerExe(), "-Quiet -ConvertOldPkg \"" + packageExeFile + "\"", true, ref exitCode) && exitCode == 0);
						if (displayWaitMsg)   // Hide progress window
							PleaseWait.PleaseWaitEnd();   // Otherwise it'll hide our below MessageBox
						if (convertedOk)
						{
							bool trouble = false;
							try { File.Delete(oldPkgFile); }
							catch { }
							try
							{
								File.Move(packageExeFile, oldPkgFile);
							}
							catch
							{
								MessageBox.Show("Package could not be renamed to:\n" + oldPkgFile);
								trouble = true;
							}
							try
							{
								File.Move(newPkgFile, packageExeFile);
							}
							catch
							{
								MessageBox.Show("New package could not be renamed to:\n" + packageExeFile);
								trouble = true;
							}
							if (!trouble)
							{
								//ret = virtPackage.Open(packageExeFile, out apiRet);
								MessageBox.Show("Package was successfully converted. Your old package was saved in:\n" + oldPkgFile);
								goto retry;   // Takes care of password etc
							}
						}
						else
							MessageBox.Show("Error converting package! " + exitCode);
					}
				}
			}

			if (ret)
			{
				regLoaded = false;
				dirty = dirtyIcon = false;
				this.OnPackageOpen(exeIcon);
				fsEditor.OnPackageOpen();

				// regEditor (threaded)
				regProgressBar.Visible = true;
				regToolStrip.Visible = false;
				regSplitContainer.Visible = false;
				regProgressTimer.Enabled = true;

				ThreadedRegLoadStop(-1);
				//2.5:was here:regLoadThread = new Thread(ThreadedRegLoad);
				//2.5:was here:regLoadThread.Start();

				tabControl.SelectedIndex = 0;
				EnableDisablePackageControls(true);
				mru.AddFile(packageExeFile);

				ret = true;
			}
			else
				ret = false;

			if (displayWaitMsg)
				PleaseWait.PleaseWaitEnd();

			return ret;
		}

		private bool PackageClose()
		{
			return PackageClose(true);
		}

		private bool PackageClose(bool disableControls)
		{
			if (virtPackage.opened == false)
				return true;
			if (this.dirty || fsEditor.dirty || regEditor.dirty)
			{
				System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
				if (MessageBox.Show(PackageEditor.Messages.Messages.saveChanges, PackageEditor.Messages.Messages.confirm, MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					saveToolStripMenuItem_Click(this, null);
					if (this.dirty || fsEditor.dirty || regEditor.dirty)    // Still not saved
						return false;
				}
			}

			// If regLoadThread is working, wait for it to finish
			ThreadedRegLoadStop(3 * 1000);

			this.OnPackageClose();
			fsEditor.OnPackageClose();
			regEditor.OnPackageClose();
			virtPackage.Close();
			this.Text = CaptionText();
			if (disableControls)
				EnableDisablePackageControls(false);
			return true;
		}

		private bool PackageCanSave(out String message)
		{
			message = "";
			if (String.IsNullOrEmpty(propertyAppID.Text))
				message += "- AppID is a required field to save a package.\r\n";
			if (String.IsNullOrEmpty(virtPackage.GetProperty("AutoLaunch")))
				message += "- The package does not have any program(s) selected to launch.\r\nPlease select a program to launch on the tab:General > Panel:Basics > Item:Startup.";
			if (propertyProt.Checked)
			{
				if (string.IsNullOrEmpty(propertyProtPassword.Text))
					message += "- No password specified.";
				else if (propertyProtPassword.Text.Length < 4)
					message += "- Password too short.";
				else if (propertyProtPassword.Text != "[UNCHANGED]" && string.IsNullOrEmpty(tbPasswordConfirm.Text))
					message += "- Please confirm password.";
				else if (propertyProtPassword.Text != "[UNCHANGED]" && propertyProtPassword.Text != tbPasswordConfirm.Text)
					message += "- Password confirmation mismatch. Please confirm password.";
			}
			if (propertyEncryption.Checked)
			{
				if (propertyEncryptUsingPassword.Checked)
				{
					if (string.IsNullOrEmpty(tbEncryptionPwd.Text))
						message += "- No encryption password specified.";
					else if (tbEncryptionPwd.Text.Length < 4)
						message += "- Encryption password too short.";
					else if (tbEncryptionPwd.Text != tbEncryptionPwdConfirm.Text)
						message += "- Encryption passwords mismatch. Please confirm password.";
				}
				else if (propertyEncryptUsingKey.Checked)
				{
					if (string.IsNullOrEmpty(tbEncryptionKey.Text) || tbEncryptionKey.Text.Length != 16 * 2)
						message += "- Please generate an encryption key.";
				}
			}
			return message == "";
		}

		private bool PackageSave(String fileName)
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			String CantSaveBecause = "";
			if (!PackageCanSave(out CantSaveBecause))
			{
				MessageBox.Show(this, CantSaveBecause, PackageEditor.Messages.Messages.cannotSave, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}

			int ret = 0;
			VirtPackage.APIRET apiRet = 0;
			virtPackage.ResetLastError();
			PleaseWait.PleaseWaitBegin(PackageEditor.Messages.Messages.savingPackage, 
				PackageEditor.Messages.Messages.saving + " " + System.IO.Path.GetFileName(fileName) + "...", virtPackage.openedFile);
			{
				ret = ret == 0 && !this.OnPackageSave() ? 1 : ret;
				ret = ret == 0 && !fsEditor.OnPackageSave() ? 2 : ret;
				ret = ret == 0 && !regEditor.OnPackageSave() ? 3 : ret;
				ret = ret == 0 && !virtPackage.SaveEx(fileName, out apiRet) ? 4 : ret;
			}
			PleaseWait.PleaseWaitEnd();

			if (ret == 0)
			{
				dirty = dirtyIcon = false;
				fsEditor.dirty = false;
				regEditor.dirty = false;
				return true;
			}
			else
			{
				string str = "", name, description;
				VirtPackage.ApiRetStr(virtPackage.GetLastError(), out name, out description);
				if (apiRet != VirtPackage.APIRET.SUCCESS)
					str += apiRet + " ";
				if (virtPackage.GetLastError() != VirtPackage.APIRET.SUCCESS)
					str += description + " ";
				str += "\n(step #" + ret + ")";
				MessageBox.Show(str, PackageEditor.Messages.Messages.cannotSave);
				return false;
			}
		}

		private string CaptionText()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			string packageEditor = resources.GetString("$this.Text");
			if (isElevatedProcess)
				packageEditor += " (Admin)";
			if (virtPackage != null && virtPackage.opened && !string.IsNullOrEmpty(virtPackage.openedFile))
				packageEditor += " - " + virtPackage.openedFile;
			return packageEditor;
		}

		private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			String message;
			if (!PackageCanSave(out message))
			{
				MessageBox.Show(this, message, PackageEditor.Messages.Messages.cannotSave, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			if (!String.IsNullOrEmpty(virtPackage.openedFile))
			{
				saveFileDialog.InitialDirectory = Path.GetDirectoryName(virtPackage.openedFile);
				saveFileDialog.FileName = Path.GetFileName(virtPackage.openedFile);

				// cbDatFile: correct open file's name
				if (cbDatFile.Checked && Path.GetExtension(virtPackage.openedFile).Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
					saveFileDialog.FileName = Path.ChangeExtension(virtPackage.openedFile, ".dat");
				if (!cbDatFile.Checked && Path.GetExtension(virtPackage.openedFile).Equals(".dat", StringComparison.InvariantCultureIgnoreCase))
					saveFileDialog.FileName = Path.ChangeExtension(virtPackage.openedFile, ".exe");
			}
			else
			{
				saveFileDialog.FileName = (cbDatFile.Checked ? "New app.cameyo.dat" : "New app.cameyo.exe");
			}
			saveFileDialog.AddExtension = true;
			saveFileDialog.Filter = (cbDatFile.Checked ? "Virtual app (*.cameyo.dat)|*.cameyo.dat" : "Virtual app (*.cameyo.exe)|*.cameyo.exe");
			saveFileDialog.DefaultExt = (cbDatFile.Checked ? "cameyo.dat" : "cameyo.exe");
reask:
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (cbDatFile.Checked && !Path.GetExtension(saveFileDialog.FileName).Equals(".dat", StringComparison.InvariantCultureIgnoreCase))
				{
					MessageBox.Show("Must have .dat extension");
					goto reask;
				}
				if (!cbDatFile.Checked && !Path.GetExtension(saveFileDialog.FileName).Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
				{
					MessageBox.Show("Must have .exe extension");
					goto reask;
				}

				// cbDatFile: Loader.exe
				// Must be copied before PackageSave, as it will apply hPkg->IconSrcFile (if any) onto Loader.exe
				if (cbDatFile.Checked)
				{
					if (!TryCopyFile(Path.Combine(Utils.MyPath(), "Loader.exe"), Path.ChangeExtension(saveFileDialog.FileName, ".exe"), true))
					{
						MessageBox.Show("Cannot copy Loader.exe to: " + Path.ChangeExtension(saveFileDialog.FileName, ".exe"));
						return;
					}
				}

				bool _dirtyIcon = this.dirtyIcon;   // Save this before PackageSave resets it
				if (PackageSave(saveFileDialog.FileName))
				{
					// Copy icon .dat -> .exe, only if icon wasn't dirty.
					// Because if it was dirty, it means PackageSave has already taken care 
					// of applying it to the accompanying Loader.exe.
					if (!_dirtyIcon)
					{
						VirtPackage.PackUtils_CopyIconsFromExeToExe(
							Path.ChangeExtension(saveFileDialog.FileName, ".dat"),
							Path.ChangeExtension(saveFileDialog.FileName, ".exe"));
					}

					virtPackage.openedFile = saveFileDialog.FileName;
					this.Text = CaptionText();
					MessageBox.Show(PackageEditor.Messages.Messages.packageSaved);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(virtPackage.openedFile))
			{
				// This is a new package.. so save as to get a filename.
				saveasToolStripMenuItem_Click(sender, e);
				return;
			}

			// cbDatFile: correct open file's name if cbDatFile was checked/unchecked during this editing session
			string packageExeFile = virtPackage.openedFile;
			if (cbDatFile.Checked/* && Path.GetExtension(packageExeFile).Equals(".exe", StringComparison.InvariantCultureIgnoreCase)*/)
				packageExeFile = Path.ChangeExtension(packageExeFile, ".dat");
			if (!cbDatFile.Checked/* && Path.GetExtension(packageExeFile).Equals(".dat", StringComparison.InvariantCultureIgnoreCase)*/)
				packageExeFile = Path.ChangeExtension(packageExeFile, ".exe");

			string tmpFileName = packageExeFile; //.new: +".new";
			//.new:TryDeleteFile(tmpFileName);

			// cbDatFile: Loader.exe
			// Must be copied before PackageSave, as it will apply hPkg->IconSrcFile (if any) onto Loader.exe
			if (cbDatFile.Checked)
			{
				if (!TryCopyFile(Path.Combine(Utils.MyPath(), "Loader.exe"), Path.ChangeExtension(tmpFileName, ".exe"), true))
				{
					MessageBox.Show("Cannot copy Loader.exe to: " + Path.ChangeExtension(tmpFileName, ".exe"));
					return;
				}
			}

			bool _dirtyIcon = this.dirtyIcon;   // Save this before PackageSave resets it
			if (PackageSave(tmpFileName))
			{
				// Copy icon .dat -> .exe, only if icon wasn't dirty.
				// Because if it was dirty, it means PackageSave has already taken care 
				// of applying it to the accompanying Loader.exe.
				if (!_dirtyIcon)
				{
					VirtPackage.PackUtils_CopyIconsFromExeToExe(
						Path.ChangeExtension(tmpFileName, ".dat"),
						Path.ChangeExtension(tmpFileName, ".exe"));
				}

				// Release (close) original file, and delete it (otherwise it won't be erasable)
				ThreadedRegLoadStop(-1);
				PackageClose(false);
				//virtPackage.Close();

				//.new:TryDeleteFile(packageExeFile);
				//.new:bool ok = TryMoveFile(tmpFileName, packageExeFile);
				VirtPackage.APIRET apiRet;
				if (!PackageOpen(packageExeFile, false, out apiRet))
				{
					MessageBox.Show(String.Format("Failed to open package. API error: {0}", apiRet));
					closeToolStripMenuItem_Click(sender, e);
					virtPackage.opened = false;
					return;
				}
				String property = virtPackage.GetProperty("AutoLaunch");
				//virtPackage.Open(packageExeFile);
				//.new:if (ok)
					MessageBox.Show("Package saved.");
				/*.new:else
					MessageBox.Show("Cannot rename: " + tmpFileName + " to: " + packageExeFile);*/
			}
			else
			{
				// Save failed. Delete .new file.
				//.new:System.IO.File.Delete(packageExeFile + ".new");
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			//openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Cameyo apps";
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Executable files (*.exe)|*.exe|Virtual app (*.cameyo.exe;*.cameyo.dat)|*.cameyo.exe;*.cameyo.dat|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				memorizedPassword = "";
				if (Path.GetExtension(openFileDialog.FileName).Equals(".exe", StringComparison.InvariantCultureIgnoreCase) &&
					File.Exists(Path.ChangeExtension(virtPackage.openedFile, ".dat")) &&
					Utils.GetFileSize(openFileDialog.FileName) < 2 * 1024 * 1024)   // Looks like Loader.exe
				{
					openFileDialog.FileName = Path.ChangeExtension(virtPackage.openedFile, ".dat");
				}
				VirtPackage.APIRET apiRet;
				if (!PackageOpen(openFileDialog.FileName, true, out apiRet))
					MessageBox.Show(String.Format("Failed to open package. API error: {0}", apiRet));
			}
		}

		private void BlueprintFilesRecurse(XmlWriter xmlOut, TreeNodeCollection folderNodes, string outFilesDir)
		{
			foreach (FolderTreeNode curFolder in folderNodes)
			{
				if (curFolder.deleted)
					continue;
				if (curFolder.childFiles != null)
				{
					foreach (FileData file in curFolder.childFiles)
					{
						if (file.deleted)
							continue;
						xmlOut.WriteStartElement("File");
						xmlOut.WriteAttributeString("source", file.virtFsNode.FileName);
						xmlOut.WriteAttributeString("targetdir", Path.GetDirectoryName(file.virtFsNode.FileName));
						//todo: if (autoLaunchFiles >>>>> )
						//    xmlOut.WriteAttributeString("autoLaunch", autoLaunchFiles);
						xmlOut.WriteEndElement();

						// Copy actual file itself
						string targetFile = Path.Combine(outFilesDir, file.virtFsNode.FileName);
						if (!string.IsNullOrEmpty(file.addedFrom))
						{
							try
							{
								Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
								File.Copy(file.addedFrom, targetFile);
							}
							catch { }
						}
						else
						{
							virtPackage.ExtractFile(file.virtFsNode.FileName, Path.GetDirectoryName(targetFile));
						}
					}
				}
				BlueprintFilesRecurse(xmlOut, curFolder.Nodes, outFilesDir);
			}
		}

		private void BlueprintRegKeysRecurse(XmlWriter xmlOut, RegistryKey regKey, string curKeyName, string curKeyPortion)
		{
			if (!string.IsNullOrEmpty(curKeyPortion))
			{
				xmlOut.WriteStartElement("Key");
				xmlOut.WriteAttributeString("path", curKeyPortion);
			}

			// Save values
			foreach (var value in regKey.GetValueNames())
			{
				var type = regKey.GetValueKind(value);
				xmlOut.WriteStartElement("Value");
				xmlOut.WriteAttributeString("name", value);
				switch (type)
				{
					case RegistryValueKind.String:
						string str = (string)regKey.GetValue(value);
						try
						{
							if (!string.IsNullOrEmpty(str))
								XmlConvert.VerifyName(str);
							xmlOut.WriteAttributeString("string", str);
						}
						catch (XmlException)
						{
							byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
							xmlOut.WriteAttributeString("str-base64", Utils.HexDump(bytes));
						}
						break;
					case RegistryValueKind.DWord:
						xmlOut.WriteAttributeString("dword", ((Int32)regKey.GetValue(value)).ToString("X"));
						break;
					case RegistryValueKind.Binary:
						byte[] bin = (byte[])regKey.GetValue(value);
						xmlOut.WriteAttributeString("bin", Utils.HexDump(bin));
						break;
				}
				xmlOut.WriteEndElement();
			}

			// Recurse on subkeys
			foreach (var key in regKey.GetSubKeyNames())
			{
				RegistryKey subRegKey = regKey.OpenSubKey(key);
				BlueprintRegKeysRecurse(xmlOut, subRegKey, Path.Combine(curKeyName, key), key);
			}

			if (!string.IsNullOrEmpty(curKeyPortion))
				xmlOut.WriteEndElement();
		}

		private void exportXmlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int licenseType = VirtPackage.LicDataLoadFromFile(null);
			if (false && licenseType < VirtPackage.LICENSETYPE_PRO)
			{
				MessageBox.Show("Insufficient license");
				return;
			}

			// TODO: require password
			FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
			saveFileDialog.ShowNewFolderButton = true;
			/*SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.AddExtension = true;
			saveFileDialog.Filter = "Cameyo Blueprint (*.xml)|*.xml";
			saveFileDialog.DefaultExt = "xml";*/
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string appID = virtPackage.GetProperty("AppID").Trim();
				string outDir = Path.Combine(saveFileDialog.SelectedPath, appID + ".Blueprint");
				if (Directory.Exists(outDir))
				{
					if (MessageBox.Show(outDir + "\nalready exists. Continue?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
						return;
				}
				else
				{
					try
					{
						if (Directory.CreateDirectory(outDir) == null)
							throw new Exception("Cannot create directory " + outDir);
					}
					catch 
					{
						MessageBox.Show("Cannot create directory: " + outDir);
						return;
					}
				}

				// Start by exporting package's properties using -ExportAllProperties
				string packageExeFile = virtPackage.openedFile;
				string propsFile = Path.Combine(outDir, "PropsTmp.xml");
				int exitCode = -1;
				bool packagerOk = (ExecProg(PackagerExe(), String.Format("-Quiet \"-ExportAllProperties:{0}\" \"{1}\"",
					propsFile, packageExeFile), true, ref exitCode) && exitCode == 0);
				if (!packagerOk)
				{
					MessageBox.Show("ExportAllProperties failed: " + exitCode.ToString());
					return;
				}

				// Complete XML with Files and Registry
				var writerSettings = new XmlWriterSettings();
				writerSettings.OmitXmlDeclaration = true;
				writerSettings.Indent = true;
				writerSettings.Encoding = Encoding.Unicode;
				using (XmlWriter xmlOut = XmlWriter.Create(Path.Combine(outDir, appID + ".xml"), writerSettings))
				{
					string outFilesDir = outDir;
					xmlOut.WriteStartDocument();
					xmlOut.WriteStartElement("CameyoPkg");

					// Read existing XML Properties/Property nodes
					xmlOut.WriteStartElement("Properties");
					{
						var xd = new System.Xml.XPath.XPathDocument(propsFile);
						var navigator = xd.CreateNavigator();
						var iterator = navigator.Select("//Properties/Property");
						foreach (System.Xml.XPath.XPathNavigator n in iterator)
						{
							n.MoveToFirstAttribute();
							var name = n.Name;
							var val = n.Value;
							xmlOut.WriteStartElement("Property");
							xmlOut.WriteAttributeString(name, val);
							xmlOut.WriteEndElement();
						}
					}
					xmlOut.WriteEndElement();
					/*var xmlReader = XmlReader.Create(propsFile);
					try
					{
						if (xmlReader.ReadToDescendant("Properties"))
						{
							while (xmlReader.Read())
							{
								var str = xmlReader.Name;
								xmlOut.WriteNode(xmlReader, true);
							}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error parsing exported properties' XML: " + ex.ToString());
						return;
					}

					// Properties
					string autoLaunch = virtPackage.GetProperty("AutoLaunch");
					xmlOut.WriteStartElement("Properties");
					{
						xmlOut.WriteStartElement("Property");
						xmlOut.WriteAttributeString("AppID", virtPackage.GetProperty("AppID"));
						xmlOut.WriteEndElement();
						xmlOut.WriteStartElement("Property");
						xmlOut.WriteAttributeString("Version", virtPackage.GetProperty("Version"));
						xmlOut.WriteEndElement();
						xmlOut.WriteStartElement("Property");
						xmlOut.WriteAttributeString("BaseDirName", virtPackage.GetProperty("BaseDirName"));
						xmlOut.WriteEndElement();
						//xmlOut.WriteStartElement("Property");
						//xmlOut.WriteAttributeString("IconFile", "Icon.exe");
						//xmlOut.WriteEndElement();
						xmlOut.WriteStartElement("Property");
						xmlOut.WriteAttributeString("StopInheritance", virtPackage.GetProperty("StopInheritance"));
						xmlOut.WriteEndElement();
						xmlOut.WriteStartElement("Property");
						xmlOut.WriteAttributeString("BuildOutput", "[AppID].exe");
						xmlOut.WriteEndElement();
						// AutoLaunch
						if (!string.IsNullOrEmpty(autoLaunch))
						{
							xmlOut.WriteStartElement("Property");
							xmlOut.WriteAttributeString("AutoLaunch", autoLaunch);
							xmlOut.WriteEndElement();
						}
					}
					xmlOut.WriteEndElement();*/

					// AutoLaunch property -> autoLaunchFiles list
					var autoLaunchFiles = new List<string>();
					/*todo:string[] autoLaunches = autoLaunch.Split(';');
					foreach (string item in autoLaunches)
					{
						string[] elements = item.Split('>');
						if (elements.Length > 0)
							autoLaunchFiles.Add(elements[0]);
					}*/

					// FileSystem
					xmlOut.WriteStartElement("FileSystem");
					if (fsFolderTree.Nodes.Count != 0 && fsFolderTree.Nodes[0].Nodes.Count != 0)
					{
						BlueprintFilesRecurse(xmlOut, fsFolderTree.Nodes[0].Nodes, outFilesDir);
					}
					xmlOut.WriteEndElement();

					// Registry
					xmlOut.WriteStartElement("Registry");
					if (regEditor.workKey == null)
						ThreadedRegLoad();   // If registry isn't loaded yet, load it now (synchronously)
					if (regEditor.workKey != null)
						BlueprintRegKeysRecurse(xmlOut, regEditor.workKey, "", "");
					xmlOut.WriteEndElement();

					// Sandbox
					/*xmlOut.WriteStartElement("Sandbox");
					{
						xmlOut.WriteStartElement("FileSystem");
						xmlOut.WriteAttributeString("access", "Full");
						xmlOut.WriteEndElement();

						xmlOut.WriteStartElement("Registry");
						xmlOut.WriteAttributeString("access", "Full");
						xmlOut.WriteEndElement();
					}
					xmlOut.WriteEndElement();*/

					xmlOut.WriteEndElement();
					xmlOut.WriteEndDocument();
					xmlOut.Flush();
					xmlOut.Close();

					try { File.Delete(propsFile); } catch { }
					MessageBox.Show("Created in:\n" + outDir);
				}
			}
		}

		private void DisplayAutoLaunch()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			if (virtPackage.GetProperty("AutoLaunch").Contains(";"))
			{
				String[] autoLaunches = virtPackage.GetProperty("AutoLaunch").Split(';');
				propertyAutoLaunch.Text = "";
				propertyAutoLaunch.AutoEllipsis = true;
				for (int i = 0; i < autoLaunches.Length; i++)
				{
					String[] items = autoLaunches[i].Split('>');
					if (items.Length < 3) continue;     // No Name
					if (propertyAutoLaunch.Text != "")
						propertyAutoLaunch.Text += ", ";
					propertyAutoLaunch.Text += VirtPackage.FriendlyShortcutName(items[2]);
				}
				propertyAutoLaunch.Text = PackageEditor.Messages.Messages.displayMenu + ": " + propertyAutoLaunch.Text;
			}
			else
			{
				String[] items = virtPackage.GetProperty("AutoLaunch").Split('>');
				if (items.Length >= 3)
					propertyAutoLaunch.Text = items[0] + " (" + VirtPackage.FriendlyShortcutName(items[2]) + ")";
				else
					propertyAutoLaunch.Text = items[0];
			}
		}

		private void DisplayIsolationMode()
		{
			// Isolation mode
			int isolationType = virtPackage.GetIsolationMode();

			propertyIsolationDataMode.CheckedChanged -= propertyIsolationMode_CheckedChanged;
			propertyIsolationIsolated.CheckedChanged -= propertyIsolationMode_CheckedChanged;
			propertyIsolationMerge.CheckedChanged -= propertyIsolationMode_CheckedChanged;
			{
				propertyIsolationDataMode.Checked = propertyIsolationIsolated.Checked = propertyIsolationMerge.Checked = false;
				propertyIsolationDataMode.Checked = (isolationType == VirtPackage.ISOLATIONMODE_DATA);
				propertyIsolationIsolated.Checked = (isolationType == VirtPackage.ISOLATIONMODE_ISOLATED);
				propertyIsolationMerge.Checked = (isolationType == VirtPackage.ISOLATIONMODE_FULL_ACCESS);
			}
			propertyIsolationDataMode.CheckedChanged += propertyIsolationMode_CheckedChanged;
			propertyIsolationIsolated.CheckedChanged += propertyIsolationMode_CheckedChanged;
			propertyIsolationMerge.CheckedChanged += propertyIsolationMode_CheckedChanged;

			//if (propertyIsolationDataMode.Checked)
			//    virtPackage.SetProperty("DataMode", "TRUE");   // Important to be able to switch to Isolated mode (to unisolate %Personal% etc)

			propertyIsolationMode_CheckedChanged(null, null);
			propertyEncryption_CheckedChanged(null, null);
		}

		private void OnPackageOpen(Icon exeIcon)
		{
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			String command;

			// AppID
			propertyAppID.Text = virtPackage.GetProperty("AppID");
			//propertyAppID.TextChanged += PropertyChange;

			// FriendlyName, Version
			propertyFriendlyName.Text = virtPackage.GetProperty("FriendlyName");
			propertyFileVersion.Text = virtPackage.GetProperty("Version");
			//propertyFriendlyName.TextChanged += PropertyChange;

			// AutoLaunch
			//FillAutoLaunchCombo(propertyAutoLaunch);
			DisplayAutoLaunch();

			// BaseDirName
			DisplayBaseDirName();

			// VirtMode
			if (virtPackage.GetProperty("VirtMode").Equals("RAM", StringComparison.InvariantCultureIgnoreCase))
				propertyVirtModeRam.Checked = true;
			else
				propertyVirtModeDisk.Checked = true;
			propertyVirtMode_CheckedChanged(this, null);

			// Isolation
			DisplayIsolationMode();

			// Icon
			if (exeIcon != null)
				propertyIcon.Image = exeIcon.ToBitmap();
			else
				propertyIcon.Image = null;

			// DataDirName
			//propertyDataDirName.Text = virtPackage.GetProperty("DataDirName");

			// StopInheritance
			propertyStopInheritance.Text = virtPackage.GetProperty("StopInheritance");

			// VolatileRegistry
			cbVolatileRegistry.Checked = 
				!virtPackage.GetProperty("RegMode").Equals("Extract", StringComparison.InvariantCultureIgnoreCase) &&
				!virtPackage.GetProperty("LegacyReg").Equals("1", StringComparison.InvariantCultureIgnoreCase);

			// Integrate
			command = GetIntegrateStartCommand();
			if (command == "")
				rdbIntegrateNone.Checked = true;
			else
			{
				if (command.EndsWith("-Integrate:once"))
					rdbIntegrateStandard.Checked = true;
				else if (command.EndsWith("-Vintegrate"))
					rdbIntegrateVirtual.Checked = true;
			}

			// CleanupOnExit
			command = GetCleanupStopCommand();
			if (command == "")
				rdbCleanNone.Checked = true;
			else
			{
				chkCleanAsk.Checked = command.Contains("-Confirm");
				chkCleanDoneDialog.Checked = !command.Contains("-Quiet");
				if (command.EndsWith("-Remove:Reg"))
					rdbCleanRegOnly.Checked = true;
				else
					rdbCleanAll.Checked = true;
			}

			// DatFile 
			cbDatFile.Checked = Path.GetExtension(virtPackage.openedFile).Equals(".dat", StringComparison.InvariantCultureIgnoreCase);

			// Expiration
			String expiration = virtPackage.GetProperty("Expiration");
			propertyExpiration.Checked = !String.IsNullOrEmpty(expiration);
			propertyExpirationDatePicker.Value = DateTime.Now;
			if (propertyExpiration.Checked)
			{
				String[] expirationItems = expiration.Split('/');
				if (expirationItems.Length == 3)
				{
					int day = Convert.ToInt32(expirationItems[0]);
					int month = Convert.ToInt32(expirationItems[1]);
					int year = Convert.ToInt32(expirationItems[2]);
					DateTime dt = new DateTime(year, month, day);
					propertyExpirationDatePicker.Value = dt;
				}
			}

			// TTL
			String ttlDays = virtPackage.GetProperty("TtlDays");
			int ttlDaysVal;
			try { ttlDaysVal = Convert.ToInt32(ttlDays); }
			catch { ttlDaysVal = 0; }
			propertyTtlDays.Checked = !String.IsNullOrEmpty(ttlDays);
			propertyTtlDaysValue.Value = ttlDaysVal;
			propertyTtlResistRemove.Checked = virtPackage.GetProperty("TtlResistRemove") == "1";

			// Package protection
			propertyProt.Checked = !string.IsNullOrEmpty(virtPackage.GetProperty("PkgProtActions")) && virtPackage.GetProperty("PkgProtActions") != "0";
			if (string.IsNullOrEmpty(virtPackage.GetProperty("PkgProtPassword")))
				propertyProtPassword.Text = "";
			else
				propertyProtPassword.Text = "[UNCHANGED]";
			tbPasswordConfirm.Text = propertyProtPassword.Text;

			// ScmDirect (direct-registration services support)
			propertyScmDirect.Visible = virtPackage.GetProperty("NewServices") == "1";
			propertyScmDirect.Checked = virtPackage.GetProperty("Services").Equals("Direct", StringComparison.InvariantCultureIgnoreCase) &&
				virtPackage.GetProperty("RegMode").Equals("Extract", StringComparison.InvariantCultureIgnoreCase);

			// DisplayLogo
			propertyDisplayLogo.Checked = string.IsNullOrEmpty(virtPackage.GetProperty("Branding"));

			// Encryption
			propertyEncryption.Checked = (!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionExts")));
			if (virtPackage.GetProperty("EncryptionMethod").Equals("Pwd", StringComparison.InvariantCultureIgnoreCase))
				propertyEncryptUsingPassword.Checked = true;
			else if (virtPackage.GetProperty("EncryptionMethod").Equals("Key", StringComparison.InvariantCultureIgnoreCase))
				propertyEncryptUsingKey.Checked = true;
			else if (virtPackage.GetProperty("EncryptionMethod").Equals("CreatePwd", StringComparison.InvariantCultureIgnoreCase))
				propertyEncryptUserCreatedPassword.Checked = true;
			tbEncryptionKey.Text = "";
			tbEncryptionPwd.Text = "";
			tbEncryptionPwdConfirm.Text = "";
			
			// EncryptionPwdHash: SHA2 hash of the real password
			if (virtPackage.GetProperty("EncryptionPwdHash").Length == (KeySizeBytes) * 2)   // SHA2 hash (32 bytes) encoded in HEX form
			{
				tbEncryptionPwd.Text = tbEncryptionPwdConfirm.Text = "[UNCHANGED]"; //virtPackage.GetProperty("EncryptionPwdHash");
			}

			// EncryptionKey: directly the key (in HEX format)
			if (virtPackage.GetProperty("EncryptionKey").Length == KeySizeBytes * 2)
				tbEncryptionKey.Text = virtPackage.GetProperty("EncryptionKey");

			// Open
			this.Text = CaptionText();
			dirty = dirtyIcon = false;
		}

		private void RemoveIfStartswith(ref String str, String value)
		{
			String newStr = str.TrimStart(' ');
			if (newStr.StartsWith(value))
				str = newStr.Remove(0, value.Length);
		}

		private string GetIntegrateStartCommand()
		{
			String[] OnStartUnvirtualized = virtPackage.GetProperty("OnStartUnvirtualized").Split(';');
			foreach (String command in OnStartUnvirtualized)
			{
				if (command.StartsWith("%MyExe%" + '>') && (command.EndsWith("-Integrate:once") || command.EndsWith("-Vintegrate")))
				{
					String checkNoRemains = command;
					RemoveIfStartswith(ref checkNoRemains, "%MyExe%");
					RemoveIfStartswith(ref checkNoRemains, ">");
					RemoveIfStartswith(ref checkNoRemains, "-Quiet");

					RemoveIfStartswith(ref checkNoRemains, "-Integrate:once");
					RemoveIfStartswith(ref checkNoRemains, "-Vintegrate");
					if (checkNoRemains == "")
						return command;
				}
			}
			return "";
		}

		private string GetCleanupStopCommand()
		{
			String[] OnStopUnvirtualized = virtPackage.GetProperty("OnStopUnvirtualized").Split(';');
			foreach (String command in OnStopUnvirtualized)
			{
				if (command.StartsWith("%MyExe%" + '>') && (command.EndsWith("-Remove") || command.EndsWith("-Remove:Reg")))
				{
					String checkNoRemains = command;
					RemoveIfStartswith(ref checkNoRemains, "%MyExe%");
					RemoveIfStartswith(ref checkNoRemains, ">");
					RemoveIfStartswith(ref checkNoRemains, "-Confirm");
					RemoveIfStartswith(ref checkNoRemains, "-Quiet");

					RemoveIfStartswith(ref checkNoRemains, "-Remove:Reg");
					RemoveIfStartswith(ref checkNoRemains, "-Remove");
					if (checkNoRemains == "")
						return command;
				}
			}
			return "";
		}

		public void OnTabActivate()
		{
			DisplayIsolationMode();
		}

		bool ProcessEncryptionPwd()
		{
			System.Security.Cryptography.Rfc2898DeriveBytes pbkdf2 = null;
			if (tbEncryptionPwd.Text == "[UNCHANGED]")   // Password hasn't changed from when the package was loaded
				return true;
			if (string.IsNullOrEmpty(tbEncryptionPwd.Text))
				return true;   // Nothing to do really
			if (!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionPwdKey")) &&
				!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionPwdHash")) &&
				!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionPwdSalt")) &&
				!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionPwdIV")))
			{
				// EncryptionPwd* properties are already set. Only need to re-generate them if password has changed (different from EncryptionPwdHash)
				string pwdSaltStr = virtPackage.GetProperty("EncryptionPwdSalt");
				var pwdSalt = Utils.HexUndump(pwdSaltStr);

				// Check if password is unchanged
				// PBKDF2 first time on entered password
				pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(tbEncryptionPwd.Text, pwdSalt, Pbkdf2Iterations);
				var enteredPwdHash = pbkdf2.GetBytes(KeySizeBytes);
				// PBKDF2 second time on entered password
				pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(enteredPwdHash, pwdSalt, Pbkdf2Iterations);
				enteredPwdHash = pbkdf2.GetBytes(KeySizeBytes);

				var savedPwdHash = Utils.HexUndump(virtPackage.GetProperty("EncryptionPwdHash"));
				bool equals = true;
				for (int i = 0; i < enteredPwdHash.Length; i++)
				{
					if (enteredPwdHash[i] != savedPwdHash[i])
						equals = false;
				}
				if (equals)
					return true;   // Password hasn't changed
			}

			bool Ret = true;
			var rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider();

			// Steps for password-based key:
			// 1. Generate a long key (this is just a randomly generated amount of bytes in hex, you can use 128 bytes).
			//    Use this key to encrypt your file with AES (meaning you use it as a password)
			byte[] randomKey = new byte[KeySizeBytes];   // Our AES algorithm uses a 128-bit key (16 bytes)
			rngCsp.GetBytes(randomKey);   // <-- The key with which files will be encrypted / decrypted

			// 2. Encrypt the key itself with AES using your password (which is a bit shorter but easier to remember. 
			//    Now AES requires this again to be either 128, 192 or 256 bits of length so you need to make your 
			//    password of that length. Hence you can just use PBKDF2 (or scrypt or bcrypt) to create a fixed key 
			//    length from your password. DO NOT STORE THIS.
			byte[] salt = new byte[SaltSize];
			rngCsp.GetBytes(salt);

			// Transform user password into a key that we can encrypt randomKey with
			pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(tbEncryptionPwd.Text), salt, Pbkdf2Iterations);
			var keyEncryptionKey = pbkdf2.GetBytes(KeySizeBytes);   // tbEncryptionPwd.Text -> Key. This key will be used for encrypting randomKey.
			var keyEncryptor = new System.Security.Cryptography.RijndaelManaged();
			keyEncryptor.BlockSize = 128;
			keyEncryptor.Mode = System.Security.Cryptography.CipherMode.CFB;
			keyEncryptor.Padding = System.Security.Cryptography.PaddingMode.None;
			keyEncryptor.Key = keyEncryptionKey;
			keyEncryptor.GenerateIV();
			var encryptor = keyEncryptor.CreateEncryptor(keyEncryptor.Key, keyEncryptor.IV);
			var msEncrypt = new MemoryStream();
			using (var csEncrypt = new System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
			{
				using (var swEncrypt = new BinaryWriter(csEncrypt))
				{
					byte[] toWrite = randomKey;//Encoding.ASCII.GetBytes("1234567890123456");
					swEncrypt.Write(toWrite);
				}
			}
			var encryptedKey = msEncrypt.ToArray();   // <-- randomKey encrypted with AES, whose key = PBKDF2(tbEncryptionPwd.Text)

			// 3. Keep a hash of your hashed password using PBKDF2 (or bcrypt or scrypt). This will allow you to check 
			//    if the password is correct before trying to decrypt your encrypted key:
			//       hash(hash(password)) --> can be stored
			//       hash(password) --> should not be stored
			pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(keyEncryptionKey, salt, Pbkdf2Iterations);   // Iterations slow down hashing (which helps against brute-force)
			var keyEncryptionKeyHashed = pbkdf2.GetBytes(KeySizeBytes);                                       // Second PBKDF2 hash

			// Store
			Ret &= virtPackage.SetProperty("EncryptionPwdKey", Utils.HexDump(encryptedKey));             // Encryption key, in encrypted form
			Ret &= virtPackage.SetProperty("EncryptionPwdHash", Utils.HexDump(keyEncryptionKeyHashed));  // Password hash, for user password validation
			Ret &= virtPackage.SetProperty("EncryptionPwdSalt", Utils.HexDump(salt));                    // Salt used for both PBKDF2 password hashes (first and second)
			Ret &= virtPackage.SetProperty("EncryptionPwdIV", Utils.HexDump(keyEncryptor.IV));           // IV used for encrypting the key with AES

			return Ret;
		}

		public bool OnPackageSave()
		{
			bool Ret = true;
			String str, oldCommand, newCommand;

			// AppID + AutoLaunch
			Ret &= virtPackage.SetProperty("AppID", propertyAppID.Text);
			Ret &= virtPackage.SetProperty("FriendlyName", propertyFriendlyName.Text);
			Ret &= virtPackage.SetProperty("Version", propertyFileVersion.Text);
			//Ret &= virtPackage.SetProperty("DataDirName", propertyDataDirName.Text);
			Ret &= virtPackage.SetProperty("StopInheritance", propertyStopInheritance.Text);
			Ret &= virtPackage.SetProperty("RegMode", cbVolatileRegistry.Checked ? "" : "Extract");
			virtPackage.SetProperty("LegacyReg", "");   // Deprecated
			if (propertyExpiration.Checked)
				Ret &= virtPackage.SetProperty("Expiration", propertyExpirationDatePicker.Value.ToString("dd/MM/yyyy"));
			else
				Ret &= virtPackage.SetProperty("Expiration", "");
			if (propertyTtlDays.Checked)
				Ret &= virtPackage.SetProperty("TtlDays", propertyTtlDaysValue.Value.ToString());
			else
				Ret &= virtPackage.SetProperty("TtlDays", "");
			Ret &= virtPackage.SetProperty("TtlResistRemove", propertyTtlResistRemove.Checked ? "1" : "0");
			if (propertyVirtModeRam.Checked)
				Ret &= virtPackage.SetProperty("VirtMode", "RAM");
			else
				Ret &= virtPackage.SetProperty("VirtMode", "DISK");

			// Package protection
			Ret &= virtPackage.SetProtection(propertyProtPassword.Text, (propertyProt.Checked ? 3 : 0), null);
			if (!string.IsNullOrEmpty(propertyProtPassword.Text) && propertyProtPassword.Text != "[UNCHANGED]")
				memorizedPassword = propertyProtPassword.Text;

			// ScmDirect (direct-registration services support)
			if (propertyScmDirect.Checked)
				Ret &= virtPackage.SetProperty("Services", "Direct") && virtPackage.SetProperty("RegMode", "Extract");
			// Conflicts with cbVolatileRegistry:
			//else
			//    Ret &= virtPackage.SetProperty("Services", "") && virtPackage.SetProperty("RegMode", "");

			// DisplayLogo
			Ret &= propertyDisplayLogo.Checked ? virtPackage.SetProperty("Branding", "") : virtPackage.SetProperty("Branding", "None");

			// Encryption
			Ret &= virtPackage.SetProperty("EncryptionExts", propertyEncryption.Checked ? "*" : "");

			// EncryptionMethod
			if (propertyEncryptUsingKey.Checked)
				Ret &= virtPackage.SetProperty("EncryptionMethod", "Key");
			else if (propertyEncryptUsingPassword.Checked)
				Ret &= virtPackage.SetProperty("EncryptionMethod", "Pwd");
			else if (propertyEncryptUserCreatedPassword.Checked)
				Ret &= virtPackage.SetProperty("EncryptionMethod", "CreatePwd");
			else
				Ret &= virtPackage.SetProperty("EncryptionMethod", "");

			// EncryptionKey
			if (tbEncryptionKey.Text.Length == 16 * 2)
				Ret &= virtPackage.SetProperty("EncryptionKey", tbEncryptionKey.Text);

			// EncryptionPwdHash
			if (tbEncryptionPwd.Text.Length == 0)
				Ret &= virtPackage.SetProperty("EncryptionPwdHash", "");
			else 
			{
				// Only if password was changed from when the package was loaded
				Ret &= ProcessEncryptionPwd();
			}

			// propertyIntegrate, propertyVintegrate
			str = virtPackage.GetProperty("OnStartUnvirtualized");
			oldCommand = GetIntegrateStartCommand();
			newCommand = "";
			if (!rdbIntegrateNone.Checked)
			{
				newCommand += ' ' + "-Quiet";
				if (rdbIntegrateStandard.Checked)
					newCommand += ' ' + "-Integrate:once";
				if (rdbIntegrateVirtual.Checked)
					newCommand += ' ' + "-Vintegrate";

				newCommand = "%MyExe%" + '>' + newCommand.Trim();
			}
			if (string.IsNullOrEmpty(oldCommand) && !string.IsNullOrEmpty(newCommand))
				str += ";" + newCommand;
			else
			{
				str = ";" + str + ";";
				str = str.Replace(";" + oldCommand + ";", ";" + newCommand + ";");
				str = str.Replace(";;", ";");
				str = str.Trim(';');
			}
			Ret &= virtPackage.SetProperty("OnStartUnvirtualized", str);

			// propertyCleanupOnExit
			str = virtPackage.GetProperty("OnStopUnvirtualized");
			oldCommand = GetCleanupStopCommand();
			newCommand = "";
			if (!rdbCleanNone.Checked)
			{
				if (chkCleanAsk.Checked)
					newCommand += ' ' + "-Confirm";
				if (!chkCleanDoneDialog.Checked)
					newCommand += ' ' + "-Quiet";

				if (rdbCleanRegOnly.Checked)
					newCommand += ' ' + "-Remove:Reg";
				if (rdbCleanAll.Checked)
					newCommand += ' ' + "-Remove";

				newCommand = "%MyExe%" + '>' + newCommand.Trim();
			}
			if (string.IsNullOrEmpty(oldCommand) && !string.IsNullOrEmpty(newCommand))
				str += ";" + newCommand;
			else
			{
				str = ";" + str + ";";
				str = str.Replace(";" + oldCommand + ";", ";" + newCommand + ";");
				str = str.Replace(";;", ";");
				str = str.Trim(';');
			}
			Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
			/*
			if (propertyCleanupOnExit.Checked)
			{
				if (!str.Contains(CleanupOnExitCmd))
				{
					if (str != "")
						str += ";";
					str += CleanupOnExitCmd;
					Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
				}
			}
			else
			{
				if (str.Contains(CleanupOnExitCmd))
				{
					str = str.Replace(CleanupOnExitCmd, "");
					str = str.Replace(";;", ";");
					str = str.Trim(';');
					Ret &= virtPackage.SetProperty("OnStopUnvirtualized", str);
				}
			}*/

			// AutoLaunch (and SaveAutoLaunchCmd + SaveAutoLaunchMenu) already set by AutoLaunchForm

			// BaseDirName & DataDirName already set by SetProperty

			// Isolation. Note: it is allowed to have no checkbox selected at all.
			virtPackage.SetIsolationMode(
				propertyIsolationIsolated.Checked ? VirtPackage.ISOLATIONMODE_ISOLATED :
				propertyIsolationMerge.Checked ? VirtPackage.ISOLATIONMODE_FULL_ACCESS :
				propertyIsolationDataMode.Checked ? VirtPackage.ISOLATIONMODE_DATA :
				VirtPackage.ISOLATIONMODE_CUSTOM);

			// SetIcon already set when icon button is pressed

			return (Ret);
		}

		private void OnPackageClose()
		{
			//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

			propertyAppID.Text = "";
			propertyFriendlyName.Text = "";
			propertyFileVersion.Text = "";
			propertyAutoLaunch.Text = "";
			propertyIcon.Image = null;
			//propertyDataDirName.Text = "";
			propertyStopInheritance.Text = "";
			//propertyCleanupOnExit.Checked = false;
		}

		private bool TryDeleteFile(String FileName)
		{
			bool ret = true;
			try
			{
				System.IO.File.Delete(FileName);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}

		private bool TryMoveFile(String srcFileName, String destFileName)
		{
			bool ret = true;
			try
			{
				System.IO.File.Move(srcFileName, destFileName);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}

		private bool TryCopyFile(String srcFileName, String destFileName, bool overwrite)
		{
			bool ret = true;
			try
			{
				System.IO.File.Copy(srcFileName, destFileName, overwrite);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PackageClose();
			memorizedPassword = "";   // Must be after PackageClose; otherwise PackageClose may call PackageSave, which may memorize password
			mru = new MRU("Software\\Cameyo\\Packager\\MRU");
			DisplayMRU();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PackageClose())
				this.Close();
		}

		private void DisplayBaseDirName()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataStorageForm));
			string propertyLocalStorageDefault = resources.GetString("propertyLocalStorageDefault.Text");
			string propertyLocalStorageExeDir = resources.GetString("propertyLocalStorageExeDir.Text");
			string propertyLocalStorageCustom = resources.GetString("propertyLocalStorageCustom.Text");

			string baseDirName = virtPackage.GetProperty("BaseDirName");
			if (baseDirName == "")
				propertyDataStorage.Text = propertyLocalStorageDefault;   // Use hard disk or USB drive (wherever application is launched from)
			else if (baseDirName.Equals("%ExeDir%\\%AppID%.cameyo.files", StringComparison.InvariantCultureIgnoreCase))
				propertyDataStorage.Text = propertyLocalStorageExeDir;   // "Under the executable's directory"
			else
				propertyDataStorage.Text = propertyLocalStorageCustom + " " + baseDirName;
		}

		private void PropertyChange(object sender, EventArgs e)
		{
			dirty = true;
		}

		private void IsolationChanged(object sender, EventArgs e)
		{
			dirty = true;
		}

		private void lnkChangeDataStorage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			DataStorageForm dataStorageForm = new DataStorageForm();
			if (dataStorageForm.Do(virtPackage, ref dirty))
				DisplayBaseDirName();
		}

		private void lnkChangeIcon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Executable files (*.exe)|*.exe|All file types|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (openFileDialog.FileName.EndsWith(".ico", StringComparison.InvariantCultureIgnoreCase))
					MessageBox.Show("Please select an executable file (.EXE) to copy the icon from.");
				else
				{
					Icon ico = Win32Function.getIconFromFile(openFileDialog.FileName);
					if (virtPackage.SetIcon(openFileDialog.FileName))
					{
						propertyIcon.Image = ico.ToBitmap();
						//propertyNewIconFileName.Text = openFileDialog.FileName;
						dirtyIcon = true;
						dirty = true;
					}
					else
						MessageBox.Show("Error: file not found");
				}
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			ThreadedRegLoadStop(3 * 1000);
		}

		private void lnkAutoLaunch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AutoLaunchForm autoLaunchForm = new AutoLaunchForm(virtPackage, fsEditor);
			String oldValue = virtPackage.GetProperty("AutoLaunch");
			if (autoLaunchForm.ShowDialog() == DialogResult.OK)
			{
				DisplayAutoLaunch();
				if (virtPackage.GetProperty("AutoLaunch") != oldValue)
					dirty = true;
			}
		}

		// dragdrop function (DragEnter) to open a new file dropping it in the main form
		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
				dragging = true;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		static public bool ExecProg(String fileName, String args, bool wait, ref int exitCode)
		{
			try
			{
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo(fileName, args);
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				procStartInfo.CreateNoWindow = true;
				procStartInfo.UseShellExecute = false;
				proc.StartInfo = procStartInfo;
				proc.Start();
				if (wait)
				{
					proc.WaitForExit();
					exitCode = proc.ExitCode;
				}
				return true;
			}
			catch
			{
			}
			return false;
		}

		static public string PackagerExe()
		{
			if (Win64.IsWin64())
				return Path.Combine(Utils.MyPath(), "Packager64.exe");
			else
				return Path.Combine(Utils.MyPath(), "Packager.exe");
		}

		// dragdrop function (DragDrop) to open a new file dropping it in the main form
		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			dragging = false;
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (files.Length > 1)
			{
				MessageBox.Show("You can drop only one file");
				return;
			}
			//if (System.IO.Path.GetExtension(System.IO.Path.GetFileNameWithoutExtension(files[0]))
			//         + System.IO.Path.GetExtension(files[0]) != ".cameyo.exe")
			if (System.IO.Path.GetFileName(files[0]).IndexOf("AppVirtDll.", StringComparison.InvariantCultureIgnoreCase) != -1)
			{
				String openedFile = "";
				CloseAndReopen_Before(ref openedFile);
				try
				{
					// Syntax: myPath\Packager.exe -ChangeEngine AppName.cameyo.exe AppVirtDll.dll
					int exitCode = 0;
					if (!ExecProg(PackagerExe(), "-ChangeEngine \"" + openedFile + "\" "+
						"\"" + files[0] + "\"", true, ref exitCode))
						MessageBox.Show("Could not execute: " + PackagerExe());
				}
				finally
				{
					CloseAndReopn_After(openedFile);
				}
				return;
			}
			else if (System.IO.Path.GetFileName(files[0]).EndsWith("Loader.exe", StringComparison.InvariantCultureIgnoreCase))
			{
				String openedFile = "";
				CloseAndReopen_Before(ref openedFile);
				try
				{
					// Syntax: myPath\Packager.exe -ChangeLoader AppName.cameyo.exe Loader.exe
					//string myPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
					int exitCode = 0;
					if (!ExecProg(openedFile, "-ChangeLoader \"" + files[0] + "\"", true, ref exitCode))
						MessageBox.Show("Could not execute: " + PackagerExe());
				}
				finally
				{
					CloseAndReopn_After(openedFile);
				}
				return;
			}
			else if (System.IO.Path.GetExtension(files[0]).ToLower() != ".exe" &&
				System.IO.Path.GetExtension(files[0]).ToLower() != ".dat")
			{
				MessageBox.Show("You can only open files with .exe extension");
				return;
			}
			// open in a new thread to avoid blocking of explorer in case of big files
			this.BeginInvoke(Del_Open, new Object[] { files[0] });
			this.Activate();
		}

		// dragdrop function (DragEnter) to add file to the tree list and file list
		private void Vfs_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				((Control)sender).Focus();
				e.Effect = DragDropEffects.Copy;
				dragging = false;
				itemHoverTimer.Interval = 2000;
				itemHoverTimer.Start();
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		// dragdrop function (DragDrop) to add file to the tree list and file list
		private void Vfs_DragDrop(object sender, DragEventArgs e)
		{
			dragging = false;
			if (itemHoverTimer.Enabled)
				itemHoverTimer.Stop();

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

			String[] paths = (String[])e.Data.GetData(DataFormats.FileDrop);
			foreach (String path in paths)
			{
				this.BeginInvoke(fsEditor.Del_AddFOrFR, new object[] { parentNode, path });
			}
		}

		// dragdrop function (DragOver) to add file to the tree list and file list
		private void fsFolderTree_DragOver(object sender, DragEventArgs e)
		{
			Point pt = fsFolderTree.PointToClient(new Point(e.X, e.Y));
			TreeNode nodeUnderCursor = fsFolderTree.GetNodeAt(pt);
			if (nodeUnderCursor != null)
			{
				fsFolderTree.SelectedNode = nodeUnderCursor;
			}
		}

		// dragdrop function to add file to the tree list and file list
		// this function allows the nodes to close while navigating in the tree to drop files and folders
		private void fsFolderTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (dragging && e.Node.Level > 0)
			{
				TreeNode oldNode = fsFolderTree.SelectedNode;
				TreeNode newNode = e.Node;
				if (oldNode.IsExpanded && newNode.Level == oldNode.Level)
					oldNode.Collapse();
				else
				{
					while (newNode.Level <= oldNode.Level)
					{
						oldNode.Collapse();
						oldNode = oldNode.Parent;
					}
				}
			}
		}

		// dragdrop function to add file to the tree list and file list
		// this function allows the nodes to open while navigating in the tree to drop files and folders
		private void fsFolderTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (dragging)
			{
				if (itemHoverTimer.Enabled)
					itemHoverTimer.Stop();
				itemHoverTimer.Interval = 900;
				itemHoverTimer.Start();
			}
		}

		// timer to open the nodes
		private void OnItemHover(object sender, EventArgs e)
		{
			itemHoverTimer.Stop();
			if (fsFolderTree.SelectedNode != null)
			{
				if (!dragging)
					dragging = true;
				fsFolderTree.SelectedNode.Expand();
			}
		}

		private void CloseAndReopen_Before(ref String openedFile)
		{
			if (this.dirty || fsEditor.dirty || regEditor.dirty)
			{
				MessageBox.Show("You have to save the package first");
				return;
			}
			openedFile = virtPackage.openedFile;
			virtPackage.Close();
		}

		private void CloseAndReopn_After(String openedFile)
		{
			if (!virtPackage.opened)
				virtPackage.Open(openedFile);
		}

		private void dropboxButton_Click(object sender, EventArgs e)
		{
#if DropBox
			String openedFile = "";
			CloseAndReopen_Before(ref openedFile);
			try
			{
				DropboxLogin dropLogin = new DropboxLogin();
				dropLogin.Publish(openedFile);
			}
			finally
			{
				CloseAndReopn_After(openedFile);
			}
#endif
		}

		private void resetCredLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			RegistryKey cameyoKey = Registry.CurrentUser.CreateSubKey(@"Software\Cameyo");
			try
			{
				cameyoKey.DeleteValue("DropBoxTokenKey");
				cameyoKey.DeleteValue("DropBoxTokenSecret");
			}
			catch
			{
				MessageBox.Show("Cannot delete login tokens, did you save them?");
			}
		}

		private void lnkCustomEvents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			CustomEventsForm customEventsForm = new CustomEventsForm(virtPackage);
			customEventsForm.ShowDialog();
			dirty |= customEventsForm.dirty;
			customEventsForm.Dispose();
		}

		private void lnkActiveDirectory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var form = new ADPermissionsForm(virtPackage);
			form.ShowDialog();
			dirty |= form.dirty;
			form.Dispose();
		}

		private void lnkAutoUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AutoUpdateForm form = new AutoUpdateForm(virtPackage);
			form.ShowDialog();
			form.Dispose();
		}

		private void regFilesList_SubItemClicked(object sender, SubItemEventArgs e)
		{
			//Mario:ToDo Bugfixes:regFilesList.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
		}

		private void regFilesList_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
		{
			Registry.SetValue(regEditor.Masterkey, regEditor.Currentkey[regFilesList.Items.IndexOf(e.Item)].ToString(), e.DisplayText);
		}

		ListViewSorter fsFilesListSorter = new FileListViewSorter();
		private void fsFilesList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			fsFilesListSorter.Sort(fsFilesList, e.Column);
		}

		ListViewSorter regFilesListSorter = new RegListViewSorter();
		private void regFilesList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			regFilesListSorter.Sort(regFilesList, e.Column);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			bool OK = PackageClose();
			e.Cancel = !OK;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			//MainForm_Resize(null, null);
		}

		private void DisplayMRU()
		{
			listViewMRU.Items.Clear();
			foreach (MRUitem item in mru.GetItems())
			{
				if (!File.Exists(item.file))
					continue;
				Icon ico = Win32Function.getIconFromFile(item.file);
				int imageId = imageListMRU.Images.Add(ico.ToBitmap(), Color.Empty);

				String fileName = Path.GetFileNameWithoutExtension(item.file);
				if (fileName.EndsWith(".virtual")) fileName = fileName.Remove(fileName.Length - 8);
				if (fileName.EndsWith(".cameyo")) fileName = fileName.Remove(fileName.Length - 7);

				ListViewItem lvItem = listViewMRU.Items.Add(fileName);
				lvItem.ImageIndex = imageId;
				lvItem.Tag = item.file;
				lvItem.ToolTipText = item.file;
				lvItem.Group = listViewMRU.Groups["recentlyEditedGroup"];
			}
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			listViewMRU.ShowItemToolTips = true;
			ListViewHelper.EnableDoubleBuffer(listViewMRU);
			Win32imports.SendMessage(listViewMRU.Handle, Win32imports.LVM_SETICONSPACING, (uint)410, (uint)410);
			DisplayMRU();
			/*foreach (DeployedApp deployedApp in VirtPackage.DeployedApps())
			{
				if (!File.Exists(deployedApp.CarrierExeName))
					continue;
				Icon ico = Win32Function.getIconFromFile(deployedApp.CarrierExeName);
				int imageId = imageListMRU.Images.Add(ico.ToBitmap(), Color.Empty);

				String fileName = deployedApp.AppID;
				if (fileName.EndsWith(".virtual")) fileName = fileName.Remove(fileName.Length - 8);
				if (fileName.EndsWith(".cameyo")) fileName = fileName.Remove(fileName.Length - 7);

				ListViewItem lvItem = listViewMRU.Items.Add(fileName);
				lvItem.ImageIndex = imageId;
				lvItem.Tag = deployedApp.CarrierExeName;
				lvItem.Group = listViewMRU.Groups["deployedAppsGroup"];
			}*/
		}

		private void rdb_CheckedChanged(object sender, EventArgs e)
		{
			bool cleanup = !rdbCleanNone.Checked;
			chkCleanAsk.Enabled = cleanup;
			chkCleanDoneDialog.Enabled = cleanup;
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!PackageClose())
				return;
			if (!virtPackage.Create("New Package ID",
				Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "AppVirtDll.dll"),
				Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Loader.exe")))
			{
				MessageBox.Show("Faild to create a new package.");
				return;
			}
			dirty = false;
			this.OnPackageOpen(null);
			fsEditor.OnPackageOpen();
			regEditor.OnPackageOpenBeforeUI();
			tabControl.SelectedIndex = 0;

			rdbIntegrateNone.Checked = true;
			rdbIntegrateStandard.Checked = false;
			rdbIntegrateVirtual.Checked = false;

			rdbCleanNone.Checked = true;
			rdbCleanRegOnly.Checked = false;
			rdbCleanAll.Checked = false;
			chkCleanAsk.Checked = true;
			chkCleanDoneDialog.Checked = false;

			EnableDisablePackageControls(true);
			regLoaded = true;
			regProgressTimer_Tick(null, null);
		}

		private void toolStripMenuItemExport_Click(object sender, EventArgs e)
		{
			regEditor.toolStripMenuItemExport_Click(sender, e);
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			regRemoveBtn.PerformClick();
		}

		private void tabRegistry_DragDrop(object sender, DragEventArgs e)
		{
			dragging = false;

			String[] paths = (String[])e.Data.GetData(DataFormats.FileDrop);
			foreach (String path in paths)
			{
				this.BeginInvoke(regEditor.Del_AddFOrF, new object[] { path });
			}
		}

		private void tabRegistry_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				((Control)sender).Focus();
				e.Effect = DragDropEffects.Copy;
				dragging = false;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void tabRegistry_DragOver(object sender, DragEventArgs e)
		{
			//
		}

		private void btnNewPackage_Click(object sender, EventArgs e)
		{
			newToolStripMenuItem_Click(sender, e);
		}

		private void btnEditPackage_Click(object sender, EventArgs e)
		{
			openToolStripMenuItem_Click(sender, e);
		}

		private void listViewMRU_Click(object sender, EventArgs e)
		{
			if (listViewMRU.SelectedItems.Count != 1)
				return;
			VirtPackage.APIRET apiRet;
			if (!PackageOpen((String)listViewMRU.SelectedItems[0].Tag, true, out apiRet))
				MessageBox.Show(String.Format("Failed to open package. API error: {0}", apiRet));
		}

		private void regImportBtn_Click(object sender, EventArgs e)
		{
			regEditor.RegFileImport();
		}

		private void regExportBtn_Click(object sender, EventArgs e)
		{
			regEditor.RegFileExport();
		}

		private void fileContextMenuDelete_Click(object sender, EventArgs e)
		{
			fsRemoveBtn.PerformClick();
		}

		private void fileContextMenuProperties_Click(object sender, EventArgs e)
		{
			fsEditor.ShowProperties();
		}

		private void fsFilesList_ItemDrag(object sender, ItemDragEventArgs e)
		{
			fsEditor.DragDropFiles((FileListViewItem)e.Item);
		}

		private void fsFolderTree_ItemDrag(object sender, ItemDragEventArgs e)
		{
			fsEditor.DragDropFiles((FolderTreeNode)e.Item);
		}

		private void lnkCapture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			int exitCode = 0;
			if (!Cameyo.OpenSrc.Common.Utils.ExecProg(PackagerExe(), null, false, ref exitCode))
				MessageBox.Show("Can't start the app packager");
			else
				Application.Exit();
		}

		private void lnkPackageEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			openToolStripMenuItem_Click(this, new EventArgs());
		}

		ToolStripMenuItem CurLanguageMenuItem()
		{
			string cultureStr = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
			foreach (ToolStripMenuItem item in langToolStripMenuItem.DropDownItems)
			{
				if (((string)item.Tag).Equals(cultureStr, StringComparison.InvariantCultureIgnoreCase))
					return item;
			}
			return englishMenuItem;
		}

		private void langMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			string cultureStr = ((string)item.Tag);
			LangUtils.SaveCulture(cultureStr);
			LangUtils.LoadCulture();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			MessageBox.Show(PackageEditor.Messages.Messages.changesWillTakeEffectOnNextStart);
		}

		private void propertyVirtMode_CheckedChanged(object sender, EventArgs e)
		{
			picRAM.Visible = picDisk.Visible = false;
			if (propertyVirtModeRam.Checked)
			{
				helpVirtMode.Text = helpVirtModeRam;
				picRAM.Visible = true;
			}
			else if (propertyVirtModeDisk.Checked)
			{
				helpVirtMode.Text = helpVirtModeDisk;
				picDisk.Visible = true;
			}
		}

		private void propertyIsolationMode_CheckedChanged(object sender, EventArgs e)
		{
			picDataMode.Visible = picIsolatedMode.Visible = picFullAccess.Visible = false;
			if (propertyIsolationDataMode.Checked)
			{
				helpIsolationMode.Text = helpIsolationModeData;
				picDataMode.Visible = true;
			}
			else if (propertyIsolationIsolated.Checked)
			{
				helpIsolationMode.Text = helpIsolationModeIsolated;
				picIsolatedMode.Visible = true;
			}
			else if (propertyIsolationMerge.Checked)
			{
				helpIsolationMode.Text = helpIsolationModeFull;
				picFullAccess.Visible = true;
			}

			// Refresh fsFolderTree & regFolderTree
			// Isolation. Note: it is allowed to have no checkbox selected at all.
			if (sender != null)    // null means this function was called manually by DisplayIsolationMode
			{
				virtPackage.SetIsolationMode(
					propertyIsolationIsolated.Checked ? VirtPackage.ISOLATIONMODE_ISOLATED :
					propertyIsolationMerge.Checked ? VirtPackage.ISOLATIONMODE_FULL_ACCESS :
					propertyIsolationDataMode.Checked ? VirtPackage.ISOLATIONMODE_DATA :
					VirtPackage.ISOLATIONMODE_CUSTOM);
				if (fsFolderTree.Nodes.Count > 0)    // Only if fsEditor is initialized
					fsEditor.RefreshFolderNodeRecursively((FolderTreeNode)fsFolderTree.Nodes[0], 0);
				if (regFolderTree.Nodes.Count > 0)   // Only if regEditor is initialized
					regEditor.RefreshFolderNodeRecursively(regFolderTree.Nodes[0], 0);
			}
		}

		private void propertyProtPassword_Enter(object sender, EventArgs e)
		{
			if (propertyProtPassword.Text == "[UNCHANGED]")
			{
				propertyProtPassword.Text = "";
				tbPasswordConfirm.Text = "";
			}
		}

		private void propertyProt_CheckedChanged(object sender, EventArgs e)
		{
			if (propertyProtPassword.Text == "[UNCHANGED]" && propertyProt.Checked)
			{
				propertyProtPassword.Text = "";
				tbPasswordConfirm.Text = "";
			}
		}

		private void lnkUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Cameyo.OpenSrc.Common.Utils.ShellExec("https://www.cameyo.com/upgrade");
		}

		public bool RequireElevation()
		{
			if (isElevatedProcess)
				return true;
			else
			{
				if (this.dirty)
					MessageBox.Show(Messages.Messages.reqElevationSaveWorkFirst, Messages.Messages.reqElevationTitle);
				else
				{
					if (MessageBox.Show(Messages.Messages.reqElevation, Messages.Messages.reqElevationTitle,
						MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
					{
						int exitCode = -1;
						string fileName = virtPackage.openedFile;   // Must be before closing the package
						closeToolStripMenuItem_Click(null, null);   // Must be done before execution, otherwise the new Package Editor will fail (sharing violation)
						string myExeName = System.IO.Path.Combine(Utils.MyPath(), "PackageEditor.exe");
						if (Cameyo.OpenSrc.Common.Utils.ShellExec(myExeName, "\"" + fileName + "\"", "runas", ref exitCode, false))
							exitToolStripMenuItem_Click(null, null);
					}
				}
				return false;
			}
		}

		private void propertyEncryption_CheckedChanged(object sender, EventArgs e)
		{
			bool b = propertyEncryption.Checked;

			propertyEncryptUsingKey.Enabled = b;
			propertyEncryptUsingPassword.Enabled = b;
			propertyEncryptUserCreatedPassword.Enabled = b;

			// EncKey
			lnkGenerateEncKey.Enabled = b && propertyEncryptUsingKey.Checked;
			tbEncryptionKey.Enabled = b && propertyEncryptUsingKey.Checked;

			// Pwd
			tbEncryptionPwd.Enabled = b && propertyEncryptUsingPassword.Checked;
			tbEncryptionPwdConfirm.Enabled = b && propertyEncryptUsingPassword.Checked;
			lnkImportPwdKey.Enabled = lnkExportPwdKey.Enabled = b && propertyEncryptUsingPassword.Checked;
		}

		private void lnkGenerateEncKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionKey")) && !generateEncKeyAlreadyWarned)
			{
				if (MessageBox.Show("You are changing the encryption key. Data previously encrypted by this application " + 
					"will no longer be accessible. Continue?", "Warning", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
					return;
				generateEncKeyAlreadyWarned = true;
			}
			var rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider();
			byte[] randomBytes = new byte[16];   // Our AES algorithm uses a 128-bit key (16 bytes)
			rngCsp.GetBytes(randomBytes);
			tbEncryptionKey.Text = Utils.HexDump(randomBytes);
		}

		private void tbEncryptionPwd_Enter(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(virtPackage.GetProperty("EncryptionPwdKey")) && !changePwdKeyAlreadyWarned)
			{
				MessageBox.Show("You are changing the encryption password. Data previously encrypted by this application " +
					"will no longer be accessible (even if you re-type the same password).");
				changePwdKeyAlreadyWarned = true;
			}
		}

		private void lnkEncryptionLearnMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Utils.ShellExec("http://www.cameyo.com/virtualapp-encryption");
		}

		private void XmlWritePropAttr(XmlWriter xmlOut, string nodeName, string attrName, string attrValue)
		{
			xmlOut.WriteStartElement(nodeName);
			xmlOut.WriteAttributeString(attrName, attrValue);
			xmlOut.WriteEndElement();
		}

		private void lnkExportPwdKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (!virtPackage.opened || string.IsNullOrEmpty(virtPackage.openedFile))
			{
				MessageBox.Show("No file is currently open");
				return;
			}

			ProcessEncryptionPwd();

			// TODO: require password?
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Path.GetDirectoryName(virtPackage.openedFile);
			saveFileDialog.FileName = Path.ChangeExtension(Path.GetFileName(virtPackage.openedFile), ".PasswordKey.xml");
			saveFileDialog.AddExtension = true;
			saveFileDialog.Filter = "Password key (*.PasswordKey.xml)|*.PasswordKey.xml";
			saveFileDialog.DefaultExt = "PasswordKey.xml";
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string xmlFileName = saveFileDialog.FileName;

				// Complete XML with Files and Registry
				var writerSettings = new XmlWriterSettings();
				writerSettings.OmitXmlDeclaration = true;
				writerSettings.Indent = true;
				writerSettings.Encoding = Encoding.Unicode;
				using (XmlWriter xmlOut = XmlWriter.Create(xmlFileName, writerSettings))
				{
					xmlOut.WriteStartDocument();
					xmlOut.WriteStartElement("CameyoPkg");
					{
						// Read existing XML Properties/Property nodes
						xmlOut.WriteStartElement("Properties");
						{
							XmlWritePropAttr(xmlOut, "Property", "EncryptionPwdKey", virtPackage.GetProperty("EncryptionPwdKey"));
							XmlWritePropAttr(xmlOut, "Property", "EncryptionPwdHash", virtPackage.GetProperty("EncryptionPwdHash"));
							XmlWritePropAttr(xmlOut, "Property", "EncryptionPwdSalt", virtPackage.GetProperty("EncryptionPwdSalt"));
							XmlWritePropAttr(xmlOut, "Property", "EncryptionPwdIV", virtPackage.GetProperty("EncryptionPwdIV"));
						}
						xmlOut.WriteEndElement();
					}
					xmlOut.WriteEndElement();
					xmlOut.WriteEndDocument();
					xmlOut.Flush();
					xmlOut.Close();

					xmlOut.Close();

					MessageBox.Show("Created in:\n" + xmlFileName);
				}
			}
		}

		private void lnkImportPwdKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Path.GetDirectoryName(virtPackage.openedFile);
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Password key (*.PasswordKey.xml)|*.PasswordKey.xml|All files (*.*)|*.*";
			openFileDialog.DefaultExt = "PasswordKey.xml";
			if (openFileDialog.ShowDialog() != DialogResult.OK)
				return;

			var xd = new System.Xml.XPath.XPathDocument(openFileDialog.FileName);
			var navigator = xd.CreateNavigator();
			var iterator = navigator.Select("//Properties/Property");
			int foundProps = 0;
			foreach (System.Xml.XPath.XPathNavigator n in iterator)
			{
				n.MoveToFirstAttribute();
				var name = n.Name;
				var val = n.Value;
				if (name == "EncryptionPwdKey" || name == "EncryptionPwdHash" || name == "EncryptionPwdSalt" || name == "EncryptionPwdIV")
				{
					virtPackage.SetProperty(name, val);
					foundProps++;
				}
			}
			if (foundProps == 4)   // All expected properties were found
			{
				tbEncryptionPwd.Text = tbEncryptionPwdConfirm.Text = "[UNCHANGED]";
				MessageBox.Show("Password key imported.");
			}
			else
			{
				MessageBox.Show("No password key found in this file.");
			}
		}
	}

	//
	// RegListViewSorter class
	class RegListViewSorter : ListViewSorter
	{
		protected override int CompareItems(ListViewItem x, ListViewItem y)
		{
			return String.Compare(x.SubItems[currentcolumn].Text, y.SubItems[currentcolumn].Text);
		}
	}

	public class FileListViewItem : ListViewItem
	{
		public ulong fileSize = 0;
		public VIRT_FILE_FLAGS flags = VIRT_FILE_FLAGS.NO_FLAGS;
	}

	class FileListViewSorter : ListViewSorter
	{
		bool isFileSizeColumn = false;
		public override void Sort(ListView List, int ColumnNumber)
		{
			isFileSizeColumn = List.Columns[ColumnNumber].Text == "Size";
			base.Sort(List, ColumnNumber);
		}
		protected override int CompareItems(ListViewItem x, ListViewItem y)
		{
			int res;
			if (isFileSizeColumn)
				res = ((FileListViewItem)x).fileSize.CompareTo(((FileListViewItem)y).fileSize);
			else
				res = String.Compare(x.SubItems[currentcolumn].Text, y.SubItems[currentcolumn].Text);
			return res;
		}
	}

	//
	// ListViewSorter class
	abstract class ListViewSorter : IComparer
	{
		protected int currentcolumn = -1;
		bool currentAsc = true;
		public virtual void Sort(ListView List, int ColumnNumber)
		{
			if (currentcolumn == ColumnNumber)
			{
				currentAsc = !currentAsc;
			}
			else
				currentAsc = true;
			currentcolumn = ColumnNumber;
			List.ListViewItemSorter = this;
			List.Sort();
		}
		public int Compare(object x, object y)
		{
			int res = CompareItems((ListViewItem)x, (ListViewItem)y);
			if (!currentAsc)
				res = -res;
			return res;
		}
		abstract protected int CompareItems(ListViewItem x, ListViewItem y);
	}

	//
	// MRUitem, class
	public class MRUitem
	{
		public MRUitem(String name, String file)
		{
			this.name = name;
			this.file = file;
		}
		public String name;
		public String file;
	}

	//
	// MRU class
	public class MRU
	{
		public int maxItems;
		private RegistryKey regKey;

		public MRU(String regKeyName)
		{
			maxItems = 8;
			try
			{
				regKey = Registry.CurrentUser.OpenSubKey(regKeyName, true);
				if (regKey == null)
					regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
				if (regKey == null)
				{
					MessageBox.Show("Cannot write to registry. Quitting.", "Fatal");
					Application.Exit();
				}
			}
			catch { }
		}

		~MRU()
		{
			regKey.Close();
		}

		private void InsertTopItem(String fileName)
		{
			// Delete maximum entry, if it exists
			regKey.DeleteValue(Convert.ToString(maxItems - 1), false);

			// Look for empty holes (or last element) to move all items up to
			int moveTo = -1;
			for (int i = 0; i < maxItems; i++)
			{
				String fileNameValue = (String)regKey.GetValue(Convert.ToString(i));
				if (fileNameValue == null)
				{
					moveTo = i;
					break;
				}
			}

			// Rename (increment) all items up to moveTo
			if (moveTo > 0)
			{
				for (int i = moveTo - 1; i >= 0; i--)
				{
					// Rename #1:filename -> #2:filename
					String fileNameValue = (String)regKey.GetValue(Convert.ToString(i));
					if (i < maxItems)
						regKey.SetValue(Convert.ToString(i + 1), fileNameValue);
				}
			}

			// Add "0" item
			try { regKey.SetValue("0", fileName); }
			catch { }
		}

		public List<MRUitem> GetItems()
		{
			List<MRUitem> result = new List<MRUitem>();

			String[] items = regKey.GetValueNames();
			List<String> list = new List<string>(items);
			list.Sort();
			for (int i = 0; i < list.Count; i++)
			{
				String fileNameValue = (String)regKey.GetValue(list[i]);
				if (fileNameValue == null || !File.Exists(fileNameValue))
					continue;
				result.Add(new MRUitem(list[i], fileNameValue));
			}
			return result;
		}

		public void AddFile(String fileName)
		{
			// First browse through MRU items, deleting those that already contain this fileName
			String[] items = regKey.GetValueNames();
			for (int i = 0; i < items.Length; i++)
			{
				String fileNameValue = (String)regKey.GetValue(items[i]);
				if (fileNameValue == null)
					continue;
				if (fileNameValue.ToUpper() == fileName.ToUpper())
					regKey.DeleteValue(items[i]);
			}

			// Add to top ("0")
			InsertTopItem(fileName);
		}
	}
}
