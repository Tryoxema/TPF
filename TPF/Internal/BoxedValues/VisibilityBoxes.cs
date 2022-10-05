using System.Windows;

namespace TPF.Internal
{
    internal static class VisibilityBoxes
    {
        internal static object VisibleBox = Visibility.Visible;

        internal static object HiddenBox = Visibility.Hidden;

        internal static object CollapsedBox = Visibility.Collapsed;

        internal static object Box(Visibility value)
        {
            switch (value)
            {
                case Visibility.Visible: return VisibleBox;
                case Visibility.Hidden: return HiddenBox;
                case Visibility.Collapsed: 
                default: return CollapsedBox;
            }
        }
    }
}