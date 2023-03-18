using System.Globalization;

namespace TPF.Controls.Specialized.Calculator
{
    internal class CalculatorValue
    {
        internal CalculatorValue(string decimalSeparator) : this(decimalSeparator, 0) { }

        internal CalculatorValue(string decimalSeparator, decimal number)
        {
            _decimalSeparator = decimalSeparator;
            _numberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            _numberFormat.NumberDecimalSeparator = _decimalSeparator;

            var integerPart = (long)number;

            if (number == integerPart) DisplayValue = integerPart.ToString();
            else DisplayValue = number.ToString(_numberFormat);
            DecimalSeparatorIndex = DisplayValue.IndexOf(_decimalSeparator);

            Overwrite = true;
        }

        string _decimalSeparator;
        NumberFormatInfo _numberFormat;

        internal string DisplayValue { get; private set; }

        internal int DecimalSeparatorIndex { get; private set; }

        internal bool Overwrite { get; set; }

        internal void AddNumber(long number)
        {
            if (Overwrite)
            {
                DisplayValue = number.ToString();
                DecimalSeparatorIndex = -1;
                Overwrite = false;
                return;
            }

            if (DisplayValue.Length >= 12) return;

            if (DisplayValue == "0") DisplayValue = number.ToString();
            else DisplayValue += number;
        }

        internal void AddDecimalSeparator()
        {
            if (Overwrite)
            {
                DisplayValue = $"0{_decimalSeparator}";
                DecimalSeparatorIndex = 1;
                Overwrite = false;
                return;
            }

            if (DecimalSeparatorIndex > -1 || DisplayValue.Length >= 12) return;

            DecimalSeparatorIndex = DisplayValue.Length;
            DisplayValue += _decimalSeparator;
        }

        internal void RemoveLast()
        {
            if (Overwrite) return;

            if (DisplayValue.Length > 1) DisplayValue = DisplayValue.Substring(0, DisplayValue.Length - 1);
            else DisplayValue = "0";

            if (DecimalSeparatorIndex > -1 && !DisplayValue.Contains(_decimalSeparator)) DecimalSeparatorIndex = -1;
        }

        internal decimal GetValue()
        {
            if (decimal.TryParse(DisplayValue, NumberStyles.Any, _numberFormat, out var result)) return result;

            return 0.0m;
        }

        internal void Clear()
        {
            DisplayValue = "0";
            DecimalSeparatorIndex = -1;
            Overwrite = true;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}