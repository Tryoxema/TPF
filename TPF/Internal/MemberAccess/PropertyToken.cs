using System.ComponentModel;
using System.Linq.Expressions;

namespace TPF.Internal
{
    internal class PropertyToken : IMemberPathToken
    {
        public PropertyToken(string propertyName)
        {
            _propertyName = propertyName;
        }

        private readonly string _propertyName;

        public Expression CreateMemberAccessExpression(Expression baseExpression)
        {
            var type = baseExpression.Type;

            var memberInfo = MemberInfoHelper.GetMemberInfo(type, _propertyName);

            if (memberInfo == null)
            {
                var propertyDescriptor = TypeDescriptor.GetProperties(type)[_propertyName];

                if (propertyDescriptor == null)
                {
                    return null;
                }

                var propertyExpression = Expression.Call(TypeDescriptorHelper.PropertyMethod.MakeGenericMethod(propertyDescriptor.PropertyType), baseExpression, Expression.Constant(_propertyName));

                return propertyExpression;
            }

            return Expression.MakeMemberAccess(baseExpression, memberInfo);
        }
    }
}