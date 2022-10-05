using System;

namespace TPF.Controls.Specialized.Calculator
{
    public class SingleValueOperation : Operation
    {
        public Func<decimal, decimal> Body { get; set; }
    }
}