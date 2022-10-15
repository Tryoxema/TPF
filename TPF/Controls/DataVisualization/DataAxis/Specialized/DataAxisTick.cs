namespace TPF.Controls.Specialized.DataAxis
{
    public class DataAxisTick
    {
        public bool IsMajorTick { get; set; }

        public double Value { get; set; }

        public double NormalizedValue { get; set; }

        public override string ToString()
        {
            return $"Value = {Value}; Normalized = {NormalizedValue}; MajorTick = {IsMajorTick}";
        }
    }
}