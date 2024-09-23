using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Seed1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Services
{
    public class SharedSeedDataService : ISharedSeedDataService
    {
        private readonly IAccountTransactionService _accountTransactionService;
        private readonly IAccountTransactionTypeService _accountTransactionTypeService;
        private readonly IAuditEventService _auditEventService;
        private readonly IAuditEventTypeService _auditEventTypeService;
        private readonly IDocumentService _documentService;
        private readonly IEmployeeService _employeeService;
        private readonly IIssueService _issueService;
        private readonly IIssueStatusService _issueStatusService;
        private readonly IIssueTypeService _issueTypeService;
        private readonly IMessageService _messageService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTypeService _messageTypeService;
        private readonly IPropertyFeatureTypeService _propertyFeatureTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyOwnerService _propertyOwnerService;
        private readonly IPropertyService _propertyService;
        private readonly ISystemValueTypeService _systemValueTypeService;
        private readonly IUserService _userService;

        public SharedSeedDataService(IAccountTransactionService accountTransactionService,
                        IAccountTransactionTypeService accountTransactionTypeService,
                        IAuditEventService auditEventService,
                        IAuditEventTypeService auditEventTypeService,
                        IDocumentService documentService,
                        IEmployeeService employeeService,
                        IIssueService issueService,
                        IIssueStatusService issueStatusService,
                        IIssueTypeService issueTypeService,
                        IMessageService messageService,
                        IMessageTemplateService messageTemplateService,
                        IMessageTypeService messageTypeService,                        
                        IPropertyFeatureTypeService propertyFeatureTypeService,
                        IPropertyGroupService propertyGroupService,
                        IPropertyOwnerService propertyOwnerService,
                        IPropertyService propertyService,
                        ISystemValueTypeService systemValueTypeService,
                        IUserService userService)
        {
            _accountTransactionService = accountTransactionService;
            _accountTransactionTypeService = accountTransactionTypeService;
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _documentService = documentService;
            _employeeService = employeeService;
            _issueService = issueService;
            _issueStatusService = issueStatusService;
            _issueTypeService = issueTypeService;
            _messageService = messageService;
            _messageTemplateService = messageTemplateService;
            _messageTypeService = messageTypeService;
            _propertyFeatureTypeService = propertyFeatureTypeService;
            _propertyGroupService = propertyGroupService;
            _propertyOwnerService = propertyOwnerService;
            _propertyService = propertyService;
            _systemValueTypeService = systemValueTypeService;
            _userService = userService;
        }

        public SharedSeed GetSeedData(int group)
        {
            var sharedSeed = new SharedSeed();

            // TODO: Clean this up. Possibly use reflection
            switch(group)
            {
                case 1: 
                    sharedSeed = GetSeedData1();
                    break;
            }

            return sharedSeed;               
        }

        private SharedSeed GetSeedData1()
        {
            var sharedSeed = new SharedSeed();
            sharedSeed.AccountTransactions = new AccountTransactionSeed1(_accountTransactionTypeService, _propertyService);
            sharedSeed.AccountTransactionTypes = new AccountTransactionTypeSeed1();
            sharedSeed.AuditEvents = new AuditEventSeed1();
            sharedSeed.AuditEventTypes = new AuditEventTypeSeed1();
            sharedSeed.Documents = new DocumentSeed1();
            sharedSeed.Employees = new EmployeeSeed1();
            sharedSeed.Issues = new IssueSeed1(_documentService, _issueStatusService, _issueTypeService, _propertyGroupService, _propertyService);
            sharedSeed.IssueStatuses = new IssueStatusSeed1();
            sharedSeed.IssueTypes = new IssueTypeSeed1();
            sharedSeed.Messages = new MessageSeed1(_documentService, _messageTypeService, _propertyOwnerService);
            sharedSeed.MessageTemplates = new MessageTemplateSeed1(_messageTypeService);
            sharedSeed.MessageTypes = new MessageTypeSeed1();
            sharedSeed.Properties = new PropertySeed1(_documentService, _propertyFeatureTypeService, _propertyGroupService, _propertyOwnerService);
            sharedSeed.PropertyFeatureTypes = new PropertyFeatureTypeSeed1();
            sharedSeed.PropertyGroups = new PropertyGroupSeed1(_documentService);
            sharedSeed.PropertyOwners = new PropertyOwnerSeed1(_documentService);
            sharedSeed.SystemValueTypes = new SystemValueTypeSeed1();
            sharedSeed.Users = new UserSeed1();

            return sharedSeed;
        }
    }
}
