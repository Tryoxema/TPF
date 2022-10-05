using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32MinMaxInfo
    {
        public Win32Point ptReserved;
        public Win32Point ptMaxSize;
        public Win32Point ptMaxPosition;
        public Win32Point ptMinTrackSize;
        public Win32Point ptMaxTrackSize;
    };
}