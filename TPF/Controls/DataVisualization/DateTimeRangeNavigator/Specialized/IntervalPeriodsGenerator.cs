using System;
using System.Collections.Generic;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    internal class IntervalPeriodsGenerator
    {
        public IntervalPeriodsGenerator(IntervalBase interval)
        {
            if (interval == null) throw new ArgumentNullException(nameof(Interval));

            Interval = interval;
        }

        public IntervalBase Interval { get; private set; }

        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }

        public List<IntervalPeriod> IntervalPeriods { get; private set; }

        public bool AreLabelsValid { get; set; }

        public void UpdatePeriod(DateTime start, DateTime end)
        {
            if (start == PeriodStart && end == PeriodEnd) return;

            PeriodStart = start;
            PeriodEnd = end;

            GeneratePeriods();
            AreLabelsValid = false;
        }

        private void GeneratePeriods()
        {
            if (PeriodEnd - PeriodStart <= TimeSpan.Zero) return;

            var intervalPeriods = new List<IntervalPeriod>();

            var start = Interval.GetIntervalStart(PeriodStart);

            for (var current = start; current < PeriodEnd;)
            {
                var nextStart = Interval.IncreaseByInterval(current, 1);

                var period = new IntervalPeriod(Interval, current, nextStart);

                intervalPeriods.Add(period);

                current = nextStart;
            }

            IntervalPeriods = intervalPeriods;
        }

        public void UpdateLabels(Func<DateTime, string> labelFormatter)
        {
            if (AreLabelsValid) return;
            
            for (var i = 0; i < IntervalPeriods.Count; i++)
            {
                var period = IntervalPeriods[i];

                period.Label = labelFormatter(period.Start);
            }

            AreLabelsValid = true;
        }
    }
}