using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class EmployeeSeed1 : IEntityList<Employee>
    {
        public Task<List<Employee>> ReadAllAsync()
        {
            var entities = new List<Employee>();

            entities.Add(new Employee()
            {                
                Name = "Employee 1",
                Active = true,
                Email = "employee1@test.com",
                Password = "xxx"
            });

            entities.Add(new Employee()
            {             
                Name = "Employee 2",
                Active = true,
                Email = "employee2@test.com",
                Password = "xxx"
            });

            entities.Add(new Employee()
            {             
                Name = "Employee 3",
                Active = true,
                Email = "employee3@test.com",
                Password = "xxx"
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<Employee> entities)
        {
            return Task.CompletedTask;
        }
    }
}
