using CFTenantPortal.Enums;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue related to property
    /// 
    /// TODO: Consider expanding to allow link to property group (E.g. Whole building)
    /// </summary>
    public class Issue
    {
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Issue type
        /// </summary>
        public string IssueTypeId { get; set; } = String.Empty;

        /// <summary>
        /// Property related to issue
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        /// <summary>
        /// Issue description
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Issue status
        /// </summary>
        public IssueStatuses Status { get; set; } = IssueStatuses.New;

        /// <summary>
        /// Created date and time
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}
