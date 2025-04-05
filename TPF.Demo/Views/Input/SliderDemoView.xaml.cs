using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class SliderDemoView : ViewBase
    {
        public SliderDemoView()
        {
            InitializeComponent();

            Orientations.Add(Orientation.Horizontal);
            Orientations.Add(Orientation.Vertical);

            ThumbModes.Add(SliderThumbMode.Single);
            ThumbModes.Add(SliderThumbMode.Range);
            ThumbModes.Add(SliderThumbMode.Custom);

            LabelPlacements.Add(TickPlacement.None);
            LabelPlacements.Add(TickPlacement.TopLeft);
            LabelPlacements.Add(TickPlacement.BottomRight);
            LabelPlacements.Add(TickPlacement.Both);

            TickPlacements.Add(TickPlacement.None);
            TickPlacements.Add(TickPlacement.TopLeft);
            TickPlacements.Add(TickPlacement.BottomRight);
            TickPlacements.Add(TickPlacement.Both);
        }

        public ObservableCollection<Orientation> Orientations { get; } = new ObservableCollection<Orientation>();
        public ObservableCollection<SliderThumbMode> ThumbModes { get; } = new ObservableCollection<SliderThumbMode>();
        public ObservableCollection<TickPlacement> LabelPlacements { get; } = new ObservableCollection<TickPlacement>();
        public ObservableCollection<TickPlacement> TickPlacements { get; } = new ObservableCollection<TickPlacement>();
    }
}