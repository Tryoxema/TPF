using System;
using System.Globalization;
using System.Reflection;

namespace TPF.Internal
{
    internal static class ValueUnboxer<T>
    {
        static ValueUnboxer()
        {
            Unbox = CreateConverter(typeof(T));
        }

        internal static readonly Converter<object, T> Unbox;

        private static Converter<object, T> CreateConverter(Type type)
        {
            if (!type.IsValueType)
            {
                return UnboxReferenceField;
            }
            if (type.IsGenericType && !type.IsGenericTypeDefinition && (typeof(Nullable<>) == type.GetGenericTypeDefinition()))
            {
                var nullableFieldMethod = typeof(ValueUnboxer<T>).GetMethod("UnboxNullableField", BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = nullableFieldMethod.MakeGenericMethod(new[] { type.GetGenericArguments()[0] });

                return (Converter<object, T>)Delegate.CreateDelegate(typeof(Converter<object, T>), genericMethod);
            }
            return UnboxValueField;
        }

#pragma warning disable IDE0051
        private static TStruct? UnboxNullableField<TStruct>(object value) where TStruct : struct
        {
            if (value == DBNull.Value || value == null) return null;

            var convertedValue = (TStruct)value;

            return (TStruct?)convertedValue;
        }
#pragma warning restore IDE0051

        private static T UnboxReferenceField(object value)
        {
            if (value != DBNull.Value) return (T)value;

            return default;
        }

        private static T UnboxValueField(object value)
        {
            if (value == DBNull.Value)
            {
                throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, "Type: {0} cannot be casted to Nullable type", typeof(T)));
            }

            return (T)value;
        }
    }
}