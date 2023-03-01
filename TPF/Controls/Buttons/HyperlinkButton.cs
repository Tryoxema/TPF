using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class HyperlinkButton : ButtonBase
    {
        static HyperlinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(typeof(HyperlinkButton)));
        }

        #region NavigateUri DependencyProperty
        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register("NavigateUri",
            typeof(Uri),
            typeof(HyperlinkButton),
            new PropertyMetadata(null));

        public Uri NavigateUri
        {
            get { return (Uri)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }
        #endregion

        #region TargetName DependencyProperty
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName",
            typeof(string),
            typeof(HyperlinkButton),
            new PropertyMetadata(null));

        public string TargetName
        {
            get { return (string)GetValue(TargetNameProperty); }
            set { SetValue(TargetNameProperty, value); }
        }
        #endregion

        #region IsVisited DependencyProperty
        public static readonly DependencyProperty IsVisitedProperty = DependencyProperty.Register("IsVisited",
            typeof(bool),
            typeof(HyperlinkButton),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsVisited
        {
            get { return (bool)GetValue(IsVisitedProperty); }
            set { SetValue(IsVisitedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region VisitedForeground DependencyProperty
        public static readonly DependencyProperty VisitedForegroundProperty = DependencyProperty.Register("VisitedForeground",
            typeof(Brush),
            typeof(HyperlinkButton),
            new PropertyMetadata(null));

        public Brush VisitedForeground
        {
            get { return (Brush)GetValue(VisitedForegroundProperty); }
            set { SetValue(VisitedForegroundProperty, value); }
        }
        #endregion

        private Hyperlink Link;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Link != null)
            {
                Link.RequestNavigate -= Link_RequestNavigate;
            }

            Link = GetTemplateChild("PART_Hyperlink") as Hyperlink;

            if (Link != null)
            {
                Link.RequestNavigate += Link_RequestNavigate;
            }
        }

        private void Link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // Die einfache ToString-Variante als Standard-Wert nehmen
            var uri = e.Uri.ToString();
            // Wenn wir eine Absolute Uri haben, nehmen wir die
            // Ich bin mir nicht sicher, ob das hier nötig/sinnvoll ist, aber wir lassen das erstmal so
            if (e.Uri.IsAbsoluteUri) uri = e.Uri.AbsoluteUri;
            // Windows bitten, das öffnen durchzuführen
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = uri, UseShellExecute = true });
            // Der Link wurde nun besucht
            IsVisited = true;
            e.Handled = true;
        }
    }
}