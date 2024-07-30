using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IPropertyFeatureTypeService
    {
        Task<List<PropertyFeatureType>> GetAll();

        Task<PropertyFeatureType> GetById(string id);

        Task Update(PropertyFeatureType propertyFeatureType);
    }
}
