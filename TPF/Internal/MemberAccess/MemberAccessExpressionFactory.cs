using System;
using System.Linq;
using System.Linq.Expressions;

namespace TPF.Internal
{
    internal static class MemberAccessExpressionFactory
    {
        internal static Func<object, object> CreateAccessExpression(Type type, string memberPath)
        {
            // Eine ParameterExpression die unser item darstellt, aus dem der Wert entnommen werden soll
            var parameterExpression = Expression.Parameter(type, "item");

            // Eine Expression generieren mit der wir auf den gewünschten Member zugreifen können
            var expression = CreateMemberAccessExpression(parameterExpression, memberPath);

            // Aus der Expression eine LambdaExpression machen
            var lambda = Expression.Lambda(expression, parameterExpression);

            // In eine Func umwandeln
            var func = lambda.Compile();

            // Eine Casting-Methode machen, damit wir unsere Typed-Function wie eine <object in, object out> Func benutzen können
            var castingMethod = typeof(MemberAccessExpressionFactory).GetMethod("ToUntypedFunc").MakeGenericMethod(new[] { type, lambda.Body.Type });

            // Unsere <object,object>-Func machen
            var result = (Func<object, object>)castingMethod.Invoke(null, new object[] { func });

            return result;
        }

        private static Expression CreateMemberAccessExpression(Expression expression, string memberPath)
        {
            foreach (var token in MemberPathTokenizer.GetTokens(memberPath))
            {
                expression = token.CreateMemberAccessExpression(expression);

                if (expression == null) return Expression.Constant(null, typeof(object));
            }

            return expression;
        }

        public static Func<object, object> ToUntypedFunc<T, TResult>(Func<T, TResult> func)
        {
            return item => func.Invoke((T)item);
        }
    }
}