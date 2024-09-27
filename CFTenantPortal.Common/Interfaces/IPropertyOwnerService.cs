using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyOwnerService : IEntityWithIDService<PropertyOwner, string>
    {       
        Task<PropertyOwner> GetByEmailAsync(string email);

        Task<List<PropertyOwner>> GetByFilterAsync(PropertyOwnerFilter propertyOwnerFilter);
    }
}
