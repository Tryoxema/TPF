using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Size
    {
        public int cx;
        public int cy;
    }
}