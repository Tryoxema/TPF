using System.Linq.Expressions;

namespace TPF.Internal
{
    internal interface IMemberPathToken
    {
        Expression CreateMemberAccessExpression(Expression baseExpression);
    }
}