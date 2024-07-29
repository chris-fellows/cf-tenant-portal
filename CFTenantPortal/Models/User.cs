namespace CFTenantPortal.Models
{

    public class User
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Email { get; set; }

        public bool Active { get; set; } = true;
    }
}
