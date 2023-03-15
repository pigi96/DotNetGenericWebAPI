using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Fakeglass.Repositories.Extensions;

public abstract class EntityCoreRepository<TEntity> : ICoreRepository<TEntity> where TEntity : EntityCore, new()
{
    protected readonly DbBaseContext _context;

    protected EntityCoreRepository(DbBaseContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<TEntity?> GetByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllByIds(List<Guid> ids)
    {
        return await _context.Set<TEntity>()
            .Where(entity => ids.Contains(entity.Id))
            .ToListAsync();
    }
    
    public virtual async Task<List<TEntity>> GetListWithFilters(List<Filter> filters)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        foreach (var filter in filters)
        {
            queryable = Convert(queryable, filter);
        }

        var filteredData = await queryable.ToListAsync();

        return filteredData;
    }
    
    public virtual async Task<List<TEntity>> GetPageWithFilters(List<Filter> filters, IPagination pagination)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        foreach (var filter in filters)
        {
            queryable = Convert(queryable, filter);
        }

        var filteredData = await Paginate(queryable, pagination).ToListAsync();

        return filteredData;
    }

    public virtual async Task<int> Count()
    {
        return await _context.Set<TEntity>().CountAsync();
    }

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        var entityAdd = await _context.Set<TEntity>().AddAsync(entity);
        return entityAdd.Entity;
    }
    
    public virtual async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        return entities;
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        var entityUpdate = _context.Set<TEntity>().Update(entity);
        return entityUpdate.Entity;
    }
    
    public virtual async Task<IEnumerable<TEntity>> UpdateRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
        return entities;
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

    public virtual async Task<bool> ExistsById(Guid id)
    {
        return await GetById(id) != null;
    }
    
    public virtual async Task<bool> ExistsByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return await GetByPredicate(predicate) != null;
    }

    public virtual async Task DeleteById(Guid id)
    {
        _context.Set<TEntity>().Remove(new TEntity { Id = id });
    }

    public virtual async Task Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }
    
    public virtual async Task<bool> DeleteByPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        var entityToDelete = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        if (entityToDelete == null) return false;

        _context.Set<TEntity>().Remove(entityToDelete);
    
        return true;
    }


    public virtual async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    private IQueryable<TF> Convert<TF>(IQueryable<TF> queryable, Filter filter)
    {
        var param = Expression.Parameter(typeof(TF), "x");
        Expression expression = Expression.Constant(true);

        var property = Expression.Property(param, filter.Property);

        var value1 = ConvertPropertyType(filter.Value1, property.Type);
        var expression1 = ConvertToNullable(Expression.Constant(value1), value1.GetType());

        var value2 = filter.Value2 != null ? ConvertPropertyType(filter.Value2, property.Type) : null;
        var expression2 = filter.Value2 != null ? ConvertToNullable(Expression.Constant(value2), value2.GetType()) : null;

        switch (filter.FilterType)
        {
            case FilterType.EXACT:
                expression = Expression.AndAlso(expression, Expression.Equal(property, expression1));
                break;
            case FilterType.LIKE:
                expression = Expression.AndAlso(expression, Expression.Call(property, "Contains", Type.EmptyTypes, expression1));
                break;
            case FilterType.BETWEEN:
                expression = Expression.AndAlso(expression, Expression.AndAlso(Expression.GreaterThanOrEqual(property, expression1), Expression.LessThanOrEqual(property, expression2)));
                break;
            case FilterType.LESS_THAN:
                expression = Expression.AndAlso(expression, Expression.LessThanOrEqual(property, expression1));
                break;
            case FilterType.MORE_THAN:
                expression = Expression.AndAlso(expression, Expression.GreaterThanOrEqual(property, expression1));
                break;
            default:
                throw new ArgumentException("Invalid filter type.");
        }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(expression, param);

            return queryable.Where(lambda);
    }

    private IQueryable<TEntity> Paginate(IQueryable<TEntity> queryable, IPagination paginationSearch)
    {
        return queryable
            .Skip((paginationSearch.GetPageNumber() - 1) * paginationSearch.GetPageSize())
            .Take(paginationSearch.GetPageSize())
            .OrderBy($"{paginationSearch.GetSortName()} {paginationSearch.GetSortDir()}");
    }

    private object ConvertPropertyType(string value, Type propertyType)
    {
        if (propertyType == typeof(int) || propertyType == typeof(int?))
        {
            return (int?)(int.Parse(value));
        }
        if (propertyType == typeof(long) || propertyType == typeof(long?))
        {
            return long.Parse(value);
        }
        if (propertyType == typeof(float) || propertyType == typeof(float?))
        {
            return float.Parse(value);
        }
        if (propertyType == typeof(double) || propertyType == typeof(double?))
        {
            return double.Parse(value);
        }
        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            return DateTime.Parse(value);
        }
        return value;
    }
    
    private Expression ConvertToNullable(Expression e1, Type type)
    {
        if (type == typeof(string))
        {
            return e1;
        }
        if (!IsNullableType(e1.Type))
        {
            e1 = Expression.Convert(e1, typeof(Nullable<>).MakeGenericType(type));
        }
        return e1;
    }
    
    private bool IsNullableType(Type t)
    {
        return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
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