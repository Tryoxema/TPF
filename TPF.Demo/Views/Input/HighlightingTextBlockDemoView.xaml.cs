using System.Collections.ObjectModel;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class HighlightingTextBlockDemoView : ViewBase
    {
        public HighlightingTextBlockDemoView()
        {
            InitializeComponent();

            TextHighlightingModes.Add(TextHighlightingMode.Bold);
            TextHighlightingModes.Add(TextHighlightingMode.Underline);
            TextHighlightingModes.Add(TextHighlightingMode.Brush);

            DemoHighlightingTextBlock.Text = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
        }

        public ObservableCollection<TextHighlightingMode> TextHighlightingModes { get; } = new ObservableCollection<TextHighlightingMode>();
    }
}