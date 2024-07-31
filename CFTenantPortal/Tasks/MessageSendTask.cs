using CFTenantPortal.Interfaces;

namespace CFTenantPortal.Tasks
{
    /// <summary>
    /// Sends messages
    /// </summary>
    public class MessageSendTask : ITaskObject
    {
        public string Id => nameof(MessageSendTask);

        public Task Execute(CancellationToken cancellationToken, Dictionary<string, object> parameters, IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }
    }
}
