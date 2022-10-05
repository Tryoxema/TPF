using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class BooleanExtension : MarkupExtension
    {
        public bool Value { get; set; }

        public BooleanExtension(bool value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}