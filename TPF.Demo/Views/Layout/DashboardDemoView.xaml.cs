using System;
using System.Collections.ObjectModel;
using TPF.Collections;

namespace TPF.Demo.Views
{
    public partial class DashboardDemoView : ViewBase
    {
        public DashboardDemoView()
        {
            InitializeComponent();

            Initialize();
        }

        public ObservableCollection<DataBarTest> DataBarTests { get; } = new ObservableCollection<DataBarTest>();

        public RangeObservableCollection<SparklineTest> SparklineTests { get; } = new RangeObservableCollection<SparklineTest>();

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
}