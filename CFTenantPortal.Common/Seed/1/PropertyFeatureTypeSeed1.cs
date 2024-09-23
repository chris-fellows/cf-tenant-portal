using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class PropertyFeatureTypeSeed1 : IEntityList<PropertyFeatureType>
    {
        public Task<List<PropertyFeatureType>> ReadAllAsync()
        {
            var entities = new List<PropertyFeatureType>();

            entities.Add(new PropertyFeatureType()
            {                
                Description = "Allocated parking space"
            });

            entities.Add(new PropertyFeatureType()
            {             
                Description = "Rental tenant"
            });

            entities.Add(new PropertyFeatureType()
            {
                Description = "Owner tenant"
            });

            entities.Add(new PropertyFeatureType()
            {             
                Description = "1 bed property"
            });

            entities.Add(new PropertyFeatureType()
            {             
                Description = "2 bed property"
            });

            entities.Add(new PropertyFeatureType()
            {             
                Description = "3 bed property"
            });

            entities.Add(new PropertyFeatureType()
            {             
                Description = "4 bed property"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<PropertyFeatureType> entities)
        {
            return Task.CompletedTask;
        }
    }
}
