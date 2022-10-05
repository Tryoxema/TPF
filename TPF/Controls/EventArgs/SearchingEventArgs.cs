using System;
using System.Collections;

namespace TPF.Controls
{
    public class SearchingEventArgs : EventArgs
    {
        public string Value { get; }

        public IEnumerable Results { get; set; }

        public SearchingEventArgs() { }

        public SearchingEventArgs(string value)
        {
            Value = value;
        }
    }
}