using System;
using System.Windows.Media;
using TPF.Skins;

namespace TPF.Controls
{
    public sealed class ResourceManager : System.ComponentModel.INotifyPropertyChanged
    {
        private ResourceManager()
        {
            ChangeSkinInternal(VS2013LightSkin.Instance);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }

        #region Properties
        static ResourceManager _resources;
        public static ResourceManager Resources
        {
            get { return _resources ?? (_resources = new ResourceManager()); }
        }

        #region SkinName
        string _skinName;
        public string SkinName
        {
            get { return _skinName; }
            set { SetProperty(ref _skinName, value); }
        }
        #endregion

        #region FontSize
        double _fontSize;
        public double FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }
        #endregion

        #region DisabledOpacity
        double _disabledOpacity;
        public double DisabledOpacity
        {
            get { return _disabledOpacity; }
            set { SetProperty(ref _disabledOpacity, value); }
        }
        #endregion

        #region RippleOpacity 
        double _rippleOpacity;
        public double RippleOpacity
        {
            get { return _rippleOpacity; }
            set { SetProperty(ref _rippleOpacity, value); }
        }
        #endregion

        #region ApplicationBackground 
        Brush _applicationBackground;
        public Brush ApplicationBackground
        {
            get { return _applicationBackground; }
            set { SetProperty(ref _applicationBackground, value); }
        }
        #endregion

        #region TextBrush
        Brush _textBrush;
        public Brush TextBrush
        {
            get { return _textBrush; }
            set { SetProperty(ref _textBrush, value); }
        }
        #endregion

        #region MouseOverTextBrush
        Brush _mouseOverTextBrush;
        public Brush MouseOverTextBrush
        {
            get { return _mouseOverTextBrush; }
            set { SetProperty(ref _mouseOverTextBrush, value); }
        }
        #endregion

        #region PressedTextBrush
        Brush _pressedTextBrush;
        public Brush PressedTextBrush
        {
            get { return _pressedTextBrush; }
            set { SetProperty(ref _pressedTextBrush, value); }
        }
        #endregion

        #region SelectedTextBrush
        Brush _selectedTextBrush;
        public Brush SelectedTextBrush
        {
            get { return _selectedTextBrush; }
            set { SetProperty(ref _selectedTextBrush, value); }
        }
        #endregion

        #region ReadOnlyTextBrush
        Brush _readOnlyTextBrush;
        public Brush ReadOnlyTextBrush
        {
            get { return _readOnlyTextBrush; }
            set { SetProperty(ref _readOnlyTextBrush, value); }
        }
        #endregion

        #region GlyphBrush
        Brush _glyphBrush;
        public Brush GlyphBrush
        {
            get { return _glyphBrush; }
            set { SetProperty(ref _glyphBrush, value); }
        }
        #endregion

        #region InputBackgroundBrush
        Brush _inputBackgroundBrush;
        public Brush InputBackgroundBrush
        {
            get { return _inputBackgroundBrush; }
            set { SetProperty(ref _inputBackgroundBrush, value); }
        }
        #endregion

        #region PrimaryBrush
        Brush _primaryBrush;
        public Brush PrimaryBrush
        {
            get { return _primaryBrush; }
            set { SetProperty(ref _primaryBrush, value); }
        }
        #endregion

        #region SelectedBrush
        Brush _selectedBrush;
        public Brush SelectedBrush
        {
            get { return _selectedBrush; }
            set { SetProperty(ref _selectedBrush, value); }
        }
        #endregion

        #region PressedBrush
        Brush _pressedBrush;
        public Brush PressedBrush
        {
            get { return _pressedBrush; }
            set { SetProperty(ref _pressedBrush, value); }
        }
        #endregion

        #region DisabledBrush
        Brush _disabledBrush;
        public Brush DisabledBrush
        {
            get { return _disabledBrush; }
            set { SetProperty(ref _disabledBrush, value); }
        }
        #endregion

