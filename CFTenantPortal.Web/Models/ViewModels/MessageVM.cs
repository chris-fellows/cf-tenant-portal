namespace CFTenantPortal.Models
{
    /// <summary>
    /// Message view model
    /// </summary>
    public class MessageVM
    {   
        public string Id { get; set; } = String.Empty;
        
        public string MessageTypeId { get; set; } = String.Empty;

        public string PropertyOwnerId { get; set; } = String.Empty;

        public string IssueId { get; set; } = String.Empty;

        public string PropertyId { get; set; } = String.Empty;

        public string Text { get; set; } = String.Empty;

        public List<string> DocumentIds { get; set; } = new List<string>();

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;

        public List<DocumentBasicVM> Documents { get; set; } = new List<DocumentBasicVM>();

        public List<EntityReference> MessageTypeList { get; set; } = new List<EntityReference>();

        public List<EntityReference> IssueList { get; set; } = new List<EntityReference>();

        public List<EntityReference> PropertyList { get; set; } = new List<EntityReference>();

        public List<EntityReference> PropertyOwnerList { get; set; } = new List<EntityReference>();
    }
}
