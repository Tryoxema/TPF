using System;

namespace TPF.Demo.Net461.Controls
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WindowMetadataAttribute : Attribute
    {
        public string Name { get; }

        public WindowType WindowType { get; }

        public WindowMetadataAttribute(string name) : this(name, WindowType.Normal)
        {
            
        }

        public WindowMetadataAttribute(string name, WindowType windowType)
        {
            Name = name;
            WindowType = windowType;
        }
    }

    public enum WindowType
    {
        Normal = 0,
        Modal = 1,
        Hidden = 2
    }
}