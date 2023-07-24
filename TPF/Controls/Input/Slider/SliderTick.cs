namespace TPF.Controls
{
    public class SliderTick
    {
        public double Value { get; set; }

        public double NormalizedValue { get; set; }

        public string LabelText { get; set; }

        public bool IsMajorTick { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"Value = {Value}; Normalized = {NormalizedValue}; LabelText = {LabelText}";
        }
    }
}