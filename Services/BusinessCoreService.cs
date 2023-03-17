using System.Linq.Expressions;
using AutoMapper;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;
using GenericWebAPI.Repositories.Contracts;
using GenericWebAPI.Services.Contracts;
using GenericWebAPI.Utilities;

namespace GenericWebAPI.Services;

public class BusinessCoreService<TEntity, TDto> : IBusinessCoreService<TEntity, TDto>
    where TEntity : EntityBase, new() 
    where TDto : DtoCore, new()
{
    protected readonly ICoreRepository<TEntity> _coreRepository;
    protected readonly IMapper _mapper;
    protected readonly IValidator<TDto> _validator;
    protected readonly IBusinessStrategy<TEntity, TDto> _businessStrategy;

    protected BusinessCoreService(ICoreRepository<TEntity> coreRepository, IMapper mapper, IValidator<TDto> validator,
        IBusinessStrategy<TEntity, TDto> businessStrategy)
    {
        _coreRepository = coreRepository;
        _mapper = mapper;
        _validator = validator;
        _businessStrategy = businessStrategy;
    }

    public virtual async Task<TDto?> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _mapper.Map<TDto>(await _coreRepository.Get(predicate));
    }
    
    public virtual async Task<TDto?> GetById(Guid id)
    {
        return _mapper.Map<TDto>(await _coreRepository.GetById(id));
    }
    
    public virtual async Task<IEnumerable<TDto>> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var entities = await _coreRepository.GetAll(predicate);

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }
    
    public virtual async Task<IEnumerable<TDto>> GetAllById(IEnumerable<Guid> ids)
    {
        var entities = await _coreRepository.GetAllById(ids);

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }

    public virtual async Task<IEnumerable<TDto>> Add(IEnumerable<TDto> dtos)
    {
        _validator.ValidateAdd(dtos);

        var entitiesToAdd = dtos.Select(dto => _mapper.Map<TEntity>(dto)).ToList();
        
        await _businessStrategy.ApplyAdd(entitiesToAdd);

        var addedEntities = await _coreRepository.Add(entitiesToAdd);

        await _coreRepository.SaveChanges();
        return addedEntities.Select(e => _mapper.Map<TDto>(e));
    }

    public virtual async Task<IEnumerable<TDto>> Update(IEnumerable<TDto> dtos)
    {
        _validator.ValidateAdd(dtos);

        var entitiesToUpdate = dtos.Select(dto => _mapper.Map<TEntity>(dto)).ToList();
        
        await _businessStrategy.ApplyAdd(entitiesToUpdate);

        var updatedEntities = await _coreRepository.Update(entitiesToUpdate);

        await _coreRepository.SaveChanges();
        return updatedEntities.Select(e => _mapper.Map<TDto>(e));
    }

    public virtual async Task Delete(Expression<Func<TEntity, bool>> predicate)
    {
        var entitiesToDelete = (await _coreRepository.GetAll(predicate)).ToList();

        await _businessStrategy.ApplyDelete(entitiesToDelete);

        await _coreRepository.Delete(entitiesToDelete);
        await _coreRepository.SaveChanges();
    }

    public virtual async Task DeleteById(IEnumerable<Guid> ids)
    {
        var entitiesToDelete = (await _coreRepository.GetAllById(ids)).ToList();

        await _businessStrategy.ApplyDelete(entitiesToDelete);

        await _coreRepository.Delete(entitiesToDelete);
        await _coreRepository.SaveChanges();
    }

    public virtual async Task<IEnumerable<TDto>> GetListWithFilters(Criteria<TEntity> criteria)
    {
        var entities = await _coreRepository.GetListWithFilters(criteria);

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }

    public virtual async Task<IEnumerable<TDto>> GetPageWithFilters(Criteria<TEntity> criteria, IPagination pagination)
    {
        var entities = await _coreRepository.GetPageWithFilters(criteria, pagination);

        return entities
            .Select(entity => _mapper.Map<TDto>(entity))
            .ToList();
    }
    
    public virtual async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return await _coreRepository.Count(predicate);
    }
    
    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return await _coreRepository.Exists(predicate);
    }
    
    public virtual async Task<bool> ExistsById(Guid id)
    {
        return await _coreRepository.ExistsById(id);
    }
}