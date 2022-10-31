using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace TPF.Internal
{
    internal class IndexerToken : IMemberPathToken
    {
        public IndexerToken(List<object> arguments)
        {
            _arguments = new ReadOnlyCollection<object>(arguments);
        }

        private readonly ReadOnlyCollection<object> _arguments;

        public Expression CreateMemberAccessExpression(Expression baseExpression)
        {
            var type = baseExpression.Type;

            var propertyInfo = MemberInfoHelper.GetIndexerPropertyInfo(type, _arguments);

            var methodInfo = propertyInfo?.GetGetMethod();

            if (methodInfo == null) return null;

            var parameters = methodInfo.GetParameters();

            var arguments = GetIndexerArguments(parameters);

            return Expression.Call(baseExpression, methodInfo, arguments);
        }

        private IEnumerable<Expression> GetIndexerArguments(ParameterInfo[] parameters)
        {
            var changeType = typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(TypeCode), typeof(IFormatProvider) });

            var parameterType = parameters[0].ParameterType;

            return _arguments.Select(x => Expression.Convert(Expression.Call(changeType, Expression.Constant(x), Expression.Constant(Type.GetTypeCode(parameterType)), Expression.Constant(CultureInfo.CurrentCulture)), parameterType));
        }
    }
}