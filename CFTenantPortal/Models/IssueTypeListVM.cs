namespace CFTenantPortal.Models
{
    public class IssueTypeListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<IssueTypeVM> IssueTypes { get; set; } = new List<IssueTypeVM>();
    }
}
