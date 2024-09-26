namespace CFTenantPortal.Models
{
    /// <summary>
    /// Audit event parameter
    /// 
    /// TODO: Consider adding a SystemValueContextType enum so that an audit event can contain multiple values
    /// for the same value type and we can indicate what they are. E.g. If we're recording an event where we're
    /// recording the EmployeeIds for a manager who updates an employee then there would be 2 context types.
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
