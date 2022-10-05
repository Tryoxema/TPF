namespace TPF.Converter
{
    public class InvertedBooleanConverter : BooleanToValueConverter<bool>
    {
        public InvertedBooleanConverter() : base(false, true) { }
    }
}