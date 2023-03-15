namespace GenericWebAPI.Filters.Filtering;

public class Filter
{
    public string Property { get; set; }
    public string Value1 { get; set; }
    public string? Value2 { get; set; }
    public FilterType FilterType { get; set; }
}