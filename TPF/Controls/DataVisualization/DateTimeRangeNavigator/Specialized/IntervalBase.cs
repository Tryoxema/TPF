using System;
using System.Windows;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public abstract class IntervalBase : DependencyObject
    {
        public abstract TimeSpan MinimumIntervalLength { get; }

        internal long SortingValue { get { return MinimumIntervalLength.Ticks; } }

        public abstract DateTime GetIntervalStart(DateTime dateTime);

        public abstract DateTime IncreaseByInterval(DateTime dateTime, int intervalCount);

        public abstract Func<DateTime, string>[] StringFormatters { get; }

        protected virtual int LabelMeasurementExampleCount { get { return 10; } }

        internal event EventHandler LabelMeasurementPropertyChanged;

        internal virtual string[,] CreateLabelMeasurementsMatrix(Func<DateTime, string>[] formatters)
        {
            var exampleCount = LabelMeasurementExampleCount;

            var matrix = new string[formatters.Length, exampleCount];

            var sampleDate = new DateTime(2000, 1, 1, 10, 10, 10, 100);

            for (int i = 0; i < exampleCount; i++)
            {
                for (int j = 0; j < formatters.Length; j++)
                {
                    var formatter = formatters[j];

                    matrix[j, i] = formatter(sampleDate);
                }

                sampleDate = IncreaseByInterval(sampleDate, 1);
            }

            return matrix;
        }

        protected static void OnMeasurementRelevantPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (IntervalBase)sender;

            instance.OnMeasurementRelevantPropertyChanged();
        }

        protected virtual void OnMeasurementRelevantPropertyChanged()
        {
            LabelMeasurementPropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}