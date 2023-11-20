using GenericWebAPI.Filters.Contract;
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
    protected readonly IBusinessExtendedService<TEntity, TDto> _businessExtendedService;

    protected LazyCoreController(IBusinessExtendedService<TEntity, TDto> businessExtendedService)
    {
        _businessExtendedService = businessExtendedService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await _businessExtendedService.GetById(id));
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _businessExtendedService.GetAll());
    }
    
    [HttpPost("list")]
    public async Task<IActionResult> GetListWithFilters(
        [FromBody] IEnumerable<Filter> filters)
    {
        return Ok(await _businessExtendedService.GetListWithFilters(filters));
    }

    [HttpPost("page")]
    public async Task<IActionResult> GetPageWithFilters(
        [FromQuery] PaginationCriteria pagination,
        [FromBody] IEnumerable<Filter> filters)
    {
        return Ok(await _businessExtendedService.GetPageWithFilters(filters, pagination));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        return Ok(await _businessExtendedService.Count());
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] List<TDto> dtos)
    {
        return Ok(await _businessExtendedService.Add(dtos));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] List<TDto> dtos)
    {
        return Ok(await _businessExtendedService.Add(dtos));
    }

    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ExistsById([FromRoute] Guid id)
    {
        return Ok(await _businessExtendedService.ExistsById(id));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] List<Guid> ids)
    {
        await _businessExtendedService.DeleteById(ids);
        return NoContent();
    }
}