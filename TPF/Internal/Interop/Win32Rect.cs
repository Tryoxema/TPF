using System;
using System.Runtime.InteropServices;

namespace TPF.Internal.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Rect
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        public Win32Rect(int left, int top, int right, int bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public void Offset(int dx, int dy)
        {
            _left += dx;
            _top += dy;
            _right += dx;
            _bottom += dy;
        }

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        public int Width
        {
            get { return _right - _left; }
        }

        public int Height
        {
            get { return _bottom - _top; }
        }

        public Win32Point Position
        {
            get { return new Win32Point { X = _left, Y = _top }; }
        }

        public Win32Size Size
        {
            get { return new Win32Size { cx = Width, cy = Height }; }
        }

        public static Win32Rect Union(Win32Rect rect1, Win32Rect rect2)
        {
            return new Win32Rect
            {
                Left = Math.Min(rect1.Left, rect2.Left),
                Top = Math.Min(rect1.Top, rect2.Top),
                Right = Math.Max(rect1.Right, rect2.Right),
                Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
            };
        }

        public override bool Equals(object obj)
        {
            try
            {
                var rect = (Win32Rect)obj;

                return rect._bottom == _bottom
                    && rect._left == _left
                    && rect._right == _right
                    && rect._top == _top;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ((_left << 16) | Utility.LOWORD(_right)) ^ ((_top << 16) | Utility.LOWORD(_bottom));
        }
    }
}