using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFUtilities.Utilities;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBAuditEventService : MongoDBBaseService<AuditEvent>, IAuditEventService
    {
        public MongoDBAuditEventService(IDatabaseConfig databaseConfig) : base(databaseConfig, "audit_events")
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


        public Task<AuditEvent?> GetByIdAsync(string id)
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

        public async Task<List<AuditEvent>> GetByFilterAsync(AuditEventFilter auditEventFilter)
        {
            // Get filter definition
            var filterDefinition = GetFilterDefinition(auditEventFilter);

            // Get filtered events page
            var auditEvents = await _entities.Find(filterDefinition)
                            .SortBy(x => x.CreatedDateTime)
                            .Skip(NumericUtilities.GetPageSkip(auditEventFilter.PageItems, auditEventFilter.PageNo))
                            .Limit(auditEventFilter.PageItems)
                            .ToListAsync();

            //var events = await _eventInstances.FindAsync(filter);

            return auditEvents;
        }

        /// <summary>
        /// Returns MongoDB filter definition for AuditEventFilter       
        /// </summary>
        /// <param name="auditEventFilter"></param>
        /// <returns></returns>
        private static FilterDefinition<AuditEvent> GetFilterDefinition(AuditEventFilter auditEventFilter)
        {
            // Set date range filter
            var filterDefinition = Builders<AuditEvent>.Filter.Gte(x => x.CreatedDateTime, auditEventFilter.StartCreatedDateTime.UtcDateTime);
            filterDefinition = filterDefinition & Builders<AuditEvent>.Filter.Lte(x => x.CreatedDateTime, auditEventFilter.EndCreatedDateTime.UtcDateTime);

            // Filter event types
            if (auditEventFilter.AuditEventTypeIds != null && auditEventFilter.AuditEventTypeIds.Any())
            {
                filterDefinition = filterDefinition & Builders<AuditEvent>.Filter.In(x => x.EventTypeId, auditEventFilter.AuditEventTypeIds.ToArray());
            }

            //var sort = Builders<EventInstance>.Sort.Ascending(x => x.CreatedDateTime);

            return filterDefinition;
        }

    }
}
