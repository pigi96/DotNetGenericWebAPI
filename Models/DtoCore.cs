using GenericWebAPI.Models.Contracts;

namespace GenericWebAPI.Models;

[Serializable]
public abstract class DtoCore : IDto
{
    public Guid Id { get; set; }
}
