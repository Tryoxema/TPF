using System;
using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseHookStruct
    {
        public Win32Point pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }
}