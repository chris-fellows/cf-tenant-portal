using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyOwnerService
    {
        Task<List<PropertyOwner>> GetAll();

        Task<PropertyOwner> GetById(string id);

        Task Update(PropertyOwner propertyOwner);
    }
}
