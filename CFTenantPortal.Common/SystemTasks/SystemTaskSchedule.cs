namespace CFTenantPortal.SystemTasks
{
    public class SystemTaskSchedule
    {     /// <summary>
          /// Execute frequency (Zero if only executed on demand)
          /// </summary>
        public TimeSpan ExecuteFrequency { get; set; }

        /// <summary>
        /// Last execute time. This is the time that it was scheduled to execute, not the actual time. The actual
        /// time may be seconds after the scheduled time.
        /// </summary>
        public DateTimeOffset LastExecuteTime { get; set; }

        /// <summary>
        /// Next execute time
        /// </summary>
        public DateTimeOffset NextExecuteTime { get; set; }

        /// <summary>
        /// Calculates the next future time to execute the task
        /// </summary>
        /// <param name="lastExecuteTime"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public DateTimeOffset CalculateNextFutureExecuteTime(DateTimeOffset lastExecuteTime, DateTimeOffset currentTime)
        {
            if (ExecuteFrequency == TimeSpan.Zero) return DateTimeOffset.MaxValue;

            // Ensure that next time is in the future
            DateTimeOffset next = lastExecuteTime;
            while (next <= currentTime)
            {
                next = next.Add(ExecuteFrequency);
            }
            return next;
        }

        /// <summary>
        /// Whether task is executing
        /// </summary>
        public bool IsExecuting { get; set; }
    }
}