        #region BorderBrush
        Brush _borderBrush;
        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { SetProperty(ref _borderBrush, value); }
        }
        #endregion

        #region MouseOverBrush
        Brush _mouseOverBrush;
        public Brush MouseOverBrush
        {
            get { return _mouseOverBrush; }
            set { SetProperty(ref _mouseOverBrush, value); }
        }
        #endregion

        #region AccentBrush
        Brush _accentBrush;
        public Brush AccentBrush
        {
            get { return _accentBrush; }
            set { SetProperty(ref _accentBrush, value); }
        }
        #endregion

        #region MouseOverAccentBrush
        Brush _mouseOverAccentBrush;
        public Brush MouseOverAccentBrush
        {
            get { return _mouseOverAccentBrush; }
            set { SetProperty(ref _mouseOverAccentBrush, value); }
        }
        #endregion

        #region FocusedAccentBrush
        Brush _focusedAccentBrush;
        public Brush FocusedAccentBrush
        {
            get { return _focusedAccentBrush; }
            set { SetProperty(ref _focusedAccentBrush, value); }
        }
        #endregion

        #region PressedAccentBrush
        Brush _pressedAccentBrush;
        public Brush PressedAccentBrush
        {
            get { return _pressedAccentBrush; }
            set { SetProperty(ref _pressedAccentBrush, value); }
        }
        #endregion

        #region HeaderBrush
        Brush _headerBrush;
        public Brush HeaderBrush
        {
            get { return _headerBrush; }
            set { SetProperty(ref _headerBrush, value); }
        }
        #endregion

        #region ErrorBrush
        Brush _errorBrush;
        public Brush ErrorBrush
        {
            get { return _errorBrush; }
            set { SetProperty(ref _errorBrush, value); }
        }
        #endregion

        #region ProgressBarBrush
        Brush _progressBarBrush;
        public Brush ProgressBarBrush
        {
            get { return _progressBarBrush; }
            set { SetProperty(ref _progressBarBrush, value); }
        }
        #endregion

        #region RippleBrush
        Brush _rippleBrush;
        public Brush RippleBrush
        {
            get { return _rippleBrush; }
            set { SetProperty(ref _rippleBrush, value); }
        }
        #endregion

        #region HyperlinkBrush
        Brush _hyperlinkBrush;
        public Brush HyperlinkBrush
        {
            get { return _hyperlinkBrush; }
            set { SetProperty(ref _hyperlinkBrush, value); }
        }
        #endregion

        #region HyperlinkVisitedBrush
        Brush _hyperlinkVisitedBrush;
        public Brush HyperlinkVisitedBrush
        {
            get { return _hyperlinkVisitedBrush; }
            set { SetProperty(ref _hyperlinkVisitedBrush, value); }
        }
        #endregion

        #region ScrollBarBackgroundBrush
        Brush _scrollBarBackgroundBrush;
        public Brush ScrollBarBackgroundBrush
        {
            get { return _scrollBarBackgroundBrush; }
            set { SetProperty(ref _scrollBarBackgroundBrush, value); }
        }
        #endregion

        #region ScrollBarBrush
        Brush _scrollBarBrush;
        public Brush ScrollBarBrush
        {
            get { return _scrollBarBrush; }
            set { SetProperty(ref _scrollBarBrush, value); }
        }
        #endregion

        #region ScrollBarMouseOverBrush
        Brush _scrollBarMouseOverBrush;
        public Brush ScrollBarMouseOverBrush
        {
            get { return _scrollBarMouseOverBrush; }
            set { SetProperty(ref _scrollBarMouseOverBrush, value); }
        }
        #endregion

        #region ScrollBarPressedBrush
        Brush _scrollBarPressedBrush;
        public Brush ScrollBarPressedBrush
        {
            get { return _scrollBarPressedBrush; }
            set { SetProperty(ref _scrollBarPressedBrush, value); }
        }
        #endregion

        #region SecondaryBrush
        Brush _secondaryBrush;
        public Brush SecondaryBrush
        {
            get { return _secondaryBrush; }
            set { SetProperty(ref _secondaryBrush, value); }
        }
        #endregion

        #region SecondaryBorderBrush
        Brush _secondaryBorderBrush;
        public Brush SecondaryBorderBrush
        {
            get { return _secondaryBorderBrush; }
            set { SetProperty(ref _secondaryBorderBrush, value); }
        }
        #endregion

        #region SecondaryMouseOverBrush
        Brush _secondaryMouseOverBrush;
        public Brush SecondaryMouseOverBrush
        {
            get { return _secondaryMouseOverBrush; }
            set { SetProperty(ref _secondaryMouseOverBrush, value); }
        }
        #endregion

        #region SecondarySelectedBrush
        Brush _secondarySelectedBrush;
        public Brush SecondarySelectedBrush
        {
            get { return _secondarySelectedBrush; }
            set { SetProperty(ref _secondarySelectedBrush, value); }
        }
        #endregion

        #region SecondaryPressedBrush
        Brush _secondaryPressedBrush;
        public Brush SecondaryPressedBrush
        {
            get { return _secondaryPressedBrush; }
            set { SetProperty(ref _secondaryPressedBrush, value); }
        }
        #endregion

        #region SecondaryMouseOverTextBrush
        Brush _secondaryMouseOverTextBrush;
        public Brush SecondaryMouseOverTextBrush
        {
            get { return _secondaryMouseOverTextBrush; }
            set { SetProperty(ref _secondaryMouseOverTextBrush, value); }
        }
        #endregion

        #region SecondaryPressedTextBrush
        Brush _secondaryPressedTextBrush;
        public Brush SecondaryPressedTextBrush
        {
            get { return _secondaryPressedTextBrush; }
            set { SetProperty(ref _secondaryPressedTextBrush, value); }
        }
        #endregion

        #region SecondaryAccentBrush
        Brush _secondaryAccentBrush;
        public Brush SecondaryAccentBrush
        {
            get { return _secondaryAccentBrush; }
            set { SetProperty(ref _secondaryAccentBrush, value); }
        }
        #endregion

        #region SecondaryMouseOverAccentBrush
        Brush _secondaryMouseOverAccentBrush;
        public Brush SecondaryMouseOverAccentBrush
        {
            get { return _secondaryMouseOverAccentBrush; }
            set { SetProperty(ref _secondaryMouseOverAccentBrush, value); }
        }
        #endregion

        #region SecondaryFocusedAccentBrush
        Brush _secondaryFocusedAccentBrush;
        public Brush SecondaryFocusedAccentBrush
        {
            get { return _secondaryFocusedAccentBrush; }
            set { SetProperty(ref _secondaryFocusedAccentBrush, value); }
        }
        #endregion

        #region SecondaryPressedAccentBrush
        Brush _secondaryPressedAccentBrush;
        public Brush SecondaryPressedAccentBrush
        {
            get { return _secondaryPressedAccentBrush; }
            set { SetProperty(ref _secondaryPressedAccentBrush, value); }
        }
        #endregion

        #region SecondaryHeaderBrush
        Brush _secondaryHeaderBrush;
        public Brush SecondaryHeaderBrush
        {
            get { return _secondaryHeaderBrush; }
            set { SetProperty(ref _secondaryHeaderBrush, value); }
        }
        #endregion

        #region SecondaryProgressBarBrush
        Brush _secondaryProgressBarBrush;
        public Brush SecondaryProgressBarBrush
        {
            get { return _secondaryProgressBarBrush; }
            set { SetProperty(ref _secondaryProgressBarBrush, value); }
        }
        #endregion
        #endregion

        public event EventHandler<SkinChangingEventArgs> SkinChanging;
        public event EventHandler<SkinChangedEventArgs> SkinChanged;

        public static void ChangeSkin(ISkin skin)
        {
            Resources.ChangeSkinInternal(skin);
        }

        private void ChangeSkinInternal(ISkin skin)
        {
            if (skin == null) return;

            var changingEventArgs = new SkinChangingEventArgs(skin);
            // Bescheid sagen, dass der Skin geändert wird
            SkinChanging?.Invoke(this, changingEventArgs);
            // Soll der Vorgang abgebrochen werden?
            if (changingEventArgs.Cancel) return;

            SkinName = skin.Name;
            FontSize = skin.FontSize;
            DisabledOpacity = skin.DisabledOpacity;
            RippleOpacity = skin.RippleOpacity;
            ApplicationBackground = skin.ApplicationBackground;
            TextBrush = skin.TextBrush;
            MouseOverTextBrush = skin.MouseOverTextBrush;
            PressedTextBrush = skin.PressedTextBrush;
            SelectedTextBrush = skin.SelectedTextBrush;
            ReadOnlyTextBrush = skin.ReadOnlyTextBrush;
            GlyphBrush = skin.GlyphBrush;
            InputBackgroundBrush = skin.InputBackgroundBrush;
            PrimaryBrush = skin.PrimaryBrush;
            SelectedBrush = skin.SelectedBrush;
            PressedBrush = skin.PressedBrush;
            DisabledBrush = skin.DisabledBrush;
            BorderBrush = skin.BorderBrush;
            MouseOverBrush = skin.MouseOverBrush;
            AccentBrush = skin.AccentBrush;
            MouseOverAccentBrush = skin.MouseOverAccentBrush;
            FocusedAccentBrush = skin.FocusedAccentBrush;
            PressedAccentBrush = skin.PressedAccentBrush;
            HeaderBrush = skin.HeaderBrush;
            ErrorBrush = skin.ErrorBrush;
            ProgressBarBrush = skin.ProgressBarBrush;
            RippleBrush = skin.RippleBrush;
            HyperlinkBrush = skin.HyperlinkBrush;
            HyperlinkVisitedBrush = skin.HyperlinkVisitedBrush;
            ScrollBarBackgroundBrush = skin.ScrollBarBackgroundBrush;
            ScrollBarBrush = skin.ScrollBarBrush;
            ScrollBarMouseOverBrush = skin.ScrollBarMouseOverBrush;
            ScrollBarPressedBrush = skin.ScrollBarPressedBrush;
            SecondaryBrush = skin.SecondaryBrush;
            SecondaryBorderBrush = skin.SecondaryBorderBrush;
            SecondaryMouseOverBrush = skin.SecondaryMouseOverBrush;
            SecondarySelectedBrush = skin.SecondarySelectedBrush;
            SecondaryPressedBrush = skin.SecondaryPressedBrush;
            SecondaryMouseOverTextBrush = skin.MouseOverTextBrush;
            SecondaryPressedTextBrush = skin.PressedTextBrush;
            SecondaryAccentBrush = skin.SecondaryAccentBrush;
            SecondaryMouseOverAccentBrush = skin.SecondaryMouseOverAccentBrush;
            SecondaryFocusedAccentBrush = skin.SecondaryFocusedAccentBrush;
            SecondaryPressedAccentBrush = skin.SecondaryPressedAccentBrush;
            SecondaryHeaderBrush = skin.SecondaryHeaderBrush;
            SecondaryProgressBarBrush = skin.SecondaryProgressBarBrush;

            // Alle Brushes müssen eingefroren werden, da an sonsten Fehler bei mehereren Threads auftreten können
            // Aus dem gleichen Grund kann auch kein DependencyObject als Basisklasse hierfür benutzt werden
            ApplicationBackground.Freeze();
            TextBrush.Freeze();
            MouseOverTextBrush.Freeze();
            PressedTextBrush.Freeze();
            SelectedTextBrush.Freeze();
            ReadOnlyTextBrush.Freeze();
            GlyphBrush.Freeze();
            InputBackgroundBrush.Freeze();
            PrimaryBrush.Freeze();
            SelectedBrush.Freeze();
            PressedBrush.Freeze();
            DisabledBrush.Freeze();
            BorderBrush.Freeze();
            MouseOverBrush.Freeze();
            AccentBrush.Freeze();
            MouseOverAccentBrush.Freeze();
            FocusedAccentBrush.Freeze();
            PressedAccentBrush.Freeze();
            HeaderBrush.Freeze();
            ErrorBrush.Freeze();
            ProgressBarBrush.Freeze();
            RippleBrush.Freeze();
            HyperlinkBrush.Freeze();
            HyperlinkVisitedBrush.Freeze();
            ScrollBarBackgroundBrush.Freeze();
            ScrollBarBrush.Freeze();
            ScrollBarMouseOverBrush.Freeze();
            ScrollBarPressedBrush.Freeze();
            SecondaryBrush.Freeze();
            SecondaryBorderBrush.Freeze();
            SecondaryMouseOverBrush.Freeze();
            SecondarySelectedBrush.Freeze();
            SecondaryPressedBrush.Freeze();
            SecondaryMouseOverTextBrush.Freeze();
            SecondaryPressedTextBrush.Freeze();
            SecondaryAccentBrush.Freeze();
            SecondaryMouseOverAccentBrush.Freeze();
            SecondaryFocusedAccentBrush.Freeze();
            SecondaryPressedAccentBrush.Freeze();
            SecondaryHeaderBrush.Freeze();
            SecondaryProgressBarBrush.Freeze();

            var changedEventArgs = new SkinChangedEventArgs(skin);
            // Bescheid sagen, dass der Skin geändert wurde
            SkinChanged?.Invoke(this, changedEventArgs);
        }
    }
}