using System.Windows;

namespace TPF.Controls
{
    public class ParsingEventArgs<T> : RoutedEventArgs
    {
        public ParsingEventArgs(RoutedEvent routedEvent, string text, T result, bool isSuccessful) : base(routedEvent)
        {
            TextToParse = text;
            Result = result;
            IsSuccessful = isSuccessful;
        }

        public string TextToParse { get; private set; }

        public T Result { get; set; }

        public bool IsSuccessful { get; set; }
    }

    public delegate void ParsingEventHandler<T>(object sender, ParsingEventArgs<T> e);
}