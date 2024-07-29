using CFTenantPortal.Enums;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Employee
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public string Password { get; set; } = String.Empty;

        public List<UserRoles> Roles { get; set; }

        public bool Active { get; set; } = true;
    }
}
