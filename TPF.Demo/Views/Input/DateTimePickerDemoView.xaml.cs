using System.Collections.ObjectModel;
using System.Windows;
using TPF.Controls;
using TPF.Controls.Specialized.Calendar;
using TPF.Controls.Specialized.DateTimePicker;

namespace TPF.Demo.Views
{
    public partial class DateTimePickerDemoView : ViewBase
    {
        public DateTimePickerDemoView()
        {
            InitializeComponent();

            Visibilities.Add(Visibility.Visible);
            Visibilities.Add(Visibility.Hidden);
            Visibilities.Add(Visibility.Collapsed);

            SelectionOnFocusModes.Add(SelectionOnFocus.Default);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtBeginning);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtEnd);
            SelectionOnFocusModes.Add(SelectionOnFocus.SelectAll);

            CalendarDisplayModes.Add(DisplayMode.MonthView);
            CalendarDisplayModes.Add(DisplayMode.YearView);
            CalendarDisplayModes.Add(DisplayMode.DecadeView);
            CalendarDisplayModes.Add(DisplayMode.CenturyView);

            DateSelectionModes.Add(DateSelectionMode.Day);
            DateSelectionModes.Add(DateSelectionMode.Month);
            DateSelectionModes.Add(DateSelectionMode.Year);

            ClockDisplayModes.Add(ClockDisplayMode.Clock);
            ClockDisplayModes.Add(ClockDisplayMode.List);

            InputModes.Add(InputMode.Date);
            InputModes.Add(InputMode.Time);
            InputModes.Add(InputMode.DateTime);

            DayOfWeekBehaviors.Add(DayOfWeekBehavior.SameWeek);
            DayOfWeekBehaviors.Add(DayOfWeekBehavior.SameDayOrNextOccurence);
        }

        public ObservableCollection<Visibility> Visibilities { get; } = new ObservableCollection<Visibility>();
        public ObservableCollection<SelectionOnFocus> SelectionOnFocusModes { get; } = new ObservableCollection<SelectionOnFocus>();
        public ObservableCollection<DisplayMode> CalendarDisplayModes { get; } = new ObservableCollection<DisplayMode>();
        public ObservableCollection<DateSelectionMode> DateSelectionModes { get; } = new ObservableCollection<DateSelectionMode>();
        public ObservableCollection<ClockDisplayMode> ClockDisplayModes { get; } = new ObservableCollection<ClockDisplayMode>();
        public ObservableCollection<InputMode> InputModes { get; } = new ObservableCollection<InputMode>();
        public ObservableCollection<DayOfWeekBehavior> DayOfWeekBehaviors { get; } = new ObservableCollection<DayOfWeekBehavior>();
    }
}