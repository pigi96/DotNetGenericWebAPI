namespace GenericWebAPI.Utilities;

public class DefaultValidator<TDto> : IValidator<TDto>
{
    public bool ValidateAdd(TDto dto)
    {
        // Do nothing by default
        return true;
    }

    public bool ValidateUpdate(TDto dto)
    {
        // Do nothing by default
        return true;
    }

    public bool ValidateDelete(TDto dto)
    {
        // Do nothing by default
        return true;
    }
}