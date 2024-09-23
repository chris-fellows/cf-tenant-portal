using CFTenantPortal.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CFTenantPortal.Services
{
    public class DatabaseAdminService : IDatabaseAdminService
    {
        private readonly IDatabaseConfig _databaseConfig;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISharedDatabaseConfigurer _sharedDatabaseConfigurer;

        public DatabaseAdminService(IDatabaseConfig databaseConfig,
            IServiceProvider serviceProvider,
            ISharedDatabaseConfigurer sharedDatabaseConfigurer)
        {
            _databaseConfig = databaseConfig;
            _serviceProvider = serviceProvider;
            _sharedDatabaseConfigurer = sharedDatabaseConfigurer;
        }

        public async Task InitialiseSharedAsync()
        {
            await _sharedDatabaseConfigurer.InitialiseAsync();
        }
        
        public async Task DeleteSharedData()
        {
            var accountTransactionService = _serviceProvider.GetRequiredService<IAccountTransactionService>();
            var accountTransactionTypeService = _serviceProvider.GetRequiredService<IAccountTransactionTypeService>();
            var auditEventService = _serviceProvider.GetRequiredService<IAuditEventService>();
            var auditEventTypeService = _serviceProvider.GetRequiredService<IAuditEventTypeService>();
            var documentService = _serviceProvider.GetRequiredService<IDocumentService>();
            var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            var issueService = _serviceProvider.GetRequiredService<IIssueService>();
            var issueStatusService = _serviceProvider.GetRequiredService<IIssueStatusService>();
            var issueTypeService = _serviceProvider.GetRequiredService<IIssueTypeService>();
            var messageService = _serviceProvider.GetRequiredService<IMessageService>();
            var messageTemplateService = _serviceProvider.GetRequiredService<IMessageTemplateService>();
            var messageTypeService = _serviceProvider.GetRequiredService<IMessageTypeService>();
            var propertyService = _serviceProvider.GetRequiredService<IPropertyService>();
            var propertyFeatureTypeService = _serviceProvider.GetRequiredService<IPropertyFeatureTypeService>();
            var propertyGroupService = _serviceProvider.GetRequiredService<IPropertyGroupService>();
            var propertyOwnerService = _serviceProvider.GetRequiredService<IPropertyOwnerService>();
            var systemValueTypeService = _serviceProvider.GetRequiredService<ISystemValueTypeService>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            await accountTransactionService.DeleteAllAsync();
            await accountTransactionTypeService.DeleteAllAsync();
            await auditEventService.DeleteAllAsync();
            await auditEventTypeService.DeleteAllAsync();
            await documentService.DeleteAllAsync();
            await employeeService.DeleteAllAsync();
            await issueService.DeleteAllAsync();           
            await issueStatusService.DeleteAllAsync();
            await issueTypeService.DeleteAllAsync();
            await messageService.DeleteAllAsync();
            await messageTemplateService.DeleteAllAsync();
            await messageTypeService.DeleteAllAsync();
            await propertyService.DeleteAllAsync();
            await propertyGroupService.DeleteAllAsync();
            await propertyOwnerService.DeleteAllAsync();
            await systemValueTypeService.DeleteAllAsync();
            await userService.DeleteAllAsync();                
        }

        public async Task LoadSharedData(int group)
        {
            var seedDataService = _serviceProvider.GetRequiredService<ISharedSeedDataService>();

            // Get seed data            
            var seedData = seedDataService.GetSeedData(group);

            // Get services
            var accountTransactionService = _serviceProvider.GetRequiredService<IAccountTransactionService>();
            var accountTransactionTypeService = _serviceProvider.GetRequiredService<IAccountTransactionTypeService>();
            var auditEventService = _serviceProvider.GetRequiredService<IAuditEventService>();
            var auditEventTypeService = _serviceProvider.GetRequiredService<IAuditEventTypeService>();
            var documentService = _serviceProvider.GetRequiredService<IDocumentService>();
            var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            var issueService = _serviceProvider.GetRequiredService<IIssueService>();
            var issueStatusService = _serviceProvider.GetRequiredService<IIssueStatusService>();
            var issueTypeService = _serviceProvider.GetRequiredService<IIssueTypeService>();
            var messageService = _serviceProvider.GetRequiredService<IMessageService>();
            var messageTemplateService = _serviceProvider.GetRequiredService<IMessageTemplateService>();
            var messageTypeService = _serviceProvider.GetRequiredService<IMessageTypeService>();
            var propertyService = _serviceProvider.GetRequiredService<IPropertyService>();
            var propertyFeatureTypeService = _serviceProvider.GetRequiredService<IPropertyFeatureTypeService>();
            var propertyGroupService = _serviceProvider.GetRequiredService<IPropertyGroupService>();
            var propertyOwnerService = _serviceProvider.GetRequiredService<IPropertyOwnerService>();
            var systemValueTypeService = _serviceProvider.GetRequiredService<ISystemValueTypeService>();
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            // Import data in particular order because some entities reference others
            await accountTransactionTypeService.ImportAsync(seedData.AccountTransactionTypes);
            await auditEventTypeService.ImportAsync(seedData.AuditEventTypes);            
            await employeeService.ImportAsync(seedData.Employees);
            await issueStatusService.ImportAsync(seedData.IssueStatuses);
            await issueTypeService.ImportAsync(seedData.IssueTypes);
            await propertyFeatureTypeService.ImportAsync(seedData.PropertyFeatureTypes);
            await messageTypeService.ImportAsync(seedData.MessageTypes);    // References MessageTemplate.Id (as default template to use)            
            await messageTemplateService.ImportAsync(seedData.MessageTemplates);    // References MessageType.Id
            await systemValueTypeService.ImportAsync(seedData.SystemValueTypes);

            await documentService.ImportAsync(seedData.Documents);

            await propertyGroupService.ImportAsync(seedData.PropertyGroups);    // References Document.Id
            await propertyOwnerService.ImportAsync(seedData.PropertyOwners);    // References Document.Id
            await propertyService.ImportAsync(seedData.Properties);     // References PropertyGroup.Id, PropertyOwner.Id, PropertyFeatureType.Id, Document.Id
            
            await accountTransactionService.ImportAsync(seedData.AccountTransactions);  // References AccountTransactionType.Id, Property.Id)            
            await auditEventService.ImportAsync(seedData.AuditEvents);  // References AuditEventType.Id                                    
            await issueService.ImportAsync(seedData.Issues);        // References IssueStatus.Id, IssueType.Id
                                                                   
            await messageService.ImportAsync(seedData.Messages);    // References MessageType.Id, PropertyOwner.Id, Issue.Id, Property.Id, Document.Id                        
            await userService.ImportAsync(seedData.Users);

            int xxx = 1000;
        }
    }
}

