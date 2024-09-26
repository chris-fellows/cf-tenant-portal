using CFTenantPortal.Enums;

namespace CFTenantPortal.Interfaces
{
    /// <summary>
    /// Interface for current request info
    /// </summary>
    public interface IRequestInfoService
    {
        /// <summary>
        /// Whether user logged in
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// User roles for logged in user (if any)
        /// </summary>
        List<UserRoles>? UserRoles { get; }

        /// <summary>
        /// User Id (EmployeeId/PropertyOwnerId) for logged in user (if any)
        /// </summary>
        string? UserId { get; }
    }
}
