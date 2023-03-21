namespace GenericWebAPI.Utilities;

public interface IValidator<TDto>
{
    bool ValidateAdd(IEnumerable<TDto> dto);
    bool ValidateUpdate(IEnumerable<TDto> dto);
    bool ValidateDelete(IEnumerable<TDto> dto);
}