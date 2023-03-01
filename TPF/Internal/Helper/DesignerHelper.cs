using System.Windows;
using System.ComponentModel;

namespace TPF.Internal
{
    internal static class DesignerHelper
    {
        static bool? _isIndesignMode;
        internal static bool IsInDesignMode
        {
            get
            {
                if (_isIndesignMode == null) _isIndesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

                return _isIndesignMode.Value;
            }
        }
    }
}