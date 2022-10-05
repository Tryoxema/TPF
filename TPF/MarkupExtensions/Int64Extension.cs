using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class Int64Extension : MarkupExtension
    {
        public long Value { get; set; }

        public Int64Extension(long value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}