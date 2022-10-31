using System;
using System.Linq;
using System.ComponentModel;
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
                if (SetProperty(ref _valuePath, value))
                {
                    if (_valuePath != null && (_valuePath.Contains('.') || _valuePath.Contains('[') || _valuePath.Contains(']'))) _isSimpleValuePath = false;
                    else _isSimpleValuePath = true;
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

        private bool _isSimpleValuePath;

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Haben wir einen einfachen Pfad oder einen verschachtelten Pfad?
            if (_isSimpleValuePath)
            {
                if (ValuePath == e.PropertyName) OnPropertyChanged(nameof(Value));
            }
            else
            {
                // Wenn es sich um einen verschachtelten Pfad handelt, dann lösen wir PropertyChanged einfach aus wenn der Anfang stimmt
                if (ValuePath != null && ValuePath.StartsWith(e.PropertyName)) OnPropertyChanged(nameof(Value));
            }
        }
    }
}