using System;
using System.Windows.Media;

namespace TPF.Skins
{
    public interface ISkin
    {
        // Der Name des Skins
        string Name { get; set; }
        // Die Schriftgröße
        double FontSize { get; set; }
        // Opacity von deaktivierten Objekten
        double DisabledOpacity { get; set; }
        // Opacity des Ripple-Effekts
        double RippleOpacity { get; set; }
        // Der Hintergrund der Anwendung
        Brush ApplicationBackground { get; set; }
        // Die Textfarbe
        Brush TextBrush { get; set; }
        // Die Textfarbe für ausgewählte Elemente
        Brush SelectedTextBrush { get; set; }
        // Die Textfarbe für ReadOnly Elemente
        Brush ReadOnlyTextBrush { get; set; }
        // Die Farbe für Glyphen und mit Path gezeichnete Icons
        Brush GlyphBrush { get; set; }
        // Der Hintergrund von Elementen, die einen Input besitzen
        Brush InputBackgroundBrush { get; set; }
        // Der Hintergrund von Elementen, die normalerweise keinen Input besitzen
        Brush PrimaryBrush { get; set; }
        // Die Farbe für Elemente im ausgewählten Zustand
        Brush SelectedBrush { get; set; }
        // Die Farbe für Button-Elemente im gedrückten Zustand
        Brush PressedBrush { get; set; }
        // Die Farbe für Elemente im IsEnabled = false Zustand
        Brush DisabledBrush { get; set; }
        // Die Farbe für den Rahmen von Elementen
        Brush BorderBrush { get; set; }
        // Der Hintergrund für Elemente, über denen die Maus schwebt
        Brush MouseOverBrush { get; set; }
        // Eine Farbe um Akzente zu setzen
        Brush AccentBrush { get; set; }
        // Eine Farbe für Akzente im MouseOver Zustand
        Brush MouseOverAccentBrush { get; set; }
        // Eine Farbe für Akzente im fokusierten Zustand
        Brush FocusedAccentBrush { get; set; }
        // Eine Farbe für Akzente im gedrückten Zustand
        Brush PressedAccentBrush { get; set; }
        // Der Hintergrund von Headern
        Brush HeaderBrush { get; set; }
        // Die Farbe für Fehler
        Brush ErrorBrush { get; set; }
        // Die Farbe für den Fortschritt im Ladebalken
        Brush ProgressBarBrush { get; set; }
        // Die Farbe für den Ripple-Effekt im MaterialControl
        Brush RippleBrush { get; set; }
        // Die Farbe für Hyperlinks
        Brush HyperlinkBrush { get; set; }
        // Die Farbe für besuchte Hyperlinks
        Brush HyperlinkVisitedBrush { get; set; }
        // Die Farbe für den Hintergrund einer Scroll-Leiste
        Brush ScrollBarBackgroundBrush { get; set; }
        // Die Farbe für den Scrollbalken
        Brush ScrollBarBrush { get; set; }
        // Die Farbe für den Scrollbalken im MouseOver Zustand
        Brush ScrollBarMouseOverBrush { get; set; }
        // Die Farbe für den Scrollbalken im gedrückten Zustand
        Brush ScrollBarPressedBrush { get; set; }
        // Eine Alternative zum PrimaryBrush
        Brush SecondaryBrush { get; set; }
        // Eine Alternative zum BorderBrush
        Brush SecondaryBorderBrush { get; set; }
        // Eine Alternative zum MouseOverBrush
        Brush SecondaryMouseOverBrush { get; set; }
        // Eine Alternative zum SelectedBrush
        Brush SecondarySelectedBrush { get; set; }
        // Eine Alternative zum PressedBrush
        Brush SecondaryPressedBrush { get; set; }
        // Eine Alternative zum AccentBrush
        Brush SecondaryAccentBrush { get; set; }
        // Eine Alternative zum MouseOverAccentBrush
        Brush SecondaryMouseOverAccentBrush { get; set; }
        // Eine Alternative zum FocusedAccentBrush
        Brush SecondaryFocusedAccentBrush { get; set; }
        // Eine Alternative zum PressedAccentBrush
        Brush SecondaryPressedAccentBrush { get; set; }
        // Eine Alternative zum HeaderBrush
        Brush SecondaryHeaderBrush { get; set; }
        // Eine Alternative zum ProgressBarBrush
        Brush SecondaryProgressBarBrush { get; set; }
    }
}