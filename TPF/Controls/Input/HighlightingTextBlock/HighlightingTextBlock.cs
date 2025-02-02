using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using TPF.Internal;

namespace TPF.Controls
{
    public class HighlightingTextBlock : FrameworkElement
    {
        static HighlightingTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightingTextBlock), new FrameworkPropertyMetadata(typeof(HighlightingTextBlock)));
        }

        public HighlightingTextBlock()
        {
            InternalTextBlock = CreateTextBlock(this);

            AddVisualChild(InternalTextBlock);
        }

        #region Text DependencyProperty
        public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner(typeof(HighlightingTextBlock),
            new PropertyMetadata(string.Empty, OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HighlightingTextBlock)sender;

            instance.UpdateText();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region Foreground DependencyProperty
        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(HighlightingTextBlock));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        #endregion

        #region Background DependencyProperty
        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(HighlightingTextBlock));

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        #endregion

        #region FontSize DependencyProperty
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(HighlightingTextBlock));

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        #endregion

        #region FontStretch DependencyProperty
        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(HighlightingTextBlock));

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }
        #endregion

        #region FontWeight DependencyProperty
        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(HighlightingTextBlock));

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }
        #endregion

        #region FontStyle DependencyProperty
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(HighlightingTextBlock));

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }
        #endregion

        #region FontFamily DependencyProperty
        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(HighlightingTextBlock));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        #endregion

        #region TextAlignment DependencyProperty
        public static readonly DependencyProperty TextAlignmentProperty = TextBlock.TextAlignmentProperty.AddOwner(typeof(HighlightingTextBlock));

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        #endregion

        #region TextWrapping DependencyProperty
        public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(HighlightingTextBlock));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        #endregion

        #region TextTrimming DependencyProperty
        public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(HighlightingTextBlock));

        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }
        #endregion

        #region Padding DependencyProperty
        public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(typeof(HighlightingTextBlock));

        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        #endregion

        #region HighlightText DependencyProperty
        public static readonly DependencyProperty HighlightTextProperty = DependencyProperty.Register("HighlightText",
            typeof(string),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(null, OnHighlightTextPropertyChanged));

        private static void OnHighlightTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HighlightingTextBlock)sender;

            instance.UpdateText();
        }

        public string HighlightText
        {
            get { return (string)GetValue(HighlightTextProperty); }
            set { SetValue(HighlightTextProperty, value); }
        }
        #endregion

        #region HighlightingMode DependencyProperty
        public static readonly DependencyProperty HighlightingModeProperty = DependencyProperty.Register("HighlightingMode",
            typeof(TextHighlightingMode),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(TextHighlightingMode.Bold, OnHighlightingModePropertyChanged));

        private static void OnHighlightingModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HighlightingTextBlock)sender;

            instance.UpdateHighlighting();
        }

        public TextHighlightingMode HighlightingMode
        {
            get { return (TextHighlightingMode)GetValue(HighlightingModeProperty); }
            set { SetValue(HighlightingModeProperty, value); }
        }
        #endregion

        #region HighlightingForeground DependencyProperty
        public static readonly DependencyProperty HighlightingForegroundProperty = DependencyProperty.Register("HighlightingForeground",
            typeof(Brush),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(null, OnHighlightingBrushPropertyChanged));

        private static void OnHighlightingBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HighlightingTextBlock)sender;

            if (instance.HighlightingMode == TextHighlightingMode.Brush) instance.UpdateHighlighting();
        }

        public Brush HighlightingForeground
        {
            get { return (Brush)GetValue(HighlightingForegroundProperty); }
            set { SetValue(HighlightingForegroundProperty, value); }
        }
        #endregion

        #region HighlightingBackground DependencyProperty
        public static readonly DependencyProperty HighlightingBackgroundProperty = DependencyProperty.Register("HighlightingBackground",
            typeof(Brush),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(null, OnHighlightingBrushPropertyChanged));

        public Brush HighlightingBackground
        {
            get { return (Brush)GetValue(HighlightingBackgroundProperty); }
            set { SetValue(HighlightingBackgroundProperty, value); }
        }
        #endregion

        #region IgnoreCase DependencyProperty
        public static readonly DependencyProperty IgnoreCaseProperty = DependencyProperty.Register("IgnoreCase",
            typeof(bool),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIgnoreCasePropertyChanged));

        private static void OnIgnoreCasePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HighlightingTextBlock)sender;

            instance.UpdateText();
        }

        public bool IgnoreCase
        {
            get { return (bool)GetValue(IgnoreCaseProperty); }
            set { SetValue(IgnoreCaseProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ContainsMatch ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey ContainsMatchPropertyKey = DependencyProperty.RegisterReadOnly("ContainsMatch",
            typeof(bool),
            typeof(HighlightingTextBlock),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty ContainsMatchProperty = ContainsMatchPropertyKey.DependencyProperty;

        public bool ContainsMatch
        {
            get { return (bool)GetValue(ContainsMatchProperty); }
            protected set { SetValue(ContainsMatchPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        internal TextBlock InternalTextBlock;

        private List<Tuple<string, bool>> _searchResults;
        internal List<Tuple<string, bool>> SearchResults
        {
            get { return _searchResults ?? (_searchResults = new List<Tuple<string, bool>>()); }
        }

        protected override Visual GetVisualChild(int index)
        {
            return InternalTextBlock;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return InternalTextBlock != null ? 1 : 0;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = new Size(0.0, 0.0);

            if (InternalTextBlock != null)
            {
                InternalTextBlock.Measure(availableSize);
                desiredSize = InternalTextBlock.DesiredSize;
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (InternalTextBlock != null) InternalTextBlock.Arrange(new Rect(new Point(0.0, 0.0), finalSize));

            return finalSize;
        }

        private static TextBlock CreateTextBlock(HighlightingTextBlock parent)
        {
            var textBlock = new TextBlock();

            textBlock.SetBinding(TextBlock.ForegroundProperty, new Binding("Foreground") { Source = parent });
            textBlock.SetBinding(TextBlock.BackgroundProperty, new Binding("Background") { Source = parent });
            textBlock.SetBinding(TextBlock.FontSizeProperty, new Binding("FontSize") { Source = parent });
            textBlock.SetBinding(TextBlock.FontStretchProperty, new Binding("FontStretch") { Source = parent });
            textBlock.SetBinding(TextBlock.FontWeightProperty, new Binding("FontWeight") { Source = parent });
            textBlock.SetBinding(TextBlock.FontStyleProperty, new Binding("FontStyle") { Source = parent });
            textBlock.SetBinding(TextBlock.FontFamilyProperty, new Binding("FontFamily") { Source = parent });
            textBlock.SetBinding(TextBlock.TextAlignmentProperty, new Binding("TextAlignment") { Source = parent });
            textBlock.SetBinding(TextBlock.TextWrappingProperty, new Binding("TextWrapping") { Source = parent });
            textBlock.SetBinding(TextBlock.TextTrimmingProperty, new Binding("TextTrimming") { Source = parent });
            textBlock.SetBinding(MarginProperty, new Binding("Margin") { Source = parent });
            textBlock.SetBinding(TextBlock.PaddingProperty, new Binding("Padding") { Source = parent });

            return textBlock;
        }

        private void UpdateText()
        {
            ContainsMatch = false;
            SearchResults.Clear();
            InternalTextBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(Text)) return;

            if (string.IsNullOrEmpty(HighlightText))
            {
                InternalTextBlock.Text = Text;
                SearchResults.Add(new Tuple<string, bool>(Text, false));
                return;
            }

            PerformSearch();

            var runs = GetRuns();

            InternalTextBlock.Inlines.AddRange(runs);
        }

        private void UpdateHighlighting()
        {
            // Die Funktion soll nur tatsächlich etwas tun wenn es auch etwas zum Hervorheben gibt
            if (!ContainsMatch) return;

            InternalTextBlock.Inlines.Clear();

            // SearchResults stellt den gesplitteten String dar.
            // Wenn hier 0 Einträge drin sind, gibt es keinen Text
            if (SearchResults.Count == 0) return;

            var runs = GetRuns();

            InternalTextBlock.Inlines.AddRange(runs);
        }

        // Durchsucht Text nach allen Vorkommen von HighlightText und trägt den aufgeteilten string in SearchResults ein
        private void PerformSearch()
        {
            SearchResults.Clear();

            var comparison = IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            // Wert aus DependencyProperty auslesen um viele Zugriffe auf GetValue zu verhindern
            var text = Text;
            var highlightText = HighlightText;

            var currentIndex = 0;

            while (true)
            {
                var matchIndex = text.IndexOf(highlightText, currentIndex, comparison);

                if (matchIndex > -1)
                {
                    var unmatched = text.Substring(currentIndex, matchIndex - currentIndex);
                    var matched = text.Substring(matchIndex, highlightText.Length);

                    SearchResults.Add(new Tuple<string, bool>(unmatched, false));
                    SearchResults.Add(new Tuple<string, bool>(matched, true));

                    ContainsMatch = true;

                    currentIndex = matchIndex + highlightText.Length;
                }
                else
                {
                    var unmatched = text.Substring(currentIndex);

                    SearchResults.Add(new Tuple<string, bool>(unmatched, false));

                    break;
                }
            }
        }

        // Wandelt den string in SearchResults um in eine Liste aus Runs
        private List<Run> GetRuns()
        {
            var runs = new List<Run>();

            for (int i = 0; i < SearchResults.Count; i++)
            {
                var result = SearchResults[i];

                runs.Add(GetRun(result.Item1, result.Item2));
            }

            return runs;
        }

        // Erstellt einen Run mit Highlighting oder ohne
        private Run GetRun(string text, bool highlight)
        {
            var run = new Run(text);

            if (highlight)
            {
                switch (HighlightingMode)
                {
                    case TextHighlightingMode.Bold: run.FontWeight = FontWeights.Bold; break;
                    case TextHighlightingMode.Underline: run.TextDecorations.Add(TextDecorations.Underline); break;
                    case TextHighlightingMode.Brush:
                    {
                        run.Foreground = HighlightingForeground;
                        run.Background = HighlightingBackground;
                        break;
                    }
                }
            }

            return run;
        }
    }
}