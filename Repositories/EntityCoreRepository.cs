using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;
using GenericWebAPI.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GenericWebAPI.Repositories;

public abstract class EntityCoreRepository<TEntity> : ICoreRepository<TEntity> where TEntity : EntityCore, new()
{
    protected readonly DbBaseContext _context;

    public EntityCoreRepository(DbBaseContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity?> Get(params Expression<Func<TEntity, bool>>[] predicates)
    {
        if (predicates == null || predicates.Length == 0)
        {
            throw new ArgumentException("At least one predicate is required", nameof(predicates));
        }

        var query = _context.Set<TEntity>().Where(predicates[0]);
        for (int i = 1; i < predicates.Length; i++)
        {
            query = query.Where(predicates[i]);
        }

        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await Get(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        else
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllById(IEnumerable<Guid> ids)
    {
        return await GetAll(e => ids.Contains(e.Id));
    }

    public virtual async Task<IEnumerable<TEntity>> Add(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return entities;
    }
    
    public virtual async Task Delete(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    public virtual async Task Delete(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await _context.Set<TEntity>().Where(predicate).ToListAsync();
        await Delete(entities);
    }

    public virtual async Task DeleteById(IEnumerable<Guid> ids)
    {
        var entity = await GetAll(e => ids.Contains(e.Id));
        await Delete(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> GetListWithFilters(Criteria<TEntity> criteria)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        var predicate = criteria.BuildExpression();
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetPageWithFilters(Criteria<TEntity> criteria, PaginationCriteria pagination)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        var predicate = criteria.BuildExpression();
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        var filteredData = await Paginate(query, pagination).ToListAsync();

        return filteredData;
    }

    public virtual async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _context.Set<TEntity>().CountAsync();
        }
        else
        {
            return await _context.Set<TEntity>().CountAsync(predicate);
        }
    }

    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate);
    }

    public virtual async Task<bool> ExistsById(Guid id)
    {
        return await Exists(e => e.Id == id);
    }

    public virtual async Task<TEntity> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, TEntity entity)
    {
        var existingEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        if (existingEntity != null)
        {
            UpdateEntity(existingEntity, entity);
        }
        else
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }
    
        return entity;
    }
    
    public virtual async Task<IEnumerable<TEntity>> AddOrUpdate(Expression<Func<TEntity, bool>> predicate, IEnumerable<TEntity> entities)
    {
        var results = new List<TEntity>();
        foreach (var entity in entities)
        {
            var existingEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (existingEntity != null)
            {
                UpdateEntity(existingEntity, entity);
                results.Add(existingEntity);
            }
            else
            {
                await _context.Set<TEntity>().AddAsync(entity);
                results.Add(entity);
            }
        }
    
        return results;
    }
    
    public virtual async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
    
    protected IQueryable<TEntity> Paginate(IQueryable<TEntity> queryable, PaginationCriteria paginationSearch)
    {
        return queryable
            .Skip((paginationSearch.Page - 1) * paginationSearch.PageSize)
            .Take(paginationSearch.PageSize)
            .OrderBy($"{paginationSearch.SortName} {paginationSearch.SortDir}");
    }
    
    protected void UpdateEntity(TEntity existingEntity, TEntity newEntity)
    {
        foreach (var property in typeof(TEntity).GetProperties())
        {
            if (property.Name == "Id")
            {
                continue;
            }
            
            var newValue = property.GetValue(newEntity);
            if (newValue != null)
            {
                property.SetValue(existingEntity, newValue);
            }
        }
    }
}