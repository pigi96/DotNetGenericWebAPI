using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace GenericWebAPI.Repositories.Contracts;

/// <summary>
/// Core functions for a usable repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ICoreRepository<TEntity> where TEntity : EntityCore, new()
{
    // Basic CRUD operations
    Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    Task<IEnumerable<TEntity>> GetAllById(IEnumerable<Guid> ids);
    Task<IEnumerable<TEntity>> Add(IEnumerable<TEntity> entities);
    Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entities);
    Task Delete(Expression<Func<TEntity, bool>> predicate);
    Task Delete(IEnumerable<TEntity> entity);
    Task DeleteById(IEnumerable<Guid> id);

    // Filters, paginations
    Task<IEnumerable<TEntity>> GetListWithFilters(Criteria<TEntity> criteria);
    Task<IEnumerable<TEntity>> GetPageWithFilters(Criteria<TEntity> criteria, IPagination pagination);
    
    // Miscellaneous
    Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null);
    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsById(Guid id);
    
    Task<TEntity> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity);
    Task<IEnumerable<TEntity>> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, IEnumerable<TEntity> entity);
    Task SaveChanges();
}