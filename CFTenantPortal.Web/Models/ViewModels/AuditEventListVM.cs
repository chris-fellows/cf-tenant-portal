namespace CFTenantPortal.Models
{
    public class AuditEventListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<AuditEventBasicVM> AuditEvents { get; set; }
    }
}
