using System;
using System.Reflection;
using System.Collections.Generic;

namespace TPF.Internal
{
    internal static class PropertyHelper
    {
        internal static object GetPropertyValueFromPath(object value, string path)
        {
            if (value == null || string.IsNullOrWhiteSpace(path)) return null;

            var currentType = value.GetType();

            object result = value;

            var pathParts = path.Split('.');

            for (int i = 0; i < pathParts.Length; i++)
            {
                var propertyName = pathParts[i];

                var brackStart = propertyName.IndexOf("[");
                var brackEnd = propertyName.IndexOf("]");

                var property = currentType.GetProperty(brackStart > 0 ? propertyName.Substring(0, brackStart) : propertyName);
                result = property.GetValue(result, null);

                if (brackStart > 0)
                {
                    string index = propertyName.Substring(brackStart + 1, brackEnd - brackStart - 1);
                    foreach (var type in result.GetType().GetInterfaces())
                    {
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                        {
                            result = typeof(PropertyHelper).GetMethod("GetDictionaryElement").MakeGenericMethod(type.GetGenericArguments()).Invoke(null, new object[] { result, index });
                            break;
                        }
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
                        {
                            result = typeof(PropertyHelper).GetMethod("GetListElement").MakeGenericMethod(type.GetGenericArguments()).Invoke(null, new object[] { result, index });
                            break;
                        }
                    }
                }

                if (result == null) return null;
                currentType = result.GetType();
            }

            return result;
        }

        internal static T GetPropertyValueFromPath<T>(object value, string path)
        {
            var propertyValue = GetPropertyValueFromPath(value, path);

            if (propertyValue == null) return default(T);

            return (T)propertyValue;
        }

        internal static T GetListElement<T>(IList<T> list, object index)
        {
            return list[Convert.ToInt32(index)];
        }

        internal static TValue GetDictionaryElement<TKey, TValue>(IDictionary<TKey, TValue> dictionary, object index)
        {
            var key = (TKey)Convert.ChangeType(index, typeof(TKey), null);
            return dictionary[key];
        }

        internal static void SetPropertyValueFromPath(object item, string path, object value)
        {
            if (item == null || path == null) return;

            var currentType = item.GetType();

            object result = item;

            var pathParts = path.Split('.');

            PropertyInfo property = null;

            for (int i = 0; i < pathParts.Length; i++)
            {
                var propertyName = pathParts[i];

                int brackStart = propertyName.IndexOf("[");
                int brackEnd = propertyName.IndexOf("]");

                property = currentType.GetProperty(brackStart > 0 ? propertyName.Substring(0, brackStart) : propertyName);
                if (i == pathParts.Length - 1) break;
                result = property.GetValue(result, null);
                currentType = result.GetType();
            }

            if (property != null) property.SetValue(item, value);
        }
    }
}