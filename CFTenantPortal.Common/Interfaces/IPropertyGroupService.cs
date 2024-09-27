using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyGroupService : IEntityWithIDService<PropertyGroup, string>
    {
        Task<List<PropertyGroup>> GetByFilterAsync(PropertyGroupFilter propertyGroupFilter);
    }
}
