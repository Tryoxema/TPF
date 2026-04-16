using System;
using System.Collections.Generic;

namespace TPF.Controls
{
    public class SnackbarMessageQueue
    {
        public event Action<SnackbarMessage> MessageEnqueued;

        public bool IsEmpty
        {
            get { lock (_lock) { return _list.Count == 0; } }
        }

        public int Count
        {
            get { lock (_lock) { return _list.Count; } }
        }

        private readonly LinkedList<SnackbarMessage> _list = new LinkedList<SnackbarMessage>();
        private readonly object _lock = new object();

        // Zeigt auf den ersten normalen Knoten (oder null, wenn keine normalen vorhanden)
        private LinkedListNode<SnackbarMessage> _normalBoundary;

        public void Enqueue(string text, SnackbarSeverity severity = SnackbarSeverity.None, TimeSpan? duration = null, string actionText = null, Action actionCallback = null)
        {
            var message = new SnackbarMessage(text, severity, duration, actionText, actionCallback);

            lock (_lock)
            {
                var node = _list.AddLast(message);

                // Wenn es noch keine normalen Nachrichten gab, ist dieser Knoten jetzt die Grenze
                if (_normalBoundary == null) _normalBoundary = node;
            }

            MessageEnqueued?.Invoke(message);
        }

        public void EnqueueWithPriority(string text, SnackbarSeverity severity = SnackbarSeverity.None, TimeSpan? duration = null, string actionText = null, Action actionCallback = null)
        {
            var message = new SnackbarMessage(text, severity, duration, actionText, actionCallback);

            lock (_lock)
            {
                if (_normalBoundary != null)
                {
                    // Direkt vor dem ersten normalen Eintrag einfügen
                    _list.AddBefore(_normalBoundary, message);
                }
                else
                {
                    // Keine normalen Nachrichten → einfach ans Ende
                    _list.AddLast(message);
                }
            }

            MessageEnqueued?.Invoke(message);
        }

        public bool TryDequeue(out SnackbarMessage message)
        {
            lock (_lock)
            {
                if (_list.First == null)
                {
                    message = null;
                    return false;
                }

                var node = _list.First;
                message = node.Value;

                // Wenn der entfernte Knoten die Grenze war, rückt die Grenze auf den nächsten Knoten
                if (node == _normalBoundary) _normalBoundary = node.Next;

                _list.RemoveFirst();
                return true;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
                _normalBoundary = null;
            }
        }
    }
}