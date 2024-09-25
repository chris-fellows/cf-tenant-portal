using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [ValidateNever]
        public List<EntityReference> MessageTypeRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> IssueRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyRefList { get; set; } = new List<EntityReference>();

        [ValidateNever]
        public List<EntityReference> PropertyOwnerRefList { get; set; } = new List<EntityReference>();
    }
}
