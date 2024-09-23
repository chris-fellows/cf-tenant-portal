namespace CFTenantPortal.SystemTasks
{
    public class SystemTasks : ISystemTasks
    {
        private readonly int _maxConcurrentTasks;
        private readonly List<SystemTaskRequest> _requests = new List<SystemTaskRequest>();
        private readonly List<ISystemTask> _systemTasks;

        public SystemTasks(List<ISystemTask> systemTasks, int maxConcurrentTasks)
        {
            _maxConcurrentTasks = maxConcurrentTasks;
            _systemTasks = systemTasks;
        }

        public int MaxConcurrentTasks => _maxConcurrentTasks;

        public List<ISystemTask> AllTasks => _systemTasks;

        public List<ISystemTask> ActiveTasks
        {
            get { return _systemTasks.Where(st => st.Schedule.IsExecuting).ToList(); }
        }

        public List<ISystemTask> OverdueTasks
        {
            get
            {
                return _systemTasks.Where(st => st.Schedule.ExecuteFrequency != TimeSpan.Zero &&
                                    st.Schedule.NextExecuteTime <= DateTimeOffset.UtcNow).ToList();
            }
        }

        public List<SystemTaskRequest> AllRequests => _requests;

        public List<SystemTaskRequest> OverdueRequests => _requests.Where(r => r.ExecuteTime <= DateTimeOffset.UtcNow).ToList();
    }
}
