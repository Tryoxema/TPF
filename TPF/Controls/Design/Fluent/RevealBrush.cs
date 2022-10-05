using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Data;
using TPF.Converter;

namespace TPF.Controls
{
    public class RevealBrushExtension : MarkupExtension
    {
        public Color Color { get; set; } = Colors.Black;

        public double Opacity { get; set; } = 1;

        public double Size { get; set; } = 100;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            var target = provider.TargetObject as DependencyObject;

            var backgroundColor = Color.FromArgb(0, Color.R, Color.G, Color.B);
            // GradientBrush erstellen
            var gradient = new RadialGradientBrush(Color, backgroundColor)
            {
                MappingMode = BrushMappingMode.Absolute,
                RadiusX = Size,
                RadiusY = Size
            };

            // Binding was die Opacity kontrolliert erstellen
            var opacityBinding = new Binding("Opacity")
            {
                Source = target,
                Path = new PropertyPath(MouseTracker.IsInsideProperty),
                Converter = new RevealBrushOpacityConverter(),
                ConverterParameter = Opacity
            };

            BindingOperations.SetBinding(gradient, Brush.OpacityProperty, opacityBinding);

            // Binding was die Position des Gradients kontrolliert erstellen
            var positionBinding = new MultiBinding()
            {
                Converter = new RelativePositionMultiValueConverter()
            };

            // 1. Binding: Parent
            positionBinding.Bindings.Add(new Binding() { Source = target, Path = new PropertyPath(MouseTracker.RootObjectProperty) });
            // 2. Binding: Control was den RevealBrush bekommen soll
            positionBinding.Bindings.Add(new Binding() { Source = target });
            // 3. Binding: Die Position der Maus im Parent-Control
            positionBinding.Bindings.Add(new Binding() { Source = target, Path = new PropertyPath(MouseTracker.PositionProperty) });

            // Bindings auf Center und Origin setzen
            BindingOperations.SetBinding(gradient, RadialGradientBrush.CenterProperty, positionBinding);
            BindingOperations.SetBinding(gradient, RadialGradientBrush.GradientOriginProperty, positionBinding);

            return gradient;
        }
    }
}