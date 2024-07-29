namespace CFTenantPortal.Models
{
    public class IssueTypeListModel
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<IssueTypeModel> IssueTypes { get; set; } = new List<IssueTypeModel>();
    }
}
