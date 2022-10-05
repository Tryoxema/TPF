using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class Int16Extension : MarkupExtension
    {
        public short Value { get; set; }

        public Int16Extension(short value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}