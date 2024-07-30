namespace CFTenantPortal.Models
{
    public class PropertyGroupListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyGroupVM> PropertyGroups { get; set; } = new List<PropertyGroupVM>();
    }
}
