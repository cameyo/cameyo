using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace Cameyo.OpenSrc.Common
{
    public class Utils
    {
        static public String MyPath()
        {
            return Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
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

        static public bool ShellExec(string cmd, string args, string verb, ref int exitCode, bool wait)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(cmd, args);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                procStartInfo.CreateNoWindow = false;
                procStartInfo.UseShellExecute = true;
                procStartInfo.Verb = verb;
                proc.StartInfo = procStartInfo;
                if (!proc.Start())
                    return false;
                if (wait)
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public bool ShellExec(string cmd, string args)
        {
            int exitCode = 0;
            return ShellExec(cmd, args, "open", ref exitCode, false);
        }

        static public bool ShellExec(string cmd)
        {
            return ShellExec(cmd, null);
        }

        static public long GetFileSize(String fileName)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
                return (fi.Length);
            }
            catch
            {
                return -1;
            }
        }

        static public String HexDump(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }

        public static byte[] HexUndump(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static public bool IsElevatedProcess()
        {
            try
            {
                var softwareKey = Registry.LocalMachine.OpenSubKey("Software", true);
                softwareKey.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class Win32Function
    {
        public static Icon getIconFromFile(String fileName)
        {
            /* The Icon.ExtractAssociatedIcon function does not support network drives. */
            Icon ico;
            if (fileName.StartsWith(@"\\"))
            {
                Win32imports.SHFILEINFO shinfo = new Win32imports.SHFILEINFO();
                IntPtr hIcon = Win32imports.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)(Win32imports.SHGFI.Icon | Win32imports.SHGFI.LargeIcon));
                ico = Icon.FromHandle(shinfo.hIcon);
            }
            else
            {
                ico = Icon.ExtractAssociatedIcon(fileName);
            }
            return ico;
        }

        public static string StrFormatByteSize64(ulong qdw)
        {
            StringBuilder StrSize = new StringBuilder(64);
            Win32imports.StrFormatByteSize64(qdw, StrSize, 64U);
            return StrSize.ToString();
        }
    }

    public class Win32imports
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [Flags]
        public enum SHGFI : int
        {
            /// <summary>get icon</summary>
            Icon = 0x000000100,
            /// <summary>get display name</summary>
            DisplayName = 0x000000200,
            /// <summary>get type name</summary>
            TypeName = 0x000000400,
            /// <summary>get attributes</summary>
            Attributes = 0x000000800,
            /// <summary>get icon location</summary>
            IconLocation = 0x000001000,
            /// <summary>return exe type</summary>
            ExeType = 0x000002000,
            /// <summary>get system icon index</summary>
            SysIconIndex = 0x000004000,
            /// <summary>put a link overlay on icon</summary>
            LinkOverlay = 0x000008000,
            /// <summary>show icon in selected state</summary>
            Selected = 0x000010000,
            /// <summary>get only specified attributes</summary>
            Attr_Specified = 0x000020000,
            /// <summary>get large icon</summary>
            LargeIcon = 0x000000000,
            /// <summary>get small icon</summary>
            SmallIcon = 0x000000001,
            /// <summary>get open icon</summary>
            OpenIcon = 0x000000002,
            /// <summary>get shell size icon</summary>
            ShellIconSize = 0x000000004,
            /// <summary>pszPath is a pidl</summary>
            PIDL = 0x000000008,
            /// <summary>use passed dwFileAttribute</summary>
            UseFileAttributes = 0x000000010,
            /// <summary>apply the appropriate overlays</summary>
            AddOverlays = 0x000000020,
            /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
            OverlayIndex = 0x000000040,
        }

        // Misc internal functions
        [DllImport("shlwapi")]
        public static extern int StrFormatByteSize64(ulong qdw, StringBuilder pszBuf, uint cchBuf);

        // SendMessage
        [DllImport("User32.dll")]
        public static extern UInt32 SendMessage(IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam);
        public const int LVM_FIRST = 0x1000;
        public const int LVM_SETICONSPACING = LVM_FIRST + 53;
    }

    public class PleaseWait
    {
        #region PleaseWaitDialog
        private class PleaseWaitMsg
        {
            public String iconFileName;
            public String title;
            public String msg;
            public PleaseWaitMsg(String title, String msg, String iconFileName)
            {
                this.title = title;
                this.msg = msg;
                this.iconFileName = iconFileName;
            }
        }

        //System.Threading.AutoResetEvent pleaseWaitDialogEvent;
        private class PleaseWaitDialog
        {
            private PictureBox icon;
            private Label msg;
            private Form dialog;
            Icon iconFile;
            //PleaseWaitMsg pleaseWaitMsg;

            private void InitializeComponent()
            {
                icon = new PictureBox();
                icon.Location = new Point(12, 12);
                icon.Size = new Size(48, 48);

                msg = new Label();
                msg.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                msg.AutoSize = true;
                msg.Location = new Point(70, 12);

                dialog = new Form();
                dialog.ClientSize = new Size(400, 70);
                dialog.Controls.Add(this.msg);
                dialog.Controls.Add(this.icon);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ControlBox = false;
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.TopMost = true;
            }

            public PleaseWaitDialog()
            {
                InitializeComponent();
            }

            public void Display(PleaseWaitMsg pleaseWaitMsg)
            {
                try
                {
                    if (!String.IsNullOrEmpty(pleaseWaitMsg.iconFileName))
                    {
                        iconFile = Win32Function.getIconFromFile(pleaseWaitMsg.iconFileName);
                        icon.Image = iconFile.ToBitmap();
                    }
                }
                catch { }
                dialog.Text = pleaseWaitMsg.title;
                msg.Text = pleaseWaitMsg.msg;
                dialog.ClientSize = new Size(Math.Max(msg.Width + 100, 250), 70);
                msg.Location = new Point(dialog.ClientSize.Width / 2 - msg.Width / 2, 12);
                dialog.Show(null);
                try
                {
                    EventWaitHandle pleaseWaitDialogEvent = AutoResetEvent.OpenExisting("pleaseWaitDialogEvent");
                    while (!pleaseWaitDialogEvent.WaitOne(10, false))
                        Application.DoEvents();
                }
                catch { }
            }
        }
        #endregion

        static public void PleaseWaitJob(object data)
        {
            PleaseWaitMsg pleaseWaitMsg = (PleaseWaitMsg)data;
            PleaseWait.PleaseWaitDialog pleaseWaitDialog = new PleaseWaitDialog();
            pleaseWaitDialog.Display(pleaseWaitMsg);
        }

        static public EventWaitHandle PleaseWaitBegin(String title, String msg, String iconFileName)
        {
            EventWaitHandle pleaseWaitDialogEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "pleaseWaitDialogEvent");
            Thread thread = new Thread(new ParameterizedThreadStart(PleaseWaitJob));
            PleaseWaitMsg pleaseWaitMsg = new PleaseWaitMsg(title, msg, iconFileName);
            thread.Start(pleaseWaitMsg);
            Thread.Sleep(500);
            return pleaseWaitDialogEvent;
        }

        static public void PleaseWaitEnd()
        {
            try
            {
                EventWaitHandle pleaseWaitDialogEvent = EventWaitHandle.OpenExisting("pleaseWaitDialogEvent");
                pleaseWaitDialogEvent.Set();
            }
            catch { }
        }
    }

    public enum ListViewExtendedStyles
    {
        /// <summary>
        /// LVS_EX_GRIDLINES
        /// </summary>
        GridLines = 0x00000001,
        /// <summary>
        /// LVS_EX_SUBITEMIMAGES
        /// </summary>
        SubItemImages = 0x00000002,
        /// <summary>
        /// LVS_EX_CHECKBOXES
        /// </summary>
        CheckBoxes = 0x00000004,
        /// <summary>
        /// LVS_EX_TRACKSELECT
        /// </summary>
        TrackSelect = 0x00000008,
        /// <summary>
        /// LVS_EX_HEADERDRAGDROP
        /// </summary>
        HeaderDragDrop = 0x00000010,
        /// <summary>
        /// LVS_EX_FULLROWSELECT
        /// </summary>
        FullRowSelect = 0x00000020,
        /// <summary>
        /// LVS_EX_ONECLICKACTIVATE
        /// </summary>
        OneClickActivate = 0x00000040,
        /// <summary>
        /// LVS_EX_TWOCLICKACTIVATE
        /// </summary>
        TwoClickActivate = 0x00000080,
        /// <summary>
        /// LVS_EX_FLATSB
        /// </summary>
        FlatsB = 0x00000100,
        /// <summary>
        /// LVS_EX_REGIONAL
        /// </summary>
        Regional = 0x00000200,
        /// <summary>
        /// LVS_EX_INFOTIP
        /// </summary>
        InfoTip = 0x00000400,
        /// <summary>
        /// LVS_EX_UNDERLINEHOT
        /// </summary>
        UnderlineHot = 0x00000800,
        /// <summary>
        /// LVS_EX_UNDERLINECOLD
        /// </summary>
        UnderlineCold = 0x00001000,
        /// <summary>
        /// LVS_EX_MULTIWORKAREAS
        /// </summary>
        MultilWorkAreas = 0x00002000,
        /// <summary>
        /// LVS_EX_LABELTIP
        /// </summary>
        LabelTip = 0x00004000,
        /// <summary>
        /// LVS_EX_BORDERSELECT
        /// </summary>
        BorderSelect = 0x00008000,
        /// <summary>
        /// LVS_EX_DOUBLEBUFFER
        /// </summary>
        DoubleBuffer = 0x00010000,
        /// <summary>
        /// LVS_EX_HIDELABELS
        /// </summary>
        HideLabels = 0x00020000,
        /// <summary>
        /// LVS_EX_SINGLEROW
        /// </summary>
        SingleRow = 0x00040000,
        /// <summary>
        /// LVS_EX_SNAPTOGRID
        /// </summary>
        SnapToGrid = 0x00080000,
        /// <summary>
        /// LVS_EX_SIMPLESELECT
        /// </summary>
        SimpleSelect = 0x00100000
    }

    public enum ListViewMessages
    {
        First = 0x1000,
        SetExtendedStyle = (First + 54),
        GetExtendedStyle = (First + 55),
    }

    /// <summary>
    /// Contains helper methods to change extended styles on ListView, including enabling double buffering.
    /// Based on Giovanni Montrone's article on <see cref="http://www.codeproject.com/KB/list/listviewxp.aspx"/>
    /// </summary>
    public class ListViewHelper
    {
        private ListViewHelper()
        {
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr handle, int messg, int wparam, int lparam);

        public static void SetExtendedStyle(Control control, ListViewExtendedStyles exStyle)
        {
            ListViewExtendedStyles styles;
            styles = (ListViewExtendedStyles)SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
            styles |= exStyle;
            SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (int)styles);
        }

        public static void EnableDoubleBuffer(Control control)
        {
            ListViewExtendedStyles styles;
            // read current style
            styles = (ListViewExtendedStyles)SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
            // enable double buffer and border select
            styles |= ListViewExtendedStyles.DoubleBuffer | ListViewExtendedStyles.BorderSelect;
            // write new style
            SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (int)styles);
        }

        public static void DisableDoubleBuffer(Control control)
        {
            ListViewExtendedStyles styles;
            // read current style
            styles = (ListViewExtendedStyles)SendMessage(control.Handle, (int)ListViewMessages.GetExtendedStyle, 0, 0);
            // disable double buffer and border select
            styles -= styles & ListViewExtendedStyles.DoubleBuffer;
            styles -= styles & ListViewExtendedStyles.BorderSelect;
            // write new style
            SendMessage(control.Handle, (int)ListViewMessages.SetExtendedStyle, 0, (int)styles);
        }
    }

    public class LangItem
    {
        public string DisplayName;
        public string ShortDisplayName;
        public string Culture;

        public LangItem(string displayName, string shortDisplayName, string culture)
        {
            this.DisplayName = displayName;
            this.ShortDisplayName = shortDisplayName;
            this.Culture = culture;
        }

        static public List<LangItem> SupportedLangs()
        {
            List<LangItem> langs = new List<LangItem>();
            langs.Add(new LangItem("English", "EN", "en-US"));
            langs.Add(new LangItem("Français", "FR", "fr-FR"));
            langs.Add(new LangItem("Español", "ES", "es-ES"));
            langs.Add(new LangItem("Deutsch", "DE", "de-DE"));
            langs.Add(new LangItem("Italian", "IT", "it-IT"));
            langs.Add(new LangItem("Turkish", "TR", "tr-TR"));
            langs.Add(new LangItem("Chinese", "CN", "zh-CN"));
            langs.Add(new LangItem("Polish", "PL", "pl-PL"));
            langs.Add(new LangItem("Arabic", "AR", "ar-SA"));
            langs.Add(new LangItem("Indonesian", "ID", "id-ID"));
            langs.Add(new LangItem("Afrikaans", "ZA", "af-ZA"));
            return langs;
        }

        static public LangItem FromDisplayName(string displayName)
        {
            List<LangItem> langs = SupportedLangs();
            foreach (LangItem lang in langs)
            {
                if (lang.DisplayName.Equals(displayName, StringComparison.InvariantCulture))
                    return lang;
            }
            return langs[0];
        }

        static public LangItem FromShortDisplayName(string shortDisplayName)
        {
            List<LangItem> langs = SupportedLangs();
            foreach (LangItem lang in langs)
            {
                if (lang.ShortDisplayName.Equals(shortDisplayName, StringComparison.InvariantCulture))
                    return lang;
            }
            return langs[0];
        }

        static public LangItem FromCulture(string culture)
        {
            List<LangItem> langs = SupportedLangs();
            foreach (LangItem lang in langs)
            {
                if (lang.Culture.Equals(culture, StringComparison.InvariantCulture))
                    return lang;
            }
            return langs[0];
        }
    }

    public class LangUtils
    {
        static public void LoadCulture()
        {
            System.Globalization.CultureInfo culture;
            try
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Cameyo", false);
                string cultureStr = (string)regKey.GetValue("Culture");
                culture = System.Globalization.CultureInfo.CreateSpecificCulture(cultureStr);
            }
            catch 
            {
                culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            }
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        }

        static public void SaveCulture(string cultureStr)
        {
            try
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Cameyo");
                regKey.SetValue("Culture", cultureStr);
            }
            catch { }
        }
    }

    public class Win64
    {
        static public bool Is64BitProcess()
        {
            return (IntPtr.Size == 8);
        }

        static public bool IsWin64()
        {
            return Is64BitProcess() || InternalCheckIsWow64();
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
