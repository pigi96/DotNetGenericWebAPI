using System.Linq.Expressions;

namespace GenericWebAPI.Utilities;

public class PredicateVisitor<TDto, TEntity> : ExpressionVisitor
{
    private readonly ParameterExpression _parameterExpr;

    public PredicateVisitor(ParameterExpression parameterExpr)
    {
        _parameterExpr = parameterExpr;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node.Type == typeof(TDto))
        {
            return _parameterExpr;
        }
        return base.VisitParameter(node);
    }
}