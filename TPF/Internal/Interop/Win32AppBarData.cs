using System;
using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32AppBarData
    {
        public int cbSize;
        public IntPtr hWnd;
        public int uCallbackMessage;
        public int uEdge;
        public Win32Rect rc;
        public bool lParam;
    }

    internal enum ABEdge
    {
        LEFT = 0,
        TOP = 1,
        RIGHT = 2,
        BOTTOM = 3
    }

    internal enum ABMsg
    {
        GETSTATE = 4,
        GETTASKBARPOS = 5,
    }
}