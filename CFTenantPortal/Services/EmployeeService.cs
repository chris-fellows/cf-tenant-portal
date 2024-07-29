using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Net;

namespace CFTenantPortal.Services
{
    public class EmployeeService : IEmployeeService
    {
        public Task<List<Employee>> GetAll()
        {
            return Task.FromResult(GetAllInternal());
        }

        public Task<Employee> GetById(string id)
        {
            return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        }

        public Task Update(Employee employee)
        {
            return Task.CompletedTask;
        }

        private List<Employee> GetAllInternal()
        {
            var employees = new List<Employee>();

            employees.Add(new Employee()
            {
                Id = "1",
                Name = "Employee 1",
                Active = true,
                Email = "employee1@test.com",
                Password = "xxx"
            });

            employees.Add(new Employee()
            {
                Id = "2",
                Name = "Employee 2",
                Active = true,
                Email = "employee2@test.com",
                Password = "xxx"
            });

            employees.Add(new Employee()
            {
                Id = "3",
                Name = "Employee 3",
                Active = true,
                Email = "employee3@test.com",
                Password = "xxx"
            });

            return employees;
        }
    }
}
