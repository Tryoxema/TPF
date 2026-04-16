using System;

namespace TPF.Controls
{
    public class SnackbarMessage
    {
        public SnackbarMessage(string text, SnackbarSeverity severity = SnackbarSeverity.None, TimeSpan? duration = null, string actionText = null, Action actionCallback = null)
        {
            Text = text ?? string.Empty;
            Severity = severity;
            Duration = duration ?? TimeSpan.FromSeconds(3);
            ActionText = actionText;
            ActionCallback = actionCallback;
        }

        public string Text { get; }

        public SnackbarSeverity Severity { get; }

        public TimeSpan Duration { get; }

        public string ActionText { get; }

        public Action ActionCallback { get; }
    }
}