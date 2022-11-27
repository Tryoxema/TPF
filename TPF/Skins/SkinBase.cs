using System.Windows.Media;

namespace TPF.Skins
{
    public abstract class SkinBase : ISkin
    {
        public SkinBase()
        {
            FontSize = 12;
            DisabledOpacity = 0.4;
            RippleOpacity = 0.5;
        }

        // Der Name des Skins
        public string Name { get; set; }
        // Die Schriftgröße
        public double FontSize { get; set; }
        // Die Opacity der Elemente im IsEnabled = false Zustand
        public double DisabledOpacity { get; set; }
        // Die Opacity für den Ripple-Effekt des MaterialControls
        public double RippleOpacity { get; set; }
        // Der Hintergrund der Anwendung
        public Brush ApplicationBackground { get; set; }
        // Die Textfarbe
        public Brush TextBrush { get; set; }
        // Die Textfarbe für Elemente im MouseOver-Zustand
        public Brush MouseOverTextBrush { get; set; }
        // Die Textfarbe für Elemente im gedrückten Zustand
        public Brush PressedTextBrush { get; set; }
        // Die Textfarbe für ausgewählte Elemente
        public Brush SelectedTextBrush { get; set; }
        // Die Textfarbe für ReadOnly Elemente
        public Brush ReadOnlyTextBrush { get; set; }
        // Die Farbe für Glyphen und mit Path gezeichnete Icons
        public Brush GlyphBrush { get; set; }
        // Der Hintergrund von Elementen, die einen Input besitzen
        public Brush InputBackgroundBrush { get; set; }
        // Der Hintergrund von Elementen, die normalerweise keinen Input besitzen
        public Brush PrimaryBrush { get; set; }
        // Die Farbe für Elemente im ausgewählten Zustand
        public Brush SelectedBrush { get; set; }
        // Die Farbe für Button-Elemente im gedrückten Zustand
        public Brush PressedBrush { get; set; }
        // Die Farbe für Elemente im IsEnabled = false Zustand
        public Brush DisabledBrush { get; set; }
        // Die Farbe für den Rahmen von Elementen
        public Brush BorderBrush { get; set; }
        // Der Hintergrund für Elemente, über denen die Maus schwebt
        public Brush MouseOverBrush { get; set; }
        // Eine Farbe um Akzente zu setzen
        public Brush AccentBrush { get; set; }
        // Eine Farbe für Akzente im MouseOver Zustand
        public Brush MouseOverAccentBrush { get; set; }
        // Eine Farbe für Akzente im fokusierten Zustand
        public Brush FocusedAccentBrush { get; set; }
        // Eine Farbe für Akzente im gedrückten Zustand
        public Brush PressedAccentBrush { get; set; }
        // Der Hintergrund von Headern
        public Brush HeaderBrush { get; set; }
        // Die Farbe für Fehler
        public Brush ErrorBrush { get; set; }
        // Die Farbe für den Fortschritt im Ladebalken
        public Brush ProgressBarBrush { get; set; }
        // Die Farbe für den Ripple-Effekt im MaterialControl
        public Brush RippleBrush { get; set; }
        // Die Farbe für Hyperlinks
        public Brush HyperlinkBrush { get; set; }
        // Die Farbe für besuchte Hyperlinks
        public Brush HyperlinkVisitedBrush { get; set; }
        // Die Farbe für den Hintergrund einer Scroll-Leiste
        public Brush ScrollBarBackgroundBrush { get; set; }
        // Die Farbe für den Scrollbalken
        public Brush ScrollBarBrush { get; set; }
        // Die Farbe für den Scrollbalken im MouseOver Zustand
        public Brush ScrollBarMouseOverBrush { get; set; }
        // Die Farbe für den Scrollbalken im gedrückten Zustand
        public Brush ScrollBarPressedBrush { get; set; }
        // Eine Alternative zum PrimaryBrush
        public Brush SecondaryBrush { get; set; }
        // Eine Alternative zum BorderBrush
        public Brush SecondaryBorderBrush { get; set; }
        // Eine Alternative zum MouseOverBrush
        public Brush SecondaryMouseOverBrush { get; set; }
        // Eine Alternative zum SelectedBrush
        public Brush SecondarySelectedBrush { get; set; }
        // Eine Alternative zum PressedBrush
        public Brush SecondaryPressedBrush { get; set; }
        // Eine Alternative zum MouseOverTextBrush
        public Brush SecondaryMouseOverTextBrush { get; set; }
        // Eine Alternative zum PressedTextBrush
        public Brush SecondaryPressedTextBrush { get; set; }
        // Eine Alternative zum AccentBrush
        public Brush SecondaryAccentBrush { get; set; }
        // Eine Alternative zum MouseOverAccentBrush
        public Brush SecondaryMouseOverAccentBrush { get; set; }
        // Eine Alternative zum FocusedAccentBrush
        public Brush SecondaryFocusedAccentBrush { get; set; }
        // Eine Alternative zum PressedAccentBrush
        public Brush SecondaryPressedAccentBrush { get; set; }
        // Eine Alternative zum HeaderBrush
        public Brush SecondaryHeaderBrush { get; set; }
        // Eine Alternative zum ProgressBarBrush
        public Brush SecondaryProgressBarBrush { get; set; }

        protected static SolidColorBrush BrushFromString(string color)
        {
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));

            return brush;
        }
    }
}