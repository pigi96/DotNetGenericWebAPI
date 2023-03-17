namespace GenericWebAPI.Utilities;

public interface IBusinessStrategy<TEntity, TDto>
{
    Task ApplyAdd(IEnumerable<TEntity> entities);
    Task ApplyUpdate(IEnumerable<TEntity> entities);
    Task ApplyDelete(IEnumerable<TEntity> entities);
}