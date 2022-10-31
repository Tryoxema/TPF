using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TPF.Internal
{
    // Eine Hilfsklasse um an MemberInfo-Objekte zu kommen
    internal static class MemberInfoHelper
    {
        internal static MemberInfo GetMemberInfo(Type type, string memberName)
        {
            var memberInfo = GetMemberInfo(type, memberName, false);

            if (memberInfo == null) GetMemberInfo(type, memberName, true);

            return memberInfo;
        }

        internal static MemberInfo GetMemberInfo(Type type, string memberName, bool staticAccess)
        {
            var flags = BindingFlags.Public | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);

            var members = type.FindMembers(MemberTypes.Property | MemberTypes.Field, flags, Type.FilterName, memberName);

            if (members.Length != 0) return members[0];

            return null;
        }

        internal static PropertyInfo GetIndexerPropertyInfo(Type type, IEnumerable<object> indexerArguments)
        {
            // Alle implementen Interfaces mit dazu nehmen
            var interfacesProperties = type.GetInterfaces().OrderBy(i => !i.IsGenericType).SelectMany(i => i.GetProperties());

            foreach (var property in type.GetProperties().Concat(interfacesProperties))
            {
                if (AreArgumentsApplicable(indexerArguments, property.GetIndexParameters()))
                {
                    return property;
                }
            }

            return null;
        }

        // Prüft ob eine Liste an objekten mit den gewünschten Parametern kompatibel ist
        private static bool AreArgumentsApplicable(IEnumerable<object> arguments, IEnumerable<ParameterInfo> parameters)
        {
            var argumentList = arguments.ToList();
            var parameterList = parameters.ToList();

            if (argumentList.Count != parameterList.Count)
            {
                return false;
            }

            for (int i = 0; i < argumentList.Count; i++)
            {
                var argument = argumentList[i];

                if (argument is System.Collections.IEnumerable enumerable && !(enumerable is string))
                {
                    foreach (var item in enumerable)
                    {
                        argument = item;
                        break;
                    }
                }

                if (!TypeHelper.TryConvert(argument, parameterList[i].ParameterType, out _))
                {
                    return false;
                }
            }

            return true;
        }
    }
}