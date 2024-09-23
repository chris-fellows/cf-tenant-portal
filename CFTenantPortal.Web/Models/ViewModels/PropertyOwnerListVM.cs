namespace CFTenantPortal.Models
{
    public class PropertyOwnerListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyOwnerVM> PropertyOwners { get; set; } = new List<PropertyOwnerVM>();
    }
}
