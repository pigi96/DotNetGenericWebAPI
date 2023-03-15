namespace GenericWebAPI.Utilities;

public interface IValidator<TDto>
{
    bool ValidateAdd(TDto dto);
    bool ValidateUpdate(TDto dto);
    bool ValidateDelete(TDto dto);
}