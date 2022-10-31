using System;
using System.Collections;
using System.Globalization;

namespace TPF.Internal
{
    internal static class TypeHelper
    {
        internal static Type GetIEnumerableType(IEnumerable enumerable)
        {
            var enumerableType = enumerable.GetType();

            if (enumerableType.IsGenericType) return enumerableType.GenericTypeArguments[0];
            else
            {
                foreach (var item in enumerable)
                {
                    return item.GetType();
                }
            }

            return null;
        }

        internal static Type GetIListType(IList list)
        {
            var listType = list.GetType();

            if (listType.IsGenericType) return listType.GenericTypeArguments[0];
            else if (list.Count > 0) return list[0].GetType();
            else return null;
        }

        internal static bool TryConvert(object value, Type type, out object returnValue)
        {
            try
            {
                var sourceType = value.GetType();
                var typeConverter = type.GetTypeConverter();

                if (typeConverter != null && typeConverter.CanConvertFrom(sourceType))
                {
                    returnValue = typeConverter.ConvertFrom(value);
                    return true;
                }

                returnValue = Convert.ChangeType(value, type.GetNonNullableType(), CultureInfo.CurrentCulture);
                return true;
            }
            catch (Exception)
            {
                returnValue = null;
                return false;
            }
        }
    }
}