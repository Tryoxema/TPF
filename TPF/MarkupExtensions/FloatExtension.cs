﻿using System;
using System.Windows.Markup;

namespace TPF.MarkupExtensions
{
    public class FloatExtension : MarkupExtension
    {
        public float Value { get; set; }

        public FloatExtension(float value)
        {
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}