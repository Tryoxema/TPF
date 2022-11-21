using System;
using TPF.Internal;

namespace TPF.Controls.Specialized.DataBar
{
    public class DataBarDataItem : DataVisualizationItemBase
    {
        string _valuePath;
        public string ValuePath
        {
            get { return _valuePath; }
            set
            {
                if (_valuePath != value)
                {
                    // Das alte Mapping abmelden und das neue anmelden
                    UnregisterPropertyMapping(_valuePath, nameof(Value));
                    _valuePath = value;
                    RegisterPropertyMapping(_valuePath, nameof(Value));
                    // Wenn sich ValuePath ändert soll ein PropertyChanged für Value ausgelöst werden
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public double Value
        {
            get
            {
                var value = GetValueFromPath(ValuePath);

                if (value != null && value.GetType().IsNumericType()) return Convert.ToDouble(value);

                return double.NaN;
            }
        }

        double _start;
        public double Start
        {
            get { return _start; }
            set { SetProperty(ref _start, value); }
        }

        double _end;
        public double End
        {
            get { return _end; }
            set { SetProperty(ref _end, value); }
        }
    }
}