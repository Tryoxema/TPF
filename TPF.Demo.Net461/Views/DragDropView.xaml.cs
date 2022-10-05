using System;
using System.Windows;
using System.Windows.Controls;
using TPF.Collections;

namespace TPF.Demo.Net461.Views
{
    public partial class DragDropView : UserControl
    {
        public DragDropView()
        {
            InitializeComponent();

            Items.AddRange(new[] { "Item 1", "Item 2", "Item 3", "Item 4" });

            Items2.AddRange(new[] { "Item 5", "Item 6", "Item 7", "Item 8" });

            Items3.AddRange(new[] { "Item 9", "Item 10" });

            Items4.AddRange(new[] { 1, 2, 3, 4 });
        }

        RangeObservableCollection<string> _items;
        public RangeObservableCollection<string> Items
        {
            get { return _items ?? (_items = new RangeObservableCollection<string>()); }
        }

        RangeObservableCollection<string> _items2;
        public RangeObservableCollection<string> Items2
        {
            get { return _items2 ?? (_items2 = new RangeObservableCollection<string>()); }
        }

        RangeObservableCollection<string> _items3;
        public RangeObservableCollection<string> Items3
        {
            get { return _items3 ?? (_items3 = new RangeObservableCollection<string>()); }
        }

        RangeObservableCollection<int> _items4;
        public RangeObservableCollection<int> Items4
        {
            get { return _items4 ?? (_items4 = new RangeObservableCollection<int>()); }
        }

        private void ListBox_DragInitialize(object sender, TPF.DragDrop.DragInitializeEventArgs e)
        {
            e.Data = e.SourceItem;
            e.AllowedEffects = DragDropEffects.All;

            var visual = new ContentControl()
            {
                Content = e.SourceItem,
                ContentTemplate = (DataTemplate)TryFindResource("DragDataTemplate")
            };

            e.DragVisual = visual;
            e.VisualOffset = new Point(-5, 0);
        }
    }
}