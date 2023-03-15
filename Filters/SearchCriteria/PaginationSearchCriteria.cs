using System.Data.SqlClient;
using GenericWebAPI.Filters.Contract;

namespace GenericWebAPI.Filters.SearchCriteria;

public class PaginationCriteria : IPagination
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortName { get; set; }
    public SortOrder? SortDir { get; set; }
}