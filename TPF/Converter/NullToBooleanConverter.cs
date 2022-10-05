using System;

namespace TPF.Converter
{
    public class NullToBooleanConverter : NullToValueConverter<bool>
    {
        public NullToBooleanConverter() : base(false, true) { }
    }
}