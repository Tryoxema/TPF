using System;
using System.Windows.Media;

namespace TPF.Skins
{
    public class VS2013DarkSkin : SkinBase
    {
        private VS2013DarkSkin()
        {
            Name = "VS2013Dark";
            ApplicationBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D30"));
            TextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F1F1"));
            SelectedTextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ReadOnlyTextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
            GlyphBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
            InputBackgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));
            PrimaryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D30"));
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            PressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            DisabledBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3F46"));
            MouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E40"));
            AccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3399FF"));
            MouseOverAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            FocusedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            PressedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            HeaderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            ErrorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3333"));
            ProgressBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3399FF"));
            RippleBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            HyperlinkBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0066FF"));
            HyperlinkVisitedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0061A3"));
            ScrollBarBackgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E42"));
            ScrollBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#686868"));
            ScrollBarMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
            ScrollBarPressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EFEBEF"));
            SecondaryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252526"));
            SecondaryBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3F46"));
            SecondaryMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E3E40"));
            SecondarySelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryPressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryMouseOverAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryFocusedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryPressedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryHeaderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryProgressBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90CAF9"));
        }

        static VS2013DarkSkin _instance;
        public static VS2013DarkSkin Instance => _instance ?? (_instance = new VS2013DarkSkin());
    }
}