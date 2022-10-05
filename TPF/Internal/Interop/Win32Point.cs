using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public int X;
        public int Y;

        internal Win32Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    };
}