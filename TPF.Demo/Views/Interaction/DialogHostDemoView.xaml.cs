namespace TPF.Demo.Views
{
    public partial class DialogHostDemoView : ViewBase
    {
        public DialogHostDemoView()
        {
            InitializeComponent();
        }

        string _dialogMessage;
        public string DialogMessage
        {
            get { return _dialogMessage; }
            set { SetProperty(ref _dialogMessage, value); }
        }
    }
}