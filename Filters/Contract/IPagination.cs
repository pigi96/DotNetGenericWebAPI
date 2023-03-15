using System.Data.SqlClient;

namespace GenericWebAPI.Filters.Contract;

public interface IPagination
{
    private static int DEFAULT_PAGE_NUMBER = 1;
    private static int DEFAULT_PAGE_SIZE = 10;
    private static string DEFAULT_SORT_NAME = "Id";
    private static SortOrder DEFAULT_SORT_DIR = SortOrder.Descending;

    public int? PageNumber { get; }
    public int? PageSize { get; }
    public string? SortName { get; }
    public SortOrder? SortDir { get; }

    public int GetPageNumber()
    {
        return PageNumber ?? DEFAULT_PAGE_NUMBER;
    }

    public int GetPageSize()
    {
        return PageSize ?? DEFAULT_PAGE_SIZE;
    }

    public string GetSortName()
    {
        return SortName ?? DEFAULT_SORT_NAME;
    }
    
    public string GetSortDir()
    {
        var sortDir = SortDir ?? DEFAULT_SORT_DIR;
        return sortDir == SortOrder.Ascending ? "asc" : "desc";
    }
}