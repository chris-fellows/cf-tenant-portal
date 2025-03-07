namespace CFTenantPortal.Interfaces
{
    /// <summary>
    /// Interface for reading entities. E.g. Read from CSV or memory
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityReader<TEntity>
    {
        Task<List<TEntity>> ReadAllAsync();        
    }
}
