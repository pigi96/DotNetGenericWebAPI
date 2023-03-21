using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace GenericWebAPI.Services.Contracts;

public interface IBusinessExtendedService<TEntity, TDto> : IBusinessCoreService<TEntity, TDto>
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    Task<IEnumerable<TEntity>> GetListWithFilters(IEnumerable<Filter> filters);
    Task<IEnumerable<TEntity>> GetPageWithFilters(IEnumerable<Filter> filters, IPagination pagination);
    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesById<TRelatedDto, TRelatedEntity>(Guid id,
        Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();
    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesByPredicate<TRelatedDto, TRelatedEntity>(
        Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();
}