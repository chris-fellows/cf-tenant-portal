namespace CFTenantPortal.Models
{
    public class EmployeeListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<EmployeeBasicVM> Employees { get; set; }

        public bool AllowCreate { get; set; }
    }
}
