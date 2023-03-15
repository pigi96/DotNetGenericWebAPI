using GenericWebAPI.Filters.Filtering;
using GenericWebAPI.Filters.SearchCriteria;
using GenericWebAPI.Models;
using GenericWebAPI.Services.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebAPI.Controllers.Extensions;

public abstract class LazyCoreController<T> : ApiControllerBase where T : DtoCore, new()
{
    protected readonly IBusinessCoreService<T> _businessCoreService;

    protected LazyCoreController(IBusinessCoreService<T> businessCoreService)
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
        [FromBody] List<Filter> filters)
    {
        return Ok(await _businessCoreService.GetListWithFilters(filters));
    }

    [HttpPost("page")]
    public async Task<IActionResult> GetPageWithFilters(
        [FromQuery] PaginationCriteria pagination,
        [FromBody] List<Filter> filters)
    {
        return Ok(await _businessCoreService.GetPageWithFilters(filters, pagination));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        return Ok(await _businessCoreService.Count());
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] T entityDto)
    {
        return Ok(await _businessCoreService.Add(entityDto));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] T entityDto)
    {
        return Ok(await _businessCoreService.Add(entityDto));
    }

    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ExistsById([FromRoute] Guid id)
    {
        return Ok(await _businessCoreService.ExistsById(id));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _businessCoreService.DeleteById(id);
        return NoContent();
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] T entityDto)
    {
        await _businessCoreService.Delete(entityDto);
        return NoContent();
    }
}