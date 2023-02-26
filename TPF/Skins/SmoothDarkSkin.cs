namespace TPF.Skins
{
    public class SmoothDarkSkin : SkinBase
    {
        private SmoothDarkSkin()
        {
            Name = "SmoothDark";
            ApplicationBackground = BrushFromString("#202020");
            TextBrush = BrushFromString("#FFFFFF");
            MouseOverTextBrush = BrushFromString("#FFFFFF");
            PressedTextBrush = BrushFromString("#FFFFFF");
            SelectedTextBrush = BrushFromString("#FFFFFF");
            ReadOnlyTextBrush = BrushFromString("#717171");
            GlyphBrush = BrushFromString("#FFFFFF");
            InputBackgroundBrush = BrushFromString("#1F1F1F");
            PrimaryBrush = BrushFromString("#343434");
            SelectedBrush = BrushFromString("#005FB8");
            PressedBrush = BrushFromString("#005FB8");
            DisabledBrush = BrushFromString("#343434");
            BorderBrush = BrushFromString("#565656");
            MouseOverBrush = BrushFromString("#565656");
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
            ScrollBarBrush = BrushFromString("#9E9E9E");
            ScrollBarMouseOverBrush = BrushFromString("#BEBEBE");
            ScrollBarPressedBrush = BrushFromString("#C8C8C8");
            SecondaryBrush = BrushFromString("#272727");
            SecondaryBorderBrush = BrushFromString("#565656");
            SecondaryMouseOverBrush = BrushFromString("#565656");
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

        static SmoothDarkSkin _instance;
        public static SmoothDarkSkin Instance => _instance ?? (_instance = new SmoothDarkSkin());
    }
}