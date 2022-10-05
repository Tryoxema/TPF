using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    [ContentProperty("Items")]
    public class RadialMenu : Control
    {
        static RadialMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialMenu), new FrameworkPropertyMetadata(typeof(RadialMenu)));
        }

        public RadialMenu()
        {
            _items = new RadialMenuItemCollection();

            _items.CollectionChanged += (s, e) =>
            {
                if (IsTemplateApplied && SubMenuItemsPath.Count == 0)
                {
                    _suppressNavigatedEvent = true;
                    ConstructPie(Items);
                    _suppressNavigatedEvent = false;
                }
            };
        }

        RadialMenuItemCollection _items;
        public ObservableCollection<RadialMenuItem> Items
        {
            get { return _items; }
        }

        #region Navigated RoutedEvent
        public static readonly RoutedEvent NavigatedEvent = EventManager.RegisterRoutedEvent("Navigated",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RadialMenu));

        public event RoutedEventHandler Navigated
        {
            add => AddHandler(NavigatedEvent, value);
            remove => RemoveHandler(NavigatedEvent, value);
        }
        #endregion

        #region IsOpen DependencyProperty
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen",
            typeof(bool),
            typeof(RadialMenu),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsOpenChanged));

        private static void OnIsOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialMenu)sender;

            instance.UpdateVisualState();
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region InnerRadiusFactor DependencyProperty
        public static readonly DependencyProperty InnerRadiusFactorProperty = DependencyProperty.Register("InnerRadiusFactor",
            typeof(double),
            typeof(RadialMenu),
            new PropertyMetadata(0.0, OnRadiusFactorChanged, CoerceRadiusFactor));

        public double InnerRadiusFactor
        {
            get { return (double)GetValue(InnerRadiusFactorProperty); }
            set { SetValue(InnerRadiusFactorProperty, value); }
        }
        #endregion

        #region NavigationRadiusFactor DependencyProperty
        public static readonly DependencyProperty NavigationRadiusFactorProperty = DependencyProperty.Register("NavigationRadiusFactor",
            typeof(double),
            typeof(RadialMenu),
            new PropertyMetadata(0.15, OnRadiusFactorChanged, CoerceRadiusFactor));

        public double NavigationRadiusFactor
        {
            get { return (double)GetValue(NavigationRadiusFactorProperty); }
            set { SetValue(NavigationRadiusFactorProperty, value); }
        }
        #endregion

        #region NavigationMenuBackgroundStyle DependencyProperty
        public static readonly DependencyProperty NavigationMenuBackgroundStyleProperty = DependencyProperty.Register("NavigationMenuBackgroundStyle",
            typeof(Style),
            typeof(RadialMenu),
            new PropertyMetadata(null));

        public Style NavigationMenuBackgroundStyle
        {
            get { return (Style)GetValue(NavigationMenuBackgroundStyleProperty); }
            set { SetValue(NavigationMenuBackgroundStyleProperty, value); }
        }
        #endregion

        #region ContentMenuBackgroundStyle DependencyProperty
        public static readonly DependencyProperty ContentMenuBackgroundStyleProperty = DependencyProperty.Register("ContentMenuBackgroundStyle",
            typeof(Style),
            typeof(RadialMenu),
            new PropertyMetadata(null));

        public Style ContentMenuBackgroundStyle
        {
            get { return (Style)GetValue(ContentMenuBackgroundStyleProperty); }
            set { SetValue(ContentMenuBackgroundStyleProperty, value); }
        }
        #endregion

        #region UseRegionMouseOver DependencyProperty
        public static readonly DependencyProperty UseRegionMouseOverProperty = DependencyProperty.Register("UseRegionMouseOver",
            typeof(bool),
            typeof(RadialMenu),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnUseRegionMouseOverChanged));

        private static void OnUseRegionMouseOverChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialMenu)sender;

            if (instance.IsTemplateApplied)
            {
                instance._suppressNavigatedEvent = true;
                instance.ConstructPie(instance.CurrentItems);
                instance._suppressNavigatedEvent = false;
            }
        }

        public bool UseRegionMouseOver
        {
            get { return (bool)GetValue(UseRegionMouseOverProperty); }
            set { SetValue(UseRegionMouseOverProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        internal Grid MasterBackgroundGrid;
        internal ItemsControl MainContent;
        protected bool IsTemplateApplied;
        private bool _suppressNavigatedEvent;

        // Die momentane Liste an Items aus denen die momentane Menüstruktur besteht
        private IList<RadialMenuItem> CurrentItems;

        // Ein Stack um ein Verlauf der Menüstruktur zu haben
        Stack<IList<RadialMenuItem>> _subMenuItemsPath;
        internal Stack<IList<RadialMenuItem>> SubMenuItemsPath
        {
            get { return _subMenuItemsPath ?? (_subMenuItemsPath = new Stack<IList<RadialMenuItem>>()); }
            set { _subMenuItemsPath = value; }
        }

        private static void OnRadiusFactorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialMenu)sender;

            if (instance.IsTemplateApplied)
            {
                instance._suppressNavigatedEvent = true;
                instance.ConstructPie(instance.CurrentItems);
                instance._suppressNavigatedEvent = false;
            }
        }

        private static object CoerceRadiusFactor(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 0.5) doubleValue = 0.5;

            return doubleValue;
        }

        protected virtual void UpdateVisualState()
        {
            UpdateVisualState(true);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            if (IsOpen)
            {
                VisualStateManager.GoToState(this, "Open", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Closed", useTransitions);
            }
        }

        public override void OnApplyTemplate()
        {
            IsTemplateApplied = false;
            base.OnApplyTemplate();

            MasterBackgroundGrid = (Grid)GetTemplateChild("PART_BackgroundGrid");
            MainContent = (ItemsControl)GetTemplateChild("PART_MainContent");

            UpdateVisualState();

            _suppressNavigatedEvent = true;
            ConstructPie(Items);
            IsTemplateApplied = true;
            _suppressNavigatedEvent = false;
        }

        // Navigiert von einem SubItem in die höhere Menüstruktur zurück
        public void NavigateBack()
        {
            if (SubMenuItemsPath.Count == 0) return;

            ConstructPie(SubMenuItemsPath.Pop());
        }

        // Zeichnet die Visuelle Struktur des Menüs
        private void ConstructPie(IList<RadialMenuItem> items)
        {
            if (MainContent != null) MainContent.ItemsSource = items;

            if (items == null) return;

            CurrentItems = items;

            if (MasterBackgroundGrid == null) return;

            MasterBackgroundGrid.Children.Clear();

            var anglePerPiece = 360.0 / items.Count;
            // Zum bestimmen von Höhe und Breite die normalen Properties nehmen falls angegeben, sonst die Min-Variante, da ActualWidth und ActualHeight meistens 0 sind
            var width = double.IsNaN(Width) ? MinWidth : Width;
            var height = double.IsNaN(Height) ? MinHeight : Height;
            // Hälfte von Width und Height zum bestimmen des Mittelpunkts berechnen
            var halfWidth = width / 2.0;
            var halfHeight = height / 2.0;

            // Alle Radi zum Erstellen der Elemente berechnen
            var radius = height / 2;
            var innerRadius = radius * InnerRadiusFactor;
            var navigationRadius = radius * NavigationRadiusFactor;
            var contentRadius = radius - navigationRadius;

            // Der äußere Donut für die NavigationButtons
            var outerDonut = new Donut()
            {
                Style = NavigationMenuBackgroundStyle,
                CenterX = halfWidth,
                CenterY = halfHeight,
                InnerRadius = contentRadius,
                OuterRadius = radius
            };

            MasterBackgroundGrid.Children.Add(outerDonut);

            // Die Ellipse die als Hintergrund für die Buttons dient
            var ellipse = new Ellipse()
            {
                Style = ContentMenuBackgroundStyle,
                Height = contentRadius * 2,
                Width = contentRadius * 2
            };

            MasterBackgroundGrid.Children.Add(ellipse);

            // Die Grids zum halten des Background-Contents erstellen und dem MasterGrid hinzufügen
            var navigationGrid = new Grid();
            var contentGrid = new Grid();
            var mouseOverGrid = new Grid()
            {
                Visibility = UseRegionMouseOver ? Visibility.Visible : Visibility.Collapsed
            };

            MasterBackgroundGrid.Children.Add(navigationGrid);
            MasterBackgroundGrid.Children.Add(contentGrid);
            MasterBackgroundGrid.Children.Add(mouseOverGrid);

            // Für alle Items in der Liste die Background-Elemente erstellen
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                var startAngle = anglePerPiece * i - (anglePerPiece / 2);
                // Normaler Hintergrund
                var piePiece = new PiePiece()
                {
                    CenterX = halfWidth,
                    CenterY = halfHeight,
                    InnerRadius = innerRadius,
                    OuterRadius = contentRadius,
                    StartAngle = startAngle,
                    AngleDelta = anglePerPiece,
                    Padding = 1
                };

                piePiece.SetBinding(Shape.FillProperty, new Binding() { Source = item, Path = new PropertyPath("Background") });

                contentGrid.Children.Add(piePiece);
                // Transparente Variante des Hintergrundelements für HitTesting, da man sich auf das normale Hintergrundelement nicht verlassen kann
                var mouseOverPiePiece = new PiePiece()
                {
                    CenterX = halfWidth,
                    CenterY = halfHeight,
                    InnerRadius = innerRadius,
                    OuterRadius = contentRadius,
                    StartAngle = startAngle,
                    AngleDelta = anglePerPiece,
                    Padding = 1,
                    Fill = Brushes.Transparent
                };

                mouseOverPiePiece.MouseEnter += (s, e) =>
                {
                    if (item.IsEnabled) VisualStateManager.GoToState(item, "MouseOver", true);
                };

                mouseOverPiePiece.MouseLeave += (s, e) =>
                {
                    if (item.IsEnabled) VisualStateManager.GoToState(item, "Normal", true);
                };

                mouseOverPiePiece.MouseLeftButtonUp += (s, e) =>
                {
                    item.RaiseClickEventAndCommand();
                };

                mouseOverGrid.Children.Add(mouseOverPiePiece);

                // Wenn das Item SubItems hat, dann muss ein NavigationButton erstellt werden
                if (item.ChildItems.Count > 0)
                {
                    var navigationButton = new RadialMenuNavigationButton();

                    navigationGrid.Children.Add(navigationButton);

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
                    {
                        navigationButton.DrawBackground(halfWidth, halfHeight, contentRadius, radius, startAngle, anglePerPiece, 1);
                    }));

                    navigationButton.MouseLeftButtonUp += (s, e) =>
                    {
                        // Die momentanen Items in den Verlauf schreiben
                        SubMenuItemsPath.Push(items);
                        // Die Struktur für die Subitems erstellen
                        ConstructPie(item.ChildItems);
                    };
                }
            }
            // Wenn es sich um ein Neuzeichnen der aktuellen Struktur handelt, wird das NavigatedEvent nicht getriggert
            if (!_suppressNavigatedEvent)
            {
                var eventArgs = new RoutedEventArgs() { RoutedEvent = NavigatedEvent };

                RaiseEvent(eventArgs);
            }
        }
    }
}