using System.Linq.Expressions;

namespace GenericWebAPI.Utilities;

public class PredicateVisitor<TDto, TEntity> : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    public PredicateVisitor(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member.DeclaringType == typeof(TDto))
        {
            var memberName = node.Member.Name;
            var entityMember = typeof(TEntity).GetProperty(memberName);
            if (entityMember == null)
            {
                throw new InvalidOperationException($"The property '{memberName}' is not defined for type '{typeof(TEntity)}'");
            }
            return Expression.MakeMemberAccess(_parameter, entityMember);
        }
        return base.VisitMember(node);
    }
}
