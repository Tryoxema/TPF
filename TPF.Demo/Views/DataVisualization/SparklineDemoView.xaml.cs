using System;
using System.Windows;
using System.Collections.Generic;
using TPF.Collections;

namespace TPF.Demo.Views
{
    public partial class SparklineDemoView : ViewBase
    {
        public SparklineDemoView()
        {
            InitializeComponent();

            Initialize();
        }

        int _dataPointCount = 20;
        public int DataPointCount
        {
            get { return _dataPointCount; }
            set { SetProperty(ref _dataPointCount, value); }
        }

        double _axisValue;
        public double AxisValue
        {
            get { return _axisValue; }
            set { SetProperty(ref _axisValue, value); }
        }

        bool _showAxis;
        public bool ShowAxis
        {
            get { return _showAxis; }
            set { SetProperty(ref _showAxis, value); }
        }

        bool _showFirstPointIndicator;
        public bool ShowFirstPointIndicator
        {
            get { return _showFirstPointIndicator; }
            set { SetProperty(ref _showFirstPointIndicator, value); }
        }

        bool _showLastPointIndicator;
        public bool ShowLastPointIndicator
        {
            get { return _showLastPointIndicator; }
            set { SetProperty(ref _showLastPointIndicator, value); }
        }

        bool _showHighPointIndicators;
        public bool ShowHighPointIndicators
        {
            get { return _showHighPointIndicators; }
            set { SetProperty(ref _showHighPointIndicators, value); }
        }

        bool _showLowPointIndicators;
        public bool ShowLowPointIndicators
        {
            get { return _showLowPointIndicators; }
            set { SetProperty(ref _showLowPointIndicators, value); }
        }

        bool _showNegativePointIndicators;
        public bool ShowNegativePointIndicators
        {
            get { return _showNegativePointIndicators; }
            set { SetProperty(ref _showNegativePointIndicators, value); }
        }

        public RangeObservableCollection<SparklineTest> SparklineTests { get; } = new RangeObservableCollection<SparklineTest>();

        private void Initialize()
        {
            GenerateDataPoints();
        }

        private void GenerateDataPoints()
        {
            var random = new Random();

            var dataPoints = new List<SparklineTest>(DataPointCount);

            for (int i = 0; i < DataPointCount; i++)
            {
                dataPoints.Add(new SparklineTest(i, random.Next(-50, 50)));
            }

            SparklineTests.Clear();
            SparklineTests.AddRange(dataPoints);
        }

        private void GenerateDataPointsButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateDataPoints();
        }
    }
}