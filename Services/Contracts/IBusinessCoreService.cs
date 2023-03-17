using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace GenericWebAPI.Services.Contracts;

public interface IBusinessCoreService<TEntity, TDto>
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    Task<TDto?> Get(Expression<Func<TEntity, bool>> predicate);
    Task<TDto?> GetById(Guid id);
    Task<IEnumerable<TDto>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    Task<IEnumerable<TDto>> GetAllById(IEnumerable<Guid> ids);
    Task<IEnumerable<TDto>> Add(IEnumerable<TDto> entities);
    Task<IEnumerable<TDto>> Update(IEnumerable<TDto> entities);
    Task Delete(Expression<Func<TEntity, bool>> predicate);
    Task DeleteById(IEnumerable<Guid> id);

    Task<IEnumerable<TDto>> GetListWithFilters(Criteria<TEntity> criteria);
    Task<IEnumerable<TDto>> GetPageWithFilters(Criteria<TEntity> criteria, IPagination pagination);
    
    Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsById(Guid id);
}