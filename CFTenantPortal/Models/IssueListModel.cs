namespace CFTenantPortal.Models
{
    /// <summary>
    /// Model for issue list
    /// </summary>
    public class IssueListModel
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<IssueModel> Issues { get; set; } = new List<IssueModel>();
    }
}
