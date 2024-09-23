using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class IssueStatusSeed1 : IEntityList<IssueStatus>
    {
        public Task<List<IssueStatus>> ReadAllAsync()
        {
            var entities = new List<IssueStatus>();

            entities.Add(new IssueStatus()
            {                
                Description = "New"
            });

            entities.Add(new IssueStatus()
            {             
                Description = "Processing"
            });

            entities.Add(new IssueStatus()
            {             
                Description = "Completed"
            });

            entities.Add(new IssueStatus()
            {             
                Description = "Cancelled"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<IssueStatus> entities)
        {
            return Task.CompletedTask;
        }
    }
}
