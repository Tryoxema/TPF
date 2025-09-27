using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class CalloutPopupOptions
    {
        public CalloutPlacement Placement { get; set; } = CalloutPlacement.Top;

        public double VerticalOffset { get; set; }

        public double HorizontalOffset { get; set; }

        public bool StaysOpen { get; set; }

        public PopupAnimation PopupAnimation { get; set; }
    }
}