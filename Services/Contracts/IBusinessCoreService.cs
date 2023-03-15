using System.Linq.Expressions;
using GenericWebAPI.Filters.Contract;
using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Models;

namespace GenericWebAPI.Services.Extensions;

public interface IBusinessCoreService<TDto> where TDto : DtoCore, new()
{ 
    Task<TDto?> GetById(Guid id);
    Task<TDto?> GetByPredicate(Expression<Func<TDto, bool>> predicate);
    Task<List<TDto>> GetAll();
    Task<List<TDto>> GetListWithFilters(List<Filter> filters);
    Task<List<TDto>> GetPageWithFilters(List<Filter> filters, IPagination pagination);
    Task<int> Count();
    Task<TDto> Add(TDto entity);
    Task<TDto> Update(TDto entity, Expression<Func<TDto, bool>> predicate);
    Task<bool> ExistsById(Guid id);
    Task<bool> ExistsByPredicate(Expression<Func<TDto, bool>> predicate);
    Task DeleteById(Guid id);
    Task Delete(TDto entity);
    Task DeleteByPredicate(Expression<Func<TDto, bool>> predicate);
}