namespace CFTenantPortal.Models
{
    public class PropertyOwnerListModel
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyOwnerModel> PropertyOwners { get; set; } = new List<PropertyOwnerModel>();
    }
}
