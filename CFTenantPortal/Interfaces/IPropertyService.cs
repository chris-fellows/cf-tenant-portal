using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyService
    {
        Task<List<Property>> GetAll();

        Task<Property> GetById(string id);

        Task<List<Property>> GetByPropertyGroup(string propertyGroupId);

        Task Update(Property property);
    }
}
