using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class IssueTypeSeed1 : IEntityList<IssueType>
    {
        public Task<List<IssueType>> ReadAllAsync()
        {
            var entities = new List<IssueType>();

            entities.Add(new IssueType()
            {
                Description = "Issue type 1"
            });

            entities.Add(new IssueType()
            {             
                Description = "Issue type 2"
            });

            entities.Add(new IssueType()
            {             
                Description = "Issue type 3"
            });

            entities.Add(new IssueType()
            {             
                Description = "Issue type 4"
            });

            entities.Add(new IssueType()
            {            
                Description = "Issue type 5"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<IssueType> entities)
        {
            return Task.CompletedTask;
        }
    }
}
