using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IIssueService : IEntityWithIDService<Issue, string>
    {
        //Task<List<Issue>> GetAll();

        //Task<Issue> GetById(string id);

        Task<List<Issue>> GetByProperty(string propertyId);

        Task<List<Issue>> GetByPropertyGroup(string propertyGroupId);

        Task<List<Issue>> GetByIssueType(string issueTypeId);

        Task<List<Issue>> GetByFilterAsync(IssueFilter issueFilter);


        //Task Update(Issue issue);
    }
}
