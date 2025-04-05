using TPF.Controls;

namespace TPF.Demo.Views
{
    public class DataBarTest : NotifyObject
    {
        public DataBarTest(double value)
        {
            Value = value;
        }

        double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}