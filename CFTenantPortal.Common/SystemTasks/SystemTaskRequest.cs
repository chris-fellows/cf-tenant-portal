namespace CFTenantPortal.SystemTasks
{
    public class SystemTaskRequest
    {  
        /// <summary>
        /// System task name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Earliest time to execute
        /// </summary>
        public DateTimeOffset ExecuteTime { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
    }
}
