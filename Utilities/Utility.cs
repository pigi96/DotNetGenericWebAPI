namespace GenericWebAPI.Utilities;

public static class Utility
{
    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static ICollection<T> ToCollection<T>(this T item)
    {
        return new List<T> { item };
    }
}