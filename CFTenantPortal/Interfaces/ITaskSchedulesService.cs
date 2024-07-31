using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface ITaskSchedulesService
    {        
        TaskSchedule GetNextOverdueTask();
    }
}
