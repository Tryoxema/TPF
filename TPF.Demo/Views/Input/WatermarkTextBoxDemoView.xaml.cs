using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class WatermarkTextBoxDemoView : ViewBase
    {
        public WatermarkTextBoxDemoView()
        {
            InitializeComponent();

            WatermarkBehaviors.Add(WatermarkBehavior.HideOnFocus);
            WatermarkBehaviors.Add(WatermarkBehavior.HideOnTextEntered);

            SelectionOnFocusModes.Add(SelectionOnFocus.Default);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtBeginning);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtEnd);
            SelectionOnFocusModes.Add(SelectionOnFocus.SelectAll);
        }

        public ObservableCollection<WatermarkBehavior> WatermarkBehaviors { get; } = new ObservableCollection<WatermarkBehavior>();
        public ObservableCollection<SelectionOnFocus> SelectionOnFocusModes { get; } = new ObservableCollection<SelectionOnFocus>();
    }
}