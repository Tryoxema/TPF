using System.Collections.ObjectModel;
using System.Windows.Controls;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class StepProgressBarDemoView : ViewBase
    {
        public StepProgressBarDemoView()
        {
            InitializeComponent();

            Orientations.Add(Orientation.Horizontal);
            Orientations.Add(Orientation.Vertical);
        }

        public ObservableCollection<Orientation> Orientations { get; } = new ObservableCollection<Orientation>();

        private void StepProgressBar_IndicatorClicked(object sender, TPF.Controls.Specialized.StepProgressBar.IndicatorClickedEventArgs e)
        {
            var progressBar = (StepProgressBar)sender;

            var index = progressBar.Items.IndexOf(e.Step);

            progressBar.SelectedIndex = index;
        }
    }
}