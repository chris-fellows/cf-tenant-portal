using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class IssueStatusService : IIssueStatusService
    {
        public Task<List<IssueStatus>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<IssueStatus> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        }

        public Task Update(IssueStatus issueStatus)
        {
            return Task.CompletedTask;
        }

        private List<IssueStatus> GetAllInternal()
        {
            var issueStatuses = new List<IssueStatus>();

            issueStatuses.Add(new IssueStatus()
            {
                Id = "1",
                Description = "New"
            });

            issueStatuses.Add(new IssueStatus()
            {
                Id = "2",
                Description = "Processing"
            });

            issueStatuses.Add(new IssueStatus()
            {
                Id = "3",
                Description = "Completed"
            });

            issueStatuses.Add(new IssueStatus()
            {
                Id = "4",
                Description = "Cancelled"
            });

            return issueStatuses;
        }
    }
}
