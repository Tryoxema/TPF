namespace TPF.Skins
{
    public class SmoothLightSkin : SkinBase
    {
        private SmoothLightSkin()
        {
            Name = "SmoothLight";
            ApplicationBackground = BrushFromString("#F0F0F0");
            TextBrush = BrushFromString("#131313");
            MouseOverTextBrush = BrushFromString("#131313");
            PressedTextBrush = BrushFromString("#FFFFFF");
            SelectedTextBrush = BrushFromString("#FFFFFF");
            ReadOnlyTextBrush = BrushFromString("#717171");
            GlyphBrush = BrushFromString("#717171");
            InputBackgroundBrush = BrushFromString("#FFFFFF");
            PrimaryBrush = BrushFromString("#FDFDFD");
            SelectedBrush = BrushFromString("#005FB8");
            PressedBrush = BrushFromString("#005FB8");
            DisabledBrush = BrushFromString("#F5F5F5");
            BorderBrush = BrushFromString("#D6D6D6");
            MouseOverBrush = BrushFromString("#C9DEF5");
            AccentBrush = BrushFromString("#3399FF");
            MouseOverAccentBrush = BrushFromString("#3399FF");
            FocusedAccentBrush = BrushFromString("#3399FF");
            PressedAccentBrush = BrushFromString("#005FB8");
            HeaderBrush = BrushFromString("#005FB8");
            ErrorBrush = BrushFromString("#FF3333");
            ProgressBarBrush = BrushFromString("#208EFC");
            RippleBrush = BrushFromString("#FFFFFF");
            HyperlinkBrush = BrushFromString("#0066FF");
            HyperlinkVisitedBrush = BrushFromString("#0061A3");
            ScrollBarBackgroundBrush = BrushFromString("#00FFFFFF");
            ScrollBarBrush = BrushFromString("#C2C3C9");
            ScrollBarMouseOverBrush = BrushFromString("#686868");
            ScrollBarPressedBrush = BrushFromString("#5B5B5B");
            SecondaryBrush = BrushFromString("#F5F5F5");
            SecondaryBorderBrush = BrushFromString("#E0E0E0");
            SecondaryMouseOverBrush = BrushFromString("#C9DEF5");
            SecondarySelectedBrush = BrushFromString("#005FB8");
            SecondaryPressedBrush = BrushFromString("#005FB8");
            SecondaryMouseOverTextBrush = BrushFromString("#000000");
            SecondaryPressedTextBrush = BrushFromString("#FFFFFF");
            SecondaryAccentBrush = BrushFromString("#005FB8");
            SecondaryMouseOverAccentBrush = BrushFromString("#005FB8");
            SecondaryFocusedAccentBrush = BrushFromString("#005FB8");
            SecondaryPressedAccentBrush = BrushFromString("#005FB8");
            SecondaryHeaderBrush = BrushFromString("#005FB8");
            SecondaryProgressBarBrush = BrushFromString("#B6D9FF");
        }

        static SmoothLightSkin _instance;
        public static SmoothLightSkin Instance => _instance ?? (_instance = new SmoothLightSkin());
    }
}