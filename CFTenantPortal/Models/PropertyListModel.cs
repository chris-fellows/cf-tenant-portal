namespace CFTenantPortal.Models
{
    public class PropertyListModel
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
    }
}
