using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFUtilities.Utilities;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBPropertyGroupService : MongoDBBaseService<PropertyGroup>, IPropertyGroupService
    {
        public MongoDBPropertyGroupService(IDatabaseConfig databaseConfig) : base(databaseConfig, "property_groups")
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


        public Task<PropertyGroup?> GetByIdAsync(string id)
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
        public async Task<List<PropertyGroup>> GetByFilterAsync(PropertyGroupFilter propertyGroupFilter)
        {
            // Get filter definition
            var filterDefinition = GetFilterDefinition(propertyGroupFilter);

            // Get filtered property owners page
            var auditEvents = await _entities.Find(filterDefinition)
                            //.SortBy(x => x.CreatedDateTime)
                            .Skip(NumericUtilities.GetPageSkip(propertyGroupFilter.PageItems, propertyGroupFilter.PageNo))
                            .Limit(propertyGroupFilter.PageItems)
                            .ToListAsync();

            //var events = await _eventInstances.FindAsync(filter);

            return auditEvents;
        }

        /// <summary>
        /// Returns MongoDB filter definition for AuditEventFilter       
        /// </summary>
        /// <param name="auditEventFilter"></param>
        /// <returns></returns>
        private static FilterDefinition<PropertyGroup> GetFilterDefinition(PropertyGroupFilter propertyGroupFilter)
        {
            // Set date range filter
            //var filterDefinition = Builders<AuditEvent>.Filter.Gte(x => x.CreatedDateTime, auditEventFilter.StartCreatedDateTime.UtcDateTime);
            //filterDefinition = filterDefinition & Builders<AuditEvent>.Filter.Lte(x => x.CreatedDateTime, auditEventFilter.EndCreatedDateTime.UtcDateTime);
            var filterDefinition = Builders<PropertyGroup>.Filter.Empty;

            // Filter on free format text
            if (!String.IsNullOrEmpty(propertyGroupFilter.Search))
            {
                filterDefinition = filterDefinition & Builders<PropertyGroup>.Filter.StringIn(x => x.Name, propertyGroupFilter.Search);
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
