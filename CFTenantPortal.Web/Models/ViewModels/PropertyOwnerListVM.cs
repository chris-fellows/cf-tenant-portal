namespace CFTenantPortal.Models
{
    public class PropertyOwnerListVM
    {
        public string HeaderText { get; set; } = String.Empty;        

        //public List<PropertyOwnerVM> PropertyOwners { get; set; } = new List<PropertyOwnerVM>();
        public List<PropertyOwnerBasicVM> PropertyOwners { get; set; } = new List<PropertyOwnerBasicVM>();

        public PropertyOwnerFilterVM Filter { get; set; }

        public bool AllowCreate { get; set; }
    }
}
