using System;
using System.Windows.Threading;

namespace TPF.Controls
{
    public class DialogHandle
    {
        internal DialogHandle(DialogHost host)
        {
            _owner = host;
        }

        private readonly DialogHost _owner;

        internal object Value { get; set; }

        public bool IsClosed { get; internal set; }

        public object DialogContent
        {
            get { return _owner.DialogContent; }
        }

        public void UpdateContent(object newContent)
        {
            _owner.DialogContent = newContent;
            _owner.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                _owner.FocusDialog();
            }));
        }

        public void Close()
        {
            Close(null);
        }

        public void Close(object result)
        {
            if (IsClosed) throw new InvalidOperationException();

            _owner.CloseDialogInternal(result);
        }
    }
}