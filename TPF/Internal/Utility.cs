using System;
using TPF.Internal.Interop;

namespace TPF.Internal
{
    internal static class Utility
    {
        // Hilfsmethode um den oberen Teil eines 32-bit Integers zu extrahieren
        internal static int HIWORD(int i)
        {
            return (short)(i >> 16);
        }

        // Hilfsmethode um den unteren Teil eines 32-bit Integers zu extrahieren
        internal static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }

        // Eine abgesicherte Methode um DeleteObject aufzurufen
        internal static void SafeDeleteObject(ref IntPtr gdiObject)
        {
            var pointer = gdiObject;

            gdiObject = IntPtr.Zero;

            if (pointer != IntPtr.Zero) NativeMethods.DeleteObject(pointer);
        }

        // Findet herraus, ob eine BitMaske in einem 32-bit Integer vorhanden ist
        internal static bool IsFlagSet(int value, int mask)
        {
            return 0 != (value & mask);
        }

        // Findet raus ob ein double auch wirklich eine Zahl ist
        internal static bool IsANumber(double value)
        {
            if (double.IsNaN(value)) return false;
            if (double.IsInfinity(value)) return false;

            return true;
        }

        // Normalisiert einen Wert für einen gewissen Zahlenbereich
        internal static double NormalizeValue(double value, double rangeStart, double rangeEnd)
        {
            if (rangeStart >= rangeEnd) return 0;

            var normalizedValue = (value - rangeStart) / (rangeEnd - rangeStart);

            return normalizedValue;
        }

        // Zwingt einen Wert dazu innerhalb des angegebenen Bereichs zu bleiben
        internal static double CoerceValue(double value, double rangeStart, double rangeEnd)
        {
            if (value < rangeStart) return rangeStart;
            if (value > rangeEnd) return rangeEnd;

            return value;
        }
    }
}