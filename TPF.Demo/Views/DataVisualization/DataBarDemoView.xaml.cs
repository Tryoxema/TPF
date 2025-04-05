using System.Collections.ObjectModel;
using TPF.Controls.Specialized.DataBar;

namespace TPF.Demo.Views
{
    public partial class DataBarDemoView : ViewBase
    {
        public DataBarDemoView()
        {
            InitializeComponent();

            Initialize();
        }

        public ObservableCollection<LabelPosition> LabelPositions { get; } = new ObservableCollection<LabelPosition>();
        public ObservableCollection<DataBarTest> DataBarTests { get; } = new ObservableCollection<DataBarTest>();

        private void Initialize()
        {
            LabelPositions.Add(LabelPosition.Left);
            LabelPositions.Add(LabelPosition.Right);
            LabelPositions.Add(LabelPosition.Center);
            LabelPositions.Add(LabelPosition.EndOfBarInside);
            LabelPositions.Add(LabelPosition.EndOfBarOutside);

            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(15));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(3));
            DataBarTests.Add(new DataBarTest(18));
            DataBarTests.Add(new DataBarTest(22));
            DataBarTests.Add(new DataBarTest(10));
            DataBarTests.Add(new DataBarTest(5));
            DataBarTests.Add(new DataBarTest(9));
        }
    }
}