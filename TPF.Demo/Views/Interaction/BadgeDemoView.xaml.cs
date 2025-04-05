using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class BadgeDemoView : ViewBase
    {
        public BadgeDemoView()
        {
            InitializeComponent();

            BadgeAlignments.Add(BadgeAlignment.Center);
            BadgeAlignments.Add(BadgeAlignment.Inside);
            BadgeAlignments.Add(BadgeAlignment.Outside);
            BadgeAlignments.Add(BadgeAlignment.Custom);
        }

        public ObservableCollection<BadgeAlignment> BadgeAlignments { get; } = new ObservableCollection<BadgeAlignment>();
    }
}