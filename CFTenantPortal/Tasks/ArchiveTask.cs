using CFTenantPortal.Interfaces;

namespace CFTenantPortal.Tasks
{
    /// <summary>
    /// Archives data. E.g. Rolls up accounting transactions
    /// </summary>
    public class ArchiveTask : ITaskObject
    {
        public string Id => nameof(ArchiveTask);

        public Task Execute(CancellationToken cancellationToken, Dictionary<string, object> parameters, IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}
