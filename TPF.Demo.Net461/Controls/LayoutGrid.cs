using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Demo.Net461.Controls
{
    public class LayoutGrid : Panel
    {
        #region Layout DependencyProperty
        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register("Layout",
            typeof(string),
            typeof(LayoutGrid),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, LayoutPropertyChanged));

        static void LayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LayoutGrid)sender;

            var layoutString = (string)e.NewValue;

            if (string.IsNullOrWhiteSpace(layoutString))
            {
                instance._layoutRows = null;
            }
            else
            {
                var parsedRows = new List<int>();

                var rows = layoutString.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < rows.Length; i++)
                {
                    var row = rows[i];

                    if (int.TryParse(row, out int result))
                    {
                        parsedRows.Add(result);
                    }
                    else
                    {
                        var splitMultiplyRow = row.Split('*').Select(x => x.Trim()).ToList();

                        if (splitMultiplyRow.Count == 2 && int.TryParse(splitMultiplyRow[0], out int counter) && int.TryParse(splitMultiplyRow[1], out int rowSize))
                        {
                            while (counter-- > 0) parsedRows.Add(rowSize);
                        }
                        else
                        {
                            parsedRows = null;

                            break;
                        }
                    }
                }

                instance._layoutRows = parsedRows?.ToArray();
            }
        }

        int[] _layoutRows;

        public string Layout
        {
            get { return (string)GetValue(LayoutProperty); }
            set { SetValue(LayoutProperty, value); }
        }
        #endregion

        #region VerticalGap DependencyProperty
        public static DependencyProperty VerticalGapProperty = DependencyProperty.Register("VerticalGap",
            typeof(double),
            typeof(LayoutGrid),
            new FrameworkPropertyMetadata(7.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double VerticalGap
        {
            get { return (double)GetValue(VerticalGapProperty); }
            set { SetValue(VerticalGapProperty, value); }
        }
        #endregion

        #region HorizontalGap DependencyProperty
        public static DependencyProperty HorizontalGapProperty = DependencyProperty.Register("HorizontalGap",
            typeof(double),
            typeof(LayoutGrid),
            new FrameworkPropertyMetadata(7.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double HorizontalGap
        {
            get { return (double)GetValue(HorizontalGapProperty); }
            set { SetValue(HorizontalGapProperty, value); }
        }
        #endregion

        #region ColumnSpan DependencyProperty
        public static DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan",
            typeof(int),
            typeof(LayoutGrid),
            new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static int GetColumnSpan(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnSpanProperty);
        }

        public static void SetColumnSpan(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnSpanProperty, value);
        }
        #endregion

        #region SkipCollapsedElements DependencyProperty
        public static DependencyProperty SkipCollapsedElementsProperty = DependencyProperty.Register("SkipCollapsedElements",
            typeof(bool),
            typeof(LayoutGrid),
            new FrameworkPropertyMetadata(true));

        public bool SkipCollapsedElements
        {
            get { return (bool)GetValue(SkipCollapsedElementsProperty); }
            set { SetValue(SkipCollapsedElementsProperty, value); }
        }
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            int currentRow = -1;
            int maxItemsInCurrentRow = 0;
            var itemsInRow = 0;
            var rowHeight = 0.0;
            double xOffset = 0, yOffset = 0;
            double itemWidth = 0;
            double maxWidth = 0;

            foreach (UIElement child in Children)
            {
                if (SkipCollapsedElements && child.Visibility == Visibility.Collapsed) continue;

                if (itemsInRow >= maxItemsInCurrentRow)
                {
                    currentRow++;
                    itemsInRow = 0;
                    // X-Offset zurücksetzen
                    xOffset = 0;
                    // Y-Offset erhöhen
                    if (currentRow > 0) yOffset += rowHeight + VerticalGap;
                    // Aktuelle Spaltenhöhe
                    rowHeight = 0;
                    // Maximale Anzahl der Spalten in der aktuellen Zeile holen
                    if (_layoutRows != null && _layoutRows.Length > currentRow) maxItemsInCurrentRow = _layoutRows[currentRow];
                    else maxItemsInCurrentRow = 1;

                    itemWidth = Math.Max(0, (availableSize.Width - ((maxItemsInCurrentRow - 1) * HorizontalGap)) / maxItemsInCurrentRow);
                }

                var columnSpan = Math.Min(Math.Max(1, GetColumnSpan(child)), maxItemsInCurrentRow);

                var realWidth = itemWidth * columnSpan + HorizontalGap * (columnSpan - 1);

                // Größe des Elements messen
                child.Measure(new Size(realWidth, availableSize.Height));

                // Spaltenhöhe auf die Maximalgröße setzen
                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);

                // Update offset
                xOffset += child.DesiredSize.Width + HorizontalGap;

                xOffset = Math.Min(xOffset, availableSize.Width);

                // MaxWidth speichern
                maxWidth = Math.Max(maxWidth, xOffset);

                itemsInRow += columnSpan;
            }

            return new Size(maxWidth, yOffset + rowHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int currentRow = -1;
            int maxItemsInCurrentRow = 0;
            var itemsInRow = 0;
            var rowHeight = 0.0;
            double xOffset = 0, yOffset = 0;
            double itemWidth = 0;

            foreach (UIElement child in Children)
            {
                if (SkipCollapsedElements && child.Visibility == Visibility.Collapsed) continue;

                if (itemsInRow >= maxItemsInCurrentRow)
                {
                    currentRow++;
                    itemsInRow = 0;
                    // X-Offset zurücksetzen
                    xOffset = 0;
                    // Y-Offset erhöhen
                    if (currentRow > 0) yOffset += rowHeight + VerticalGap;
                    // Aktuelle Spaltenhöhe
                    rowHeight = 0;
                    // Maximale Anzahl der Spalten in der aktuellen Zeile holen
                    if (_layoutRows != null && _layoutRows.Length > currentRow) maxItemsInCurrentRow = _layoutRows[currentRow];
                    else maxItemsInCurrentRow = 1;

                    itemWidth = Math.Max(0, (finalSize.Width - ((maxItemsInCurrentRow - 1) * HorizontalGap)) / maxItemsInCurrentRow);
                }

                var columnSpan = Math.Min(Math.Max(1, GetColumnSpan(child)), maxItemsInCurrentRow);

                var realWidth = itemWidth * columnSpan + HorizontalGap * (columnSpan - 1);

                child.Arrange(new Rect(xOffset, yOffset, realWidth, child.DesiredSize.Height));

                // Sicherstellen das die Höhe der Spalte passt
                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);

                // Update offset
                xOffset += realWidth + HorizontalGap;

                xOffset = Math.Min(xOffset, finalSize.Width);

                itemsInRow += columnSpan;
            }

            return finalSize;
        }
    }
}