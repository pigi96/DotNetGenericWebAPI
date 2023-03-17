using System.Linq.Expressions;
using AutoMapper;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;
using GenericWebAPI.Repositories.Contracts;
using GenericWebAPI.Services.Contracts;
using GenericWebAPI.Utilities;

namespace GenericWebAPI.Services;

public class BusinessExtendedService<TEntity, TDto> : BusinessCoreService<TEntity, TDto>, IBusinessExtendedService<TEntity, TDto>
    where TEntity : EntityBase, new() 
    where TDto : DtoCore, new()
{
    private readonly IExtendedRepository<TEntity> _extendedRepository;

    public BusinessExtendedService(IExtendedRepository<TEntity> extendedRepository, IMapper mapper, IValidator<TDto> validator,
        IBusinessStrategy<TEntity, TDto> businessStrategy) : base(extendedRepository, mapper, validator, businessStrategy)
    {
        _extendedRepository = extendedRepository;
    }


    public virtual async Task<IEnumerable<TEntity>> GetListWithFilters(IEnumerable<Filter> filters)
    {
        var entites = await _extendedRepository.GetListWithFilters(filters);

        return entites
            .Select(entity => _mapper.Map<TEntity>(entity))
            .ToList();
    }

    public virtual async Task<IEnumerable<TEntity>> GetPageWithFilters(IEnumerable<Filter> filters, IPagination pagination)
    {
        var entites = await _extendedRepository.GetPageWithFilters(filters, pagination);

        return entites
            .Select(entity => _mapper.Map<TEntity>(entity))
            .ToList();
    }

    public virtual async Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesById<TRelatedDto, TRelatedEntity>(Guid id,
        Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property) 
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new()
    {
        var entities = await _extendedRepository.GetRelatedEntitiesById(id, property);

        return entities
            .Select(entity => _mapper.Map<TRelatedDto>(entity))
            .ToList();
    }

    public virtual async Task<IEnumerable<TRelatedDto>> GetRelatedEntitiesByPredicate<TRelatedDto, TRelatedEntity>(
        Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> property)
        where TRelatedEntity : EntityCore, new()
        where TRelatedDto : DtoCore, new()
    {
        var entities = await _extendedRepository.GetRelatedEntitiesByPredicate(predicate, property);

        return entities
            .Select(entity => _mapper.Map<TRelatedDto>(entity))
            .ToList();
    }

    private static Expression<Func<TEntity, bool>> ConvertPredicate<TDto, TEntity>(
        Expression<Func<TDto, bool>> predicate)
    {
        var parameterExpr = Expression.Parameter(typeof(TEntity));
        var visitor = new PredicateVisitor<TDto, TEntity>(parameterExpr);
        var bodyExpr = visitor.Visit(predicate.Body);
        return Expression.Lambda<Func<TEntity, bool>>(bodyExpr, parameterExpr);
    }
}