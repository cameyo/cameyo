using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cameyo.Player
{
    /// <summary>
    /// Interaction logic for Playing.xaml
    /// </summary>
    public partial class Playing : Window
    {
        AppDisplay appDisplay = null;
        Process proc = null;
        string targetFileName = null;
        System.Net.WebClient webClient = null;
        ServerClient Server = ServerSingleton.Instance.ServerClient;

        public enum AppAction
        {
            None,
            Download,
            Play
        }
        AppAction appAction;

        public Playing(AppDisplay appDisplay, AppAction appAction, string targetFileName)
        {
            InitializeComponent();
            this.appDisplay = appDisplay;
            this.appAction = appAction;
            this.targetFileName = targetFileName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ParameterizedThreadStart threadStart;
            Thread thread;
            switch (appAction)
            {
                case AppAction.Download:
                    threadStart = new ParameterizedThreadStart(DownloadApp);
                    thread = new Thread(threadStart);
                    thread.Start(appDisplay);
                    break;

                case AppAction.Play:
                    threadStart = new ParameterizedThreadStart(PlayApp);
                    thread = new Thread(threadStart);
                    thread.Start(appDisplay);
                    break;
            }
        }

        public void DownloadApp(object data)
        {
            webClient = new System.Net.WebClient();
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            var url = Server.ServerUrl() + "/apps/" + appDisplay.PkgId + "/download?" + Server.AuthUrl();
            //MessageBox.Show();
            webClient.DownloadFileAsync(new Uri(url), targetFileName);
        }

        void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Close();
            }));
        }

        void webClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                ProgressLbl.Text = e.ProgressPercentage.ToString() + "%";
            }));
        }

        public void PlayApp(object data)
        {
            // Start Packager.exe -Play ...
            var appDisplay = (AppDisplay)data;
            var packagerExe = Utils.MyPath("Packager.exe");
            try
            {
                var procStartInfo = new ProcessStartInfo(packagerExe, string.Format("-Quiet -Play \"{0}\"", appDisplay.PkgId));
                procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.UseShellExecute = false;
                proc = new Process();
                proc.StartInfo = procStartInfo;
                if (proc.Start())
                {
                    proc.WaitForExit();
                    var exitCode = proc.ExitCode;
                    StopAnim("Rotate");
                    if (exitCode == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            AppStartGrid.Visibility = Visibility.Visible;
                            StartAnim("AppStart");
                        }));
                        return;
                    }
                    else
                        MessageBox.Show("Error playing app. Please try again.");
                }
                else
                    StopAnim("Rotate");
            }
            catch
            {
                MessageBox.Show("Error running Cameyo. Please try again.");
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.Close();
            }));
        }

        private void StartAnim(string storyName)
        {
            var story = this.TryFindResource(storyName) as System.Windows.Media.Animation.Storyboard;
            story.Begin(this, true);
        }

        private void StopAnim(string storyName)
        {
            var story = this.TryFindResource(storyName) as System.Windows.Media.Animation.Storyboard;
            story.Stop(this);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (proc != null)
            {
                try
                {
                    proc.Kill();
                }
                catch { }
            }
            if (appAction == AppAction.Play)
                DialogResult = true;
            else if (appAction == AppAction.Download)
            {
                webClient.CancelAsync();
                Utils.KillFile(targetFileName);
                Close();
            }
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            if (appAction == AppAction.Play)
                DialogResult = true;
            else if (appAction == AppAction.Download)
                Close();
        }

        // Window resize / drag / close
        private void DragAreaGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void AppStartStoryboard_Completed(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.Close();
            }));
        }
    }
}
