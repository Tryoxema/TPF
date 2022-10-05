using System;
using System.Windows;

namespace TPF.Controls.Specialized.Calendar
{
    public class CalendarDateChangedEventArgs : RoutedEventArgs
    {
        internal CalendarDateChangedEventArgs(RoutedEvent routedEvent, DateTime? addedDate, DateTime? removedDate)
        {
            RoutedEvent = routedEvent;
            AddedDate = addedDate;
            RemovedDate = removedDate;
        }

        public DateTime? AddedDate { get; }

        public DateTime? RemovedDate { get; }
    }

    public delegate void CalendarDateChangedEventHandler(object sender, CalendarDateChangedEventArgs e);
}