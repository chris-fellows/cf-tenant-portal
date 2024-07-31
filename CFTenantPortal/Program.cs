using CFTenantPortal.Data;
using CFTenantPortal.Extensions;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using CFTenantPortal.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add services for data
builder.Services.AddScoped<IAccountTransactionService, AccountTransactionService>();
builder.Services.AddScoped<IAccountTransactionTypeService, AccountTransactionTypeService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IIssueStatusService, IssueStatusService>();
builder.Services.AddScoped<IIssueTypeService, IssueTypeService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageTemplateService, MessageTemplateService>();
builder.Services.AddScoped<IMessageTypeService, MessageTypeService>();
builder.Services.AddScoped<IPropertyFeatureTypeService, PropertyFeatureTypeService>();
builder.Services.AddScoped<IPropertyGroupService, PropertyGroupService>();
builder.Services.AddScoped<IPropertyOwnerService, PropertyOwnerService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add task schedules
builder.Services.AddSingleton((scope) =>
{
    var taskSchedules = new List<TaskSchedule>()
    {
        new TaskSchedule()
        {
            TaskId = nameof(ArchiveTask),
            Frequency = TimeSpan.FromDays(1)
        },
        new TaskSchedule()
        {
            TaskId = nameof(MessageSendTask),
            Frequency = TimeSpan.FromMinutes(10)
        },
        new TaskSchedule()
        {
            TaskId = nameof(MonitoringTask),
            Frequency = TimeSpan.FromMinutes(5)
        }
    };
    return new TaskSchedulesService(taskSchedules);
});

// Add service for executing tasks
builder.Services.AddHostedService<TaskBackgroundService>();

// Add tasks
builder.Services.RegisterAllTypes<ITaskObject>(new[] { typeof(Program).Assembly }, ServiceLifetime.Scoped);

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

app.Run();

