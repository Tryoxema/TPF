using TPF.Skins;

namespace TPF.Controls
{
    public class SkinChanger
    {
        private ISkin _skin;
        public ISkin Skin
        {
            get { return _skin; }
            set
            {
                if (_skin != value) ResourceManager.ChangeSkin(value);

                _skin = value;
            }
        }
    }
}