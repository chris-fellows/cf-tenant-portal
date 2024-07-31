using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class PropertyOwnerService : IPropertyOwnerService
    {
        public Task<List<PropertyOwner>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<PropertyOwner> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(po => po.Id == id));
        }

        public Task Update(PropertyOwner propertyOwner)
        {
            return Task.CompletedTask;
        }

        private List<PropertyOwner> GetAllInternal()
        {
            var propertyOwners = new List<PropertyOwner>();

            propertyOwners.Add(new PropertyOwner()
            {
                Id = "1",
                Name = "Owner 1",
                Email = "owner1@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                DocumentIds = new List<string>() { "2", "3" }
            });

            propertyOwners.Add(new PropertyOwner()
            {
                Id = "2",
                Name = "Owner 2",
                Email = "owner2@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                DocumentIds = new List<string>() { "2", "3" }
            });

            propertyOwners.Add(new PropertyOwner()
            {
                Id = "3",
                Name = "Owner 3",
                Email = "owner3@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            propertyOwners.Add(new PropertyOwner()
            {
                Id = "4",
                Name = "Owner 4",
                Email = "owner4@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            propertyOwners.Add(new PropertyOwner()
            {
                Id = "5",
                Name = "Owner 5",
                Email = "owner5@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            return propertyOwners;
        }
    }
}
