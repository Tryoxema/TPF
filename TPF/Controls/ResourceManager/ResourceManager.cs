using System;
using System.Windows;
using System.Windows.Media;
using TPF.Skins;

namespace TPF.Controls
{
    public sealed class ResourceManager : DependencyObject
    {
        private ResourceManager()
        {
            ChangeSkinInternal(VS2013LightSkin.Instance);
        }

        #region Properties
        static ResourceManager _resources;
        public static ResourceManager Resources
        {
            get { return _resources ?? (_resources = new ResourceManager()); }
        }

        #region SkinName DependencyProperty
        public static readonly DependencyProperty SkinNameProperty = DependencyProperty.Register(nameof(SkinName), typeof(string), typeof(ResourceManager));

        public string SkinName
        {
            get { return (string)GetValue(SkinNameProperty); }
            set { SetValue(SkinNameProperty, value); }
        }
        #endregion

        #region FontSize DependencyProperty
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(ResourceManager));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        #endregion

        #region DisabledOpacity DependencyProperty
        public static readonly DependencyProperty DisabledOpacityProperty = DependencyProperty.Register(nameof(DisabledOpacity), typeof(double), typeof(ResourceManager));

        public double DisabledOpacity
        {
            get { return (double)GetValue(DisabledOpacityProperty); }
            set { SetValue(DisabledOpacityProperty, value); }
        }
        #endregion

        #region RippleOpacity DependencyProperty
        public static readonly DependencyProperty RippleOpacityProperty = DependencyProperty.Register(nameof(RippleOpacity), typeof(double), typeof(ResourceManager));

        public double RippleOpacity
        {
            get { return (double)GetValue(RippleOpacityProperty); }
            set { SetValue(RippleOpacityProperty, value); }
        }
        #endregion

        #region ApplicationBackground DependencyProperty
        public static readonly DependencyProperty ApplicationBackgroundProperty = DependencyProperty.Register(nameof(ApplicationBackground), typeof(Brush), typeof(ResourceManager));

        public Brush ApplicationBackground
        {
            get { return (Brush)GetValue(ApplicationBackgroundProperty); }
            set { SetValue(ApplicationBackgroundProperty, value); }
        }
        #endregion

