using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace TPF.Demo.Views
{
    public partial class ComparerDemoView : ViewBase
    {
        public ComparerDemoView()
        {
            InitializeComponent();

            Orientations.Add(Orientation.Horizontal);
            Orientations.Add(Orientation.Vertical);
        }

        public ObservableCollection<Orientation> Orientations { get; } = new ObservableCollection<Orientation>();
    }
}