using TPF.Controls;

namespace TPF.Demo.Views
{
    public class SparklineTest : NotifyObject
    {
        public SparklineTest(double x, double y)
        {
            X = x;
            Y = y;
        }

        double _x;
        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        double _y;
        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }
    }
}