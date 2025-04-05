using System.Collections.ObjectModel;
using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class HamburgerMenuDemoView : ViewBase
    {
        public HamburgerMenuDemoView()
        {
            InitializeComponent();

            DisplayModes.Add(HamburgerMenuDisplayMode.Minimal);
            DisplayModes.Add(HamburgerMenuDisplayMode.Compact);
            DisplayModes.Add(HamburgerMenuDisplayMode.Expanded);

            Visibilities.Add(Visibility.Visible);
            Visibilities.Add(Visibility.Hidden);
            Visibilities.Add(Visibility.Collapsed);
        }

        public ObservableCollection<HamburgerMenuDisplayMode> DisplayModes { get; } = new ObservableCollection<HamburgerMenuDisplayMode>();
        public ObservableCollection<Visibility> Visibilities { get; } = new ObservableCollection<Visibility>();
    }
}