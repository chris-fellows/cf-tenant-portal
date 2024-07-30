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
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Property (if any) related to issue
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        /// <summary>
        /// Property group (if any) related to issue
        /// </summary>
        public string PropertyGroupId { get; set; } = String.Empty;

        /// <summary>
        /// Issue description
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Issue status
        /// </summary>
        public string StatusId { get; set; } = String.Empty;

        /// <summary>
        /// Employee (if any) that created issue
        /// </summary>
        public string CreatedEmployeeId { get; set; } = String.Empty;

        /// <summary>
        /// Property owner (if any) that created issue
        /// </summary>
        public string CreatedPropertyOwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Created date and time
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}
