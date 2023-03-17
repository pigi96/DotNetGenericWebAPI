using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;
using GenericWebAPI.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GenericWebAPI.Repositories;

public abstract class EntityExtendedRepository<TEntity> : EntityCoreRepository<TEntity>, IExtendedRepository<TEntity> where TEntity : EntityCore, new()
{
    protected readonly DbBaseContext _context;

    public EntityExtendedRepository(DbBaseContext context) : base(context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<TEntity>> GetListWithFilters(IEnumerable<Filter> filters)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        foreach (var filter in filters)
        {
            queryable = Convert(queryable, filter);
        }

        var filteredData = await queryable.ToListAsync();

        return filteredData;
    }
    
    public virtual async Task<IEnumerable<TEntity>> GetPageWithFilters(IEnumerable<Filter> filters, IPagination pagination)
    {
        var queryable = _context.Set<TEntity>().AsQueryable();

        foreach (var filter in filters)
        {
            queryable = Convert(queryable, filter);
        }

        var filteredData = await Paginate(queryable, pagination).ToListAsync();

        return filteredData;
    }

    public virtual async Task<IEnumerable<TRelatedEntity>> GetRelatedEntitiesById<TRelatedEntity>(Guid id, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property) where TRelatedEntity : EntityCore, new()
    {
        return await _context.Set<TEntity>()
            .Where(e => e.Id == id)
            .SelectMany(property)
            .ToListAsync();
    }
    
    public virtual async Task<IEnumerable<TRelatedEntity>> GetRelatedEntitiesByPredicate<TRelatedEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property) where TRelatedEntity : EntityCore, new()
    {
        return await _context.Set<TEntity>()
            .Where(predicate)
            .SelectMany(property)
            .ToListAsync();
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

}