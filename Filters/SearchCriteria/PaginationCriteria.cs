using System.Data.SqlClient;

namespace GenericWebAPI.Filters.SearchCriteria;

public class PaginationCriteria
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortName { get; set; } = "Id";
    public SortOrder SortDir { get; set; } = SortOrder.Descending;
}