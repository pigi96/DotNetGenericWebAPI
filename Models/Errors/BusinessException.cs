namespace GenericWebAPI.Models.Errors;

public class BusinessException : ApiException
{
    public static readonly BusinessException BUSINESS_EXCEPTION = new (404, "Business Exception");
    public static readonly BusinessException USER_NOT_EXISTS = new (404, "User doesn't exist");
    public static readonly BusinessException USER_EXISTS = new (409, "User is already signed up");

    public BusinessException(int statusCode, string message) : base(statusCode, message)
    {
    }
}