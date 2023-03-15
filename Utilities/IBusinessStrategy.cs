namespace GenericWebAPI.Utilities;

public interface IBusinessStrategy<TEntity, TDto>
{
    Task ApplyAdd(TDto dto);
    Task ApplyUpdate(TEntity entity, TDto dto);
    Task ApplyDelete(TEntity entity);
}