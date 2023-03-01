using System.Windows;

namespace TPF.Controls.Specialized.Calendar
{
    public class CalendarModeChangedEventArgs : RoutedEventArgs
    {
        internal CalendarModeChangedEventArgs(DisplayMode oldMode, DisplayMode newMode) : base(Controls.Calendar.CalendarModeChangedEvent)
        {
            OldMode = oldMode;
            NewMode = newMode;
        }

        public DisplayMode OldMode { get; }

        public DisplayMode NewMode { get; }
    }

    public delegate void CalendarModeChangedEventHandler(object sender, CalendarModeChangedEventArgs e);
}