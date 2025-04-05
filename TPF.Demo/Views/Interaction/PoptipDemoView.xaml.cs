using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class PoptipDemoView : ViewBase
    {
        public PoptipDemoView()
        {
            InitializeComponent();

            PoptipPlacements.Add(PoptipPlacement.TopLeft);
            PoptipPlacements.Add(PoptipPlacement.Top);
            PoptipPlacements.Add(PoptipPlacement.TopRight);
            PoptipPlacements.Add(PoptipPlacement.BottomLeft);
            PoptipPlacements.Add(PoptipPlacement.Bottom);
            PoptipPlacements.Add(PoptipPlacement.BottomRight);
            PoptipPlacements.Add(PoptipPlacement.LeftTop);
            PoptipPlacements.Add(PoptipPlacement.Left);
            PoptipPlacements.Add(PoptipPlacement.LeftBottom);
            PoptipPlacements.Add(PoptipPlacement.RightTop);
            PoptipPlacements.Add(PoptipPlacement.Right);
            PoptipPlacements.Add(PoptipPlacement.RightBottom);
        }

        public ObservableCollection<PoptipPlacement> PoptipPlacements { get; } = new ObservableCollection<PoptipPlacement>();

        PoptipPlacement _placement = PoptipPlacement.Top;
        public PoptipPlacement Placement
        {
            get { return _placement; }
            set { SetProperty(ref _placement, value); }
        }

        double _offset = 5;
        public double Offset
        {
            get { return _offset; }
            set { SetProperty(ref _offset, value); }
        }
    }
}