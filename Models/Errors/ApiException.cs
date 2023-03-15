namespace GenericWebAPI.Models.Errors;

public abstract class ApiException : Exception, IEquatable<ApiException>
{
    public int StatusCode { get; private set; }
    public string Message { get; private set; }

    protected ApiException(int statusCode, string message) =>
        (StatusCode, Message) = (statusCode, message);
    
    public bool Equals(ApiException? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return StatusCode == other.StatusCode && Message.Equals(other.Message);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ApiException)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StatusCode, Message);
    }
}