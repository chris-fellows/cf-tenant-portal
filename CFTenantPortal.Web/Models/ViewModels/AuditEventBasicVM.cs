using System.ComponentModel.DataAnnotations;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Audit event basic. The entity contains properties for the first parameter which can then be displayed
    /// </summary>
    public class AuditEventBasicVM
    {
        public string Id { get; set; } = String.Empty;
        
        public string EventTypeId { get; set; } = String.Empty;

        [Display(Name = "Event Type")]
        public string EventTypeDescription { get; set; } = String.Empty;

        public AuditEventParameterVM FirstParameter { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}
