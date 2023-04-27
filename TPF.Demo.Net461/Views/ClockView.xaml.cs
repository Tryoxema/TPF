using System.Windows.Controls;
using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Net461.Views
{
    public partial class ClockView : UserControl
    {
        public ClockView()
        {
            InitializeComponent();

            DisplayModes.Add(ClockDisplayMode.Clock);
            DisplayModes.Add(ClockDisplayMode.List);
        }

        ObservableCollection<ClockDisplayMode> _displayModes;
        public ObservableCollection<ClockDisplayMode> DisplayModes
        {
            get { return _displayModes ?? (_displayModes = new ObservableCollection<ClockDisplayMode>()); }
        }
    }
}