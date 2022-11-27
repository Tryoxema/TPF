namespace TPF.Skins
{
    public class VS2013LightSkin : SkinBase
    {
        private VS2013LightSkin()
        {
            Name = "VS2013Light";
            ApplicationBackground = BrushFromString("#FFFFFF");
            TextBrush = BrushFromString("#000000");
            MouseOverTextBrush = BrushFromString("#000000");
            PressedTextBrush = BrushFromString("#FFFFFF");
            SelectedTextBrush = BrushFromString("#FFFFFF");
            ReadOnlyTextBrush = BrushFromString("#717171");
            GlyphBrush = BrushFromString("#717171");
            InputBackgroundBrush = BrushFromString("#FFFFFF");
            PrimaryBrush = BrushFromString("#EEEEF2");
            SelectedBrush = BrushFromString("#007ACC");
            PressedBrush = BrushFromString("#007ACC");
            DisabledBrush = BrushFromString("#F5F5F5");
            BorderBrush = BrushFromString("#CCCEDB");
            MouseOverBrush = BrushFromString("#C9DEF5");
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
            ScrollBarBackgroundBrush = BrushFromString("#E8E8EC");
            ScrollBarBrush = BrushFromString("#C2C3C9");
            ScrollBarMouseOverBrush = BrushFromString("#686868");
            ScrollBarPressedBrush = BrushFromString("#5B5B5B");
            SecondaryBrush = BrushFromString("#F5F5F5");
            SecondaryBorderBrush = BrushFromString("#CCCEDB");
            SecondaryMouseOverBrush = BrushFromString("#C9DEF5");
            SecondarySelectedBrush = BrushFromString("#007ACC");
            SecondaryPressedBrush = BrushFromString("#007ACC");
            SecondaryMouseOverTextBrush = BrushFromString("#000000");
            SecondaryPressedTextBrush = BrushFromString("#FFFFFF");
            SecondaryAccentBrush = BrushFromString("#007ACC");
            SecondaryMouseOverAccentBrush = BrushFromString("#007ACC");
            SecondaryFocusedAccentBrush = BrushFromString("#007ACC");
            SecondaryPressedAccentBrush = BrushFromString("#007ACC");
            SecondaryHeaderBrush = BrushFromString("#007ACC");
            SecondaryProgressBarBrush = BrushFromString("#C9DEF5");
        }

        static VS2013LightSkin _instance;
        public static VS2013LightSkin Instance => _instance ?? (_instance = new VS2013LightSkin());
    }
}