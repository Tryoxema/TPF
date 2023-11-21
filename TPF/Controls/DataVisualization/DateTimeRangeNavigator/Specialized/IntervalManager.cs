using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    internal class IntervalManager
    {
        public IntervalManager(Controls.DateTimeRangeNavigator navigator)
        {
            if (navigator == null) throw new ArgumentNullException(nameof(navigator));

            _navigator = navigator;

            Intervals.CollectionChanged += Intervals_CollectionChanged;
        }

        private readonly Controls.DateTimeRangeNavigator _navigator;
        private readonly List<IntervalPeriodsGenerator> _generators = new List<IntervalPeriodsGenerator>();
        private readonly List<LabelMeasurement> _labelMeasurements = new List<LabelMeasurement>();

        public IntervalCollection Intervals
        {
            get { return _navigator.Intervals; }
        }

        public IntervalBase CurrentGroupInterval
        {
            get { return _navigator.CurrentGroupInterval; }
            private set { _navigator.CurrentGroupInterval = value; }
        }

        public IntervalBase CurrentItemInterval
        {
            get { return _navigator.CurrentItemInterval; }
            private set { _navigator.CurrentItemInterval = value; }
        }

        double _pixelsPerTick;
        public double PixelsPerTick
        {
            get { return _pixelsPerTick; }
            set
            {
                if (_pixelsPerTick != value)
                {
                    InvalidateLabels();
                    _navigator.UpdateSelectionThumb();
                }
                _pixelsPerTick = value;
            }
        }

        private void Intervals_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncIntervalPeriodGenerators();
            SyncLabelMeasurements();
            DetermineCurrentIntervals();
        }

        private void SyncIntervalPeriodGenerators()
        {
            var uniqueIntervals = Intervals.OrderedIntervals.ToList();

            for (var i = 0; i < uniqueIntervals.Count; i++)
            {
                var interval = uniqueIntervals[i];

                if (_generators.Any(x => x.Interval == interval)) continue;

                _generators.Add(new IntervalPeriodsGenerator(interval));
            }
        }

        private void SyncLabelMeasurements()
        {
            var uniqueIntervals = Intervals.OrderedIntervals.ToList();

            for (var i = 0; i < uniqueIntervals.Count; i++)
            {
                var interval = uniqueIntervals[i];

                // EventHandler aus Sicherheitsgründen einmal abhängen und dann wieder anhängen um doppeltes anhängen zu vermeiden
                interval.LabelMeasurementPropertyChanged -= Interval_LabelMeasurementPropertyChanged;
                interval.LabelMeasurementPropertyChanged += Interval_LabelMeasurementPropertyChanged;

                for (int j = 0; j < interval.StringFormatters.Length; j++)
                {
                    var formatter = interval.StringFormatters[j];

                    var measurement = _labelMeasurements.FirstOrDefault(x => x.Interval == interval && x.FormatterIndex == j);

                    if (measurement != null) continue;

                    _labelMeasurements.Add(new LabelMeasurement(interval, j));
                }
            }
        }

        private void Interval_LabelMeasurementPropertyChanged(object sender, EventArgs e)
        {
            var interval = (IntervalBase)sender;

            var generator = GetPeriodsGenerator(interval);

            // Label invalidieren, damit die geändert werden
            if (generator != null) generator.AreLabelsValid = false;
            // Label aktualisieren
            if (CurrentGroupInterval == interval || CurrentItemInterval == interval) UpdateLabels();
            // Alte Messungen invalidieren
            foreach (var measurement in _labelMeasurements.Where(x => x.Interval == interval))
            {
                measurement.InvalidateMeasurements();
            }
            // Label nochmal invalidieren
            // Das wird hier in der Reihenfolge gemacht, damit der Text schonmal angepasst wird
            // Nach dem nächsten Messen soll nochmal ausgewertet werden, ob sich das Label doch ändern muss
            if (generator != null) generator.AreLabelsValid = false;
        }

        public void DetermineCurrentIntervals()
        {
            if (Intervals.Count == 0)
            {
                CurrentGroupInterval = null;
                CurrentItemInterval = null;
                return;
            }

            var start = _navigator.VisibleStart;
            var end = _navigator.VisibleEnd;

            var difference = end - start;

            var largestInterval = Intervals.OrderedIntervals.LastOrDefault(x => x.MinimumIntervalLength <= difference);

            if (largestInterval == null)
            {
                CurrentGroupInterval = null;
                CurrentItemInterval = null;
                return;
            }

            var intervalIndex = Intervals.OrderedIntervals.IndexOf(largestInterval);

            if (intervalIndex == 0)
            {
                CurrentGroupInterval = null;
                CurrentItemInterval = largestInterval;
            }
            else
            {
                var previousInterval = Intervals.OrderedIntervals[intervalIndex - 1];

                CurrentGroupInterval = largestInterval;
                CurrentItemInterval = previousInterval;
            }
        }

        private IntervalPeriodsGenerator GetPeriodsGenerator(IntervalBase interval)
        {
            var generator = _generators.FirstOrDefault(x => x.Interval == interval);

            return generator;
        }

        public List<IntervalPeriod> GetIntervalPeriods(IntervalBase interval)
        {
            var generator = GetPeriodsGenerator(interval);

            // Dürfte nie vorkommen aber sicher ist sicher
            if (generator == null) return new List<IntervalPeriod>();

            generator.UpdatePeriod(_navigator.Start, _navigator.End);

            // Eine Kopie der Liste zurückgeben, damit nicht aus versehen die Liste geändert wird
            return generator.IntervalPeriods.ToList();
        }

        public bool RequiresIntervalMeasuring(IntervalBase interval, LabelType labelType)
        {
            var hasUnmeasuredLabels = _labelMeasurements.Any(x => x.Interval == interval && (labelType == LabelType.Group ? double.IsNaN(x.GroupWidth) : double.IsNaN(x.ItemWidth)));

            return hasUnmeasuredLabels;
        }

        public void SaveLabelMeasurement(IntervalBase interval, LabelType labelType, int index, double value)
        {
            var labelMeasurement = _labelMeasurements.FirstOrDefault(x => x.Interval == interval && x.FormatterIndex == index);

            if (labelMeasurement == null) return;

            switch (labelType)
            {
                case LabelType.Group: labelMeasurement.GroupWidth = value; break;
                case LabelType.Item: labelMeasurement.ItemWidth = value; break;
            }
        }

        private void InvalidateLabels()
        {
            for (int i = 0; i < _generators.Count; i++)
            {
                var generator = _generators[i];

                generator.AreLabelsValid = false;
            }
        }

        public void UpdateLabels()
        {
            if (CurrentGroupInterval != null)
            {
                var generator = GetPeriodsGenerator(CurrentGroupInterval);

                if (generator != null) generator.UpdateLabels(FindBestLabelFormatter(generator.Interval, LabelType.Group));
            }

            if (CurrentItemInterval != null)
            {
                var generator = GetPeriodsGenerator(CurrentItemInterval);

                if (generator != null) generator.UpdateLabels(FindBestLabelFormatter(generator.Interval, LabelType.Item));
            }
        }

        private Func<DateTime, string> FindBestLabelFormatter(IntervalBase interval, LabelType labelType)
        {
            var minimumWidth = interval.MinimumIntervalLength.Ticks * PixelsPerTick;

            var labelMeasurements = _labelMeasurements.Where(x => x.Interval == interval);
            var formatters = interval.StringFormatters;

            Func<DateTime, string> bestFormatter = null;
            int index = 0;

            foreach (var measurement in labelMeasurements)
            {
                var requiredSize = labelType == LabelType.Group ? measurement.GroupWidth : measurement.ItemWidth;

                if (minimumWidth >= requiredSize)
                {
                    bestFormatter = formatters[index];
                    break;
                }

                index++;
            }

            if (bestFormatter == null) bestFormatter = formatters.Last();
            
            return bestFormatter;
        }
    }
}