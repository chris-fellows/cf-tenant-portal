using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IIssueStatusService
    {
        Task<List<IssueStatus>> GetAll();

        Task<IssueStatus> GetById(string id);

        Task Update(IssueStatus issueStatus);
    }
}
