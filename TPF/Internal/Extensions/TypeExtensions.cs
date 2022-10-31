using System;
using System.ComponentModel;

namespace TPF.Internal
{
    internal static class TypeExtensions
    {
        internal static TypeConverter GetTypeConverter(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var converter = TypeDescriptor.GetConverter(type);

            return converter;
        }

        internal static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        internal static Type GetNonNullableType(this Type type)
        {
            return type.IsNullableType() ? type.GetGenericArguments()[0] : type;
        }

        internal static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}