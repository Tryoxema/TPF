using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using TPF.Collections;
using TPF.Controls;

namespace TPF.Demo.Net461.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            Initialize();
        }

        ObservableCollection<DataBarTest> _dataBarTests;
        public ObservableCollection<DataBarTest> DataBarTests
        {
            get { return _dataBarTests ?? (_dataBarTests = new ObservableCollection<DataBarTest>()); }
        }

        RangeObservableCollection<SparklineTest> _sparklineTests;
        public RangeObservableCollection<SparklineTest> SparklineTests
        {
            get { return _sparklineTests ?? (_sparklineTests = new RangeObservableCollection<SparklineTest>()); }
        }

        private void Initialize()
        {
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(15));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(3));
            DataBarTests.Add(new DataBarTest(18));
            DataBarTests.Add(new DataBarTest(22));
            DataBarTests.Add(new DataBarTest(10));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(9));
            DataBarTests.Add(new DataBarTest(6));

            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                SparklineTests.Add(new SparklineTest(i, random.Next(-20, 20)));
            }
        }
    }

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