using System;
using TPF.Skins;

namespace TPF.Controls
{
    public class SkinChangedEventArgs : EventArgs
    {
        public ISkin Skin { get; }

        public SkinChangedEventArgs(ISkin skin)
        {
            Skin = skin;
        }
    }
}