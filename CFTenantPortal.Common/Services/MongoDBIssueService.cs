using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFUtilities.Utilities;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBIssueService : MongoDBBaseService<Issue>, IIssueService
    {
        public MongoDBIssueService(IDatabaseConfig databaseConfig) : base(databaseConfig, "issues")
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


        public Task<Issue?> GetByIdAsync(string id)
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

        //public Task<List<Issue>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<Issue> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(i => i.Id == id));
        //}

        public Task<List<Issue>> GetByProperty(string propertyId)
        {
            var issues = GetAll().Where(i => i.PropertyId == propertyId).ToList();
            return Task.FromResult(issues);
        }

        public Task<List<Issue>> GetByPropertyGroup(string propertyGroupId)
        {
            var issues = GetAll().Where(i => i.PropertyGroupId == propertyGroupId).ToList();
            return Task.FromResult(issues);
        }

        public Task<List<Issue>> GetByIssueType(string issueTypeId)
        {
            var issues = GetAll().Where(i => i.TypeId == issueTypeId).ToList();
            return Task.FromResult(issues);
        }

        public async Task<List<Issue>> GetByFilterAsync(IssueFilter issueFilter)
        {
            // Get filter definition
            var filterDefinition = GetFilterDefinition(issueFilter);

            // Get filtered events page
            var auditEvents = await _entities.Find(filterDefinition)
                            .SortBy(x => x.CreatedDateTime)
                            .Skip(NumericUtilities.GetPageSkip(issueFilter.PageItems, issueFilter.PageNo))
                            .Limit(issueFilter.PageItems)
                            .ToListAsync();

            //var events = await _eventInstances.FindAsync(filter);

            return auditEvents;
        }

        /// <summary>
        /// Returns MongoDB filter definition for AuditEventFilter       
        /// </summary>
        /// <param name="auditEventFilter"></param>
        /// <returns></returns>
        private static FilterDefinition<Issue> GetFilterDefinition(IssueFilter issueFilter)
        {
            // Set date range filter
            //var filterDefinition = Builders<AuditEvent>.Filter.Gte(x => x.CreatedDateTime, auditEventFilter.StartCreatedDateTime.UtcDateTime);
            //filterDefinition = filterDefinition & Builders<AuditEvent>.Filter.Lte(x => x.CreatedDateTime, auditEventFilter.EndCreatedDateTime.UtcDateTime);
            var filterDefinition = Builders<Issue>.Filter.Empty;

            // Filter issue references
            if (issueFilter.References != null && issueFilter.References.Any())
            {
                filterDefinition = filterDefinition & Builders<Issue>.Filter.In(x => x.Reference, issueFilter.References.ToArray());
            }

            // Filter issue statuses
            if (issueFilter.IssueStatusIds != null && issueFilter.IssueStatusIds.Any())
            {
                filterDefinition = filterDefinition & Builders<Issue>.Filter.In(x => x.StatusId, issueFilter.IssueStatusIds.ToArray());
            }

            // Filter issue types
            if (issueFilter.IssueTypeIds != null && issueFilter.IssueTypeIds.Any())
            {
                filterDefinition = filterDefinition & Builders<Issue>.Filter.In(x => x.TypeId, issueFilter.IssueTypeIds.ToArray());
            }

            //var sort = Builders<EventInstance>.Sort.Ascending(x => x.CreatedDateTime);

            return filterDefinition;
        }

        //public Task Update(Issue issue)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<Issue> GetAllInternal()
        //{
        //    var issues = new List<Issue>();

        //    issues.Add(new Issue()
        //    {
        //        Id = "1",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 1",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "1",
        //        PropertyId = "1",
        //        StatusId = "1",
        //        DocumentIds = new List<string>() { "1", "2" }
        //    });

        //    issues.Add(new Issue()
        //    {
        //        Id = "2",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 2",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "1",
        //        PropertyId = "1",
        //        StatusId = "1"
        //    });

        //    issues.Add(new Issue()
        //    {
        //        Id = "3",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 3",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "2",
        //        PropertyId = "2",
        //        StatusId = "1",
        //        DocumentIds = new List<string>() { "3" }
        //    });

        //    issues.Add(new Issue()
        //    {
        //        Id = "4",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 4",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "2",
        //        PropertyId = "3",
        //        StatusId = "1"
        //    });

        //    issues.Add(new Issue()
        //    {
        //        Id = "5",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 5 for property group",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "2",
        //        PropertyGroupId = "1",
        //        StatusId = "2",
        //        DocumentIds = new List<string>() { "4" }
        //    });

        //    issues.Add(new Issue()
        //    {
        //        Id = "6",
        //        Reference = Guid.NewGuid().ToString(),
        //        Description = "Issue 6 for property group",
        //        CreatedDateTime = DateTime.Now,
        //        TypeId = "2",
        //        PropertyGroupId = "2",
        //        StatusId = "2"
        //    });

        //    return issues;
        //}
    }
}
