using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class UserSeed1 : IEntityList<User>
    {
        public Task<List<User>> ReadAllAsync()
        {
            var entities = new List<User>();

            //entities.Add(new User()
            //{
            //    Active = true,
            //    Email = "chrissmith@domain1.com",
            //    Name = "Chris Smith"
            //});

            //entities.Add(new User()
            //{
            //    Active = true,
            //    Email = "bobsmith@domain1.com",
            //    Name = "Bob Smith"
            //});

            //entities.Add(new User()
            //{
            //    Active = false,
            //    Email = "alansmith@domain1.com",
            //    Name = "Alan Smith"
            //});

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<User> entities)
        {
            return Task.CompletedTask;
        }
    }
}
