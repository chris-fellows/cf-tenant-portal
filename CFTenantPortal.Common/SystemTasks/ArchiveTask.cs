namespace CFTenantPortal.SystemTasks
{
    /// <summary>
    /// Archives data. E.g. Rolls up accounting transactions
    /// </summary>
    public class ArchiveTask : ISystemTask
    {
        public string Name => "Archive";

        public bool IsRunOnStartup => false;

        private readonly SystemTaskSchedule _schedule;
        public ArchiveTask(SystemTaskSchedule schedule)
        {
            _schedule = schedule;
        }

        public SystemTaskSchedule Schedule => _schedule;

        public Task ExecuteAsync(CancellationToken cancellationToken, IServiceProvider serviceProvider, Dictionary<string, object> parameters)
        {
            return Task.CompletedTask;
        }
    }
}
