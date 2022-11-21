namespace TPF.Data
{
    public struct DoubleRange
    {
        public DoubleRange(double start, double end)
        {
            Start = start;
            End = end;
        }

        public double Start { get; private set; }

        public double End { get; private set; }

        public double Delta
        {
            get { return End - Start; }
        }

        public bool Contains(double value)
        {
            return value >= Start && value <= End;
        }

        public double GetRelativePoint(double value)
        {
            if (Delta == 0 || double.IsNaN(Delta)) return 0;

            return (value - Start) / Delta;
        }

        public static bool operator ==(DoubleRange range1, DoubleRange range2)
        {
            return range1.Start == range2.Start && range1.End == range2.End;
        }

        public static bool operator !=(DoubleRange range1, DoubleRange range2)
        {
            return !(range1 == range2);
        }

        public override bool Equals(object obj)
        {
            return obj is DoubleRange range ? this == range : false;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}