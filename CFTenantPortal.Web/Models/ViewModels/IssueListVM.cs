namespace CFTenantPortal.Models
{
    /// <summary>
    /// Model for issue list
    /// </summary>
    public class IssueListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<IssueBasicVM> Issues { get; set; } = new List<IssueBasicVM>();        

        public bool AllowCreate { get; set; }
    }
}
