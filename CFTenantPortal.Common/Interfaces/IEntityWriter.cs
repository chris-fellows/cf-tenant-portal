namespace CFTenantPortal.Interfaces
{
    /// <summary>
    /// Interface for writing entities. E.g. Write to CSV.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityWriter<TEntity>
    {
        Task WriteAllAsync(List<TEntity> entities);
    }
}
