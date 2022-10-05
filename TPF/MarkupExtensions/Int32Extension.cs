using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class Int32Extension : MarkupExtension
    {
        public int Value { get; set; }

        public Int32Extension(int value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}