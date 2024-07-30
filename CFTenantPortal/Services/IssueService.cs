using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class IssueService : IIssueService
    {
        public Task<List<Issue>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<Issue> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(i => i.Id == id));
        }

        public Task<List<Issue>> GetByProperty(string propertyId)
        {
            var issues = GetAllInternal().Where(i => i.PropertyId == propertyId).ToList();
            return Task.FromResult(issues);
        }

        public Task<List<Issue>> GetByPropertyGroup(string propertyGroupId)
        {
            var issues = GetAllInternal().Where(i => i.PropertyGroupId == propertyGroupId).ToList();
            return Task.FromResult(issues);
        }

        public Task<List<Issue>> GetByIssueType(string issueTypeId)
        {
            var issues = GetAllInternal().Where(i => i.TypeId == issueTypeId).ToList();
            return Task.FromResult(issues);
        }

        public Task Update(Issue issue)
        {
            return Task.CompletedTask;
        }

        private List<Issue> GetAllInternal()
        {
            var issues = new List<Issue>();

            issues.Add(new Issue()
            {
                Id = "1",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 1",
                CreatedDateTime = DateTime.Now,
                TypeId = "1",
                PropertyId = "1",
                StatusId = "1"
            });

            issues.Add(new Issue()
            {
                Id = "2",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 2",
                CreatedDateTime = DateTime.Now,
                TypeId = "1",
                PropertyId = "1",
                StatusId = "1"
            });

            issues.Add(new Issue()
            {
                Id = "3",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 3",
                CreatedDateTime = DateTime.Now,
                TypeId = "2",
                PropertyId = "2",
                StatusId = "1"
            });

            issues.Add(new Issue()
            {
                Id = "4",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 4",
                CreatedDateTime = DateTime.Now,
                TypeId = "2",
                PropertyId = "3",
                StatusId = "1"
            });

            issues.Add(new Issue()
            {
                Id = "5",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 5 for property group",
                CreatedDateTime = DateTime.Now,
                TypeId = "2",
                PropertyGroupId = "1",
                StatusId = "2"
            });

            issues.Add(new Issue()
            {
                Id = "6",
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 6 for property group",
                CreatedDateTime = DateTime.Now,
                TypeId = "2",
                PropertyGroupId = "2",
                StatusId = "2"
            });

            return issues;
        }
    }
}
