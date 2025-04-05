using System.Collections.ObjectModel;
using System.Windows;
using TPF.Controls;
using Dock = System.Windows.Controls.Dock;

namespace TPF.Demo.Views
{
    public partial class TabControlDemoView : ViewBase
    {
        public TabControlDemoView()
        {
            InitializeComponent();

            TabStripPlacements.Add(Dock.Left);
            TabStripPlacements.Add(Dock.Top);
            TabStripPlacements.Add(Dock.Right);
            TabStripPlacements.Add(Dock.Bottom);
        }

        public ObservableCollection<Dock> TabStripPlacements { get; } = new ObservableCollection<Dock>();

        private void DemoTabControl_AddButtonClicked(object sender, RoutedEventArgs e)
        {
            var tabItem = new TabItem()
            {
                Header = "TabItem",
                Content = "Content",
            };

            DemoTabControl.Items.Add(tabItem);
        }
    }
}