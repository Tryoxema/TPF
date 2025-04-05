using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class ClockDemoView : ViewBase
    {
        public ClockDemoView()
        {
            InitializeComponent();

            DisplayModes.Add(ClockDisplayMode.Clock);
            DisplayModes.Add(ClockDisplayMode.List);
        }

        public ObservableCollection<ClockDisplayMode> DisplayModes { get; } = new ObservableCollection<ClockDisplayMode>();
    }
}