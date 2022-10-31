using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace TPF.Internal
{
    internal static class TypeDescriptorHelper
    {
        internal static readonly MethodInfo PropertyMethod = typeof(TypeDescriptorHelper).GetMethod("Property", new[] { typeof(object), typeof(string) });

        public static T Property<T>(object component, string propertyName)
        {
            var propertyNamesStack = new Stack<string>(propertyName.Split('.').Reverse());

            var propertyValue = GetPropertyValueRecursive<T>(component, propertyNamesStack);

            return ValueUnboxer<T>.Unbox(propertyValue);
        }

        private static object GetPropertyValueRecursive<T>(object componentInstance, Stack<string> propertyNamesStack)
        {
            if (componentInstance == null)
            {
                return default(T);
            }

            var propertyName = propertyNamesStack.Pop();
            var propertyDescriptor = TypeDescriptor.GetProperties(componentInstance)[propertyName];

            if (propertyDescriptor == null)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "Property with specified name: {0} cannot be found on component: {1}", propertyName, componentInstance);

                throw new ArgumentException(message, nameof(propertyNamesStack));
            }

            componentInstance = propertyDescriptor.GetValue(componentInstance);

            if (propertyNamesStack.Count > 0)
            {
                return GetPropertyValueRecursive<T>(componentInstance, propertyNamesStack);
            }

            return componentInstance;
        }
    }
}