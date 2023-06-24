using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace TPF.Controls
{
    public class GlyphExtension : MarkupExtension
    {
        public Geometry Geometry { get; set; }

        public Brush Stroke { get; set; }

        public Brush Fill { get; set; }

        public GlyphExtension() { }

        public GlyphExtension(Geometry geometry)
        {
            Geometry = geometry;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var fill = Fill;
            var stroke = Stroke;
            if (fill == null && stroke == null) fill = Brushes.Black;

            var image = new DrawingImage(new GeometryDrawing(fill, new Pen(stroke, 1.0), Geometry));

            // Freeze für Performance
            image.Freeze();

            return image;
        }
    }
}