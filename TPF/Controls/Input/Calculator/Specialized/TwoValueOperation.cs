using System;

namespace TPF.Controls.Specialized.Calculator
{
    public class TwoValueOperation : Operation
    {
        public Func<decimal, decimal, decimal> Body { get; set; }
    }
}