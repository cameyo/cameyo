using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using System.Web;
using System.IO;

namespace Cameyo.Player
{
    /// <summary>
    /// Interaction logic for PkgCreateWin.xaml
    /// </summary>
    public partial class PkgCreateWin : Window
    {
        ServerClient Server = ServerSingleton.Instance.ServerClient;
        string PkgLocation, PkgAppName, PkgIconPath;   // Information about the resulting package
        bool PkgUploaded = false;
        long RequestId = 0;
        string InstallerPath, InstallerArgs;
        string CannotOnlinePackagerReason = "Please provide an installer file first.";
        Brush EnabledColor, DisabledColor;

        enum PkgStatus
        {
            StatusReadyToPackage = 1,      // Ready for VmDispatch
            StatusAwaitingArgs = 2,        // Awaiting user action
            StatusPackaging = 3,
            StatusFailed = 4,
            StatusDone = 5
        };

        public PkgCreateWin()
        {
            InitializeComponent();
            EnabledColor = SnapshotBtn.Foreground;
            DisabledColor =  new SolidColorBrush(System.Windows.Media.Colors.LightGray);
            SetUiMode(UiMode.WaitingForFile);
        }

        enum UiMode
        {
            WaitingForFile,
            WaitingForButtonClick,
            Working,
            AdditionalInfoNeeded,
            Success,
            Failure,
            DownloadingUploading,
        }

        void SetUiMode(UiMode mode)
        {
            switch (mode)
            {
                case UiMode.WaitingForFile:
                    //ButtonsPanel.Visibility = Visibility.Collapsed;
                    break;
                case UiMode.WaitingForButtonClick:
                    DragDropImg.Visibility = Visibility.Collapsed;
                    ButtonsPanel.Visibility = Visibility.Visible;
                    IconImg.Visibility = Visibility.Visible;
                    ArgsBtn.Visibility = Visibility.Visible;
                    break;
                case UiMode.Working:
                    PreloaderStart();
                    BrowseBtn.Visibility = Visibility.Collapsed;
                    ButtonsPanel.Visibility = Visibility.Collapsed;   // Hide the buttons to avoid user interaction
                    AnimPanel.Visibility = Visibility.Visible;
                    DragDropImg.Visibility = Visibility.Collapsed;
                    IconImg.Visibility = Visibility.Collapsed;
                    ArgsBtn.Visibility = Visibility.Collapsed;
                    break;
                case UiMode.AdditionalInfoNeeded:
                    PreloaderStop();
                    AdditionalInfoBtn.Visibility = Visibility.Visible;
                    CloseBtn.Visibility = Visibility.Visible;
                    break;
                case UiMode.Failure:
                    PreloaderStop();
                    CloseBtn.Visibility = Visibility.Visible;
                    break;
                case UiMode.Success:
                    PreloaderStop();
                    DownloadUploadBtnTxt.Text = (PkgLocation.Contains("://") ? "Download" : "Upload");
                    DownloadUploadBtn.Visibility = (!PkgUploaded ? Visibility.Visible : Visibility.Collapsed);   // Don't want the "Download" button to appear after user uploaded the package...
                    PkgExploreBtn.Visibility = Visibility.Visible;
                    CloseBtn.Visibility = Visibility.Visible;
                    IconImg.Visibility = Visibility.Visible;
                    break;
                case UiMode.DownloadingUploading:
                    PreloaderStart();
                    BrowseBtn.Visibility = PkgExploreBtn.Visibility = CloseBtn.Visibility = DownloadUploadBtn.Visibility = Visibility.Collapsed;
                    ButtonsPanel.Visibility = Visibility.Collapsed;   // Hide the buttons to avoid user interaction
                    AnimPanel.Visibility = Visibility.Visible;
                    DragDropImg.Visibility = Visibility.Collapsed;
                    IconImg.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        void OnFileSelect(string fileName)
        {
            if (ValidatePackagingMethods(fileName, false))
            {
                DisplayAssociatedIcon(fileName);
                SetUiMode(UiMode.WaitingForButtonClick);
            }
        }

        private void OnlinePackagerBtn_Click(object sender, RoutedEventArgs e)
        {
            ValidatePackagingMethods(InstallerPath, true);
            if (!string.IsNullOrEmpty(CannotOnlinePackagerReason))
            {
                MessageBox.Show(CannotOnlinePackagerReason);
                return;
            }
            string args = "";
            if (!string.IsNullOrEmpty(InstallerArgs))
                args = "&args=" + InstallerArgs;
            var url = Server.BuildUrl("PkgSubmitFile", true, args);
            var webClient = new WebClient();
            webClient.UploadProgressChanged += UploadProgressChanged;
            webClient.UploadFileCompleted += SubmitPkgFileUploadCompleted;
            webClient.UploadFileAsync(new Uri(url), InstallerPath);
            ProgressText("Uploading");
            SetUiMode(UiMode.Working);
        }

        private void SandboxCaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressText("Capturing installation...");
            SetUiMode(UiMode.Working);
            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(StartGhostCapture));
            thread.Start();
        }

