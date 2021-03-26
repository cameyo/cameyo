using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Net;

namespace Cameyo.Player
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public class Credentials
        {
            public string Login;
            public string Password;
        }

        ServerClient Server = ServerSingleton.Instance.ServerClient;

        public Login()
        {
            Utils.RequireTls12();
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            string login = "", password = "";
            if (Server.AuthCached(ref login, ref password))
            {
                DoLogin(login, password);
            }
        }

        private void LoginAsync(object data)   // Runs on a non-UI thread
        {
            var credentials = (Credentials)data;
            bool success = false, error = false;
            try
            {
                success = Server.Auth(credentials.Login, credentials.Password, true);
            }
            catch (Exception ex)
            {
                success = false;
                error = true;
                if (ex is WebException)
                {
                    var wex = (WebException)ex;
                    if (wex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var response = wex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            if (response.StatusCode == HttpStatusCode.Forbidden)
                                error = false;   // No error; incorrect login
                        }
                    }
                }
            }
            
            // License activation
            if (!error && success)
            {
                Server.License.Type = 0;
                if (!string.IsNullOrEmpty(Server.AccountInfo.LicData))
                {
                    // -LicRegister:Machine/User 1231231231ABCABCABC
                    var packagerExe = Utils.MyPath("Packager.exe");
                    if (System.IO.File.Exists(packagerExe))
                    {
                        int exitCode = 0;   // Will contain LicenseType
                        Utils.ShellExec(packagerExe, string.Format("-Quiet \"-LicRegister:{0}/{1}\" {2}",
                            Environment.MachineName, Environment.UserName, Server.AccountInfo.LicData), ref exitCode, true, true);
                        Server.License.Type = (License.LicenseType)exitCode;
                        //str += (LicenseType ^ 0xF8).ToString("X2");
                    }
                }
            }

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                //success = true; error = false;
                PreloaderStop();
                if (error || !success)
                {
                    LoginBox.Text = credentials.Login;
                    PasswordBox.Password = credentials.Password;
                }
                if (error)
                    MessageBox.Show("Error connecting to server.");
                else
                {
                    if (success)
                    {
                        Hide();
                        //var mainWindow = new PkgCreateWin();
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                    else
                        MessageBox.Show("Wrong credentials");
                }
            }));
        }

        private void DoLogin(string login, string password)
        {
            var threadStart = new ParameterizedThreadStart(LoginAsync);
            var thread = new Thread(threadStart);
            PreloaderStart();
            thread.Start(new Credentials() { Login = login, Password = password });
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text.Trim();
            var password = PasswordBox.Password.Trim();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill your Cameyo account credentials or register a new account.");
                return;
            }
            DoLogin(login, password);
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

        private void ForgotBtn_Click(object sender, RoutedEventArgs e)
        {
            Utils.ShellExec(Server.ServerUrl() + "/forgot", false);
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            Utils.ShellExec(Server.ServerUrl() + "/register", false);
        }

        private void PreloaderStart()
        {
            AnimPanel.Visibility = System.Windows.Visibility.Visible;
            InputPanel.Visibility = System.Windows.Visibility.Hidden;
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Begin(this, true);
        }

        private void PreloaderStop()
        {
            AnimPanel.Visibility = System.Windows.Visibility.Hidden;
            InputPanel.Visibility = System.Windows.Visibility.Visible;
            var story = this.TryFindResource("PreloaderRotate") as System.Windows.Media.Animation.Storyboard;
            story.Stop(this);
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (PasswordBox.Password == "")
                PasswordHint.Visibility = Visibility.Visible;
            else
                PasswordHint.Visibility = Visibility.Collapsed;
        }
    }
}
