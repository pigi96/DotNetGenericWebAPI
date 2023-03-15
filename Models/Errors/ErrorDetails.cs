using System.Text.Json;

namespace GenericWebAPI.Models.Errors;

[Serializable]
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string ExceptionDetails { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}