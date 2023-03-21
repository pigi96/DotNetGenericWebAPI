namespace GenericWebAPI.Utilities;

public class DefaultValidator<TDto> : IValidator<TDto>
{
    public bool ValidateAdd(IEnumerable<TDto> dto)
    {
        // Do nothing by default
        return true;
    }

    public bool ValidateUpdate(IEnumerable<TDto> dto)
    {
        // Do nothing by default
        return true;
    }

    public bool ValidateDelete(IEnumerable<TDto> dto)
    {
        // Do nothing by default
        return true;
    }
}