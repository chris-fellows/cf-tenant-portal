using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBPropertyOwnerService : MongoDBBaseService<PropertyOwner>, IPropertyOwnerService
    {
        public MongoDBPropertyOwnerService(IDatabaseConfig databaseConfig) : base(databaseConfig, "property_owners")
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


        public Task<PropertyOwner?> GetByIdAsync(string id)
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

        //public Task<List<PropertyOwner>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<PropertyOwner> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(po => po.Id == id));
        //}

        //public Task Update(PropertyOwner propertyOwner)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<PropertyOwner> GetAllInternal()
        //{
        //    var propertyOwners = new List<PropertyOwner>();

        //    propertyOwners.Add(new PropertyOwner()
        //    {
        //        Id = "1",
        //        Name = "Owner 1",
        //        Email = "owner1@myproperty.com",
        //        Phone = "1234567890",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        },
        //        DocumentIds = new List<string>() { "2", "3" }
        //    });

        //    propertyOwners.Add(new PropertyOwner()
        //    {
        //        Id = "2",
        //        Name = "Owner 2",
        //        Email = "owner2@myproperty.com",
        //        Phone = "1234567890",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        },
        //        DocumentIds = new List<string>() { "2", "3" }
        //    });

        //    propertyOwners.Add(new PropertyOwner()
        //    {
        //        Id = "3",
        //        Name = "Owner 3",
        //        Email = "owner3@myproperty.com",
        //        Phone = "1234567890",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        }
        //    });

        //    propertyOwners.Add(new PropertyOwner()
        //    {
        //        Id = "4",
        //        Name = "Owner 4",
        //        Email = "owner4@myproperty.com",
        //        Phone = "1234567890",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        }
        //    });

        //    propertyOwners.Add(new PropertyOwner()
        //    {
        //        Id = "5",
        //        Name = "Owner 5",
        //        Email = "owner5@myproperty.com",
        //        Phone = "1234567890",
        //        Address = new Address()
        //        {
        //            Line1 = "100 High Street",
        //            County = "Berkshire",
        //            Town = "Maidenhead",
        //            Postcode = "SL1 8AX",
        //        }
        //    });

        //    return propertyOwners;
        //}
    }
}
