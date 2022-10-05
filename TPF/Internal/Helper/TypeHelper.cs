using System;
using System.Collections;

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
    }
}