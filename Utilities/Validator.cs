using GenericWebAPI.Models.Errors;

namespace GenericWebAPI.Utilities;

public class Validator<T>
{
    private readonly T _value;
    private readonly List<(Func<T, (bool, string)> Validator, string fieldName, ValidationException apiException)> _validators;

    public Validator(T value)
    {
        _value = value;
        _validators = new List<(Func<T, (bool, string)>, string, ValidationException)>();
    }

    public Validator<T> AddFieldValidator<TField>(Func<T, TField> fieldSelector, string fieldName, params (Func<TField, (bool, string)>, ValidationException)[] validators)
    {
        foreach (var (validator, apiException) in validators)
        {
            (bool, string) FieldValidator(T value)
            {
                var fieldValue = fieldSelector(value);
                var validationResult = validator(fieldValue);
                return (validationResult.Item1, validationResult.Item2);
            }

            _validators.Add((FieldValidator, fieldName, apiException));
        }
        return this;
    }
    
    public Validator<T> Add(Func<T, (bool, string)> validator, string fieldName, ValidationException apiException)
    {
        _validators.Add((validator, fieldName, apiException));
        return this;
    }

    public void Validate()
    {
        foreach (var (validator, fieldName, apiException) in _validators)
        {
            var field = validator(_value);
            if (!field.Item1)
            {
                throw apiException.ThrowValidation(fieldName, field.Item2);
            }
        }
    }
}
