using System;
using TPF.Controls.Specialized.Calculator;

namespace TPF.Controls
{
    public static class CalculatorOperations
    {
        static CalculatorOperations()
        {
            Add = new TwoValueOperation()
            {
                DisplayText = "+",
                Type = OperationType.Operator,
                Body = decimal.Add
            };

            Subtract = new TwoValueOperation()
            {
                DisplayText = "-",
                Type = OperationType.Operator,
                Body = decimal.Subtract
            };

            Multiply = new TwoValueOperation()
            {
                DisplayText = "*",
                Type = OperationType.Operator,
                Body = decimal.Multiply
            };

            Divide = new TwoValueOperation()
            {
                DisplayText = "/",
                Type = OperationType.Operator,
                Body = decimal.Divide
            };

            Percent = new TwoValueOperation()
            {
                DisplayText = null,
                Type = OperationType.Percent,
                Body = GetPercentage
            };

            Negate = new SingleValueOperation()
            {
                DisplayText = "negate",
                Type = OperationType.Function,
                Body = decimal.Negate
            };

            SquareRoot = new SingleValueOperation()
            {
                DisplayText = "sqrt",
                Type = OperationType.Function,
                Body = Sqrt
            };

            Reciprocal = new SingleValueOperation()
            {
                DisplayText = "reciproc",
                Type = OperationType.Function,
                Body = Reciproc
            };
        }

        public static TwoValueOperation Add { get; private set; }

        public static TwoValueOperation Subtract { get; private set; }

        public static TwoValueOperation Multiply { get; private set; }

        public static TwoValueOperation Divide { get; private set; }

        public static TwoValueOperation Percent { get; private set; }

        public static SingleValueOperation Negate { get; private set; }

        public static SingleValueOperation SquareRoot { get; private set; }

        public static SingleValueOperation Reciprocal { get; private set; }

        private static decimal GetPercentage(decimal first, decimal second)
        {
            return first * (second / 100);
        }

        private static decimal Sqrt(decimal value)
        {
            if (value < 0) throw new OverflowException("Cannot calculate square root from a negative number");

            decimal current = (decimal)Math.Sqrt((double)value), previous;

            do
            {
                previous = current;

                if (previous == 0.0M) return 0;

                current = (previous + value / previous) / 2;
            }
            while (Math.Abs(previous - current) > 0.0m);

            return current;
        }

        private static decimal Reciproc(decimal value)
        {
            return 1 / value;
        }
    }
}