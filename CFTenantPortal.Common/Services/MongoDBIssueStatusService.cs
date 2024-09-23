using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBIssueStatusService : MongoDBBaseService<IssueStatus>, IIssueStatusService
    {
        public MongoDBIssueStatusService(IDatabaseConfig databaseConfig) : base(databaseConfig, "issue_statuses")
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


        public Task<IssueStatus?> GetByIdAsync(string id)
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

        //public Task<List<IssueStatus>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<IssueStatus> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        //public Task Update(IssueStatus issueStatus)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<IssueStatus> GetAllInternal()
        //{
        //    var issueStatuses = new List<IssueStatus>();

        //    issueStatuses.Add(new IssueStatus()
        //    {
        //        Id = "1",
        //        Description = "New"
        //    });

        //    issueStatuses.Add(new IssueStatus()
        //    {
        //        Id = "2",
        //        Description = "Processing"
        //    });

        //    issueStatuses.Add(new IssueStatus()
        //    {
        //        Id = "3",
        //        Description = "Completed"
        //    });

        //    issueStatuses.Add(new IssueStatus()
        //    {
        //        Id = "4",
        //        Description = "Cancelled"
        //    });

        //    return issueStatuses;
        //}
    }
}
