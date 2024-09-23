using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System.Net;

namespace CFTenantPortal.Services
{
    public class MongoDBEmployeeService : MongoDBBaseService<Employee>, IEmployeeService
    {
        public MongoDBEmployeeService(IDatabaseConfig databaseConfig) : base(databaseConfig, "employees")
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


        public Task<Employee?> GetByIdAsync(string id)
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

        //public Task<List<Employee>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<Employee> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(e => e.Id == id));
        //}

        //public Task Update(Employee employee)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<Employee> GetAllInternal()
        //{
        //    var employees = new List<Employee>();

        //    employees.Add(new Employee()
        //    {
        //        Id = "1",
        //        Name = "Employee 1",
        //        Active = true,
        //        Email = "employee1@test.com",
        //        Password = "xxx"
        //    });

        //    employees.Add(new Employee()
        //    {
        //        Id = "2",
        //        Name = "Employee 2",
        //        Active = true,
        //        Email = "employee2@test.com",
        //        Password = "xxx"
        //    });

        //    employees.Add(new Employee()
        //    {
        //        Id = "3",
        //        Name = "Employee 3",
        //        Active = true,
        //        Email = "employee3@test.com",
        //        Password = "xxx"
        //    });

        //    return employees;
        //}
    }
}
