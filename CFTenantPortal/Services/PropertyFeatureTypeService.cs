using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class PropertyFeatureTypeService : IPropertyFeatureTypeService
    {
        public Task<List<PropertyFeatureType>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<PropertyFeatureType> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        }

        public Task Update(PropertyFeatureType propertyFeatureType)
        {
            return Task.CompletedTask;
        }

        private List<PropertyFeatureType> GetAllInternal()
        {
            var propertyFeatureTypes = new List<PropertyFeatureType>();

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "1",
                Description = "Allocated parking space"
            });

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "2",
                Description = "Rental tenant"
            });

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "3",
                Description = "1 bed property"
            });

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "4",
                Description = "2 bed property"
            });

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "5",
                Description = "3 bed property"
            });

            propertyFeatureTypes.Add(new PropertyFeatureType()
            {
                Id = "6",
                Description = "4 bed property"
            });

            return propertyFeatureTypes;
        }
    }
}
