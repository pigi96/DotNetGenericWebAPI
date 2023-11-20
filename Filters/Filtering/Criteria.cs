using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Models;
using LinqKit;

namespace GenericWebAPI.Filters.Filtering;

public class Criteria<TEntity> : ICriteria<TEntity> where TEntity : EntityCore
{
    protected List<Expression<Func<TEntity, bool>>> Predicates { get; set; }

    public Criteria()
    {
        Predicates = new List<Expression<Func<TEntity, bool>>>();
    }
    
    public void AddPredicate(object value, Expression<Func<TEntity, bool>> predicate)
    {
        if (value != null)
        {
            Predicates.Add(predicate);
        }
    }

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

    public virtual void BuildFilterExpression()
    {
        throw new NotImplementedException("Derived classes must override the BuildFilterExpression() method");
    }
}