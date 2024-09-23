using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBPropertyService : MongoDBBaseService<Property>,  IPropertyService
    {
        public MongoDBPropertyService(IDatabaseConfig databaseConfig) : base(databaseConfig, "properties")
        {

        }

        //public Task<List<AccountTransaction>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<AccountTransaction> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}


        public Task<Property?> GetByIdAsync(string id)
        {
            return _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        //public Task<AccountTransaction?> GetByNameAsync(string name)
        //{
        //    return _entities.Find(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public Task DeleteByIdAsync(string id)
        {
            return _entities.DeleteOneAsync(id);
        }

        //public Task<List<Property>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<Property> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(p => p.Id == id));
        //}

        public async Task<List<Property>> GetByPropertyGroup(string propertyGroupId)
        {
            var items = GetAll().Where(pg => pg.GroupId == propertyGroupId).ToList();
            return items;
        }

        public async Task<List<Property>> GetByPropertyOwner(string propertyOwnerId)
        {
            var items = GetAll().Where(pg => pg.OwnerId == propertyOwnerId).ToList();
            return items;
        }

        //public Task Update(Property property)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<Property> GetAllInternal()
        //{
        //    var properties = new List<Property>();

        //    properties.Add(new Property()
        //    {
        //        Id = "1",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",                    
        //        },
        //        GroupId = "1",
        //        OwnerId = "1",
        //        DocumentIds = new List<string>() { "3", "4" },
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    properties.Add(new Property()
        //    {
        //        Id = "2",
        //        Address = new Address()
        //        {
        //            Line1 = "101 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        },
        //        GroupId = "1",
        //        OwnerId = "2",
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    properties.Add(new Property()
        //    {
        //        Id = "3",
        //        Address = new Address()
        //        {
        //            Line1 = "102 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        },
        //        GroupId = "1",
        //        OwnerId = "3",
        //        DocumentIds = new List<string>() { "3", "4" },
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    properties.Add(new Property()
        //    {
        //        Id = "4",
        //        Address = new Address()
        //        {
        //            Line1 = "50 Church Street",
        //            County = "Berkshire",
        //            Town = "Cookham",
        //            Postcode = "SL3 9YT",
        //        },
        //        GroupId = "2",
        //        OwnerId = "4",
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    properties.Add(new Property()
        //    {
        //        Id = "5",
        //        Address = new Address()
        //        {
        //            Line1 = "51 Church Street",
        //            County = "Berkshire",
        //            Town = "Cookham",
        //            Postcode = "SL3 9YT",
        //        },
        //        GroupId = "2",
        //        OwnerId = "5",
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    properties.Add(new Property()
        //    {
        //        Id = "6",
        //        Address = new Address()
        //        {
        //            Line1 = "52 Church Street",
        //            County = "Berkshire",
        //            Town = "Cookham",
        //            Postcode = "SL3 9YT",
        //        },
        //        GroupId = "2",
        //        OwnerId = "5",   // Owns multiple properties
        //        FeatureTypeIds = new List<string>() { "1", "3" }
        //    });

        //    return properties;

        //}
    }
}
