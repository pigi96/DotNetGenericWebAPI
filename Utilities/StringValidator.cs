namespace GenericWebAPI.Utilities;

public static class StringValidator
{
    public static (bool, string) BiggerThan(string value, int min)
    {
        return (value.Length > min, $"'{value}' is not bigger than (including) {min}");
    }
    
    public static (bool, string) BiggerThanExclusive(string value, int min)
    {
        return (value.Length >= min, $"'{value}' is not bigger than (excluding) {min}");
    }
    
    public static (bool, string) SmallerThan(string value, int max)
    {
        return (value.Length < max, $"'{value}' is not smaller than (including) {max}");
    }
    
    public static (bool, string) SmallerThanExclusive(string value, int max)
    {
        return (value.Length <= max, $"'{value}' is not smaller than (excluding) {max}");
    }
}