using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TPF.Controls;

namespace TPF.Demo.Windows
{
    public partial class SkinEditorWindow : ChromelessWindow, System.ComponentModel.INotifyPropertyChanged
    {
        public SkinEditorWindow()
        {
            InitializeComponent();
        }

        #region SetProperty
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        Color _propertyColor;
        public Color PropertyColor
        {
            get { return _propertyColor; }
            set { SetProperty(ref _propertyColor, value); }
        }

        #region ApplicationBackground
        private void LoadApplicationBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ApplicationBackground is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyApplicationBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ApplicationBackground = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region TextBrush
        private void LoadTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.TextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.TextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region MouseOverTextBrush
        private void LoadMouseOverTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.MouseOverTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyMouseOverTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.MouseOverTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region PressedTextBrush
        private void LoadPressedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.PressedTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyPressedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.PressedTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SelectedTextBrush
        private void LoadSelectedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SelectedTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySelectedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SelectedTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ReadOnlyTextBrush
        private void LoadReadOnlyTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ReadOnlyTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyReadOnlyTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ReadOnlyTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region GlyphBrush
        private void LoadGlyphBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.GlyphBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyGlyphBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.GlyphBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region InputBackgroundBrush
        private void LoadInputBackgroundBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.InputBackgroundBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyInputBackgroundBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.InputBackgroundBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region PrimaryBrush
        private void LoadPrimaryBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.PrimaryBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyPrimaryBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.PrimaryBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SelectedBrush
        private void LoadSelectedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SelectedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySelectedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SelectedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region PressedBrush
        private void LoadPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.PressedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.PressedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region DisabledBrush
        private void LoadDisabledBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.DisabledBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyDisabledBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.DisabledBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region BorderBrush
        private void LoadBorderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.BorderBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyBorderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.BorderBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region MouseOverBrush
        private void LoadMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.MouseOverBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.MouseOverBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region AccentBrush
        private void LoadAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.AccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.AccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region MouseOverAccentBrush
        private void LoadMouseOverAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.MouseOverAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyMouseOverAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.MouseOverAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region FocusedAccentBrush
        private void LoadFocusedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.FocusedAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyFocusedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.FocusedAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region PressedAccentBrush
        private void LoadPressedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.PressedAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyPressedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.PressedAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region HeaderBrush
        private void LoadHeaderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.HeaderBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyHeaderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.HeaderBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ErrorBrush
        private void LoadErrorBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ErrorBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyErrorBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ErrorBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ProgressBarBrush
        private void LoadProgressBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ProgressBarBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyProgressBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ProgressBarBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region RippleBrush
        private void LoadRippleBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.RippleBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyRippleBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.RippleBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region HyperlinkBrush
        private void LoadHyperlinkBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.HyperlinkBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyHyperlinkBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.HyperlinkBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region HyperlinkVisitedBrush
        private void LoadHyperlinkVisitedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.HyperlinkVisitedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyHyperlinkVisitedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.HyperlinkVisitedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ScrollBarBackgroundBrush
        private void LoadScrollBarBackgroundBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ScrollBarBackgroundBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyScrollBarBackgroundBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ScrollBarBackgroundBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ScrollBarBrush
        private void LoadScrollBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ScrollBarBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyScrollBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ScrollBarBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ScrollBarMouseOverBrush
        private void LoadScrollBarMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ScrollBarMouseOverBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyScrollBarMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ScrollBarMouseOverBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region ScrollBarPressedBrush
        private void LoadScrollBarPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.ScrollBarPressedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplyScrollBarPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.ScrollBarPressedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryBrush
        private void LoadSecondaryBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryBorderBrush
        private void LoadSecondaryBorderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryBorderBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryBorderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryBorderBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryMouseOverBrush
        private void LoadSecondaryMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryMouseOverBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryMouseOverBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryMouseOverBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondarySelectedBrush
        private void LoadSecondarySelectedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondarySelectedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondarySelectedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondarySelectedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryPressedBrush
        private void LoadSecondaryPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryPressedBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryPressedBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryPressedBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryMouseOverTextBrush
        private void LoadSecondaryMouseOverTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryMouseOverTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryMouseOverTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryMouseOverTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryPressedTextBrush
        private void LoadSecondaryPressedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryPressedTextBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryPressedTextBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryPressedTextBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryAccentBrush
        private void LoadSecondaryAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryMouseOverAccentBrush
        private void LoadSecondaryMouseOverAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryMouseOverAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryMouseOverAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryMouseOverAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryFocusedAccentBrush
        private void LoadSecondaryFocusedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryFocusedAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryFocusedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryFocusedAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryPressedAccentBrush
        private void LoadSecondaryPressedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryPressedAccentBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryPressedAccentBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryPressedAccentBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryHeaderBrush
        private void LoadSecondaryHeaderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryHeaderBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryHeaderBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryHeaderBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion

        #region SecondaryProgressBarBrush
        private void LoadSecondaryProgressBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResourceManager.Resources.SecondaryProgressBarBrush is SolidColorBrush brush)
            {
                PropertyColor = brush.Color;
            }
        }

        private void ApplySecondaryProgressBarBrushButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceManager.Resources.SecondaryProgressBarBrush = new SolidColorBrush(PropertyColor);
        }
        #endregion
    }
}