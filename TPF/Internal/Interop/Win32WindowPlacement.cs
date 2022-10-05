using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal class Win32WindowPlacement
    {
        public int length = Marshal.SizeOf(typeof(Win32WindowPlacement));
        public int flags;
        public SW showCmd;
        public Win32Point ptMinPosition;
        public Win32Point ptMaxPosition;
        public Win32Rect rcNormalPosition;
    }
}