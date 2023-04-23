namespace TPF.Controls
{
    public struct ClockValue
    {
        public ClockValue(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString("#00");
        }
    }
}