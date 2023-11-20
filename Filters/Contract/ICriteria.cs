using System.Linq.Expressions;
using GenericWebAPI.Models;

namespace GenericWebAPI.Filters.Contract;

public interface ICriteria<TEntity> where TEntity : EntityCore
{
    Expression<Func<TEntity, bool>> BuildExpression();
    void BuildFilterExpression();
}