namespace GenericWebAPI.Models;

public class PagedResult<T>
{
    public IEnumerable<T> Results { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalElements { get; init; }
    public int TotalPages { get; init; }
}