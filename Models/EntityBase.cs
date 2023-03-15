namespace GenericWebAPI.Models;

public abstract class EntityBase : EntityCore
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }
}