using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;
using GenericWebAPI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebAPI.Controllers;

public abstract class LazyCoreController<TDto, TEntity> : ApiControllerBase
    where TDto : DtoCore, new()
    where TEntity : EntityCore, new()
{
    protected readonly IBusinessCoreService<TEntity, TDto> _businessCoreService;

    protected LazyCoreController(IBusinessCoreService<TEntity, TDto> businessCoreService)
    {
        _businessCoreService = businessCoreService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await _businessCoreService.GetById(id));
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _businessCoreService.GetAll());
    }
    
    [HttpPost("list")]
    public async Task<IActionResult> GetListWithFilters(
        [FromBody] Criteria<TEntity> filters)
    {
        return Ok(await _businessCoreService.GetListWithFilters(filters));
    }

    [HttpPost("page")]
    public async Task<IActionResult> GetPageWithFilters(
        [FromQuery] PaginationCriteria pagination,
        [FromBody] Criteria<TEntity> filters)
    {
        return Ok(await _businessCoreService.GetPageWithFilters(filters, pagination));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        return Ok(await _businessCoreService.Count());
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] List<TDto> dtos)
    {
        return Ok(await _businessCoreService.Add(dtos));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] List<TDto> dtos)
    {
        return Ok(await _businessCoreService.Add(dtos));
    }

    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ExistsById([FromRoute] Guid id)
    {
        return Ok(await _businessCoreService.ExistsById(id));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] List<Guid> ids)
    {
        await _businessCoreService.DeleteById(ids);
        return NoContent();
    }
}