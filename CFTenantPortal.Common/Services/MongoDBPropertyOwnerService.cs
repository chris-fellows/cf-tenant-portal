using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFUtilities.Utilities;
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

        public Task<PropertyOwner?> GetByEmailAsync(string email)
        {
            return _entities.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<PropertyOwner>> GetByFilterAsync(PropertyOwnerFilter propertyOwnerFilter)
        {
            // Get filter definition
            var filterDefinition = GetFilterDefinition(propertyOwnerFilter);

            // Get filtered property owners page
            var auditEvents = await _entities.Find(filterDefinition)
                            //.SortBy(x => x.CreatedDateTime)
                            .Skip(NumericUtilities.GetPageSkip(propertyOwnerFilter.PageItems, propertyOwnerFilter.PageNo))
                            .Limit(propertyOwnerFilter.PageItems)
                            .ToListAsync();

            //var events = await _eventInstances.FindAsync(filter);

            return auditEvents;
        }

        /// <summary>
        /// Returns MongoDB filter definition for AuditEventFilter       
        /// </summary>
        /// <param name="auditEventFilter"></param>
        /// <returns></returns>
        private static FilterDefinition<PropertyOwner> GetFilterDefinition(PropertyOwnerFilter propertyOwnerFilter)
        {
            // Set date range filter
            //var filterDefinition = Builders<AuditEvent>.Filter.Gte(x => x.CreatedDateTime, auditEventFilter.StartCreatedDateTime.UtcDateTime);
            //filterDefinition = filterDefinition & Builders<AuditEvent>.Filter.Lte(x => x.CreatedDateTime, auditEventFilter.EndCreatedDateTime.UtcDateTime);
            var filterDefinition = Builders<PropertyOwner>.Filter.Empty;

            // Filter on free format text
            if (!String.IsNullOrEmpty(propertyOwnerFilter.Search))
            {
                filterDefinition = filterDefinition & Builders<PropertyOwner>.Filter.StringIn(x => x.Address.ToSummary(), propertyOwnerFilter.Search);
            }

            //// Filter property groups
            //if (propertyFilter.PropertyGroupIds != null && propertyFilter.PropertyGroupIds.Any())
            //{
            //    filterDefinition = filterDefinition & Builders<Property>.Filter.In(x => x.GroupId, propertyFilter.PropertyGroupIds.ToArray());
            //}

            //// Filter property owners
            //if (propertyFilter.PropertyOwnerIds != null && propertyFilter.PropertyOwnerIds.Any())
            //{
            //    filterDefinition = filterDefinition & Builders<Property>.Filter.In(x => x.OwnerId, propertyFilter.PropertyOwnerIds.ToArray());
            //}

            //var sort = Builders<EventInstance>.Sort.Ascending(x => x.CreatedDateTime);

            return filterDefinition;
        }
    }
}
