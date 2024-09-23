using CFTenantPortal.Enums;
using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface ISystemValueTypeService : IEntityWithIDService<SystemValueType, string>
    {
        //Task<List<SystemValueType>> GetAll();

        //Task<SystemValueType> GetById(string id);

        Task<SystemValueType> GetByEnum(SystemValueTypes systemValueType);
    }
}
