using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TPF.Controls
{
    public class Blueprint : Control
    {
        static Blueprint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Blueprint), new FrameworkPropertyMetadata(typeof(Blueprint)));
        }

        #region LineBrush DependencyProperty
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush",
            typeof(Brush),
            typeof(Blueprint),
            new PropertyMetadata(null, OnDrawingPropertyChanged));

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }
        #endregion

        #region CellSize DependencyProperty
        public static readonly DependencyProperty CellSizeProperty = DependencyProperty.Register("CellSize",
            typeof(double),
            typeof(Blueprint),
            new PropertyMetadata(10d, OnDrawingPropertyChanged));

        public double CellSize
        {
            get { return (double)GetValue(CellSizeProperty); }
            set { SetValue(CellSizeProperty, value); }
        }
        #endregion

        private static void OnDrawingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Blueprint)sender;

            instance.UpdateDrawingBrush();
        }

        private void UpdateDrawingBrush()
        {
            if (_cellsHost == null) return;

            var cellSize = Math.Max(CellSize, 0);

            var geometry = new RectangleGeometry(new Rect(0, 0, 50, 50));
            var pen = new Pen(LineBrush, 1);
            var drawing = new GeometryDrawing()
            {
                Geometry = geometry,
                Pen = pen,
            };
            var brush = new DrawingBrush()
            {
                Drawing = drawing,
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
                Viewport = new Rect(0, 0, cellSize, cellSize)
            };

            // Alles einfrieren für Performance
            geometry.Freeze();
            pen.Freeze();
            drawing.Freeze();
            brush.Freeze();

            _cellsHost.Fill = brush;
        }

        private Rectangle _cellsHost;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _cellsHost = GetTemplateChild("PART_CellsHost") as Rectangle;

            UpdateDrawingBrush();
        }
    }
}