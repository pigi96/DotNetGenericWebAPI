namespace GenericWebAPI.Utilities;

public class DefaultBusinessStrategy<TEntity, TDto> : IBusinessStrategy<TEntity, TDto>
{
    // Object is added as DTO and apply logic to it before adding it to DB
    public async Task ApplyAdd(IEnumerable<TEntity> entities)
    {
        // Do nothing by default
    }

    // Object is added as DTO, retrieve the existing ENTITY and apply logic to it before adding it to DB
    public async Task ApplyUpdate(IEnumerable<TEntity> entities)
    {
        // Do nothing by default
    }

    // Object is deleted by ID, retrieve the existing ENTITY and apply logic to it before deleting it from DB
    public async Task ApplyDelete(IEnumerable<TEntity> entity)
    {
        // Do nothing by default
    }
}