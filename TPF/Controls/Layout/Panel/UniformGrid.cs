using System;
using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class UniformGrid : Panel
    {
        #region Columns DependencyProperty
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns",
            typeof(int),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ValidateColumns));

        private static bool ValidateColumns(object value)
        {
            return (int)value >= 0;
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        #endregion

        #region Rows DependencyProperty
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
            typeof(int),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ValidateRows));

        private static bool ValidateRows(object value)
        {
            return (int)value >= 0;
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        #endregion

        #region FirstColumn DependencyProperty
        public static readonly DependencyProperty FirstColumnProperty = DependencyProperty.Register("FirstColumn",
            typeof(int),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ValidateFirstColumn));

        private static bool ValidateFirstColumn(object value)
        {
            return (int)value >= 0;
        }

        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }
        #endregion

        #region IgnoreCollapsedChildren DependencyProperty
        public static readonly DependencyProperty IgnoreCollapsedChildrenProperty = DependencyProperty.Register("IgnoreCollapsedChildren",
            typeof(bool),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool IgnoreCollapsedChildren
        {
            get { return (bool)GetValue(IgnoreCollapsedChildrenProperty); }
            set { SetValue(IgnoreCollapsedChildrenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HideFirstRow DependencyProperty
        public static readonly DependencyProperty HideFirstRowProperty = DependencyProperty.Register("HideFirstRow",
            typeof(bool),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool HideFirstRow
        {
            get { return (bool)GetValue(HideFirstRowProperty); }
            set { SetValue(HideFirstRowProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HideFirstColumn DependencyProperty
        public static readonly DependencyProperty HideFirstColumnProperty = DependencyProperty.Register("HideFirstColumn",
            typeof(bool),
            typeof(UniformGrid),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public bool HideFirstColumn
        {
            get { return (bool)GetValue(HideFirstColumnProperty); }
            set { SetValue(HideFirstColumnProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private int _columns;
        private int _rows;

        protected override Size MeasureOverride(Size availableSize)
        {
            ComputeLayout();

            var childWidth = availableSize.Width / Math.Max(1, _columns - (HideFirstColumn ? 1 : 0));
            var childHeigt = availableSize.Height / Math.Max(1, _rows - (HideFirstRow ? 1 : 0));

            var maxChildSize = new Size(childWidth, childHeigt);

            double maxWidth = 0.0, maxHeight = 0.0;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                child.Measure(maxChildSize);

                var desiredSize = child.DesiredSize;

                if (maxWidth < desiredSize.Width) maxWidth = desiredSize.Width;
                if (maxHeight < desiredSize.Height) maxHeight = desiredSize.Height;
            }

            return new Size(maxWidth * _columns, maxHeight * _rows);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var childWidth = finalSize.Width / Math.Max(1, _columns - (HideFirstColumn ? 1 : 0));
            var childHeigt = finalSize.Height / Math.Max(1, _rows - (HideFirstRow ? 1 : 0));

            var childBounds = new Rect(0, 0, childWidth, childHeigt);
            // Begrenzer für X
            var maxX = finalSize.Width - 1.0;

            // Startpunkt verlegen, wenn in FirstColumn ein Wert > 0 steht und die erste Zeile nicht versteckt wird
            if (!HideFirstRow) childBounds.X += childBounds.Width * Math.Max(0, FirstColumn - (HideFirstColumn ? 1 : 0));

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                // Soll das aktuelle Element versteckt werden?
                if (HideFirstRow && i + FirstColumn <= _columns - 1 || HideFirstColumn && (i + FirstColumn) % _columns == 0)
                {
                    child.Arrange(new Rect(0, 0, 0, 0));
                    continue;
                }
                else child.Arrange(childBounds);

                // Wenn Elemente nicht Collapsed sind oder der Zustand auch mit einbezogen werden soll, zum nächsten Platz weitergehen
                if (!IgnoreCollapsedChildren || child.Visibility != Visibility.Collapsed)
                {
                    childBounds.X += childWidth;

                    if (childBounds.X >= maxX)
                    {
                        childBounds.Y += childBounds.Height;
                        childBounds.X = 0;
                    }
                }
            }

            return finalSize;
        }

        private void ComputeLayout()
        {
            _columns = Columns;
            _rows = Rows;

            if (FirstColumn >= _columns) FirstColumn = 0;

            // Wenn Rows oder Columns 0 sind, dann Children durchgehen und selber ausrechnen
            if (_rows == 0 || _columns == 0)
            {
                var childrenForLayoutCount = 0;

                // Sollen Children mit Visibility = Collapsed berücksichtigt werden?
                if (IgnoreCollapsedChildren)
                {
                    for (int i = 0, count = InternalChildren.Count; i < count; i++)
                    {
                        var child = InternalChildren[i];

                        if (child.Visibility != Visibility.Collapsed) childrenForLayoutCount++;
                    }
                }
                else childrenForLayoutCount = InternalChildren.Count;

                // Wir wollen immer mindestens eine Row und Column haben, da in Measure und Arrange durch diese Werte geteilt wird
                if (childrenForLayoutCount == 0) childrenForLayoutCount = 1;

                if (_rows == 0)
                {
                    if (_columns > 0) _rows = (childrenForLayoutCount + FirstColumn + (_columns - 1)) / _columns;
                    else
                    {
                        // Wenn wir hier sind, wurden Rows und Columns beide nicht gesetzt, also ordnen wir alles rechteckig an
                        _rows = (int)Math.Sqrt(childrenForLayoutCount);

                        // Wenn durch das Abschneiden der Nachkommastellen durch das Casten in einen int etwas verloren gegangen ist, dann +1 für die Rows
                        if (_rows * _rows < childrenForLayoutCount) _rows++;

                        // Columns = Rows für quadratisches Layout
                        _columns = _rows;
                    }
                }
                else if (_columns == 0) _columns = (childrenForLayoutCount + _rows - 1) / _rows;
            }
        }
    }
}