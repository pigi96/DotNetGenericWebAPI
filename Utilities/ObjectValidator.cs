namespace GenericWebAPI.Utilities;

public class ObjectValidator<T>
{
    public static (bool, string) Exists(T? value)
    {
        return (value != null, $"{value} is missing");
    }
}