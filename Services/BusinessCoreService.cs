using System.Linq.Expressions;
using AutoMapper;
using Fakeglass.Repositories.Extensions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;
using GenericWebAPI.Services.Extensions;
using GenericWebAPI.Utilities;

namespace Fakeglass.Services.Extensions;

public abstract class BusinessCoreService<TEntity, TDto> : IBusinessCoreService<TDto> where TEntity : EntityBase, new() where TDto : DtoCore, new()
{
    protected readonly ICoreRepository<TEntity> _coreRepository;
    protected readonly IMapper _mapper;
    protected readonly IValidator<TDto> _validator;
    protected readonly IBusinessStrategy<TEntity, TDto> _businessStrategy;

    protected BusinessCoreService(ICoreRepository<TEntity> coreRepository, IMapper mapper, IValidator<TDto> validator, IBusinessStrategy<TEntity, TDto> businessStrategy)
    {
        _coreRepository = coreRepository;
        _mapper = mapper;
        _validator = validator;
        _businessStrategy = businessStrategy;
    }

    public async Task<TDto?> GetById(Guid id)
    {
        return _mapper.Map<TDto>(await _coreRepository.GetById(id));
    }

    public async Task<TDto?> GetByPredicate(Expression<Func<TDto, bool>> predicate)
    {
        var entityPredicate = ConvertPredicate<TDto, TEntity>(predicate);
        return _mapper.Map<TDto>(await _coreRepository.GetByPredicate(entityPredicate));
    }
    
    public async Task<List<TDto>> GetAll()
    {
        var entities = await _coreRepository.GetAll();

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }

    public async Task<List<TDto>> GetListWithFilters(List<Filter> filters)
    {
        var entities = await _coreRepository.GetListWithFilters(filters);

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }

    public async Task<List<TDto>> GetPageWithFilters(List<Filter> filters, IPagination pagination)
    {
        var entities = await _coreRepository.GetPageWithFilters(filters, pagination);
        
        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }

    public async Task<int> Count()
    {
        return await _coreRepository.Count();
    }

    public async Task<TDto> Add(TDto dto)
    {
        _validator.ValidateAdd(dto);
        await _businessStrategy.ApplyAdd(dto);
        
        var model = await _coreRepository.Add(_mapper.Map<TEntity>(dto));
        
        await _coreRepository.SaveChanges();
        return _mapper.Map<TDto>(model);
    }

    public async Task<TDto> Update(TDto dto, Expression<Func<TDto, bool>> predicate = null)
    {
        _validator.ValidateUpdate(dto);

        var entity = predicate != null
            ? await _coreRepository.GetByPredicate(ConvertPredicate<TDto, TEntity>(predicate))
            : await _coreRepository.GetById(dto.Id);
        
        _businessStrategy.ApplyUpdate(entity, dto);
        
        var modelUpdated = await _coreRepository.Update(_mapper.Map<TEntity>(dto));
        
        await _coreRepository.SaveChanges();
        return _mapper.Map<TDto>(modelUpdated);
    }

    public async Task<bool> ExistsById(Guid id)
    {
        return await _coreRepository.ExistsById(id);
    }

    public Task<bool> ExistsByPredicate(Expression<Func<TDto, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteById(Guid id)
    {
        var entity = await _coreRepository.GetById(id);

        await _businessStrategy.ApplyDelete(entity);
        
        await _coreRepository.DeleteById(id);
        await _coreRepository.SaveChanges();
    }

    public async Task Delete(TDto dto)
    {
        var entity = await _coreRepository.GetById(dto.Id);
        
        await _businessStrategy.ApplyDelete(entity);
        
        await _coreRepository.Delete(_mapper.Map<TEntity>(dto));
        await _coreRepository.SaveChanges();
    }

    public async Task DeleteByPredicate(Expression<Func<TDto, bool>> predicate)
    {
        var entity = await _coreRepository.GetByPredicate(ConvertPredicate<TDto, TEntity>(predicate));

        await _businessStrategy.ApplyDelete(entity);

        await _coreRepository.Delete(entity);
        await _coreRepository.SaveChanges();
    }
    
    private static Expression<Func<TEntity, bool>> ConvertPredicate<TDto, TEntity>(Expression<Func<TDto, bool>> predicate)
    {
        var parameterExpr = Expression.Parameter(typeof(TEntity));
        var visitor = new PredicateVisitor<TDto, TEntity>(parameterExpr);
        var bodyExpr = visitor.Visit(predicate.Body);
        return Expression.Lambda<Func<TEntity, bool>>(bodyExpr, parameterExpr);
    }
}