using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class NumericTextBoxDemoView : ViewBase
    {
        public NumericTextBoxDemoView()
        {
            InitializeComponent();

            SelectionOnFocusModes.Add(SelectionOnFocus.Default);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtBeginning);
            SelectionOnFocusModes.Add(SelectionOnFocus.CaretAtEnd);
            SelectionOnFocusModes.Add(SelectionOnFocus.SelectAll);
        }

        public ObservableCollection<SelectionOnFocus> SelectionOnFocusModes { get; } = new ObservableCollection<SelectionOnFocus>();
    }
}