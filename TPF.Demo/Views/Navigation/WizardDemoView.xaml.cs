using System;
using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class WizardDemoView : ViewBase
    {
        public WizardDemoView()
        {
            InitializeComponent();

            Manager = new NotificationManager();
            Notifications.Manager = Manager;
            Initialize();
        }

        public readonly NotificationManager Manager;

        bool _canContinue;
        public bool CanContinue
        {
            get { return _canContinue; }
            set { SetProperty(ref _canContinue, value); }
        }

        bool _canFinish;
        public bool CanFinish
        {
            get { return _canFinish; }
            set { SetProperty(ref _canFinish, value); }
        }

        private void Initialize()
        {
            StartPage.Header = new HeaderModel()
            {
                Title = "Start",
                Description = "Die erste Seite"
            };

            MiddlePage.Header = new HeaderModel()
            {
                Title = "Dateneingabe",
                Description = "Hier kannst du deine Seele verkaufen"
            };

            LastPage.Header = new HeaderModel()
            {
                Title = "Abschluss",
                Description = "Die letzte Seite zum Abschließen des Vorgangs"
            };
        }

        private void Wizard_Finish(object sender, RoutedEventArgs e)
        {
            Manager.CreateNotification()
                .Header("Fertig")
                .Message("Der Vorgang wurde abgeschlossen")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                .Dismiss().WithButton("OK")
                .Queue();
        }

        private void Wizard_Cancel(object sender, RoutedEventArgs e)
        {
            Manager.CreateNotification()
                .Header("Abbruch")
                .Message("Der Vorgang wurde abgebrochen")
                .Dismiss().WithDelay(TimeSpan.FromSeconds(3))
                .Dismiss().WithButton("OK")
                .Queue();
        }
    }

    class HeaderModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}