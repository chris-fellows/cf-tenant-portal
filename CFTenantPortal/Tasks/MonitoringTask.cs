using CFTenantPortal.Interfaces;

namespace CFTenantPortal.Tasks
{
    /// <summary>
    /// Performs monitoring of system
    /// </summary>
    public class MonitoringTask : ITaskObject
    {
        public string Id => nameof(MonitoringTask);

        public Task Execute(CancellationToken cancellationToken, Dictionary<string, object> parameters, IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}
