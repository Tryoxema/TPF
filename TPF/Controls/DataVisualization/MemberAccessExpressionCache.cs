using System;
using System.Collections.Generic;
using TPF.Internal;

namespace TPF.Controls
{
    internal static class MemberAccessExpressionCache
    {
        static MemberAccessExpressionCache()
        {
            _expressionsCache = new Dictionary<TypePathTuple, Func<object, object>>();
        }

        private static readonly Dictionary<TypePathTuple, Func<object, object>> _expressionsCache;

        public static Func<object, object> GetMemberAccessExpression(Type type, string memberPath)
        {
            var tuple = new TypePathTuple(type, memberPath);

            if (!_expressionsCache.TryGetValue(tuple, out var expression))
            {
                expression = MemberAccessExpressionFactory.CreateAccessExpression(type, memberPath);
                _expressionsCache.Add(tuple, expression);
            }

            return expression;
        }

        private struct TypePathTuple
        {
            public TypePathTuple(Type type, string path)
            {
                Type = type;
                Path = path;
            }

            public Type Type { get; private set; }

            public string Path { get; private set; }

            public static bool operator ==(TypePathTuple tuple1, TypePathTuple tuple2)
            {
                return tuple1.Type == tuple2.Type && tuple1.Path == tuple2.Path;
            }

            public static bool operator !=(TypePathTuple tuple1, TypePathTuple tuple2)
            {
                return !(tuple1 == tuple2);
            }

            public override bool Equals(object obj)
            {
                return obj is TypePathTuple tuple && this == tuple;
            }

            public override int GetHashCode()
            {
                var typeHashCode = Type != null ? Type.GetHashCode() : 0;
                var pathHashCode = Path != null ? Path.GetHashCode() : 0;

                return typeHashCode ^ pathHashCode;
            }
        }
    }
}