using System.Collections.ObjectModel;
using System.Windows.Controls;
using TPF.Controls;

namespace TPF.Demo.Net461.Views
{
    public partial class StepProgressBarView : UserControl
    {
        public StepProgressBarView()
        {
            InitializeComponent();

            Orientations.Add(Orientation.Horizontal);
            Orientations.Add(Orientation.Vertical);
        }

        ObservableCollection<Orientation> _orientations;
        public ObservableCollection<Orientation> Orientations
        {
            get { return _orientations ?? (_orientations = new ObservableCollection<Orientation>()); }
        }

        private void StepProgressBar_IndicatorClicked(object sender, TPF.Controls.Specialized.StepProgressBar.IndicatorClickedEventArgs e)
        {
            var progressBar = (StepProgressBar)sender;

            var index = progressBar.Items.IndexOf(e.Step);

            progressBar.SelectedIndex = index;
        }
    }
}