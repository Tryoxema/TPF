using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class RatingItem : ContentControl
    {
        static RatingItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingItem), new FrameworkPropertyMetadata(typeof(RatingItem)));
        }

        #region Geometry DependencyProperty
        public static readonly DependencyProperty GeometryProperty = Rating.GeometryProperty.AddOwner(typeof(RatingItem));

        public Geometry Geometry
        {
            get { return (Geometry)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = Rating.IsReadOnlyProperty.AddOwner(typeof(RatingItem));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double),
            typeof(RatingItem),
            new PropertyMetadata(0.0));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region VisibleValue DependencyProperty
        public static readonly DependencyProperty VisibleValueProperty = DependencyProperty.Register("VisibleValue",
            typeof(double),
            typeof(RatingItem),
            new PropertyMetadata(0.0));

        public double VisibleValue
        {
            get { return (double)GetValue(VisibleValueProperty); }
            set { SetValue(VisibleValueProperty, value); }
        }
        #endregion

        #region SelectedBackground DependencyProperty
        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register("SelectedBackground",
            typeof(Brush),
            typeof(RatingItem),
            new PropertyMetadata(null));

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }
        #endregion

        #region SelectedBorderBrush DependencyProperty
        public static readonly DependencyProperty SelectedBorderBrushProperty = DependencyProperty.Register("SelectedBorderBrush",
            typeof(Brush),
            typeof(RatingItem),
            new PropertyMetadata(null));

        public Brush SelectedBorderBrush
        {
            get { return (Brush)GetValue(SelectedBorderBrushProperty); }
            set { SetValue(SelectedBorderBrushProperty, value); }
        }
        #endregion

        internal Rating ParentRating;

        internal int Index { get; set; }

        bool _mouseDown;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Wenn uns noch kein Rating-Control zugewiesen wurde, selber suchen
            if (ParentRating == null)
            {
                ParentRating = this.ParentOfType<Rating>();
            }

            if (ParentRating != null)
            {
                // Wurde uns keine Geometry übergeben?
                if (Geometry == null) Geometry = ParentRating.Geometry;
                // Wenn wir nicht vom ParentRating generiert wurden, den Index raussuchen
                if (Index == 0) Index = ParentRating.GetIndex(this);
            }
        }

        // Gibt zurück, ob das Control oder das ParentControl ReadOnly ist
        private bool IsControlReadOnly()
        {
            // Zuerst ein eventuelles ParentControl prüfen
            if (ParentRating?.IsReadOnly == true) return true;
            else return IsReadOnly;
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (IsControlReadOnly()) return;

            if (ParentRating != null) ParentRating.SelectionChanged(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsControlReadOnly()) return;

            var precision = ParentRating?.Precision ?? RatingPrecision.Exact;

            var point = e.GetPosition(this);

            var ratio = point.X / ActualWidth;

            if (double.IsInfinity(ratio)) ratio = 0;

            switch (precision)
            {
                case RatingPrecision.Full:
                {
                    ratio = 1;
                    break;
                }
                case RatingPrecision.Half:
                {
                    if (ratio > 0.5) ratio = 1;
                    else ratio = 0.5;
                    break;
                }
                case RatingPrecision.Exact: break;
            }
            VisibleValue = ratio;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseDown = false;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsControlReadOnly()) return;

            _mouseDown = true;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (IsControlReadOnly()) return;

            // Nur reagieren, wenn dieses Item der Ursprung des Klicks ist
            if (!_mouseDown) return;

            Value = VisibleValue;
            _mouseDown = false;

            if (ParentRating != null) ParentRating.SetValue(this);
        }
    }
}