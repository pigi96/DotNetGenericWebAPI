using System.Linq.Expressions;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;

namespace GenericWebAPI.Repositories.Contracts;

/// <summary>
/// Extended edition
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IExtendedRepository<TEntity> : ICoreRepository<TEntity> where TEntity : EntityCore, new()
{
    // Filters, paginations
    Task<IEnumerable<TEntity>> GetListWithFilters(IEnumerable<Filter> filters);
    Task<IEnumerable<TEntity>> GetPageWithFilters(IEnumerable<Filter> filters, PaginationCriteria pagination);
    Task<IEnumerable<TRelatedEntity>> GetRelatedEntitiesById<TRelatedEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property) where TRelatedEntity : EntityCore, new();
    Task<IEnumerable<TRelatedEntity>> GetRelatedEntitiesByPredicate<TRelatedEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property) where TRelatedEntity : EntityCore, new();
}