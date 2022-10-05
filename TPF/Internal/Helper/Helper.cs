using System;
using System.Windows;

namespace TPF.Internal
{
    internal static class Helper
    {
        internal static int GetKeyboardDelay()
        {
            int delay = SystemParameters.KeyboardDelay;
            // Werte 0,1,2,3 entsprechen 250,500,750,1000ms
            if (delay < 0 || delay > 3) delay = 0;
            return (delay + 1) * 250;
        }

        internal static int GetKeyboardSpeed()
        {
            int speed = SystemParameters.KeyboardSpeed;
            // Werte 0,...,31 entsprechen 1000/2.5=400,...,1000/30 ms
            if (speed < 0 || speed > 31) speed = 31;
            return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;
        }

        internal static Point ComputeCartesianCoordinate(Point center, double angle, double radius)
        {
            var radiansAngle = Math.PI / 180.0 * (angle - 90);
            var x = radius * Math.Cos(radiansAngle);
            var y = radius * Math.Sin(radiansAngle);

            return new Point(x + center.X, y + center.Y);
        }
    }
}