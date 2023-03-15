namespace GenericWebAPI.Utilities;

public static class NumberValidator<T> where T : struct, IComparable<T>
{
    public static (bool, string) IsPositive(T? value)
    {
        return (value.GetValueOrDefault().CompareTo(default(T)) > 0, $"{value} is not positive");
    }
    
    public static (bool, string?) IsNegative(T? value)
    {
        return (value.GetValueOrDefault().CompareTo(default(T)) < 0, $"{value} is not negative");
    }
    
    public static (bool, string) IsPositiveOrZero(T? value)
    {
        return (value.GetValueOrDefault().CompareTo(default(T)) >= 0, $"{value} is not positive or zero");
    }
    
    public static (bool, string) IsNegativeOrZero(T? value)
    {
        return (value.GetValueOrDefault().CompareTo(default(T)) <= 0, $"{value} is not negative or zero");
    }

    public static (bool, string) BiggerThan(T? value, T min)
    {
        return (value.GetValueOrDefault().CompareTo(min) >= 0, $"{value} is not bigger than (including) {min}");
    }
    
    public static (bool, string) BiggerThanExclusive(T? value, T min)
    {
        return (value.GetValueOrDefault().CompareTo(min) > 0, $"{value} is not bigger than (excluding) {min}");
    }
    
    public static (bool, string) SmallerThan(T? value, T max)
    {
        return (value.GetValueOrDefault().CompareTo(max) <= 0, $"{value} is not smaller than (including) {max}");
    }
    
    public static (bool, string) SmallerThanExclusive(T? value, T max)
    {
        return (value.GetValueOrDefault().CompareTo(max) < 0, $"{value} is not smaller than (excluding) {max}");
    }
}