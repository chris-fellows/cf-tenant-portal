namespace CFTenantPortal.SystemTasks
{
    /// <summary>
    /// Interface for background task
    /// </summary>
    public interface ISystemTask
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        string Name { get; }

        bool IsRunOnStartup { get; }

        /// <summary>
        /// Task schedule
        /// </summary>
        SystemTaskSchedule Schedule { get; }

        /// <summary>
        /// Executes task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken, IServiceProvider serviceProvider, Dictionary<string, object> parameters);
    }
}
