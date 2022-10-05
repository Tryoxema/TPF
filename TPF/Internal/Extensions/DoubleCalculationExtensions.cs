using System;

namespace TPF.Internal
{
    internal static class DoubleCalculationExtensions
    {
        internal static bool IsZero(this double value)
        {
            return Math.Abs(value) < 10.0 * double.Epsilon;
        }

        internal static bool IsCloseOrEqual(this double value1, double value2)
        {
            if (value1 == value2) return true;

            var epsilon = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * double.Epsilon;
            var delta = value1 - value2;

            return (-epsilon < delta) && (epsilon > delta);
        }
    }
}