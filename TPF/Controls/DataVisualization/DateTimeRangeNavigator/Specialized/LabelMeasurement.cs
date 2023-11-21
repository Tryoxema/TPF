namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    internal class LabelMeasurement
    {
        public LabelMeasurement(IntervalBase interval, int formatterIndex)
        {
            Interval = interval;
            FormatterIndex = formatterIndex;
            GroupWidth = double.NaN;
            ItemWidth = double.NaN;
        }

        public IntervalBase Interval { get; private set; }

        public int FormatterIndex { get; private set; }

        public double GroupWidth { get; set; }

        public double ItemWidth { get; set; }

        public void InvalidateMeasurements()
        {
            GroupWidth = double.NaN;
            ItemWidth = double.NaN;
        }
    }
}