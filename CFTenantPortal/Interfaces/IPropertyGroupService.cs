using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyGroupService
    {
        Task<List<PropertyGroup>> GetAll();

        Task<PropertyGroup> GetById(string id);

        Task Update(PropertyGroup propertyGroup);
    }
}
