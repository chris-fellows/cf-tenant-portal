namespace CFTenantPortal.Models
{
    /// <summary>
    /// Account transaction type. E.g. Management fees request
    /// </summary>
    public class AccountTransactionType
    {
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Internal name for lookup
        /// </summary>
        public string InternalName { get; set; } = String.Empty;
    }
}
