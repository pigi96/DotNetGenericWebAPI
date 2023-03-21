using System.Linq.Expressions;
using LinqKit;

namespace GenericWebAPI.Filters.Filtering;

public class Criteria<TEntity> where TEntity : class
{
    public Criteria()
    {
        Predicates = new List<Expression<Func<TEntity, bool>>>();
    }

    public List<Expression<Func<TEntity, bool>>> Predicates { get; set; }

    public Expression<Func<TEntity, bool>> BuildExpression()
    {
        if (!Predicates.Any())
        {
            return null;
        }

        var predicate = PredicateBuilder.New<TEntity>();
        foreach (var expression in Predicates)
        {
            predicate = predicate.And(expression);
        }

        return predicate;
    }
}