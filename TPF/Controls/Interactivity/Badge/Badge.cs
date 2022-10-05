using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TPF.Controls
{
    public class Badge : ContentControl
    {
        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(HorizontalAlignment.Right, OnPositioningChanged));
            VerticalAlignmentProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(VerticalAlignment.Top, OnPositioningChanged));
            VisibilityProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        }

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(Badge),
            new PropertyMetadata(new CornerRadius(), OnCornerRadiusChanged));

        static void OnCornerRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Badge)sender;

            instance.UpdateLayer();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region Instance Attached DependencyProperty
        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance",
            typeof(Badge),
            typeof(Badge),
            new PropertyMetadata(null, OnInstanceChanged));

        static void OnInstanceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is FrameworkElement element)) return;

            if (e.NewValue is Badge instance) instance.OnInstanceChanged(element);
        }

        public static Badge GetInstance(DependencyObject element)
        {
            return (Badge)element.GetValue(InstanceProperty);
        }

        public static void SetInstance(DependencyObject element, Badge value)
        {
            element.SetValue(InstanceProperty, value);
        }
        #endregion

        #region HorizontalPosition DependencyProperty
        public static readonly DependencyProperty HorizontalPositionProperty = DependencyProperty.Register("HorizontalPosition",
            typeof(double),
            typeof(Badge),
            new PropertyMetadata(double.NaN, OnPositioningChanged, ConstrainPosition));

        public double HorizontalPosition
        {
            get { return (double)GetValue(HorizontalPositionProperty); }
            set { SetValue(HorizontalPositionProperty, value); }
        }
        #endregion

        #region HorizontalBadgeAlignment DependencyProperty
        public static readonly DependencyProperty HorizontalBadgeAlignmentProperty = DependencyProperty.Register("HorizontalBadgeAlignment",
            typeof(BadgeAlignment),
            typeof(Badge),
            new PropertyMetadata(BadgeAlignment.Center, OnPositioningChanged));

        public BadgeAlignment HorizontalBadgeAlignment
        {
            get { return (BadgeAlignment)GetValue(HorizontalBadgeAlignmentProperty); }
            set { SetValue(HorizontalBadgeAlignmentProperty, value); }
        }
        #endregion

        #region HorizontalBadgePosition DependencyProperty
        public static readonly DependencyProperty HorizontalBadgePositionProperty = DependencyProperty.Register("HorizontalBadgePosition",
            typeof(double),
            typeof(Badge),
            new PropertyMetadata(0.5, OnPositioningChanged, ConstrainBadgePosition));

        public double HorizontalBadgePosition
        {
            get { return (double)GetValue(HorizontalBadgePositionProperty); }
            set { SetValue(HorizontalBadgePositionProperty, value); }
        }
        #endregion

        #region VerticalPosition DependencyProperty
        public static readonly DependencyProperty VerticalPositionProperty = DependencyProperty.Register("VerticalPosition",
            typeof(double),
            typeof(Badge),
            new PropertyMetadata(double.NaN, OnPositioningChanged, ConstrainPosition));

        public double VerticalPosition
        {
            get { return (double)GetValue(VerticalPositionProperty); }
            set { SetValue(VerticalPositionProperty, value); }
        }
        #endregion

        #region VerticalBadgeAlignment DependencyProperty
        public static readonly DependencyProperty VerticalBadgeAlignmentProperty = DependencyProperty.Register("VerticalBadgeAlignment",
            typeof(BadgeAlignment),
            typeof(Badge),
            new PropertyMetadata(BadgeAlignment.Center, OnPositioningChanged));

        public BadgeAlignment VerticalBadgeAlignment
        {
            get { return (BadgeAlignment)GetValue(VerticalBadgeAlignmentProperty); }
            set { SetValue(VerticalBadgeAlignmentProperty, value); }
        }
        #endregion

        #region VerticalBadgePosition DependencyProperty
        public static readonly DependencyProperty VerticalBadgePositionProperty = DependencyProperty.Register("VerticalBadgePosition",
            typeof(double),
            typeof(Badge),
            new PropertyMetadata(0.5, OnPositioningChanged, ConstrainBadgePosition));

        public double VerticalBadgePosition
        {
            get { return (double)GetValue(VerticalBadgePositionProperty); }
            set { SetValue(VerticalBadgePositionProperty, value); }
        }
        #endregion

        static void OnPositioningChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Badge)sender;

            instance.UpdatePosition();
        }

        static void OnVisibilityChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Badge)sender;

            instance.UpdateLayer();
        }

        internal static object ConstrainPosition(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (!double.IsNaN(doubleValue))
            {
                if (doubleValue < 0) doubleValue = 0;
                else if (doubleValue > 1) doubleValue = 1;
            }

            return doubleValue;
        }

        internal static object ConstrainBadgePosition(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;
            else if (double.IsNaN(doubleValue)) doubleValue = 0.5;

            return doubleValue;
        }

        internal FrameworkElement Owner;
        internal BadgeAdorner Adorner;

        protected virtual void OnInstanceChanged(FrameworkElement owner)
        {
            if (Adorner != null) Adorner.Remove();

            Owner = owner;
            if (DataContext == null) DataContext = owner.DataContext;

            owner.Loaded += Owner_Loaded;
        }

        private void CreateAdorner()
        {
            if (Owner == null) return;

            if (Adorner != null) Adorner.Remove();

            var adorner = new BadgeAdorner(Owner, this);

            Adorner = adorner;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (Adorner == null || Owner == null) return;

            double x = 0, y = 0;

            var size = RenderSize;
            var ownerSize = Owner.RenderSize;

            // Horizontale Position berechnen
            if (!double.IsNaN(HorizontalPosition))
            {
                // Über die Breite des Owner-Elements und dem Faktor aus HorizontalPosition x berechnen
                x = ownerSize.Width * HorizontalPosition;
            }
            else
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left: x = 0; break;
                    case HorizontalAlignment.Stretch:
                    case HorizontalAlignment.Center: x = ownerSize.Width * 0.5; break;
                    case HorizontalAlignment.Right: x = ownerSize.Width; break;
                }
            }

            // An BadgeAlignment anpassen
            switch (HorizontalBadgeAlignment)
            {
                default:
                case BadgeAlignment.Center: x -= size.Width * 0.5; break;
                case BadgeAlignment.Inside: x -= size.Width; break;
                case BadgeAlignment.Outside: break;
                case BadgeAlignment.Custom: x -= size.Width * HorizontalBadgePosition; break;
            }

            // Vertikale Position berechnen
            if (!double.IsNaN(VerticalPosition))
            {
                // Über die Höhe des Owner-Elements und dem Faktor aus VerticalPosition y berechnen
                y = ownerSize.Height * VerticalPosition;
            }
            else
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top: y = 0; break;
                    case VerticalAlignment.Stretch:
                    case VerticalAlignment.Center: y = ownerSize.Height * 0.5; break;
                    case VerticalAlignment.Bottom: y = ownerSize.Height; break;
                }
            }

            // An BadgeAlignment anpassen
            switch (VerticalBadgeAlignment)
            {
                default:
                case BadgeAlignment.Center: y -= size.Height * 0.5; break;
                case BadgeAlignment.Inside: break;
                case BadgeAlignment.Outside: y -= size.Height; break;
                case BadgeAlignment.Custom: y -= size.Height * VerticalBadgePosition; break;
            }

            var point = new Point(x, y);

            Adorner.MoveElement(point);
        }

        private void UpdateLayer()
        {
            if (Adorner == null) return;

            Adorner.Update();
        }

        private void Owner_Loaded(object sender, RoutedEventArgs e)
        {
            var instance = (FrameworkElement)sender;

            var decorator = instance.ParentOfType<AdornerDecorator>();

            if (decorator == null) return;

            var badge = GetInstance(instance);

            if (badge == null)
            {
                instance.Loaded -= Owner_Loaded;
                return;
            }

            badge.CreateAdorner();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdatePosition();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            UpdateLayer();
        }
    }
}