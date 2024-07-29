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
            return null;
        }

        public Task<List<Issue>> GetByProperty(string propertyId)
        {
            var issues = GetAllInternal().Where(i => i.PropertyId == propertyId).ToList();
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
                Description = "Issue 1",
                CreatedDateTime = DateTime.Now,
                IssueTypeId = "1",
                PropertyId = "1",
                Status = Enums.IssueStatuses.New
            });

            issues.Add(new Issue()
            {
                Id = "2",
                Description = "Issue 2",
                CreatedDateTime = DateTime.Now,
                IssueTypeId = "1",
                PropertyId = "1",
                Status = Enums.IssueStatuses.New
            });

            issues.Add(new Issue()
            {
                Id = "3",
                Description = "Issue 3",
                CreatedDateTime = DateTime.Now,
                IssueTypeId = "2",
                PropertyId = "2",
                Status = Enums.IssueStatuses.New
            });

            issues.Add(new Issue()
            {
                Id = "3",
                Description = "Issue 4",
                CreatedDateTime = DateTime.Now,
                IssueTypeId = "2",
                PropertyId = "3",
                Status = Enums.IssueStatuses.New
            });

            return issues;
        }
    }
}
