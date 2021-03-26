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
using System.Runtime.Serialization;
using System.Threading;
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
        string CannotUploadExistingPkgReason = "Please select a Cameyo package file first.";
        Brush EnabledColor, DisabledColor;
        public bool UploadOnlyMode = false;

        enum PkgStatus
        {
            StatusReadyToPackage = 1,      // Ready for VmDispatch
            StatusAwaitingArgs = 2,        // Awaiting user action
            StatusPackaging = 3,
            StatusFailed = 4,
            StatusDone = 5
        };

        enum APIRET   // Copied from PackageAPI.cs
        {
            SUCCESS = 0,
            FAILURE = 1,
            VIRTFILES_DB_ERROR = 2,
            VIRTFILES_ZIP_ERROR = 3,
            NOT_FOUND = 5,
            INVALID_PARAMETER = 6,
            FILE_CREATE_ERROR = 7,
            PE_RESOURCE_ERROR = 8,
            MEMORY_ERROR = 9,
            COMMIT_ERROR = 10,
            VIRTREG_DEPLOY_ERROR = 11,
            OUTPUT_ERROR = 12,
            INSUFFICIENT_BUFFER = 13,
            LOADLIBRARY_ERROR = 14,
            VIRTFILES_INI_ERROR = 15,
            APP_NOT_DEPLOYED = 16,
            INSUFFICIENT_PRIVILEGES = 17,
            _32_64_BIT_MISMATCH = 18,
            DOTNET_REQUIRED = 19,
            CANCELLED = 20,
            INJECTION_FAILED = 21,
            OLD_VERSION = 22,
            PASSWORD_REQUIRED = 23,
            PASSWORD_MISMATCH = 24,
        }

        public enum UiMode
        {
            WaitingForFile,
            WaitingForButtonClick,
            Working,
            AdditionalInfoNeeded,
            Success,
            Failure,
            DownloadingUploading,
        }

        public PkgCreateWin()
        {
            InitializeComponent();
            EnabledColor = SnapshotBtn.Foreground;
            DisabledColor = new SolidColorBrush(System.Windows.Media.Colors.LightGray);
            SetUiMode(UiMode.WaitingForFile);
        }

        public void SetUiMode(UiMode mode)
        {
            switch (mode)
            {
                case UiMode.WaitingForFile:
                    if (UploadOnlyMode)
                    {
                        StatusTxt.Text = "Drag & Drop your Cameyo package here";
                        OnlinePackagerBtn.Visibility = RemoteInstallBtn.Visibility = Visibility.Collapsed;
                        SnapshotBtn.Visibility = Visibility.Collapsed;
                        UploadBtn.Visibility = Visibility.Visible;
                        SandboxCaptureBtn.Visibility = Visibility.Collapsed;
                    }
                    //ButtonsPanel.Visibility = Visibility.Collapsed;
                    break;
                case UiMode.WaitingForButtonClick:
                    DragDropImg.Visibility = Visibility.Collapsed;
                    ButtonsPanel.Visibility = Visibility.Visible;
                    IconImg.Visibility = Visibility.Visible;
                    if (!UploadOnlyMode)
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
            // Upload-only mode
            if (UploadOnlyMode)
            {
                bool isCameyoFile = false;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (Utils.IsCameyoFile(fs))
                    {
                        isCameyoFile = true;
                    }
                    fs.Close();
                }
                if (!isCameyoFile)
                {
                    MessageBox.Show("This is not a a Cameyo package.");
                    return;
                }
            }

            if (ValidatePackagingMethods(fileName))
            {
                DisplayAssociatedIcon(fileName);
                SetUiMode(UiMode.WaitingForButtonClick);
            }
        }

        private void RemoteInstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InstallerPath)) return;
            ValidatePackagingMethods(InstallerPath);

            // ToDo: validate installer file

            // Check if online packaging is possible (i.e. account limits)
            if (!string.IsNullOrEmpty(CannotOnlinePackagerReason))
            {
                MessageBox.Show(CannotOnlinePackagerReason);
                return;
            }

            // Submit file
            string args = "";
            if (!string.IsNullOrEmpty(InstallerArgs))
                args = "&args=" + InstallerArgs;
            var url = Server.BuildUrl("RdpCapture", false, "&client=Play.WinTSC");
            url += "&" + Server.AuthUrl();
            var webClient = new WebClient();
            webClient.UploadProgressChanged += UploadProgressChanged;
            webClient.UploadFileCompleted += SubmitRdpCaptureFileUploadCompleted;
            webClient.UploadFileAsync(new Uri(url), InstallerPath);
            ProgressText("Uploading");
            SetUiMode(UiMode.Working);
        }

        private void OnlinePackagerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InstallerPath)) return;
            ValidatePackagingMethods(InstallerPath);

            // Validate cloud packaging: unattended install?
            bool silentInstallAvailable = Utils.SilentInstallAvailable(Utils.MyPath("SilentInstall.exe"), InstallerPath);
            if (!silentInstallAvailable)
            {
                if (MessageBox.Show("This file does not support unattended installation. " +
                    "You will need to finalize this request online. Would you like to proceed?",
                    "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;
            }

            // Check if online packaging is possible (i.e. account limits)
            if (!string.IsNullOrEmpty(CannotOnlinePackagerReason))
            {
                MessageBox.Show(CannotOnlinePackagerReason);
                return;
            }

            // Submit file
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
            if (string.IsNullOrEmpty(InstallerPath)) return;
            ProgressText("Capturing installation...");
            SetUiMode(UiMode.Working);
            var thread = new Thread(new ThreadStart(StartGhostCapture));
            thread.Start();
        }

        private void SnapshotBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressText("Snapshot capture in progress.");
            SetUiMode(UiMode.Working);
            PreloaderStop();   // Since Packager will be mostly waiting for user's input
            var thread = new Thread(new ThreadStart(StartPackagerCapture));
            thread.Start();
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InstallerPath)) return;
            ValidatePackagingMethods(InstallerPath);
            if (!string.IsNullOrEmpty(CannotUploadExistingPkgReason))
            {
                MessageBox.Show(CannotUploadExistingPkgReason);
                return;
            }
            PkgLocation = InstallerPath;
            SubmitFile();
        }

        void SubmitFile()
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
                SubmitFile();
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
                    ExpirationTxt.Text = "Expiration in " + Server.AccountInfo.PkgDurationDays + " days";
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

            this.Activate();
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
                                DisplayError("Packaging failed. Please try another method.");
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

                    Thread.Sleep(6 * 1000);
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

        // RdpInfoData: returned by packager.aspx?op=RdpInfo
        [DataContract]
        public class RdpInfoData
        {
            [DataMember]
            public int status { get; set; }
            [DataMember]
            public int remainingTimeSec { get; set; }
            [DataMember]
            public string pkgId { get; set; }   // Defined as string although it's a long?, because DeserializeJson throws an exception when this is ""
            [DataMember]
            public long lastStatusChangeTicks { get; set; }
        }

        public enum RdpTokenStatus
        {
            None = 0,
            Queued = 1,
            Processing = 2,
            ReadyForConnection = 3,
            ApplicationReady = 4,
            AppHasQuit = 5,
            Closed = 6,
            PkgBuilt = 7,   // For Capture mode
            Error = 0x10000000,
            ErrorPreparing = 0x10000001,
            ErrorServerUnavailable = 0x10000002,
            ErrorCapacity = 0x10000003,
        }

        string lastPkgId = null;
        void WaitForRdpCapturePkg(object data)
        {
            var rdpTokenId = (string)data;
            int retry = 0;
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                this.Hide();   // Disappear while RDP session is in progress
            }));
            while (true)
            {
                try
                {
                    var resp = Server.SendRequest("RdpInfo", false, "&token=" + rdpTokenId);
                    var rdpInfo = ServerClient.DeserializeJson<RdpInfoData>(resp);
                    if (rdpInfo != null)
                    {
                        if (rdpInfo.status == (int)RdpTokenStatus.PkgBuilt &&
                            rdpInfo.pkgId != null && rdpInfo.pkgId != lastPkgId)
                        {
                            lastPkgId = rdpInfo.pkgId;
                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                            {
                                var pkgInfo = Server.AppDetails(lastPkgId, false);
                                PkgIconPath = pkgInfo.IconUrl;  // ToDo
                                PkgAppName = pkgInfo.AppID;   // ToDo
                                PkgLocation = Server.ServerUrl() + "/apps/" + lastPkgId;   // ?auth= must be added by consuming functions

                                // Show form and bring it to front
                                this.WindowState = WindowState.Minimized;
                                this.Show();   // Restore
                                this.WindowState = WindowState.Normal;

                                DisplayResultingPkg();
                            }));
                            //break;
                        }
                        //if (rdpInfo.status >= (int)RdpTokenStatus.Error)
                        if (rdpInfo.status != (int)RdpTokenStatus.PkgBuilt &&
                            rdpInfo.status != (int)RdpTokenStatus.ApplicationReady &&   // The only other correct values
                            rdpInfo.status != (int)RdpTokenStatus.ReadyForConnection)   // The only other correct values
                        {
                            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                            {
                                this.Show();   // Restore
                                DisplayError("Failed creating package. Please retry.");
                            }));
                            break;
                        }
                    }
                    retry = 0;   // No technical error
                }
                catch (Exception ex)
                {
                    if (retry++ >= 3)
                    {
                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                        {
                            this.Show();   // Restore
                            DisplayError("Failed creating package (b). Please retry.\n" + ex.ToString());
                        }));
                        break;
                    }
                }
                Thread.Sleep(5 * 1000);
            }
        }

        // RdpAuthData: returned by packager.aspx?op=RdpCapture
        [DataContract]
        class RdpAuthParamsData
        {
            [DataMember(Name = "rdp-token")]
            public string rdp_token { get; set; }
            // Ignoring all other parameters
        }
        [DataContract]
        class RdpAuthData
        {
            [DataMember]
            public RdpAuthParamsData parameters { get; set; }
        }

        void SubmitRdpCaptureFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Upload failed: " + e.Error.Message);
                }));
                return;
            }

            // The upload is finished. Read response.
            string resp = System.Text.Encoding.ASCII.GetString(e.Result).Trim();
            if (string.IsNullOrEmpty(resp))
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Upload failed.");
                }));
                return;
            }
            if (resp.StartsWith("ERR:", StringComparison.InvariantCultureIgnoreCase))
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var errStr = resp.Substring("ERR:".Length).Trim();
                    if (errStr.Equals("Account limit reached", StringComparison.InvariantCultureIgnoreCase))
                        DisplayError("Account quota reached. Consider upgrading your account.");
                    else
                        DisplayError("Error: " + errStr);
                }));
                return;
            }

            var thread = new Thread(new ParameterizedThreadStart(StartRdpCapturePlay));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(resp);
        }

        void StartRdpCapturePlay(object data)
        {
            var resp = (string)data;

            // Transmit resulting JSON to Packager
            var rdpAuthJsonFile = System.IO.Path.GetTempFileName();
            var rdpAuth = ServerClient.DeserializeJson<RdpAuthData>(resp);
            if (rdpAuth == null)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Failed starting session.");
                }));
                return;
            }
            string rdpTokenId = rdpAuth.parameters.rdp_token;
            File.WriteAllText(rdpAuthJsonFile, resp);   // Transmit to Packager, he'll know what to do with it...

            // Start Play session
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                borderMain.Opacity = 0.3;
                var serverApp = new ServerApp() { PkgId = rdpAuthJsonFile };
                var appDisplay = new AppDisplay(serverApp);
                var playing = new Playing(appDisplay, Playing.AppAction.Play, null);
                playing.Owner = this;
                playing.ShowDialog();
                borderMain.Opacity = 1;
            }));

            // Wait for resulting package or failure
            var thread = new Thread(new ParameterizedThreadStart(WaitForRdpCapturePkg));
            thread.Start(rdpTokenId);
            //WaitForRdpCapturePkg(rdpTokenId);
            //Close();
        }

        void SubmitPkgFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    DisplayError("Upload failed: " + e.Error.Message);
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

            if (requestStatus == 0 || RequestId == 0)
            {
                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    if (errCode == (int)APIRET.INSUFFICIENT_BUFFER)            // Used for indicating account quota is over
                        DisplayError("Account quota reached. Consider upgrading your account.");
                    else if (errCode == (int)APIRET.INSUFFICIENT_PRIVILEGES)   // When modifying an existing package to which the user has no access
                        DisplayError("Insufficient permissions.");
                    else if (requestStatus == (int)PkgStatus.StatusFailed)
                        DisplayError("Packaging failed.");
                    else
                        DisplayError("Submission failed.");
                }));
                return;
            }

            var thread = new Thread(new ThreadStart(WaitForPkgCreation));
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
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
            {
                ProgressText("Uploading " + e.ProgressPercentage.ToString() + "%");
            }));
        }

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
            {
                ProgressText("Downloading " + e.ProgressPercentage.ToString() + "%");
            }));
        }

        bool ValidatePackagingMethods(string fileName)
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

                // Init
                CannotOnlinePackagerReason = CannotUploadExistingPkgReason = "";

                // Validate cloud packaging: unattended install?
                /*bool silentInstallAvailable = Utils.SilentInstallAvailable(Utils.MyPath("SilentInstall.exe"), fileName);
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
                }*/

                // Validate cloud packaging: account limitations?
                if (string.IsNullOrEmpty(CannotOnlinePackagerReason))
                {
                    var accountMaxMb = (long)Server.AccountInfo.FileUploadMbLimit * 1024 * 1024;
                    if (fi.Length >= accountMaxMb)
                    {
                        CannotOnlinePackagerReason = CannotUploadExistingPkgReason =
                            string.Format("Your account is limited to {0} MB. Consider upgrading.", Server.AccountInfo.FileUploadMbLimit); ;
                    }
                }

                // Enable / disable buttons
                if (string.IsNullOrEmpty(CannotOnlinePackagerReason))
                    OnlinePackagerBtn.Foreground = RemoteInstallBtn.Foreground = EnabledColor;
                else
                    OnlinePackagerBtn.Foreground = RemoteInstallBtn.Foreground = DisabledColor;

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