        private void SnapshotBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressText("Snapshot capture in progress.");
            SetUiMode(UiMode.Working);
            PreloaderStop();   // Since Packager will be mostly waiting for user's input
            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(StartPackagerCapture));
            thread.Start();
        }

        private void BrowseBtnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog.FileNames.Count() == 1)
                    OnFileSelect(openFileDialog.FileNames[0]);
            }
        }

        private void ArgsBtnClick(object sender, RoutedEventArgs e)
        {
            Utils.ShowInputDialog("Installer arguments", ref InstallerArgs);
        }

        private void PkgExploreBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PkgLocation))
                return;   // Shouldn't happen
            if (PkgLocation.Contains("://"))
                Utils.ShellExec(PkgLocation + "?" + Server.AuthUrl(), false);
            else
            {
                Utils.ShellExec("explorer.exe", string.Format("/select,\"{0}\"", PkgLocation), false);
            }
        }

        private void DownloadUploadBtnClick(object sender, RoutedEventArgs e)
        {
            if (PkgLocation.Contains("://"))
            {
                // Download request
                var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.FileName = PkgAppName + ".cameyo.exe";
                saveFileDialog.AddExtension = true;
                saveFileDialog.Filter = "Cameyo packages (*.cameyo.exe)|*.cameyo.exe|All file types|*.*";
                saveFileDialog.DefaultExt = "cameyo.exe";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SetUiMode(UiMode.DownloadingUploading);
                    var webClient = new WebClient();
                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadPkgCompleted);
                    webClient.DownloadProgressChanged += DownloadProgressChanged;
                    webClient.DownloadFileAsync(new Uri(PkgLocation + "/download" + "?" + Server.AuthUrl()), saveFileDialog.FileName);
                }
            }
            else
            {
                // Upload request
                PkgUploaded = true;   // Avoid the "Download" button from now on
                var url = Server.BuildUrl("PkgSubmitFile", true, null);
                var webClient = new WebClient();
                webClient.UploadProgressChanged += UploadProgressChanged;
                webClient.UploadFileCompleted += SubmitPkgFileUploadCompleted;
                webClient.UploadFileAsync(new Uri(url), PkgLocation);
                ProgressText("Uploading");
                SetUiMode(UiMode.DownloadingUploading);
            }
        }

        void AdditionalInfoBtnClick(object sender, RoutedEventArgs e)
        {
            if (RequestId != 0)
                Utils.ShellExec(string.Format("{0}/pkgrAdditionalInfo.aspx?reqId={1}&{2}", Server.ServerUrl(), RequestId, Server.AuthUrl()), false);
        }

        private void PreloaderStart()
        {
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Begin(this, true);
        }

        private void PreloaderStop()
        {
            AnimPanel.Visibility = Visibility.Collapsed;
            //DragDropImg.Visibility = Visibility.Visible;
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Stop(this);
        }

        private void ProgressText(string txt)
        {
            StatusTxt.Text = txt;
        }

        /*public static Icon GetIconForFile(string filename, ShellIconSize size)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            SHGetFileInfo(filename, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)size);
            return Icon.FromHandle(shinfo.hIcon);
        }*/

        private void DisplayResultingPkg()
        {
            SetUiMode(UiMode.Success);
            ProgressText(PkgAppName);

            // Online package
            if (PkgIconPath.Contains("://"))
            {
                // PkgIconPath
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(PkgIconPath, UriKind.Absolute);
                bitmap.EndInit();
                IconImg.Source = bitmap;

                // Expiration
                if (Server.AccountInfo.PkgDurationDays > 0)
                {
                    ExpirationTxt.Text += "Expiration in " + Server.AccountInfo.PkgDurationDays + " days";
                    ExpirationTxt.Visibility = Visibility.Visible;
                }
                UrlBox.Text = PkgLocation;
                UrlBox.Visibility = Visibility.Visible;

                // Force main window to refresh private library
                var wnd = (Cameyo.Player.MainWindow)Owner;
                wnd.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    wnd.ForceMyLibraryRefresh();
                }));
            }
            else if (PkgIconPath.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))
            {
                DisplayAssociatedIcon(PkgIconPath);
            }
        }

        private void DisplayAssociatedIcon(string iconPath)
        {
            var icon = System.Drawing.Icon.ExtractAssociatedIcon(iconPath);
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            IconImg.Source = wpfBitmap;
        }

        private void DisplayError(string title)   // Must be called from UI thread
        {
            SetUiMode(UiMode.Failure);
            ProgressText(title);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DragAreaGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        void OnFileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                OnFileSelect(files[0]);
            }
        }

        void WaitForPkgCreation()
        {
            while (true)
            {
                try
                {
                    var resp = Server.SendRequest("PkgStatus", false, "&reqId=" + RequestId.ToString());
                    var items = resp.Split(';');
                    if (items.Count() < 2)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                        {
                            DisplayError("Failed. Please try another capture method.");
                        }));
                        break;
                    }
                    PkgStatus pkgStatus = (PkgStatus)Convert.ToInt32(items[0]);
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        switch (pkgStatus)
                        {
                            case PkgStatus.StatusReadyToPackage:
                                ProgressText("Queued...");
                                break;
                            case PkgStatus.StatusPackaging:
                                ProgressText("Packaging...");
                                break;
                            case PkgStatus.StatusAwaitingArgs:
                                SetUiMode(UiMode.AdditionalInfoNeeded);
                                ProgressText("Your input is needed. Please continue online:");
                                break;
                            case PkgStatus.StatusFailed:
                                DisplayError("Failed.");
                                break;
                            case PkgStatus.StatusDone:
                                if (items.Count() >= 7)
                                {
                                    long pkgId = Convert.ToInt64(items[1]);
                                    PkgIconPath = items[2];
                                    PkgAppName = items[6];
                                    PkgLocation = Server.ServerUrl() + "/apps/" + pkgId;   // ?auth= must be added by consuming functions
                                    DisplayResultingPkg();
                                }
                                else
                                    DisplayError("Packaging succeeded, but invalid data was returned.");   // Should never happen
                                break;
                        }
                    }));

                    if (pkgStatus == PkgStatus.StatusAwaitingArgs)
                        break;
                    else if (pkgStatus == PkgStatus.StatusFailed)
                        break;
                    else if (pkgStatus == PkgStatus.StatusDone)
                        break;

                    System.Threading.Thread.Sleep(6 * 1000);
                }
                catch
                {
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        DisplayError("Failed creating package. Please retry.");
                    }));
                    break;
                }
            }
        }

        void SubmitPkgFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Upload failed.");
                }));
                return;
            }

            // The upload is finished, clean up
            string resp = System.Text.Encoding.ASCII.GetString(e.Result);

            // Interpret result
            var items = resp.Split('\n');
            int requestStatus = 0, errCode = 0;
            //long requestId = 0;
            foreach (var item in items)
            {
                var pair = item.Split('=');
                if (pair.Count() != 2)
                    continue;
                var name = pair[0];
                var value = pair[1];
                if (name == "requestStatus")
                    requestStatus = Convert.ToInt32(value);
                else if (name == "requestId")
                    RequestId = Convert.ToInt64(value);
                else if (name == "errCode")
                    errCode = Convert.ToInt32(value);
            }

            var thread = new System.Threading.Thread(new System.Threading.ThreadStart(WaitForPkgCreation));
            thread.Start();
        }

        void DownloadPkgCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Failed downloading package.");
                }));
                return;
            }
            DisplayResultingPkg();
        }

        void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                ProgressText("Uploading " + e.ProgressPercentage.ToString() + "%");
            }));
        }

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                ProgressText("Downloading " + e.ProgressPercentage.ToString() + "%");
            }));
        }

        bool ValidatePackagingMethods(string fileName, bool pressed)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;
                if (!File.Exists(fileName))
                    throw new Exception("File not found");
                FileInfo fi = new FileInfo(fileName);
                StatusTxt.Text = System.IO.Path.GetFileName(fileName);
                InstallerPath = fileName;

                // Validate cloud packaging: unattended install?
                bool silentInstallAvailable = Utils.SilentInstallAvailable(Utils.MyPath("SilentInstall.exe"), fileName);
                if (silentInstallAvailable)
                {
                    var story = this.TryFindResource("OnlinePackagerHighlight") as System.Windows.Media.Animation.Storyboard;
                    //story.Begin(this, true);
                    CannotOnlinePackagerReason = "";
                }
                else
                {
                    CannotOnlinePackagerReason = "File does not support unattended installation.";
                    if (pressed)
                    {
                        if (MessageBox.Show("This file does not support unattended installation. " +
                            "You will need to finalize this request online. Would you like to proceed?",
                            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            CannotOnlinePackagerReason = "";
                    }
                }

                // Validate cloud packaging: account limitations?
                if (string.IsNullOrEmpty(CannotOnlinePackagerReason))
                {
                    var accountMaxMb = Server.AccountInfo.FileUploadMbLimit * 1024 * 1024;
                    if (fi.Length >= accountMaxMb)
                    {
                        CannotOnlinePackagerReason = string.Format("Your account is limited to {0} MB. Consider upgrading.",
                            Server.AccountInfo.FileUploadMbLimit); ;
                    }
                }

                // Enable / disable buttons
                if (string.IsNullOrEmpty(CannotOnlinePackagerReason))
                    OnlinePackagerBtn.Foreground = EnabledColor;
                else
                    OnlinePackagerBtn.Foreground = DisabledColor;

                // Validate Sandbox capture
                SandboxCaptureBtn.Foreground = EnabledColor;

                return true;
            }
            catch (Exception ex)
            {
                CannotOnlinePackagerReason = ex.Message;
                StatusTxt.Text = "Error: " + ex.Message;
                return false;
            }
        }

        private void StartPackagerCapture()
        {
            try
            {
                // Cmd
                string cmd = "";
                if (!string.IsNullOrEmpty(InstallerPath))
                {
                    cmd += " -exec \"" + InstallerPath + "\"";
                    if (!string.IsNullOrEmpty(InstallerArgs))
                        cmd += " " + InstallerArgs;
                }

                // Packager.exe
                var exeName = Utils.MyPath("Packager.exe");
                int exitCode = 0;
                string ResponseFile = System.IO.Path.GetTempFileName();
                if (!Cameyo.Utils.ShellExec(exeName, "\"-ResponseFile:" + ResponseFile + "\"" + cmd, ref exitCode, true, false))
                    throw new Exception("Error executing: " + exeName);
                if (exitCode != 0)
                    throw new Exception("Capture failed with error " + exitCode.ToString() + ".");

                // Read response file
                ReadPackagerResponseFile(ResponseFile, out PkgAppName, out PkgLocation, out PkgIconPath);

                // Display result
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayResultingPkg();
                }));
            }
            catch (Exception ex)
            {
                // Error handling
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Failed: " + ex.Message);
                }));
            }
        }

        private void StartGhostCapture()
        {
            try
            {
                // Cmd
                string cmd = "\"" + InstallerPath + "\"";
                if (!string.IsNullOrEmpty(InstallerArgs))
                    cmd += " " + InstallerArgs;

                // Packager.exe
                var exeName = Utils.MyPath("Packager.exe");
                int exitCode = 0;
                string ResponseFile = System.IO.Path.GetTempFileName();
                if (!Cameyo.Utils.ShellExec(exeName, "-Quiet \"-ResponseFile:" + ResponseFile + "\" -GhostCapture " + cmd, ref exitCode, true, false))
                    throw new Exception("Error executing: " + exeName);
                if (exitCode != 0)
                    throw new Exception("Capture failed with error " + exitCode.ToString() + ".");

                // Read response file
                ReadPackagerResponseFile(ResponseFile, out PkgAppName, out PkgLocation, out PkgIconPath);

                // Display result
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayResultingPkg();
                }));
            }
            catch (Exception ex)
            {
                // Error handling
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Failed: " + ex.Message);
                }));
            }
        }

        private void ReadPackagerResponseFile(string ResponseFile, out string PkgAppName, out string PkgLocation, out string PkgIconPath)
        {
            // Read response file
            var lines = File.ReadAllText(ResponseFile, Encoding.Unicode).Split('\n');
            Utils.KillFile(ResponseFile);
            PkgAppName = PkgLocation = PkgIconPath = "";
            foreach (var line in lines)
            {
                int pos = line.IndexOf('=');
                if (pos == -1)
                    continue;
                var item = line.Substring(0, pos);
                var value = line.Substring(pos + 1);
                if (item == "AppID")
                    PkgAppName = value;
                else if (item == "OutputExe")
                    PkgIconPath = PkgLocation = value;
            }
            if (string.IsNullOrEmpty(PkgAppName) || string.IsNullOrEmpty(PkgLocation))
                throw new Exception("Could not read capture results.");
        }
    }
}
