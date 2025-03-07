namespace CFTenantPortal.Models
{
    public class SendMessageVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public string MessageTypeId { get; set; } = String.Empty;

        public string Text { get; set; } = String.Empty;

        public string IssueId { get; set; } = String.Empty;

        public string PropertyOwnerId { get; set; } = String.Empty;

        public string PropertyId { get; set; } = String.Empty;

        public List<EntityReference> IssueRefList { get; set; } = new List<EntityReference>();

        public List<EntityReference> MessageTypeRefList { get; set; } = new List<EntityReference>();

        public List<EntityReference> PropertyOwnerRefList { get; set; } = new List<EntityReference>();

        public List<EntityReference> PropertyRefList { get; set; } = new List<EntityReference>();
    }
}
