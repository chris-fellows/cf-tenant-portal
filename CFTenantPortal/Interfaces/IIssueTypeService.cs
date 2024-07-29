using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IIssueTypeService
    {
        Task<List<IssueType>> GetAll();

        Task<IssueType> GetById(string id);

        Task Update(IssueType issueType);
    }
}
