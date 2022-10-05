using System;
using System.Windows.Media;

namespace TPF.Skins
{
    public class VS2013LightSkin : SkinBase
    {
        private VS2013LightSkin()
        {
            Name = "VS2013Light";
            ApplicationBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            TextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            SelectedTextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            ReadOnlyTextBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#717171"));
            GlyphBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#717171"));
            InputBackgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            PrimaryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEF2"));
            SelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            PressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            DisabledBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCEDB"));
            MouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9DEF5"));
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
            ScrollBarBackgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            ScrollBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C2C3C9"));
            ScrollBarMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#686868"));
            ScrollBarPressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5B5B5B"));
            SecondaryBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));
            SecondaryBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCEDB"));
            SecondaryMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9DEF5"));
            SecondarySelectedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryPressedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryMouseOverAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryFocusedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryPressedAccentBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryHeaderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));
            SecondaryProgressBarBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9DEF5"));
        }

        static VS2013LightSkin _instance;
        public static VS2013LightSkin Instance => _instance ?? (_instance = new VS2013LightSkin());
    }
}