namespace CFTenantPortal.Models
{
    /// <summary>
    /// Schedule for task
    /// </summary>
    public class TaskSchedule
    {
        public string TaskId { get; set; } = String.Empty;

        public TimeSpan Frequency { get; set; } = TimeSpan.Zero;

        public DateTimeOffset ExecutedDateTime { get; set; } = DateTimeOffset.MinValue;
    }
}
