

namespace CFTenantPortal.SystemTasks
{
    /// <summary>
    /// Performs monitoring of system
    /// </summary>
    public class MonitoringTask : ISystemTask
    {
        public string Name => "Monitoring";

        public bool IsRunOnStartup => false;

        private readonly SystemTaskSchedule _schedule;
        public MonitoringTask(SystemTaskSchedule schedule)
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
