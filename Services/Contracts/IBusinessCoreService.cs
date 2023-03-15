using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace GenericWebAPI.Services.Extensions;

public interface IBusinessCoreService<TDto, TEntity>
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    Task<TDto?> GetById(Guid id);
    Task<TDto?> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task<List<TDto>> GetAll();
    Task<List<TDto>> GetListWithFilters(List<Filter> filters);
    Task<List<TDto>> GetPageWithFilters(List<Filter> filters, IPagination pagination);
    Task<int> Count();
    Task<TDto> Add(TDto entity);
    Task<TDto> Update(TDto entity, Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsById(Guid id);
    Task<bool> ExistsByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task DeleteById(Guid id);
    Task Delete(TDto entity);
    Task DeleteByPredicate(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesById<TRelatedDto, TRelatedEntity>(Guid id,
        Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();

    Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesByPredicate<TRelatedDto, TRelatedEntity>(
        Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new();
}