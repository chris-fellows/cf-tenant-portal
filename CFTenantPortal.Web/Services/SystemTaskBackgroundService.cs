using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.SystemTasks;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace CFTenantPortal.Services
{
    /// <summary>
    /// Executes tasks in background
    /// </summary>
    public class SystemTaskBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISystemTasks _systemTasks;

        private class TaskInfo
        {
            public Task Task { get; set; }

            public ISystemTask SystemTask { get; set; }
        }

        public SystemTaskBackgroundService(IServiceProvider serviceProvider,
                                            ISystemTasks systemTasks)
        {
            _serviceProvider = serviceProvider;
            _systemTasks = systemTasks;
        }

        /// <summary>
        /// Handles completed tasks        
        /// </summary>
        /// <param name="taskInfos"></param>
        /// <returns></returns>
        private static void HandleCompletedTasks(List<TaskInfo> taskInfos)
        {
            var completedTasks = taskInfos.Where(ti => ti.Task.IsCompleted).ToList();

            while (completedTasks.Any())
            {
                var completedTask = completedTasks.First();
                completedTasks.Remove(completedTask);
                taskInfos.Remove(completedTask);
                if (completedTask.Task.Exception != null)   // Error
                {
                    System.Diagnostics.Debug.WriteLine($"System task {completedTask.SystemTask.Name} error: {completedTask.Task.Exception.Message}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"System task {completedTask.SystemTask.Name} completed");
                }
            }
        }

        /// <summary>
        /// Runs tasks that must run at startup before HTTP pipeline
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteStartupTasks(CancellationToken cancellationToken)
        {
            // Get system tasks to run
            var systemTasks = _systemTasks.AllTasks.Where(st => st.IsRunOnStartup).ToList();

            // Start each task
            List<TaskInfo> activeTaskInfos = new List<TaskInfo>();
            foreach (var systemTask in systemTasks)
            {
                // Wait if too many active tasks
                while (activeTaskInfos.Count >= _systemTasks.MaxConcurrentTasks)
                {
                    HandleCompletedTasks(activeTaskInfos);
                    await Task.Delay(500);
                }

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                var taskInfo = new TaskInfo()
                {
                    Task = ExecuteSystemTaskAsync(cancellationTokenSource.Token, new Dictionary<string, object>(), systemTask),
                    SystemTask = systemTask
                };
                activeTaskInfos.Add(taskInfo);

                HandleCompletedTasks(activeTaskInfos);
            }

            // Wait for tasks to complete
            while (activeTaskInfos.Any())
            {
                HandleCompletedTasks(activeTaskInfos);
                await Task.Delay(500);
            }
        }

        /// <summary>
        /// Executes overdue scheduled tasks
        /// </summary>
        /// <param name="activeTaskInfos"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private void ExecuteScheduledTasks(List<TaskInfo> activeTaskInfos, CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested &&
                activeTaskInfos.Count < _systemTasks.MaxConcurrentTasks)
            {
                // Get overdue tasks
                var overdueTasks = _systemTasks.OverdueTasks.Where(st => !activeTaskInfos.Any(ti => ti.SystemTask.Name == st.Name)).ToList();

                // Start each overdue task, abort if max tasks executing
                while (overdueTasks.Any() &&
                    activeTaskInfos.Count < _systemTasks.MaxConcurrentTasks)
                {
                    // Get next task
                    var systemTask = overdueTasks.First();
                    overdueTasks.Remove(systemTask);

                    // Start task
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    var taskInfo = new TaskInfo()
                    {
                        Task = ExecuteSystemTaskAsync(cancellationTokenSource.Token, new Dictionary<string, object>(), systemTask),
                        SystemTask = systemTask
                    };
                    activeTaskInfos.Add(taskInfo);
                }
            }
        }

        /// <summary>
        /// Executes overdue requested tasks
        /// </summary>
        /// <returns></returns>
        private void ExecuteRequestedTasks(List<TaskInfo> activeTaskInfos, CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested &&
                activeTaskInfos.Count < _systemTasks.MaxConcurrentTasks)
            {
                // Get system task to execute, not already executing
                var overdueTaskRequests = _systemTasks.OverdueRequests.Where(st => !activeTaskInfos.Any(ti => ti.SystemTask.Name == st.Name)).ToList();

                // Execute system task
                while (overdueTaskRequests.Any() &&
                    activeTaskInfos.Count < _systemTasks.MaxConcurrentTasks)
                {
                    // Get next task request
                    var systemTaskRequest = overdueTaskRequests.First();
                    overdueTaskRequests.Remove(systemTaskRequest);
                    var systemTask = _systemTasks.AllTasks.First(st => st.Name == systemTaskRequest.Name);

                    // Start task
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    var taskInfo = new TaskInfo()
                    {
                        Task = ExecuteSystemTaskAsync(cancellationTokenSource.Token, new Dictionary<string, object>(), systemTask),
                        SystemTask = systemTask
                    };
                    activeTaskInfos.Add(taskInfo);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<TaskInfo> activeTaskInfos = new List<TaskInfo>();

            // Process system tasks until stopped. Ensure that active tasks are completed before leaving
            while (!stoppingToken.IsCancellationRequested ||
                    activeTaskInfos.Any())
            {
                // Check completed tasks
                HandleCompletedTasks(activeTaskInfos);

                // Execute scheduled tasks                
                ExecuteScheduledTasks(activeTaskInfos, stoppingToken);

                // Execute requested tasks                
                ExecuteRequestedTasks(activeTaskInfos, stoppingToken);

                // Wait before next loop
                await Task.Delay(activeTaskInfos.Any() ||
                            _systemTasks.OverdueTasks.Any() ||
                            _systemTasks.OverdueRequests.Any() ? 500 : 10000);
            }
        }

        /// <summary>
        /// Executes system task asynchronously
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <param name="systemTask"></param>
        /// <returns></returns>
        protected Task ExecuteSystemTaskAsync(CancellationToken cancellationToken, Dictionary<string, object> parameters, ISystemTask systemTask)
        {
            return Task.Factory.StartNew(() =>
            {
                // Set last execute time to scheduled time
                systemTask.Schedule.LastExecuteTime = systemTask.Schedule.NextExecuteTime;

                // Set next execute time based on last execute time
                systemTask.Schedule.NextExecuteTime = systemTask.Schedule.CalculateNextFutureExecuteTime(systemTask.Schedule.LastExecuteTime, systemTask.Schedule.LastExecuteTime);

                System.Diagnostics.Debug.WriteLine($"{DateTimeOffset.UtcNow.ToString()} : Executing system task {systemTask.Name} (Execute Time={systemTask.Schedule.LastExecuteTime}, Next Execute Time={systemTask.Schedule.NextExecuteTime}");

                // Create task scope
                using (var scope = _serviceProvider.CreateScope())
                {
                    systemTask.ExecuteAsync(cancellationToken, scope.ServiceProvider, parameters).Wait();
                }
            });
        }


        //private IServiceProvider _serviceProvider;

        //private class TaskInfo
        //{
        //    public TaskSchedule TaskSchedule { get; set; }

        //    public Task Task { get; set; }
        //}

        //public TaskBackgroundService(IServiceProvider serviceProvider)
        //{
        //    _serviceProvider = serviceProvider;
        //}

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    Console.WriteLine("Task background service started");

        //    // Get task schedules
        //    ITaskSchedulesService taskSchedulesService = null;
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        taskSchedulesService = scope.ServiceProvider.GetService<ITaskSchedulesService>();                            
        //    }

        //    // Execute until stop requested
        //    var taskInfos = new List<TaskInfo>();
        //    while (!stoppingToken.IsCancellationRequested || taskInfos.Any())
        //    {
        //        Console.WriteLine("Task background service active");
        //        CheckTasksCompleted(taskInfos);

        //        // Get task
        //        var taskSchedule = taskSchedulesService.GetNextOverdueTask();
        //        if (taskSchedule != null)
        //        {
        //            var taskInfo = new TaskInfo() { TaskSchedule = taskSchedule };                    
        //            taskInfo.Task = ExecuteTaskAsync(stoppingToken, null, _serviceProvider, taskSchedule);
        //            taskInfos.Add(taskInfo);
        //        }

        //        await Task.Delay(30000, stoppingToken);
        //        CheckTasksCompleted(taskInfos);
        //    }

        //    Console.WriteLine("Task background service stopped");
        //}

        ///// <summary>
        ///// Checks completed tasks
        ///// </summary>
        ///// <param name="taskInfos"></param>
        //private void CheckTasksCompleted(List<TaskInfo> taskInfos)
        //{
        //    var taskInfosCompleted = taskInfos.Where(ti => ti.Task.IsCompleted).ToList();

        //    while (taskInfosCompleted.Any())
        //    {
        //        var taskInfo = taskInfosCompleted.First();
        //        taskInfosCompleted.Remove(taskInfo);
        //        taskInfos.Remove(taskInfo);
        //    }
        //}

        ///// <summary>
        ///// Executes the task asynchronously
        ///// </summary>
        ///// <param name="serviceProvider"></param>
        ///// <param name="taskSchedule"></param>
        ///// <returns></returns>
        //private Task ExecuteTaskAsync(CancellationToken cancellationToken, 
        //                            Dictionary<string, object> parameters,
        //                            IServiceProvider serviceProvider, 
        //                            TaskSchedule taskSchedule)
        //{
        //    var task = Task.Factory.StartNew(() =>
        //    {
        //        using (var scope = serviceProvider.CreateScope())
        //        {
        //            var taskObject = scope.ServiceProvider.GetServices<ISystemTask>().FirstOrDefault(t => t.Name == taskSchedule.TaskId);

        //            taskObject.Execute(cancellationToken, parameters, scope.ServiceProvider);
        //        }
        //    });

        //    return task;

        //}
    }
}
