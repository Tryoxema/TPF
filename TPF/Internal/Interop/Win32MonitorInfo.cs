using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal class Win32MonitorInfo
    {
        public int cbSize = Marshal.SizeOf(typeof(Win32MonitorInfo));
        public Win32Rect rcMonitor;
        public Win32Rect rcWork;
        public int dwFlags;
    }
}