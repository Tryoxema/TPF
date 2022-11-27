namespace TPF.Skins
{
    public class VS2013DarkSkin : SkinBase
    {
        private VS2013DarkSkin()
        {
            Name = "VS2013Dark";
            ApplicationBackground = BrushFromString("#2D2D30");
            TextBrush = BrushFromString("#F1F1F1");
            MouseOverTextBrush = BrushFromString("#F1F1F1");
            PressedTextBrush = BrushFromString("#FFFFFF");
            SelectedTextBrush = BrushFromString("#FFFFFF");
            ReadOnlyTextBrush = BrushFromString("#999999");
            GlyphBrush = BrushFromString("#999999");
            InputBackgroundBrush = BrushFromString("#252526");
            PrimaryBrush = BrushFromString("#2D2D30");
            SelectedBrush = BrushFromString("#007ACC");
            PressedBrush = BrushFromString("#007ACC");
            DisabledBrush = BrushFromString("#252526");
            BorderBrush = BrushFromString("#3F3F46");
            MouseOverBrush = BrushFromString("#3E3E40");
            AccentBrush = BrushFromString("#3399FF");
            MouseOverAccentBrush = BrushFromString("#007ACC");
            FocusedAccentBrush = BrushFromString("#007ACC");
            PressedAccentBrush = BrushFromString("#007ACC");
            HeaderBrush = BrushFromString("#007ACC");
            ErrorBrush = BrushFromString("#FF3333");
            ProgressBarBrush = BrushFromString("#3399FF");
            RippleBrush = BrushFromString("#FFFFFF");
            HyperlinkBrush = BrushFromString("#0066FF");
            HyperlinkVisitedBrush = BrushFromString("#0061A3");
            ScrollBarBackgroundBrush = BrushFromString("#3E3E42");
            ScrollBarBrush = BrushFromString("#686868");
            ScrollBarMouseOverBrush = BrushFromString("#9E9E9E");
            ScrollBarPressedBrush = BrushFromString("#EFEBEF");
            SecondaryBrush = BrushFromString("#252526");
            SecondaryBorderBrush = BrushFromString("#3F3F46");
            SecondaryMouseOverBrush = BrushFromString("#3E3E40");
            SecondarySelectedBrush = BrushFromString("#007ACC");
            SecondaryPressedBrush = BrushFromString("#007ACC");
            SecondaryMouseOverTextBrush = BrushFromString("#F1F1F1");
            SecondaryPressedTextBrush = BrushFromString("#FFFFFF");
            SecondaryAccentBrush = BrushFromString("#007ACC");
            SecondaryMouseOverAccentBrush = BrushFromString("#007ACC");
            SecondaryFocusedAccentBrush = BrushFromString("#007ACC");
            SecondaryPressedAccentBrush = BrushFromString("#007ACC");
            SecondaryHeaderBrush = BrushFromString("#007ACC");
            SecondaryProgressBarBrush = BrushFromString("#90CAF9");
        }

        static VS2013DarkSkin _instance;
        public static VS2013DarkSkin Instance => _instance ?? (_instance = new VS2013DarkSkin());
    }
}