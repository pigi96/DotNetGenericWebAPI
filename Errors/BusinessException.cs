namespace GenericWebAPI.Models.Errors;

public class BusinessException : ApiException
{
    public static readonly BusinessException BUSINESS_EXCEPTION = new (404, "Business Exception");

    public BusinessException(int statusCode, string message) : base(statusCode, message)
    {
    }
}