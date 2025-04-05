using TPF.Collections;

namespace TPF.Demo.Views
{
    public partial class DragDropDemoView : ViewBase
    {
        public DragDropDemoView()
        {
            InitializeComponent();

            Initialize();
        }

        public RangeObservableCollection<string> Items1 { get; } = new RangeObservableCollection<string>();
        public RangeObservableCollection<string> Items2 { get; } = new RangeObservableCollection<string>();

        public RangeObservableCollection<int> Items3 { get; } = new RangeObservableCollection<int>();
        public RangeObservableCollection<int> Items4 { get; } = new RangeObservableCollection<int>();

        private void Initialize()
        {
            Items1.AddRange(new[] { "Item 1", "Item 2", "Item 3", "Item 4" });
            Items2.AddRange(new[] { "Item 5", "Item 6", "Item 7", "Item 8" });

            Items3.AddRange(new[] { 1, 2, 3, 4, 5 });
            Items4.AddRange(new[] { 6, 7, 8, 9 });
        }
    }
}