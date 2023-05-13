using System;

namespace TPF.Controls
{
    public class SpecialDay
    {
        public SpecialDay(string name, int dayDifferenceFromToday)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            DayDifferenceFromToday = dayDifferenceFromToday;
        }

        public string Name { get; set; }

        public int DayDifferenceFromToday { get; set; }
    }
}