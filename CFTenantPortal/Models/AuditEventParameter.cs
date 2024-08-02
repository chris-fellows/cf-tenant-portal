using CFTenantPortal.Enums;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Audit event parameter
    /// </summary>
    public class AuditEventParameter
    {
        /// <summary>
        /// System type of value
        /// </summary>
        public string SystemValueTypeId { get; set; } = String.Empty;

        /// <summary>
        /// Value
        /// </summary>
        public object? Value { get; set; }
    }
}
