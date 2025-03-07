using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class EmployeeSeed1 : IEntityReader<Employee>
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

            /*
            // Store encrypted password & salt            
            foreach(var employee in entities)
            {
                var data = passwordService.Encrypt(employee.Password);
                employee.Password = data[0];
                employee.Salt = data[1];
            }
            */

            return Task.FromResult(entities);
        }
    }
}
