using CFTenantPortal.Data;
using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using CFTenantPortal.SystemTasks;
using CFUtilities.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Set data location. For testing then we might want to set it to in-memory data
var dataLocationType = DataLocationTypes.MongoDB;

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Configure database config from appSettings.json
builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(nameof(DatabaseConfig)));
builder.Services.AddSingleton<IDatabaseConfig>(sp => sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);

// Set database admin service
builder.Services.AddScoped<IDatabaseAdminService, DatabaseAdminService>();

// Set data seedservice
builder.Services.AddScoped<ISharedSeedDataService, SharedSeedDataService>();

switch(dataLocationType)
{
    case DataLocationTypes.MongoDB:
        // Set database configurer
        builder.Services.AddScoped<ISharedDatabaseConfigurer, MongoDBSharedConfigurer>();

        // Add services for data
        builder.Services.AddScoped<IAccountTransactionService, MongoDBAccountTransactionService>();
        builder.Services.AddScoped<IAccountTransactionTypeService, MongoDBAccountTransactionTypeService>();
        builder.Services.AddScoped<IAuditEventService, MongoDBAuditEventService>();
        builder.Services.AddScoped<IAuditEventTypeService, MongoDBAuditEventTypeService>();
        builder.Services.AddScoped<IDocumentService, MongoDBDocumentService>();
        builder.Services.AddScoped<IEmployeeService, MongoDBEmployeeService>();
        builder.Services.AddScoped<IIssueService, MongoDBIssueService>();
        builder.Services.AddScoped<IIssueStatusService, MongoDBIssueStatusService>();
        builder.Services.AddScoped<IIssueTypeService, MongoDBIssueTypeService>();
        builder.Services.AddScoped<IMessageService, MongoDBMessageService>();
        builder.Services.AddScoped<IMessageTemplateService, MongoDBMessageTemplateService>();
        builder.Services.AddScoped<IMessageTypeService, MongoDBMessageTypeService>();
        builder.Services.AddScoped<IPropertyFeatureTypeService, MongoDBPropertyFeatureTypeService>();
        builder.Services.AddScoped<IPropertyGroupService, MongoDBPropertyGroupService>();
        builder.Services.AddScoped<IPropertyOwnerService, MongoDBPropertyOwnerService>();
        builder.Services.AddScoped<IPropertyService, MongoDBPropertyService>();
        builder.Services.AddScoped<ISystemValueTypeService, MongoDBSystemValueTypeService>();
        builder.Services.AddScoped<IUserService, MongoDBUserService>();
        break;
}

// Set system tasks
builder.Services.AddSingleton<ISystemTasks>((scope) =>
{
    var systemTasks = new List<ISystemTask>()
    {
        new ArchiveTask(new SystemTaskSchedule()
        {
                ExecuteFrequency = TimeSpan.FromHours(24)
        }),
            new MessageSendTask(new SystemTaskSchedule()
        {
                ExecuteFrequency = TimeSpan.FromMinutes(15)
        }),
                new MonitoringTask(new SystemTaskSchedule()
        {
                ExecuteFrequency = TimeSpan.FromMinutes(15)
        })
    };

    systemTasks.RemoveAll(st => st == null);   // Allow configurable list of tasks (E.g. Not used in test mode)

    // Set next execute time
    systemTasks.ForEach(st => st.Schedule.NextExecuteTime =
        st.Schedule.CalculateNextFutureExecuteTime(DateTimeUtilities.GetStartOfDay(DateTimeOffset.UtcNow), DateTimeOffset.UtcNow));

    // Set last execute time
    systemTasks.ForEach(st => st.Schedule.LastExecuteTime =
        st.Schedule.NextExecuteTime.Subtract(st.Schedule.ExecuteFrequency));
    return new SystemTasks(systemTasks, 5);
});

// Add service for executing system tasks
builder.Services.AddHostedService<SystemTaskBackgroundService>();

// Add tasks
//builder.Services.RegisterAllTypes<ITaskObject>(new[] { typeof(Program).Assembly }, ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


// Initialise
using (var scope = app.Services.CreateScope())
{    
    // Initialise shared DB
    var databaseAdminService = scope.ServiceProvider.GetRequiredService<IDatabaseAdminService>();
    //await databaseAdminService.DeleteSharedData();
    await databaseAdminService.InitialiseSharedAsync();    
    //await databaseAdminService.LoadSharedData(1);
    
    // Execute system tasks that must execute before HTTP pipeline runs
    var systemTaskBackgroundService = scope.ServiceProvider.GetServices<IHostedService>()
                        .OfType<SystemTaskBackgroundService>().FirstOrDefault();
    systemTaskBackgroundService.ExecuteStartupTasks(new CancellationTokenSource().Token).Wait();
}

app.Run();

