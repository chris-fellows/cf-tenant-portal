using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAll();

        Task<Employee> GetById(string id);

        Task Update(Employee employee);
    }
}
