namespace CFTenantPortal.Interfaces
{
    public interface IEntityList<TEntity>
    {
        Task<List<TEntity>> ReadAllAsync();

        Task WriteAllAsync(List<TEntity> entities);
    }
}
