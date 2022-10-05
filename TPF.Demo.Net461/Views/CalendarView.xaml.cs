using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TPF.Controls;

namespace TPF.Demo.Net461.Views
{
    public partial class CalendarView : UserControl
    {
        public CalendarView()
        {
            InitializeComponent();

            InitializeSpecialDates();
        }

        private void InitializeSpecialDates()
        {
            _specialDates = new List<SpecialDate>()
            {
                new SpecialDate() { Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), Template = TryFindResource("SpecialDateRedTemplate") as DataTemplate },
                new SpecialDate() { Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15), Template = TryFindResource("SpecialDateBlueTemplate") as DataTemplate },
                new SpecialDate() { Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), Template = TryFindResource("SpecialDateGreenTemplate") as DataTemplate }
            };
        }

        List<SpecialDate> _specialDates;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _specialDates.Count; i++)
            {
                var date = _specialDates[i];

                DemoCalendar.SpecialDates.Add(date);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DemoCalendar.SpecialDates.Clear();
        }
    }
}