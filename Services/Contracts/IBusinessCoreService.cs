using System.Linq.Expressions;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;
using GenericWebAPI.Utilities;

namespace GenericWebAPI.Services.Contracts;

public interface IBusinessCoreService<TEntity, TDto>
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    Task<TDto?> Get(Expression<Func<TEntity, bool>> predicate);
    Task<TDto?> GetById(Guid id);
    Task<IEnumerable<TDto>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    Task<IEnumerable<TDto>> GetAllById(IEnumerable<Guid> ids);
    Task<IEnumerable<TDto>> Add(ICollection<TDto>? entities = null, IValidator<TDto>? validator = null, IBusinessStrategy<TEntity, TDto>? businessStrategy = null);
    Task<IEnumerable<TDto>> Update(ICollection<TDto>? entities = null, IValidator<TDto>? validator = null, IBusinessStrategy<TEntity, TDto>? businessStrategy = null);
    Task Delete(Expression<Func<TEntity, bool>> predicate, IBusinessStrategy<TEntity, TDto>? businessStrategy = null);
    Task DeleteById(IEnumerable<Guid> id, IBusinessStrategy<TEntity, TDto>? businessStrategy = null);

    Task<IEnumerable<TDto>> GetListWithFilters(Criteria<TEntity> criteria);
    Task<PagedResult<TDto>> GetPageWithFilters(Criteria<TEntity> criteria, PaginationCriteria pagination);
    
    Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsById(Guid id);
}