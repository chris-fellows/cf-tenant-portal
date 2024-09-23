namespace CFTenantPortal.Interfaces
{
    /// <summary>
    /// Interface for service to manage an entity with an ID
    /// </summary>
    /// <typeparam name="TEntityType">Entity</typeparam>
    /// <typeparam name="TIDType">Type of entity Id</typeparam>
    public interface IEntityWithIDService<TEntityType, TIDType>
    {
        /// <summary>
        /// Imports from list
        /// </summary>
        /// <param name="eventTypeList"></param>
        /// <returns></returns>
        Task ImportAsync(IEntityList<TEntityType> entityList);

        /// <summary>
        /// Exports to list
        /// </summary>
        /// <param name="eventTypeList"></param>
        /// <returns></returns>
        Task ExportAsync(IEntityList<TEntityType> entityList);

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntityType> GetAll();

        /// <summary>
        /// Gets event type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntityType?> GetByIdAsync(TIDType id);

        ///// <summary>
        ///// Gets event type by name
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //Task<TEntityType?> GetByNameAsync(string name);

        /// <summary>
        /// Adds event type
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<TEntityType> AddAsync(TEntityType entity);

        /// <summary>
        /// Updates event type
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<TEntityType> UpdateAsync(TEntityType entity);

        /// <summary>
        /// Deletes all event types
        /// </summary>
        /// <returns></returns>
        Task DeleteAllAsync();

        /// <summary>
        /// Deletes event type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteByIdAsync(string id);
    }
}
