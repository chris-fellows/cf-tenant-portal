using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyService : IEntityWithIDService<Property, string>
    {
        //Task<List<Property>> GetAll();

        //Task<Property> GetById(string id);

        Task<List<Property>> GetByPropertyGroup(string propertyGroupId);

        Task<List<Property>> GetByPropertyOwner(string propertyOwnerId);

        //Task Update(Property property);
    }
}
