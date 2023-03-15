namespace GenericWebAPI.Models.Errors;

public class ValidationException : ApiException
{
    public static readonly ValidationException VALIDATION_PROPERTY = new (404, "Field [{0}] has issue: [{1}]");

    ValidationException(int statusCode, string message) : base(statusCode, message)
    {
    }

    public ValidationException ThrowValidation(string property, string reason)
    {
        return new ValidationException(StatusCode, string.Format(Message, property, reason));
    }
}