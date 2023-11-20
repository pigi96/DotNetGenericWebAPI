using System.Linq.Expressions;

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

    public static void AddIfHasValue<T>(this List<Expression<Func<T, bool>>> predicates, object nullableObject,
        Expression<Func<T, bool>> predicate)
    {
        if (nullableObject != null)
        {
            predicates.Add(predicate);
        }
    }
}