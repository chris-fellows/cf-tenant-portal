using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace CFTenantPortal.Services
{
    /// <summary>
    /// Executes tasks in background
    /// </summary>
    public class TaskBackgroundService : BackgroundService
    {
        private IServiceProvider _serviceProvider;

        private class TaskInfo
        {
            public TaskSchedule TaskSchedule { get; set; }

            public Task Task { get; set; }
        }

        public TaskBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Task background service started");

            // Get task schedules
            ITaskSchedulesService taskSchedulesService = null;
            using (var scope = _serviceProvider.CreateScope())
            {
                taskSchedulesService = scope.ServiceProvider.GetService<ITaskSchedulesService>();                            
            }

            // Execute until stop requested
            var taskInfos = new List<TaskInfo>();
            while (!stoppingToken.IsCancellationRequested || taskInfos.Any())
            {
                Console.WriteLine("Task background service active");
                CheckTasksCompleted(taskInfos);

                // Get task
                var taskSchedule = taskSchedulesService.GetNextOverdueTask();
                if (taskSchedule != null)
                {
                    var taskInfo = new TaskInfo() { TaskSchedule = taskSchedule };                    
                    taskInfo.Task = ExecuteTaskAsync(stoppingToken, null, _serviceProvider, taskSchedule);
                    taskInfos.Add(taskInfo);
                }

                await Task.Delay(30000, stoppingToken);
                CheckTasksCompleted(taskInfos);
            }

            Console.WriteLine("Task background service stopped");
        }

        /// <summary>
        /// Checks completed tasks
        /// </summary>
        /// <param name="taskInfos"></param>
        private void CheckTasksCompleted(List<TaskInfo> taskInfos)
        {
            var taskInfosCompleted = taskInfos.Where(ti => ti.Task.IsCompleted).ToList();

            while (taskInfosCompleted.Any())
            {
                var taskInfo = taskInfosCompleted.First();
                taskInfosCompleted.Remove(taskInfo);
                taskInfos.Remove(taskInfo);
            }
        }

        /// <summary>
        /// Executes the task asynchronously
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="taskSchedule"></param>
        /// <returns></returns>
        private Task ExecuteTaskAsync(CancellationToken cancellationToken, 
                                    Dictionary<string, object> parameters,
                                    IServiceProvider serviceProvider, 
                                    TaskSchedule taskSchedule)
        {
            var task = Task.Factory.StartNew(() =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var taskObject = scope.ServiceProvider.GetServices<ITaskObject>().FirstOrDefault(t => t.Id == taskSchedule.TaskId);

                    taskObject.Execute(cancellationToken, parameters, scope.ServiceProvider);
                }
            });

            return task;

        }
    }
}
