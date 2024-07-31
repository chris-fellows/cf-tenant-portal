using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    public class TaskSchedulesService : ITaskSchedulesService
    {
        private readonly List<TaskSchedule> _taskSchedules;        

        public TaskSchedulesService(List<TaskSchedule> taskSchedules)
        {            
            _taskSchedules = taskSchedules;
        }

        public TaskSchedule GetNextOverdueTask()
        {
            return null;
        }
    }
}