        #region TextBrush DependencyProperty
        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register(nameof(TextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush TextBrush
        {
            get { return (Brush)GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }
        #endregion

        #region MouseOverTextBrush DependencyProperty
        public static readonly DependencyProperty MouseOverTextBrushProperty = DependencyProperty.Register(nameof(MouseOverTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush MouseOverTextBrush
        {
            get { return (Brush)GetValue(MouseOverTextBrushProperty); }
            set { SetValue(MouseOverTextBrushProperty, value); }
        }
        #endregion

        #region PressedTextBrush DependencyProperty
        public static readonly DependencyProperty PressedTextBrushProperty = DependencyProperty.Register(nameof(PressedTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush PressedTextBrush
        {
            get { return (Brush)GetValue(PressedTextBrushProperty); }
            set { SetValue(PressedTextBrushProperty, value); }
        }
        #endregion

        #region SelectedTextBrush DependencyProperty
        public static readonly DependencyProperty SelectedTextBrushProperty = DependencyProperty.Register(nameof(SelectedTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SelectedTextBrush
        {
            get { return (Brush)GetValue(SelectedTextBrushProperty); }
            set { SetValue(SelectedTextBrushProperty, value); }
        }
        #endregion

        #region ReadOnlyTextBrush DependencyProperty
        public static readonly DependencyProperty ReadOnlyTextBrushProperty = DependencyProperty.Register(nameof(ReadOnlyTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ReadOnlyTextBrush
        {
            get { return (Brush)GetValue(ReadOnlyTextBrushProperty); }
            set { SetValue(ReadOnlyTextBrushProperty, value); }
        }
        #endregion

        #region GlyphBrush DependencyProperty
        public static readonly DependencyProperty GlyphBrushProperty = DependencyProperty.Register(nameof(GlyphBrush), typeof(Brush), typeof(ResourceManager));

        public Brush GlyphBrush
        {
            get { return (Brush)GetValue(GlyphBrushProperty); }
            set { SetValue(GlyphBrushProperty, value); }
        }
        #endregion

        #region InputBackgroundBrush DependencyProperty
        public static readonly DependencyProperty InputBackgroundBrushProperty = DependencyProperty.Register(nameof(InputBackgroundBrush), typeof(Brush), typeof(ResourceManager));

        public Brush InputBackgroundBrush
        {
            get { return (Brush)GetValue(InputBackgroundBrushProperty); }
            set { SetValue(InputBackgroundBrushProperty, value); }
        }
        #endregion

        #region PrimaryBrush DependencyProperty
        public static readonly DependencyProperty PrimaryBrushProperty = DependencyProperty.Register(nameof(PrimaryBrush), typeof(Brush), typeof(ResourceManager));

        public Brush PrimaryBrush
        {
            get { return (Brush)GetValue(PrimaryBrushProperty); }
            set { SetValue(PrimaryBrushProperty, value); }
        }
        #endregion

        #region SelectedBrush DependencyProperty
        public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        #endregion

        #region PressedBrush DependencyProperty
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register(nameof(PressedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush PressedBrush
        {
            get { return (Brush)GetValue(PressedBrushProperty); }
            set { SetValue(PressedBrushProperty, value); }
        }
        #endregion

        #region DisabledBrush DependencyProperty
        public static readonly DependencyProperty DisabledBrushProperty = DependencyProperty.Register(nameof(DisabledBrush), typeof(Brush), typeof(ResourceManager));

        public Brush DisabledBrush
        {
            get { return (Brush)GetValue(DisabledBrushProperty); }
            set { SetValue(DisabledBrushProperty, value); }
        }
        #endregion

        #region BorderBrush DependencyProperty
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(ResourceManager));

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }
        #endregion

        #region MouseOverBrush DependencyProperty
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register(nameof(MouseOverBrush), typeof(Brush), typeof(ResourceManager));

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }
        #endregion

        #region AccentBrush DependencyProperty
        public static readonly DependencyProperty AccentBrushProperty = DependencyProperty.Register(nameof(AccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush AccentBrush
        {
            get { return (Brush)GetValue(AccentBrushProperty); }
            set { SetValue(AccentBrushProperty, value); }
        }
        #endregion

        #region MouseOverAccentBrush DependencyProperty
        public static readonly DependencyProperty MouseOverAccentBrushProperty = DependencyProperty.Register(nameof(MouseOverAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush MouseOverAccentBrush
        {
            get { return (Brush)GetValue(MouseOverAccentBrushProperty); }
            set { SetValue(MouseOverAccentBrushProperty, value); }
        }
        #endregion

        #region FocusedAccentBrush DependencyProperty
        public static readonly DependencyProperty FocusedAccentBrushProperty = DependencyProperty.Register(nameof(FocusedAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush FocusedAccentBrush
        {
            get { return (Brush)GetValue(FocusedAccentBrushProperty); }
            set { SetValue(FocusedAccentBrushProperty, value); }
        }
        #endregion

        #region PressedAccentBrush DependencyProperty
        public static readonly DependencyProperty PressedAccentBrushProperty = DependencyProperty.Register(nameof(PressedAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush PressedAccentBrush
        {
            get { return (Brush)GetValue(PressedAccentBrushProperty); }
            set { SetValue(PressedAccentBrushProperty, value); }
        }
        #endregion

        #region HeaderBrush DependencyProperty
        public static readonly DependencyProperty HeaderBrushProperty = DependencyProperty.Register(nameof(HeaderBrush), typeof(Brush), typeof(ResourceManager));

        public Brush HeaderBrush
        {
            get { return (Brush)GetValue(HeaderBrushProperty); }
            set { SetValue(HeaderBrushProperty, value); }
        }
        #endregion

        #region ErrorBrush DependencyProperty
        public static readonly DependencyProperty ErrorBrushProperty = DependencyProperty.Register(nameof(ErrorBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }
        #endregion

        #region ProgressBarBrush DependencyProperty
        public static readonly DependencyProperty ProgressBarBrushProperty = DependencyProperty.Register(nameof(ProgressBarBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ProgressBarBrush
        {
            get { return (Brush)GetValue(ProgressBarBrushProperty); }
            set { SetValue(ProgressBarBrushProperty, value); }
        }
        #endregion

        #region RippleBrush DependencyProperty
        public static readonly DependencyProperty RippleBrushProperty = DependencyProperty.Register(nameof(RippleBrush), typeof(Brush), typeof(ResourceManager));

        public Brush RippleBrush
        {
            get { return (Brush)GetValue(RippleBrushProperty); }
            set { SetValue(RippleBrushProperty, value); }
        }
        #endregion

        #region HyperlinkBrush DependencyProperty
        public static readonly DependencyProperty HyperlinkBrushProperty = DependencyProperty.Register(nameof(HyperlinkBrush), typeof(Brush), typeof(ResourceManager));

        public Brush HyperlinkBrush
        {
            get { return (Brush)GetValue(HyperlinkBrushProperty); }
            set { SetValue(HyperlinkBrushProperty, value); }
        }
        #endregion

        #region HyperlinkVisitedBrush DependencyProperty
        public static readonly DependencyProperty HyperlinkVisitedBrushProperty = DependencyProperty.Register(nameof(HyperlinkVisitedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush HyperlinkVisitedBrush
        {
            get { return (Brush)GetValue(HyperlinkVisitedBrushProperty); }
            set { SetValue(HyperlinkVisitedBrushProperty, value); }
        }
        #endregion

        #region ScrollBarBackgroundBrush DependencyProperty
        public static readonly DependencyProperty ScrollBarBackgroundBrushProperty = DependencyProperty.Register(nameof(ScrollBarBackgroundBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ScrollBarBackgroundBrush
        {
            get { return (Brush)GetValue(ScrollBarBackgroundBrushProperty); }
            set { SetValue(ScrollBarBackgroundBrushProperty, value); }
        }
        #endregion

        #region ScrollBarBrush DependencyProperty
        public static readonly DependencyProperty ScrollBarBrushProperty = DependencyProperty.Register(nameof(ScrollBarBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ScrollBarBrush
        {
            get { return (Brush)GetValue(ScrollBarBrushProperty); }
            set { SetValue(ScrollBarBrushProperty, value); }
        }
        #endregion

        #region ScrollBarMouseOverBrush DependencyProperty
        public static readonly DependencyProperty ScrollBarMouseOverBrushProperty = DependencyProperty.Register(nameof(ScrollBarMouseOverBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ScrollBarMouseOverBrush
        {
            get { return (Brush)GetValue(ScrollBarMouseOverBrushProperty); }
            set { SetValue(ScrollBarMouseOverBrushProperty, value); }
        }
        #endregion

        #region ScrollBarPressedBrush DependencyProperty
        public static readonly DependencyProperty ScrollBarPressedBrushProperty = DependencyProperty.Register(nameof(ScrollBarPressedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush ScrollBarPressedBrush
        {
            get { return (Brush)GetValue(ScrollBarPressedBrushProperty); }
            set { SetValue(ScrollBarPressedBrushProperty, value); }
        }
        #endregion

        #region SecondaryBrush DependencyProperty
        public static readonly DependencyProperty SecondaryBrushProperty = DependencyProperty.Register(nameof(SecondaryBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryBrush
        {
            get { return (Brush)GetValue(SecondaryBrushProperty); }
            set { SetValue(SecondaryBrushProperty, value); }
        }
        #endregion

        #region SecondaryBorderBrush DependencyProperty
        public static readonly DependencyProperty SecondaryBorderBrushProperty = DependencyProperty.Register(nameof(SecondaryBorderBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryBorderBrush
        {
            get { return (Brush)GetValue(SecondaryBorderBrushProperty); }
            set { SetValue(SecondaryBorderBrushProperty, value); }
        }
        #endregion

        #region SecondaryMouseOverBrush DependencyProperty
        public static readonly DependencyProperty SecondaryMouseOverBrushProperty = DependencyProperty.Register(nameof(SecondaryMouseOverBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryMouseOverBrush
        {
            get { return (Brush)GetValue(SecondaryMouseOverBrushProperty); }
            set { SetValue(SecondaryMouseOverBrushProperty, value); }
        }
        #endregion

        #region SecondarySelectedBrush DependencyProperty
        public static readonly DependencyProperty SecondarySelectedBrushProperty = DependencyProperty.Register(nameof(SecondarySelectedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondarySelectedBrush
        {
            get { return (Brush)GetValue(SecondarySelectedBrushProperty); }
            set { SetValue(SecondarySelectedBrushProperty, value); }
        }
        #endregion

        #region SecondaryPressedBrush DependencyProperty
        public static readonly DependencyProperty SecondaryPressedBrushProperty = DependencyProperty.Register(nameof(SecondaryPressedBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryPressedBrush
        {
            get { return (Brush)GetValue(SecondaryPressedBrushProperty); }
            set { SetValue(SecondaryPressedBrushProperty, value); }
        }
        #endregion

        #region SecondaryMouseOverTextBrush DependencyProperty
        public static readonly DependencyProperty SecondaryMouseOverTextBrushProperty = DependencyProperty.Register(nameof(SecondaryMouseOverTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryMouseOverTextBrush
        {
            get { return (Brush)GetValue(SecondaryMouseOverTextBrushProperty); }
            set { SetValue(SecondaryMouseOverTextBrushProperty, value); }
        }
        #endregion

        #region SecondaryPressedTextBrush DependencyProperty
        public static readonly DependencyProperty SecondaryPressedTextBrushProperty = DependencyProperty.Register(nameof(SecondaryPressedTextBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryPressedTextBrush
        {
            get { return (Brush)GetValue(SecondaryPressedTextBrushProperty); }
            set { SetValue(SecondaryPressedTextBrushProperty, value); }
        }
        #endregion

        #region SecondaryAccentBrush DependencyProperty
        public static readonly DependencyProperty SecondaryAccentBrushProperty = DependencyProperty.Register(nameof(SecondaryAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryAccentBrush
        {
            get { return (Brush)GetValue(SecondaryAccentBrushProperty); }
            set { SetValue(SecondaryAccentBrushProperty, value); }
        }
        #endregion

        #region SecondaryMouseOverAccentBrush DependencyProperty
        public static readonly DependencyProperty SecondaryMouseOverAccentBrushProperty = DependencyProperty.Register(nameof(SecondaryMouseOverAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryMouseOverAccentBrush
        {
            get { return (Brush)GetValue(SecondaryMouseOverAccentBrushProperty); }
            set { SetValue(SecondaryMouseOverAccentBrushProperty, value); }
        }
        #endregion

        #region SecondaryFocusedAccentBrush DependencyProperty
        public static readonly DependencyProperty SecondaryFocusedAccentBrushProperty = DependencyProperty.Register(nameof(SecondaryFocusedAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryFocusedAccentBrush
        {
            get { return (Brush)GetValue(SecondaryFocusedAccentBrushProperty); }
            set { SetValue(SecondaryFocusedAccentBrushProperty, value); }
        }
        #endregion

        #region SecondaryPressedAccentBrush DependencyProperty
        public static readonly DependencyProperty SecondaryPressedAccentBrushProperty = DependencyProperty.Register(nameof(SecondaryPressedAccentBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryPressedAccentBrush
        {
            get { return (Brush)GetValue(SecondaryPressedAccentBrushProperty); }
            set { SetValue(SecondaryPressedAccentBrushProperty, value); }
        }
        #endregion

        #region SecondaryHeaderBrush DependencyProperty
        public static readonly DependencyProperty SecondaryHeaderBrushProperty = DependencyProperty.Register(nameof(SecondaryHeaderBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryHeaderBrush
        {
            get { return (Brush)GetValue(SecondaryHeaderBrushProperty); }
            set { SetValue(SecondaryHeaderBrushProperty, value); }
        }
        #endregion

        #region SecondaryProgressBarBrush DependencyProperty
        public static readonly DependencyProperty SecondaryProgressBarBrushProperty = DependencyProperty.Register(nameof(SecondaryProgressBarBrush), typeof(Brush), typeof(ResourceManager));

        public Brush SecondaryProgressBarBrush
        {
            get { return (Brush)GetValue(SecondaryProgressBarBrushProperty); }
            set { SetValue(SecondaryProgressBarBrushProperty, value); }
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

            var changedEventArgs = new SkinChangedEventArgs(skin);
            // Bescheid sagen, dass der Skin geändert wurde
            SkinChanged?.Invoke(this, changedEventArgs);
        }
    }
}