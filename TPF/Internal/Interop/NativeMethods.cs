using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace TPF.Internal.Interop
{
    internal static class NativeMethods
    {
        private const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, GWL index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, GWL index, int newStyle);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out Win32Rect lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("user32.dll")]
        internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(IntPtr hwnd, Win32WindowPlacement lpwndpl);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] Win32MonitorInfo lpmi);

        [DllImport("user32.dll")]
        internal static extern uint GetDoubleClickTime();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowDC(IntPtr window);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern uint GetPixel(IntPtr dc, int x, int y);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateRectRgnIndirect([In] ref Win32Rect lprc);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("gdi32.dll")]
        internal static extern CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, RGN fnCombineMode);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, HookCallback lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hwnd);

        [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SHAppBarMessage(ABMsg dwMessage, ref Win32AppBarData pData);

        [DllImport("ntdll.dll")]
        internal static extern void RtlGetVersion(out RTL_OSVERSIONINFOW lpVersionInformation);

        // Position des Cursors holen
        internal static Win32Point GetCursorPosition()
        {
            var point = new Win32Point();

            GetCursorPos(ref point);

            return point;
        }

        internal static Color GetColorAt(int x, int y)
        {
            var window = GetDesktopWindow();
            var dc = GetWindowDC(window);
            var pixel = (int)GetPixel(dc, x, y);

            ReleaseDC(window, dc);

            return Color.FromArgb(255, (byte)(pixel & 0xff), (byte)((pixel >> 8) & 0xff), (byte)((pixel >> 16) & 0xff));
        }

        internal static Win32WindowPlacement GetWindowPlacement(IntPtr hwnd)
        {
            var windowPlacement = new Win32WindowPlacement();

            if (!GetWindowPlacement(hwnd, windowPlacement))
            {
                throw new Win32Exception();
            }
            return windowPlacement;
        }

        internal static Win32MonitorInfo GetMonitorInfo(IntPtr hMonitor)
        {
            var monitorInfo = new Win32MonitorInfo();

            if (!GetMonitorInfo(hMonitor, monitorInfo))
            {
                throw new Win32Exception();
            }
            return monitorInfo;
        }

        internal static Version GetOsVersion()
        {
            RtlGetVersion(out var osInfo);

            if (osInfo.DwMajorVersion == 0)
            {
                return Environment.OSVersion.Version;
            }

            return new Version((int)osInfo.DwMajorVersion, (int)osInfo.DwMinorVersion, (int)osInfo.DwBuildNumber);
        }

        // Transparent-Flag im Window-Style setzen
        internal static void MakeWindowTransparent(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;

            MakeWindowTransparent(hwnd);
        }

        internal static void MakeWindowTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL.EXSTYLE);

            SetWindowLong(hwnd, GWL.EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_OSVERSIONINFOW
    {
        public uint DwOSVersionInfoSize;
        public uint DwMajorVersion;
        public uint DwMinorVersion;
        public uint DwBuildNumber;
        public uint DwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string SzCSDVersion;
    }

    internal delegate IntPtr HookCallback(int code, IntPtr wParam, IntPtr lParam);

    internal delegate IntPtr WndProc(IntPtr hwnd, WM uMsg, IntPtr wParam, IntPtr lParam);
}