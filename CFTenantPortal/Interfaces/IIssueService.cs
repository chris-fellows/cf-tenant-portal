using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IIssueService
    {
        Task<List<Issue>> GetAll();

        Task<Issue> GetById(string id);

        Task<List<Issue>> GetByProperty(string propertyId);

        Task Update(Issue issue);
    }
}
