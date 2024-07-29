using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class PropertyService : IPropertyService
    {
        public Task<List<Property>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<Property> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(p => p.Id == id));
        }

        public async Task<List<Property>> GetByPropertyGroup(string propertyGroupId)
        {
            var items = GetAllInternal().Where(pg => pg.GroupId == propertyGroupId).ToList();
            return items;
        }

        public Task Update(Property property)
        {
            return Task.CompletedTask;
        }

        private List<Property> GetAllInternal()
        {
            var properties = new List<Property>();

            properties.Add(new Property()
            {
                Id = "1",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",                    
                },
                GroupId = "1",
                OwnerId = "1",
            });

            properties.Add(new Property()
            {
                Id = "2",
                Address = new Address()
                {
                    Line1 = "101 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                GroupId = "1",
                OwnerId = "2",
            });

            properties.Add(new Property()
            {
                Id = "3",
                Address = new Address()
                {
                    Line1 = "102 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                GroupId = "1",
                OwnerId = "3",
            });

            properties.Add(new Property()
            {
                Id = "4",
                Address = new Address()
                {
                    Line1 = "50 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = "2",
                OwnerId = "4",
            });

            properties.Add(new Property()
            {
                Id = "5",
                Address = new Address()
                {
                    Line1 = "51 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = "2",
                OwnerId = "5",
            });

            properties.Add(new Property()
            {
                Id = "6",
                Address = new Address()
                {
                    Line1 = "52 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = "2",
                OwnerId = "5",   // Owns multiple properties
            });

            return properties;

        }
    }
}
