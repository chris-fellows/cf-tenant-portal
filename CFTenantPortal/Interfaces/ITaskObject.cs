namespace CFTenantPortal.Interfaces
{
    /// <summary>
    /// Interface for background task
    /// </summary>
    public interface ITaskObject
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Executes task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task Execute(CancellationToken cancellationToken, Dictionary<string, object> parameters, IServiceProvider serviceProvider);
    }
}
