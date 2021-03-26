using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cameyo.Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerClient Server = ServerSingleton.Instance.ServerClient;
        List<ServerApp> Apps = new List<ServerApp>();
        string CurLib;

        public MainWindow()
        {
            InitializeComponent();

            //ProxyAuth.AuthIfNeeded();

            // XP: avoid message "Could not establish trust relationship for SSL/TLS secure channel"
            if (Environment.OSVersion.Version.Major == 5)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Libs
            var libs = Server.AccountInfo.libs;
            var curLib = libs[0];

            // CategorySplitBtn: libs
            CategorySplitBtn.Content = curLib.DisplayName;
            CategorySplitBtn.Items.Clear();
            foreach (var lib in libs)
            {
                var menuItem = new MenuItem() { Tag = lib, Header = lib.DisplayName };
                menuItem.Click += OnLibSelection;
                CategorySplitBtn.ContextMenu.Items.Add(menuItem);
            }
            SearchBox.TextChanged += SearchBox_TextChanged;

            LoginBtn.Content = Server.Login;
            if (!string.IsNullOrEmpty(Server.AccountInfo.StorageProviderName))
                StorageBtn.Content = Server.AccountInfo.StorageProviderName;
            else
                StorageBtn.Content = "No storage";
            OnLibSelect(curLib.Id, true);   // Select first lib
            ShowDetails(null);   // Hide the details pane
            ProdType.Text = Server.License.ProdTypeStr().ToUpper();
        }

        void OnLibSelect(string libId, bool allowCache)
        {
            CurLib = libId;
            var apps = Server.PkgList(libId, allowCache);
            Apps.Clear();
            lvApps.Items.Clear();
            NoAppsLbl.Visibility = (apps.Count == 0 ? Visibility.Visible : Visibility.Hidden);
            foreach (var app in apps)
            {
                Apps.Add(app);
                lvApps.Items.Add(new AppDisplay(app));
            }

            // Start LoadIconsAsync
            var threadStart = new ParameterizedThreadStart(LoadIconsAsync);
            var thread = new Thread(threadStart);
            var lvAppItems = new List<AppDisplay>();
            for (int i = 0; i < lvApps.Items.Count; i++)
                lvAppItems.Add((AppDisplay)(lvApps.Items[i]));
            thread.Start(lvAppItems);
        }

        void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTxt = SearchBox.Text;
            var query = from n in Apps where n.AppID.ToLower().Contains(searchTxt) select n;
            lvApps.Items.Clear();
            foreach (var app in query)
                lvApps.Items.Add(new AppDisplay(app));
        }

        // Lib Selection
        void OnLibSelection(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            MenuItem menuItem = e.OriginalSource as MenuItem;
            var lib = (ServerLib)(menuItem.Tag);
            CategorySplitBtn.Content = lib.DisplayName;
            OnLibSelect(lib.Id, true);
        }

        // Storage click
        void OnStorageClick(object sender, RoutedEventArgs e)
        {
            Utils.ShellExec(Server.ServerUrl() + "/profile", false);
        }

        // Pkg creation click
        void OnPkgCreateClick(object sender, RoutedEventArgs e)
        {
            var pkgCreateWin = new PkgCreateWin();
            pkgCreateWin.Owner = this;
            pkgCreateWin.Show();
            /*string url = Server.ServerUrl() + "/add";
            url += "?" + Server.AuthUrl();
            Utils.ShellExec(url, false);*/
        }

        // Pkg upload click
        void OnPkgUploadClick(object sender, RoutedEventArgs e)
        {
            var pkgCreateWin = new PkgCreateWin();
            pkgCreateWin.Owner = this;
            pkgCreateWin.UploadOnlyMode = true;
            pkgCreateWin.SetUiMode(PkgCreateWin.UiMode.WaitingForFile);
            pkgCreateWin.Show();
        }

        // Profile settings click
        void OnProfileSettingsClick(object sender, RoutedEventArgs e)
        {
            string url = Server.ServerUrl() + "/profile";
            url += "?" + Server.AuthUrl();
            Utils.ShellExec(url, false);
        }

        // Logout click
        private void OnLogoutClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Would you like to log out?", "Logout", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            Server.DeleteUserCache();
            Hide();
            var login = new Login();
            login.Show();
            Close();
        }

        // App selection
        private void OnAppSelection(object sender, SelectionChangedEventArgs e)
        {
            var appDisplay = (AppDisplay)lvApps.SelectedItem;
            ShowDetails(appDisplay);
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var app = (AppDisplay)lvApps.SelectedItem;
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = app.Name + ".cameyo";            // Default file name
            dlg.DefaultExt = ".exe";                        // Default file extension
            dlg.Filter = "Executable files (.exe)|*.exe";   // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                var playing = new Playing((AppDisplay)lvApps.SelectedItem, Playing.AppAction.Download, dlg.FileName);
                playing.Owner = this;
                playing.Show();
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            // XP: avoid message "Could not establish trust relationship for SSL/TLS secure channel"
            // Packager.exe -Play doesn't work on XP: "CHttpClient:SetLocation: InternetOpenUrl failed, LE=12031"
            if (Environment.OSVersion.Version.Major == 5)
            {
                MessageBox.Show("Playing apps is not supported on Windows XP");
                return;
            }

            borderMain.Opacity = 0.3;
            var playing = new Playing((AppDisplay)lvApps.SelectedItem, Playing.AppAction.Play, null);
            playing.Owner = this;
            playing.ShowDialog();
            borderMain.Opacity = 1;
        }

        private void ShowDetailsAsync(object data)
        {
            var appDisplay = (AppDisplay)data;
            var appDetails = Server.AppDetails(appDisplay.PkgId, true);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                if (DetailsPkgId.Text.Contains(appDisplay.PkgId))   // Are we still displaying the same app?
                {
                    if (appDetails.ShortDesc == null || appDetails.ShortDesc.Trim() == "")
                        DetailsDescription.Text = "No description available.";
                    else
                        DetailsDescription.Text = appDetails.ShortDesc;
                    PreloaderStop();
                }
            }));
        }

        private void ShowDetails(AppDisplay appDisplay)
        {
            if (appDisplay == null)
            {
                AppsGrid.RowDefinitions[2].Height = new GridLength(0);
                return;
            }
            AppsGrid.RowDefinitions[2].Height = new GridLength(200);

            // Fill in available info
            if (File.Exists(appDisplay.ImagePath))
            {
                try
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(appDisplay.ImagePath);
                    img.EndInit();
                    DetailsImg.Source = img;
                }
                catch 
                {
                    DetailsImg.Source = null;
                }
            }
            else
                DetailsImg.Source = null;

            //DetailsVersion.Text = "v" + appDisplay.Version;
            DetailsName.Text = appDisplay.Name;

            DetailsPkgId.Text = "#" + appDisplay.PkgId;         // Used also to synchronize between this routine and ShowDetailsAsync
            //DetailsSize.Text = Utils.BytesToStr(appDisplay.Size);
            //DetailsVersion.Text = "v" + appDisplay.Version;
            DetailsSize.Visibility = Visibility.Collapsed;      // Deprecated
            DetailsVersion.Visibility = Visibility.Collapsed;   // Deprecated
            DownloadBtn.Visibility = Visibility.Collapsed;      // Deprecated

            // Empty async items
            DetailsDescription.Text = "";

            // Start ShowDetailsAsync
            PreloaderStart();
            var threadStart = new ParameterizedThreadStart(ShowDetailsAsync);
            var thread = new Thread(threadStart);
            thread.Start(appDisplay);
        }

        void LoadIconsAsync(object data)
        {
            Directory.CreateDirectory(Server.ImgCacheDir());
            var lvAppItems = (List<AppDisplay>)data;
            for (int i = 0; i < lvAppItems.Count; i++)
            {
                var lvAppItem = lvAppItems[i];
                if (!lvAppItem.ImageLoaded)
                {
                    if (!Server.DownloadIcon(lvAppItem.ImageUrl, lvAppItem.PkgId))
                        continue;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        ((AppDisplay)lvAppItems[i]).ImagePath = Server.LocalIconFile(lvAppItem.PkgId);
                        ((AppDisplay)lvAppItems[i]).ImageLoaded = true;
                    }));
                }
            }
        }

        // Window resize / drag / close
        private void DragAreaGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void MaxRestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            // Webdings: "1" = Maximize, "2" = Restore
            string maximizeTxt = "1", restoreTxt = "2";
            if (MaxRestoreTxt.Text == maximizeTxt)
            {
                WindowState = WindowState.Maximized;
                MaxRestoreTxt.Text = restoreTxt;
            }
            else
            {
                WindowState = WindowState.Normal;
                MaxRestoreTxt.Text = maximizeTxt;
            }
        }
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void PreloaderStart()
        {
            ellipse.Visibility = System.Windows.Visibility.Visible;
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Begin(this, true);
        }

        private void PreloaderStop()
        {
            ellipse.Visibility = System.Windows.Visibility.Hidden;
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Stop(this);
        }

        // XP: avoid message "Could not establish trust relationship for SSL/TLS secure channel"
        private static bool ValidateRemoteCertificate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert,
            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors policyErrors)
        {
            bool result = true;
            if (sender is System.Net.HttpWebRequest)
            {
                string requestServer = ((System.Net.HttpWebRequest)sender).RequestUri.ToString();
                if (requestServer.Equals("online.cameyo.com") && !cert.Subject.ToLower().Contains("cn=online.cameyo.com"))
                    result = false;
            }
            return result;
        }

        public void ForceMyLibraryRefresh()
        {
            foreach (var lib in Server.AccountInfo.libs)
            {
                if (!lib.Id.Equals("public", StringComparison.InvariantCultureIgnoreCase))
                    Server.PkgList(lib.Id, false);   // Force cache refresh
            }
            OnLibSelect(CurLib, true);
        }
    }
}
