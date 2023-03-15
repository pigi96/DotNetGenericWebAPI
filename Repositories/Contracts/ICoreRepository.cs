using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace Fakeglass.Repositories.Extensions;

public interface ICoreRepository<TEntity> where TEntity : EntityCore, new()
{
    Task<TEntity?> GetById(Guid id);
    Task<TEntity?> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAll();
    Task<List<TEntity>> GetAllByIds(List<Guid> ids);
    Task<List<TEntity>> GetListWithFilters(List<Filter> filters);
    Task<List<TEntity>> GetPageWithFilters(List<Filter> filters, IPagination pagination);
    Task<int> Count();
    Task<TEntity> Add(TEntity entity);
    Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);
    Task<TEntity> Update(TEntity entity);
    Task<IEnumerable<TEntity>> UpdateRange(IEnumerable<TEntity> entities);
    Task<TEntity> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity);
    Task<IEnumerable<TEntity>> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, IEnumerable<TEntity> entity);
    Task<bool> ExistsById(Guid id);
    Task<bool> ExistsByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task DeleteById(Guid id);
    Task Delete(TEntity entity);
    Task<bool> DeleteByPredicate(Expression<Func<TEntity, bool>> predicate);
    Task SaveChanges();
}