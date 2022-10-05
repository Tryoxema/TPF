using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class ByteExtension : MarkupExtension
    {
        public byte Value { get; set; }

        public ByteExtension(byte value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}