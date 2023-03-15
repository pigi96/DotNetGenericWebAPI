
namespace GenericWebAPI.Models;

public abstract class DtoBase : DtoCore
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
}