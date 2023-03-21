using System.Collections;

namespace GenericWebAPI.Utilities;

public interface IBusinessStrategy<TEntity, TDto>
{
    Task ApplyAdd(ICollection<TEntity> entities);
    Task ApplyUpdate(ICollection<TEntity> entities);
    Task ApplyDelete(ICollection<TEntity> entities);
}