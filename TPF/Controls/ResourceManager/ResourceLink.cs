using System;
using System.Windows;
using System.Windows.Data;
using TPF.Skins;

namespace TPF.Controls
{
    public class ResourceLink : Binding
    {
        public ResourceLink()
        {
            Source = ResourceManager.Resources;
        }

        public ResourceLink(string resourceKey)
        {
            Source = ResourceManager.Resources;
            Path = new PropertyPath(resourceKey);
        }

        private ResourceKeys _key;
        public ResourceKeys Key
        {
            get { return _key; }
            set
            {
                _key = value;
                // Den Text aus dem Enum benutzen um die Ressource zu finden
                Path = new PropertyPath(_key.ToString());
            }
        }
    }
}