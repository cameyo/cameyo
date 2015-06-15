using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using MsdnMag;			// For LocalCbtHook
using Microsoft.Win32;	// For RegKey

namespace MsgBoxCheck
{
    #region Class HookEventArgs
    public class HookEventArgs : EventArgs
    {
        public int HookCode;	// Hook code
        public IntPtr wParam;	// WPARAM argument
        public IntPtr lParam;	// LPARAM argument
    }
    #endregion

    #region Enum HookType
    // Hook Types
    public enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }
    #endregion

    #region Class LocalWindowsHook
    public class LocalWindowsHook
    {
        // ************************************************************************
        // Filter function delegate
        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
        // ************************************************************************

        // ************************************************************************
        // Internal properties
        protected IntPtr m_hhook = IntPtr.Zero;
        protected HookProc m_filterFunc = null;
        protected HookType m_hookType;
        // ************************************************************************

        // ************************************************************************
        // Event delegate
        public delegate void HookEventHandler(object sender, HookEventArgs e);
        // ************************************************************************

        // ************************************************************************
        // Event: HookInvoked 
        public event HookEventHandler HookInvoked;
        protected void OnHookInvoked(HookEventArgs e)
        {
            if (HookInvoked != null)
                HookInvoked(this, e);
        }
        // ************************************************************************

        // ************************************************************************
        // Class constructor(s)
        public LocalWindowsHook(HookType hook)
        {
            m_hookType = hook;
            m_filterFunc = new HookProc(this.CoreHookProc);
        }
        public LocalWindowsHook(HookType hook, HookProc func)
        {
            m_hookType = hook;
            m_filterFunc = func;
        }
        // ************************************************************************

        // ************************************************************************
        // Default filter function
        protected int CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
                return CallNextHookEx(m_hhook, code, wParam, lParam);

            // Let clients determine what to do
            HookEventArgs e = new HookEventArgs();
            e.HookCode = code;
            e.wParam = wParam;
            e.lParam = lParam;
            OnHookInvoked(e);

            // Yield to the next hook in the chain
            return CallNextHookEx(m_hhook, code, wParam, lParam);
        }
        // ************************************************************************

        // ************************************************************************
        // Install the hook
        public void Install()
        {
            m_hhook = SetWindowsHookEx(
                m_hookType,
                m_filterFunc,
                IntPtr.Zero,
#pragma warning disable 618
                (int)AppDomain.GetCurrentThreadId());   // CS0618 is OK
#pragma warning restore 618
        }
        // ************************************************************************

        // ************************************************************************
        // Uninstall the hook
        public void Uninstall()
        {
            UnhookWindowsHookEx(m_hhook);
        }
        // ************************************************************************


        #region Win32 Imports
        // ************************************************************************
        // Win32: SetWindowsHookEx()
        [DllImport("user32.dll")]
        protected static extern IntPtr SetWindowsHookEx(HookType code,
            HookProc func,
            IntPtr hInstance,
            int threadID);
        // ************************************************************************

        // ************************************************************************
        // Win32: UnhookWindowsHookEx()
        [DllImport("user32.dll")]
        protected static extern int UnhookWindowsHookEx(IntPtr hhook);
        // ************************************************************************

        // ************************************************************************
        // Win32: CallNextHookEx()
        [DllImport("user32.dll")]
        protected static extern int CallNextHookEx(IntPtr hhook,
            int code, IntPtr wParam, IntPtr lParam);
        // ************************************************************************
        #endregion
    }
    #endregion

    #region Enum CbtHookAction
    // CBT hook actions
    public enum CbtHookAction : int
    {
        HCBT_MOVESIZE = 0,
        HCBT_MINMAX = 1,
        HCBT_QS = 2,
        HCBT_CREATEWND = 3,
        HCBT_DESTROYWND = 4,
        HCBT_ACTIVATE = 5,
        HCBT_CLICKSKIPPED = 6,
        HCBT_KEYSKIPPED = 7,
        HCBT_SYSCOMMAND = 8,
        HCBT_SETFOCUS = 9
    }
    #endregion


    #region Class CbtEventArgs
    public class CbtEventArgs : EventArgs
    {
        public IntPtr Handle;			// Win32 handle of the window
        public string Title;			// caption of the window
        public string ClassName;		// class of the window
        public bool IsDialogWindow;		// whether is a popup dialog
    }
    #endregion


    #region Class LocalCbtHook
    public class LocalCbtHook : LocalWindowsHook
    {
        // ************************************************************************
        // Event delegate
        public delegate void CbtEventHandler(object sender, CbtEventArgs e);
        // ************************************************************************

        // ************************************************************************
        // Events 
        public event CbtEventHandler WindowCreated;
        public event CbtEventHandler WindowDestroyed;
        public event CbtEventHandler WindowActivated;
        // ************************************************************************


        // ************************************************************************
        // Internal properties
        protected IntPtr m_hwnd = IntPtr.Zero;
        protected string m_title = "";
        protected string m_class = "";
        protected bool m_isDialog = false;
        // ************************************************************************


        // ************************************************************************
        // Class constructor(s)
        public LocalCbtHook()
            : base(HookType.WH_CBT)
        {
            this.HookInvoked += new HookEventHandler(CbtHookInvoked);
        }
        public LocalCbtHook(HookProc func)
            : base(HookType.WH_CBT, func)
        {
            this.HookInvoked += new HookEventHandler(CbtHookInvoked);
        }
        // ************************************************************************


        // ************************************************************************
        // Handles the hook event
        private void CbtHookInvoked(object sender, HookEventArgs e)
        {
            CbtHookAction code = (CbtHookAction)e.HookCode;
            IntPtr wParam = e.wParam;
            IntPtr lParam = e.lParam;

            // Handle hook events (only a few of available actions)
            switch (code)
            {
                case CbtHookAction.HCBT_CREATEWND:
                    HandleCreateWndEvent(wParam, lParam);
                    break;
                case CbtHookAction.HCBT_DESTROYWND:
                    HandleDestroyWndEvent(wParam, lParam);
                    break;
                case CbtHookAction.HCBT_ACTIVATE:
                    HandleActivateEvent(wParam, lParam);
                    break;
            }

            return;
        }
        // ************************************************************************


        // ************************************************************************
        // Handle the CREATEWND hook event
        private void HandleCreateWndEvent(IntPtr wParam, IntPtr lParam)
        {
            // Cache some information
            UpdateWindowData(wParam);

            // raise event
            OnWindowCreated();
        }
        // ************************************************************************


        // ************************************************************************
        // Handle the DESTROYWND hook event
        private void HandleDestroyWndEvent(IntPtr wParam, IntPtr lParam)
        {
            // Cache some information
            UpdateWindowData(wParam);

            // raise event
            OnWindowDestroyed();
        }
        // ************************************************************************


        // ************************************************************************
        // Handle the ACTIVATE hook event
        private void HandleActivateEvent(IntPtr wParam, IntPtr lParam)
        {
            // Cache some information
            UpdateWindowData(wParam);

            // raise event
            OnWindowActivated();
        }
        // ************************************************************************


        // ************************************************************************
        // Read and store some information about the window
        private void UpdateWindowData(IntPtr wParam)
        {
            // Cache the window handle
            m_hwnd = wParam;

            // Cache the window's class name
            StringBuilder sb1 = new StringBuilder();
            sb1.Capacity = 40;
            GetClassName(m_hwnd, sb1, 40);
            m_class = sb1.ToString();

            // Cache the window's title bar
            StringBuilder sb2 = new StringBuilder();
            sb2.Capacity = 256;
            GetWindowText(m_hwnd, sb2, 256);
            m_title = sb2.ToString();

            // Cache the dialog flag
            m_isDialog = (m_class == "#32770");
        }
        // ************************************************************************


        // ************************************************************************
        // Helper functions that fire events by executing user code
        protected virtual void OnWindowCreated()
        {
            if (WindowCreated != null)
            {
                CbtEventArgs e = new CbtEventArgs();
                PrepareEventData(e);
                WindowCreated(this, e);
            }
        }
        protected virtual void OnWindowDestroyed()
        {
            if (WindowDestroyed != null)
            {
                CbtEventArgs e = new CbtEventArgs();
                PrepareEventData(e);
                WindowDestroyed(this, e);
            }
        }
        protected virtual void OnWindowActivated()
        {
            if (WindowActivated != null)
            {
                CbtEventArgs e = new CbtEventArgs();
                PrepareEventData(e);
                WindowActivated(this, e);
            }
        }
        // ************************************************************************


        // ************************************************************************
        // Prepare the event data structure
        private void PrepareEventData(CbtEventArgs e)
        {
            e.Handle = m_hwnd;
            e.Title = m_title;
            e.ClassName = m_class;
            e.IsDialogWindow = m_isDialog;
        }
        // ************************************************************************



        #region Win32 Imports
        // ************************************************************************
        // Win32: GetClassName
        [DllImport("user32.dll")]
        protected static extern int GetClassName(IntPtr hwnd,
            StringBuilder lpClassName, int nMaxCount);
        // ************************************************************************

        // ************************************************************************
        // Win32: GetWindowText
        [DllImport("user32.dll")]
        protected static extern int GetWindowText(IntPtr hwnd,
            StringBuilder lpString, int nMaxCount);
        // ************************************************************************
        #endregion
    }
    #endregion

    public class MessageBox
    {
        protected LocalCbtHook m_cbt;
        protected IntPtr m_hwnd = IntPtr.Zero;
        protected IntPtr m_hwndBtn = IntPtr.Zero;
        protected bool m_bInit = false;
        protected bool m_bCheck = false;
        protected string m_strCheck;

        public MessageBox()
        {
            m_cbt = new LocalCbtHook();
            m_cbt.WindowCreated += new LocalCbtHook.CbtEventHandler(WndCreated);
            m_cbt.WindowDestroyed += new LocalCbtHook.CbtEventHandler(WndDestroyed);
            m_cbt.WindowActivated += new LocalCbtHook.CbtEventHandler(WndActivated);
        }

        public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(strKey);
            try
            {
                if (Convert.ToBoolean(regKey.GetValue(strValue, false)))
                    return dr;
            }
            catch
            {
                // No processing needed...the convert might throw an exception,
                // but if so we proceed as if the value was false.
            }

            m_strCheck = strCheck;
            m_cbt.Install();
            dr = System.Windows.Forms.MessageBox.Show(strText, strTitle, buttons, icon);
            m_cbt.Uninstall();

            regKey.SetValue(strValue, m_bCheck);
            return dr;
        }

        public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle, MessageBoxButtons buttons)
        {
            return Show(strKey, strValue, dr, strCheck, strText, strTitle, buttons, MessageBoxIcon.None);
        }

        public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText, string strTitle)
        {
            return Show(strKey, strValue, dr, strCheck, strText, strTitle, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public DialogResult Show(string strKey, string strValue, DialogResult dr, string strCheck, string strText)
        {
            return Show(strKey, strValue, dr, strCheck, strText, "", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void WndCreated(object sender, CbtEventArgs e)
        {
            if (e.IsDialogWindow)
            {
                m_bInit = false;
                m_hwnd = e.Handle;
            }
        }

        private void WndDestroyed(object sender, CbtEventArgs e)
        {
            if (e.Handle == m_hwnd)
            {
                m_bInit = false;
                m_hwnd = IntPtr.Zero;
                if (BST_CHECKED == (int)SendMessage(m_hwndBtn, BM_GETCHECK, IntPtr.Zero, IntPtr.Zero))
                    m_bCheck = true;
            }
        }

        private void WndActivated(object sender, CbtEventArgs e)
        {
            if (m_hwnd != e.Handle)
                return;

            // Not the first time
            if (m_bInit)
                return;
            else
                m_bInit = true;

            // Get the current font, either from the static text window
            // or the message box itself
            IntPtr hFont;
            IntPtr hwndText = GetDlgItem(m_hwnd, 0xFFFF);
            if (hwndText != IntPtr.Zero)
                hFont = SendMessage(hwndText, WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
            else
                hFont = SendMessage(m_hwnd, WM_GETFONT, IntPtr.Zero, IntPtr.Zero);
            Font fCur = Font.FromHfont(hFont);

            // Get the x coordinate for the check box.  Align it with the icon if possible,
            // or one character height in
            int x = 0;
            IntPtr hwndIcon = GetDlgItem(m_hwnd, 0x0014);
            if (hwndIcon != IntPtr.Zero)
            {
                RECT rcIcon = new RECT();
                GetWindowRect(hwndIcon, rcIcon);
                POINT pt = new POINT();
                pt.x = rcIcon.left;
                pt.y = rcIcon.top;
                ScreenToClient(m_hwnd, pt);
                x = pt.x;
            }
            else
                x = (int)fCur.GetHeight();

            // Get the y coordinate for the check box, which is the bottom of the
            // current message box client area
            RECT rc = new RECT();
            GetClientRect(m_hwnd, rc);
            int y = rc.bottom - rc.top;

            // Resize the message box with room for the check box
            GetWindowRect(m_hwnd, rc);
            MoveWindow(m_hwnd, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top + (int)fCur.GetHeight() * 2, true);

            m_hwndBtn = CreateWindowEx(0, "button", m_strCheck, BS_AUTOCHECKBOX | WS_CHILD | WS_VISIBLE | WS_TABSTOP,
                x, y, rc.right - rc.left - x, (int)fCur.GetHeight(),
                m_hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            SendMessage(m_hwndBtn, WM_SETFONT, hFont, new IntPtr(1));
        }

        #region Win32 Imports
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_CHILD = 0x40000000;
        private const int WS_TABSTOP = 0x00010000;
        private const int WM_SETFONT = 0x00000030;
        private const int WM_GETFONT = 0x00000031;
        private const int BS_AUTOCHECKBOX = 0x00000003;
        private const int BM_GETCHECK = 0x00F0;
        private const int BST_CHECKED = 0x0001;

        [DllImport("user32.dll")]
        protected static extern void DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetDlgItem(IntPtr hwnd, int id);

        [DllImport("user32.dll")]
        protected static extern int GetWindowRect(IntPtr hwnd, RECT rc);

        [DllImport("user32.dll")]
        protected static extern int GetClientRect(IntPtr hwnd, RECT rc);

        [DllImport("user32.dll")]
        protected static extern void MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        protected static extern int ScreenToClient(IntPtr hwnd, POINT pt);

        [DllImport("user32.dll", EntryPoint = "MessageBox")]
        protected static extern int _MessageBox(IntPtr hwnd, string text, string caption,
            int options);

        [DllImport("user32.dll")]
        protected static extern IntPtr SendMessage(IntPtr hwnd,
            int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        protected static extern IntPtr CreateWindowEx(
            int dwExStyle,			// extended window style
            string lpClassName,		// registered class name
            string lpWindowName,	// window name
            int dwStyle,			// window style
            int x,					// horizontal position of window
            int y,					// vertical position of window
            int nWidth,				// window width
            int nHeight,			// window height
            IntPtr hWndParent,      // handle to parent or owner window
            IntPtr hMenu,			// menu handle or child identifier
            IntPtr hInstance,		// handle to application instance
            IntPtr lpParam			// window-creation data
            );

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion
    }
}
