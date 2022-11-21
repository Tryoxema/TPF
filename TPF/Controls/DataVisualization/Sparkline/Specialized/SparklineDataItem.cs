using System;
using TPF.Internal;

namespace TPF.Controls.Specialized.Sparkline
{
    public class SparklineDataItem : DataVisualizationItemBase
    {
        string _xValuePath;
        public string XValuePath
        {
            get { return _xValuePath; }
            set
            {
                if (_xValuePath != value)
                {
                    // Das alte Mapping abmelden und das neue anmelden
                    UnregisterPropertyMapping(_xValuePath, nameof(XValue));
                    _xValuePath = value;
                    RegisterPropertyMapping(_xValuePath, nameof(XValue));
                }
            }
        }

        public double XValue
        {
            get
            {
                var value = GetValueFromPath(XValuePath);

                if (value != null && value.GetType().IsNumericType()) return Convert.ToDouble(value);

                return double.NaN;
            }
        }

        string _yValuePath;
        public string YValuePath
        {
            get { return _yValuePath; }
            set
            {
                if (_yValuePath != value)
                {
                    // Das alte Mapping abmelden und das neue anmelden
                    UnregisterPropertyMapping(_yValuePath, nameof(YValue));
                    _yValuePath = value;
                    RegisterPropertyMapping(_yValuePath, nameof(YValue));
                }
            }
        }

        public double YValue
        {
            get
            {
                var value = GetValueFromPath(YValuePath);

                if (value != null && value.GetType().IsNumericType()) return Convert.ToDouble(value);

                return double.NaN;
            }
        }
    }
}