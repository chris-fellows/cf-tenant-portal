using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBIssueTypeService : MongoDBBaseService<IssueType>, IIssueTypeService
    {
        public MongoDBIssueTypeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "issue_types")
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


        public Task<IssueType?> GetByIdAsync(string id)
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

        //public Task<List<IssueType>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<IssueType> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        //public Task Update(IssueType issueType)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<IssueType> GetAllInternal()
        //{
        //    var issueTypes = new List<IssueType>();

        //    issueTypes.Add(new IssueType()
        //    {
        //        Id = "1",                
        //        Description = "Issue type 1"
        //    });

        //    issueTypes.Add(new IssueType()
        //    {
        //        Id = "2",
        //        Description = "Issue type 2"
        //    });

        //    issueTypes.Add(new IssueType()
        //    {
        //        Id = "3",
        //        Description = "Issue type 3"
        //    });

        //    issueTypes.Add(new IssueType()
        //    {
        //        Id = "4",
        //        Description = "Issue type 4"
        //    });

        //    issueTypes.Add(new IssueType()
        //    {
        //        Id = "5",
        //        Description = "Issue type 5"
        //    });

        //    return issueTypes;
        //}
    }
}
