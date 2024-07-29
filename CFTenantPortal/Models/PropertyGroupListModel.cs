namespace CFTenantPortal.Models
{
    public class PropertyGroupListModel
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyGroupModel> PropertyGroups { get; set; } = new List<PropertyGroupModel>();
    }
}
