namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property list view model
    /// </summary>
    public class PropertyListVM
    {
        public string HeaderText { get; set; } = String.Empty;

        public List<PropertyBasicVM> Properties { get; set; } = new List<PropertyBasicVM>();
    }
}
