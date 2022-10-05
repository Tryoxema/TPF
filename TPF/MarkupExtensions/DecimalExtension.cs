using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class DecimalExtension : MarkupExtension
    {
        public decimal Value { get; set; }

        public DecimalExtension(decimal value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}