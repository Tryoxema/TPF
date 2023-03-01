using System.Windows;
using System.Windows.Media;
using System.Reflection;

namespace TPF.Internal
{
    internal static class DpiHelper
    {
        static DpiHelper()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);

            var pixelsPerInchX = (int)dpiXProperty.GetValue(null, null);
            DpiX = pixelsPerInchX;
            var pixelsPerInchY = (int)dpiYProperty.GetValue(null, null);
            DpiY = pixelsPerInchY;

            _transformToLogical = Matrix.Identity;
            _transformToLogical.Scale(96d / (double)pixelsPerInchX, 96d / (double)pixelsPerInchY);
            _transformToDevice = Matrix.Identity;
            _transformToDevice.Scale((double)pixelsPerInchX / 96d, (double)pixelsPerInchY / 96d);
        }

        internal static double DpiX = 0d;
        internal static double DpiY = 0d;

        private static Matrix _transformToDevice;
        private static Matrix _transformToLogical;

        internal static Point LogicalPixelsToDevice(Point logicalPoint)
        {
            return _transformToDevice.Transform(logicalPoint);
        }

        internal static Rect LogicalRectToDevice(Rect logicalRectangle)
        {
            Point topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top));
            Point bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom));

            return new Rect(topLeft, bottomRight);
        }
    }
}