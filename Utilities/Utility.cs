namespace GenericWebAPI.Utilities;

public static class Utility
{
    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
        yield return item;
    }
}