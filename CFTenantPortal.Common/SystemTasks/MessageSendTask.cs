namespace CFTenantPortal.SystemTasks
{
    /// <summary>
    /// Sends messages
    /// </summary>
    public class MessageSendTask : ISystemTask
    {
        public string Name => "Send message";

        public bool IsRunOnStartup => false;

        private readonly SystemTaskSchedule _schedule;
        public MessageSendTask(SystemTaskSchedule schedule)
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
