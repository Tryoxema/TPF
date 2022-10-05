using System;
using TPF.Skins;

namespace TPF.Controls
{
    public class SkinChangingEventArgs : EventArgs
    {
        public ISkin Skin { get; }

        public bool Cancel { get; set; }

        public SkinChangingEventArgs(ISkin skin)
        {
            Skin = skin;
        }
    }
}