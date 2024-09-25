namespace CFTenantPortal.Models
{
    public class PropertyGroupListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        //public List<PropertyGroupVM> PropertyGroups { get; set; } = new List<PropertyGroupVM>();
        public List<PropertyGroupBasicVM> PropertyGroups { get; set; } = new List<PropertyGroupBasicVM>();

        public PropertyGroupFilterVM Filter { get; set; }

        public bool AllowCreate { get; set; }
    }
}
