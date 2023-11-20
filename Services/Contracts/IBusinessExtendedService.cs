using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;

namespace GenericWebAPI.Services.Contracts;

public interface IBusinessExtendedService<TEntity, TDto> : IBusinessCoreService<TEntity, TDto>
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    Task<IEnumerable<TDto>> GetListWithFilters(IEnumerable<Filter> filters);
    Task<PagedResult<TDto>> GetPageWithFilters(IEnumerable<Filter> filters, PaginationCriteria pagination);
    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesById<TRelatedDto, TRelatedEntity>(Guid id,
        Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();
    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesByPredicate<TRelatedDto, TRelatedEntity>(
        Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();
}