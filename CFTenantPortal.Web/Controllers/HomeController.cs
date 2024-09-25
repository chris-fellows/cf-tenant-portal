using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Abstractions;
using System.Diagnostics;
using System.Net.WebSockets;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.DotNet.Scaffolding.Shared.Messaging;
//using NuGet.Protocol.Plugins;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace CFTenantPortal.Controllers
{
    public class HomeController : Controller
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
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IMessageTypeService _messageTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyOwnerService _propertyOwnerService;
        private readonly IPropertyService _propertyService;
        private readonly ISystemValueTypeService _systemValueTypeService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                IAccountTransactionService accountTransactionService,
                IAccountTransactionTypeService accountTransactionTypeService,
                IAuditEventService auditEventService,
                IAuditEventTypeService auditEventTypeService,
                IDocumentService documentService,
                IEmployeeService employeeService,
                IIssueService issueService,
                IIssueStatusService issueStatusService,
                IIssueTypeService issueTypeService,
                IMapper mapper,
                IMessageService messageService,
                IMessageTypeService messageTypeService,
                IPropertyGroupService propertyGroupService,
                IPropertyOwnerService propertyOwnerService,
                IPropertyService propertyService,
                ISystemValueTypeService systemValueTypeService) 
        {
            _logger = logger;
            _accountTransactionService = accountTransactionService;
            _accountTransactionTypeService = accountTransactionTypeService;
            _auditEventService = auditEventService;
            _auditEventTypeService = auditEventTypeService;
            _documentService = documentService;
            _employeeService = employeeService;
            _issueService = issueService;
            _issueStatusService = issueStatusService;
            _issueTypeService = issueTypeService;
            _mapper = mapper;
            _messageService = messageService;
            _messageTypeService = messageTypeService;
            _propertyGroupService = propertyGroupService;
            _propertyOwnerService = propertyOwnerService;
            _propertyService = propertyService;
            _systemValueTypeService = systemValueTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns view to add or update issue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Issue(string id)
        {
            var employees = _employeeService.GetAll();
            var issueStatuses = _issueStatusService.GetAll();
            var issueTypes = _issueTypeService.GetAll();
            var messageTypes = _messageTypeService.GetAll();
            var properties = _propertyService.GetAll();
            var propertyGroups = _propertyGroupService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();

            //var entityRefNone = EntityReference.None;
            
            if (String.IsNullOrEmpty(id))   // Create issue
            {                              
                var issueStatus = issueStatuses.First();
                var issueType = issueTypes.First();
                var property = properties.First();
                var createdByEmployee = employees.First();
                var createdByPropertyOwner = propertyOwners.First();

                var model = new IssueVM()
                {
                    AllowSave = true,
                    HeaderText = "New Issue",
                    Reference = Guid.NewGuid().ToString(),
                    Description = "New",
                    TypeId = issueType.Id,
                    PropertyId = EntityReference.None.Id,
                    PropertyGroupId = EntityReference.None.Id,
                    CreatedEmployeeId = EntityReference.None.Id,
                    CreatedPropertyOwnerId = EntityReference.None.Id,     
                    DocumentList = new DocumentListVM()
                    {
                        Documents = new List<DocumentBasicVM>()
                    },
                    EmployeeList = employees.Select(e =>
                    {
                        return new EntityReference()
                        {
                            Id = e.Id,
                            Name = e.Name
                        };
                    }).ToList(),
                    IssueStatusId = issueStatus.Id,
                    IssueStatusList = issueStatuses.Select(i =>
                    {
                        return new EntityReference()
                        {
                            Id = i.Id,
                            Name = i.Description
                        };
                    }).ToList(),
                    IssueTypeList = issueTypes.Select(i =>
                    {
                        return new EntityReference()
                        {
                            Id = i.Id,
                            Name = i.Description
                        };
                    }).ToList(),
                    MessageList = new MessageListVM()
                    {
                        Messages= new List<MessageBasicVM>()
                    },
                    PropertyGroupList = propertyGroups.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Name
                        };
                    }).ToList(),
                    PropertyList = properties.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Address.ToSummary()
                        };
                    }).ToList(),
                    PropertyOwnerList = propertyOwners.Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = po.Name
                        };
                    }).ToList(),
                };

                // Add none for optional
                model.EmployeeList.Insert(0, EntityReference.None);
                model.PropertyGroupList.Insert(0, EntityReference.None);
                model.PropertyList.Insert(0, EntityReference.None);                
                model.PropertyOwnerList.Insert(0, EntityReference.None);

                return View(model);
            }
            else     // Display or edit issue
            {
                var issue = _issueService.GetByIdAsync(id).Result;          

                var issueType = _issueTypeService.GetByIdAsync(issue.TypeId).Result;
                var property = String.IsNullOrEmpty(issue.PropertyId) ?
                               null :
                               _propertyService.GetByIdAsync(issue.PropertyId).Result;                
         
                var documents = issue.DocumentIds == null ?
                           new List<DocumentBasicVM>() :
                           issue.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
                           .Select(document => new DocumentBasicVM()
                           {
                               Id = document.Id,
                               Name = document.Name,
                               AllowDelete = true
                           }).ToList();
                           
                var model = new IssueVM()
                {
                    AllowSave = true,
                    HeaderText = "Issue",
                    Id = issue.Id,
                    Reference = issue.Reference,
                    Description = issue.Description,
                    TypeId = issue.TypeId,
                    PropertyId = issue.PropertyId,
                    PropertyGroupId = issue.PropertyGroupId,
                    CreatedEmployeeId = issue.CreatedEmployeeId,
                    CreatedPropertyOwnerId = issue.CreatedPropertyOwnerId,                    
                    DocumentList = new DocumentListVM()
                    {
                        Documents = documents
                    },
                    EmployeeList = employees.Select(e =>
                    {
                        return new EntityReference()
                        {
                            Id = e.Id,
                            Name = e.Name
                        };
                    }).ToList(),
                    IssueStatusId = issue.StatusId,
                    IssueStatusList = issueStatuses.Select(i =>
                    {
                        return new EntityReference()
                        {
                            Id = i.Id,
                            Name = i.Description
                        };
                    }).ToList(),
                    IssueTypeList = issueTypes.Select(i =>
                    {
                        return new EntityReference()
                        {
                            Id = i.Id,
                            Name = i.Description
                        };
                    }).ToList(),
                    MessageList = new MessageListVM()
                    {

                    },
                    PropertyGroupList = propertyGroups.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Name
                        };
                    }).ToList(),
                    PropertyList  = properties.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Address.ToSummary()
                        };
                    }).ToList(),
                    PropertyOwnerList = propertyOwners.Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = po.Name
                        };
                    }).ToList(),
                };                

                // Load messages
                model.MessageList.Messages = _messageService.GetByIssue(issue.Id).Result.Select(m =>
                {
                    var messageType = messageTypes.First(mt => mt.Id == m.MessageTypeId);
                    var messageProperty = String.IsNullOrEmpty(m.PropertyId) ? null : properties.First(p => p.Id == m.PropertyId);
                    var messagePropertyOwner = String.IsNullOrEmpty(m.PropertyOwnerId) ? null : propertyOwners.First(po => po.Id == m.PropertyOwnerId);

                    return new MessageBasicVM()
                    {
                        Id = m.Id,
                        IssueReference = issue.Reference,
                        PropertyId = m.PropertyId,
                        PropertyName = (messageProperty == null ? "" : messageProperty.Address.ToSummary()),
                        PropertyOwnerName = (messagePropertyOwner == null ? "" : messagePropertyOwner.Name),
                        PropertyOwnerId = m.PropertyOwnerId,
                        TypeDescription = messageType.Description,
                        AllowDelete = true
                    };
                }).ToList();

                // Add none for optional
                model.EmployeeList.Insert(0, EntityReference.None);
                model.PropertyGroupList.Insert(0, EntityReference.None);
                model.PropertyList.Insert(0, EntityReference.None);
                model.PropertyOwnerList.Insert(0, EntityReference.None);

                return View(model);
            }
        }

        /// <summary>
        /// Returns view to add or update property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Property(string id)
        {
            var messageTypes = _messageTypeService.GetAll();
            var propertyGroups = _propertyGroupService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();

            if (String.IsNullOrEmpty(id))   // New property
            {              
                var model = new PropertyVM()
                {
                    AllowSave = true,
                    HeaderText = "New Property",
                    Address = new AddressVM()
                    {

                    },
                    DocumentList = new DocumentListVM() { Documents = new List<DocumentBasicVM>() },                  
                    //Issues = new List<IssueBasicVM>(),
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = false,
                        Issues = new List<IssueBasicVM>()
                    },
                    MessageList = new MessageListVM()
                    {
                        Messages = new List<MessageBasicVM>()
                    },
                    AccountTransactions = new List<AccountTransactionBasicVM>(),
                    PropertyGroupId = propertyGroups.OrderBy(pg => pg.Name).First().Id,
                    PropertyGroupList = propertyGroups.OrderBy(pg => pg.Name).Select(pg =>
                    {
                        return new EntityReference()
                        {
                            Id = pg.Id,
                            Name = pg.Name
                        };
                    }).ToList(),
                    PropertyOwnerId = propertyOwners.OrderBy(po => po.Name).First().Id,
                    PropertyOwnerList = propertyOwners.OrderBy(po => po.Name).Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = po.Name
                        };
                    }).ToList(),
                };

                return View(model);
            }
            else    // Display or edit property
            {
                var property = _propertyService.GetByIdAsync(id).Result;
                var propertyGroup = _propertyGroupService.GetByIdAsync(property.GroupId).Result;
                var propertyOwner = _propertyOwnerService.GetByIdAsync(property.OwnerId).Result;

                var accountTransactionTypes = _accountTransactionTypeService.GetAll();

                var issueStatuses = _issueStatusService.GetAll();
                var issueTypes = _issueTypeService.GetAll();

                var documents = property.DocumentIds == null ?
                   new List<DocumentBasicVM>() :
                   property.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
                   .Select(document => new DocumentBasicVM()
                   {
                       Id = document.Id,
                       Name = document.Name,
                       AllowDelete = true
                   }).ToList();

                // TODO: Use auto mapping
                var model = new PropertyVM()
                {
                    AllowSave = true,
                    HeaderText = "Property",
                    Id = property.Id,
                    Address = new AddressVM()
                    {
                        Line1 = property.Address.Line1,
                        Line2 = property.Address.Line2,
                        Town = property.Address.Town,
                        County = property.Address.County,
                        Postcode = property.Address.Postcode
                    },
                    PropertyGroupId = property.GroupId,
                    PropertyOwnerId = property.OwnerId,
                    DocumentList = new DocumentListVM()
                    {
                        Documents = documents
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = true
                    },
                    MessageList = new MessageListVM()
                    {

                    },
                    PropertyGroupList = propertyGroups.OrderBy(pg => pg.Name).Select(pg =>
                    {
                        return new EntityReference()
                        {
                            Id = pg.Id,
                            Name = pg.Name
                        };
                    }).ToList(),
                    PropertyOwnerList = propertyOwners.OrderBy(po => po.Name).Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = $"{po.Name} ({po.Email})"
                        };
                    }).ToList()
                };

                // Load accounting transactions
                model.AccountTransactions = _accountTransactionService.GetByProperty(property.Id).Result.OrderBy(at => at.CreatedDateTime).Select(at =>
                {
                    var accountTransactionType = accountTransactionTypes.First(att => att.Id == at.TypeId);

                    return new AccountTransactionBasicVM()
                    {
                        Id = at.Id,
                        CreatedDateTime = at.CreatedDateTime,
                        Reference = at.Reference,
                        TypeDescription = accountTransactionType.Description,
                        Value = at.Value
                    };
                }).ToList();                

                // Load issues
                model.IssueList.Issues = _issueService.GetByProperty(property.Id).Result.Select(i =>
                {
                    var issueStatus = issueStatuses.First(s => s.Id == i.StatusId);
                    var issueType = issueTypes.First(it => it.Id == i.TypeId);

                    return new IssueBasicVM()
                    {
                        Id = i.Id,
                        Reference = i.Reference,
                        Description = i.Description,
                        TypeDescription = issueType.Description,
                        PropertyOrBuilderDescription = property.Address.ToSummary(),
                        StatusDescription = issueStatus.Description,
                        PropertyId = property.Id,
                        AllowDelete = true
                    };
                }).ToList();

                // Load messages                
                model.MessageList.Messages = _messageService.GetByProperty(property.Id).Result.Select(m =>
                {
                    var messageType = messageTypes.First(mt => mt.Id == m.MessageTypeId);
                    var messageIssue = String.IsNullOrEmpty(m.IssueId) ? null : _issueService.GetByIdAsync(m.IssueId).Result;
                    var messagePropertyOwner = String.IsNullOrEmpty(m.PropertyOwnerId) ? null : propertyOwners.First(po => po.Id == m.PropertyOwnerId);

                    return new MessageBasicVM()
                    {
                        Id = m.Id,
                        IssueReference = (messageIssue == null ? "" : messageIssue.Reference),
                        PropertyId = m.PropertyId,
                        PropertyName = (property == null ? "" : property.Address.ToSummary()),
                        PropertyOwnerName = (messagePropertyOwner == null ? "" : messagePropertyOwner.Name),
                        PropertyOwnerId = m.PropertyOwnerId,
                        TypeDescription = messageType.Description,
                        AllowDelete = true
                    };                 
                }).ToList();

                return View(model);
            }
        }

        /// <summary>
        /// Returns view to add or update employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Employee(string id)
        {            
            if (String.IsNullOrEmpty(id))   // New employee
            {
                var model = new EmployeeVM()
                {
                    HeaderText = "New Employee",
                    Name = "New",
                    Active = true,
                    Roles = new List<Enums.UserRoles>()
                };

                return View(model);
            }
            else    // Display or edit property
            {
                var employee = _employeeService.GetByIdAsync(id).Result;


                var model = new EmployeeVM()
                {
                    HeaderText = "Edit Employee",
                    Id = employee.Id,
                    Active = employee.Active,
                    Email = employee.Email,
                    Name = employee.Name,
                    Roles = employee.Roles,                    
                };
               
                return View(model);
            }
        }

        /// <summary>
        /// Returns view to add or update message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Message(string id)
        {
            var messageTypes = _messageTypeService.GetAll();
            var properties = _propertyService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();

            if (String.IsNullOrEmpty(id))   // New message
            {
                var model = new MessageVM()
                {
                    DocumentIds = new List<string>(),
                    Documents = new List<DocumentBasicVM>(),
                    IssueList= new List<EntityReference>(),        
                    MessageTypeList = messageTypes.Select(mt =>
                    {
                        return new EntityReference()
                        {
                            Id = mt.Id,
                            Name = mt.Description
                        };
                    }).ToList(),
                    PropertyList = properties.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Address.ToSummary()
                        };
                    }).ToList(),
                    PropertyOwnerList = propertyOwners.Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = po.Name
                        };
                    }).ToList(),
                };

                // Add None for optional properties
                model.IssueList.Insert(0, EntityReference.None);
                model.PropertyList.Insert(0, EntityReference.None);
                model.PropertyOwnerList.Insert(0, EntityReference.None);

                return View(model);
            }
            else    // Display or edit property
            {
                var message = _messageService.GetByIdAsync(id).Result;

                var documents = message.DocumentIds == null ?
                       new List<DocumentBasicVM>() :
                       message.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
                       .Select(document => new DocumentBasicVM()
                       {
                           Id = document.Id,
                           Name = document.Name,
                           AllowDelete = true
                       }).ToList();

                var model = new MessageVM()
                {
                    CreatedDateTime = message.CreatedDateTime,
                    DocumentIds = message.DocumentIds,
                    Documents = documents,
                    Id = message.Id,
                    IssueId = message.IssueId,
                    IssueList  = new List<EntityReference>(),
                    MessageTypeId = message.MessageTypeId,
                    MessageTypeList = messageTypes.Select(mt =>
                    {
                        return new EntityReference()
                        {
                            Id = mt.Id,
                            Name = mt.Description
                        };
                    }).ToList(),
                    PropertyId = message.PropertyId,
                    PropertyList = properties.Select(p =>
                    {
                        return new EntityReference()
                        {
                            Id = p.Id,
                            Name = p.Address.ToSummary()
                        };
                    }).ToList(),
                    PropertyOwnerId = message.PropertyOwnerId,
                    PropertyOwnerList = propertyOwners.Select(po =>
                    {
                        return new EntityReference()
                        {
                            Id = po.Id,
                            Name = po.Name
                        };
                    }).ToList(),
                    Text = message.Text
                };

                // Add None for optional properties
                model.IssueList.Insert(0, EntityReference.None);
                model.PropertyList.Insert(0, EntityReference.None);
                model.PropertyOwnerList.Insert(0, EntityReference.None);

                return View(model);
            }
        }

        /// <summary>
        /// Returns view to view audit event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult AuditEvent(string id)
        {
            // Get data
            var auditEventTypes = _auditEventTypeService.GetAll();
            //var properties = _propertyService.GetAll();            
            //var propertyOwners = _propertyOwnerService.GetAll();
            var systemValueTypes = _systemValueTypeService.GetAll().ToList();

            // Get audit event
            var auditEvent = _auditEventService.GetByIdAsync(id).Result;

            // Get audit event type
            var auditEventType = auditEventTypes.First(aet => aet.Id == auditEvent.EventTypeId);

            var model = new AuditEventVM()
            {
                Id = auditEvent.Id,
                HeaderText = "Audit Event",
                CreatedDateTime = auditEvent.CreatedDateTime,
                TypeDescription = auditEventType.Description,
                Parameters = auditEvent.Parameters.Select(p => GetAuditEventParameterVM(p, systemValueTypes)).ToList()
            };
       
            return View(model);            
        }

        /// <summary>
        /// Gets AuditEventParameterVM for AuditEventParameter. Parameter will typically be displayed as a link which opens the 
        /// entity detail. E.g. Link for property address opens property page.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="systemValueTypes"></param>
        /// <returns></returns>
        private AuditEventParameterVM GetAuditEventParameterVM(AuditEventParameter parameter, List<SystemValueType> systemValueTypes)
        {
            AuditEventParameterVM parameterVM = new AuditEventParameterVM();
            var systemValueTypeProperty = systemValueTypes.First(svt => svt.Id == parameter.SystemValueTypeId);                       
            switch (systemValueTypeProperty.ValueType)
            {
                case SystemValueTypes.DocumentId:
                    var document = _documentService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = document.Id;
                    parameterVM.EntityDescription = document.Name;
                    parameterVM.EntityDetailRoute = "Document";
                    parameterVM.EntityTypeDescription = "Document";
                    break;
                case SystemValueTypes.EmployeeId:
                    var employee = _employeeService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = employee.Id;
                    parameterVM.EntityDescription = employee.Name;
                    parameterVM.EntityDetailRoute = nameof(this.Employee);
                    parameterVM.EntityTypeDescription = "Employee";
                    break;
                case SystemValueTypes.IssueId:
                    var issue = _issueService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = issue.Id;
                    parameterVM.EntityDescription = issue.Reference;
                    parameterVM.EntityDetailRoute = nameof(this.Issue);
                    parameterVM.EntityTypeDescription = "Issue";
                    break;
                case SystemValueTypes.IssueTypeId:
                    var issueType = _issueTypeService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = issueType.Id;
                    parameterVM.EntityDescription = issueType.Description;
                    parameterVM.EntityDetailRoute = "IssueType";
                    parameterVM.EntityTypeDescription = "Issue Type";
                    break;
                case SystemValueTypes.MessageId:
                    var message = _messageService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = message.Id;
                    parameterVM.EntityDescription = message.Text;
                    parameterVM.EntityDetailRoute = nameof(this.Message);
                    parameterVM.EntityTypeDescription = "Message";
                    break;
                case SystemValueTypes.MessageTypeId:
                    var messageType = _messageTypeService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = messageType.Id;
                    parameterVM.EntityDescription = messageType.Description;
                    parameterVM.EntityDetailRoute = "MessageType";
                    parameterVM.EntityTypeDescription = "Message Type";
                    break;
                case SystemValueTypes.PropertyGroupId:
                    var propertyGroup = _propertyGroupService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = propertyGroup.Id;
                    parameterVM.EntityDescription = propertyGroup.Name;
                    parameterVM.EntityDetailRoute = nameof(this.PropertyGroup);
                    parameterVM.EntityTypeDescription = "Property Group";
                    break;
                case SystemValueTypes.PropertyId:
                    var property = _propertyService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = property.Id;
                    parameterVM.EntityDescription = property.Address.ToSummary();
                    parameterVM.EntityDetailRoute = nameof(this.Property);
                    parameterVM.EntityTypeDescription = "Property";
                    break;
                case SystemValueTypes.PropertyOwnerId:
                    var propertyOwner = _propertyOwnerService.GetByIdAsync(parameter.Value.ToString()).Result;
                    parameterVM.EntityId = propertyOwner.Id;
                    parameterVM.EntityDescription = propertyOwner.Name;
                    parameterVM.EntityDetailRoute = nameof(this.PropertyOwner);
                    parameterVM.EntityTypeDescription = "Owner";
                    break;
                default:   // Default to displaying the parameter value
                    parameterVM.EntityTypeDescription = systemValueTypeProperty.Description;
                    parameterVM.EntityDescription = parameter.Value.ToString();
                    break;
            }

            return parameterVM;
        }

        //private List<AuditEventParameterVM> GetAuditEventParameterVMs(List<AuditEventParameter> auditEventParameters, List<SystemValueType> systemValueTypes)
        //{            
        //    var parameters = new List<AuditEventParameterVM>();
        //    // Get parameters to display      
        //    foreach (var parameter in auditEventParameters)
        //    {
        //        AuditEventParameterVM parameterVM = new AuditEventParameterVM();
        //        var systemValueTypeProperty = systemValueTypes.First(svt => svt.Id == parameter.SystemValueTypeId);
        //        switch (systemValueTypeProperty.ValueType)
        //        {
        //            case SystemValueTypes.DocumentId:
        //                var document = _documentService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = document.Id;
        //                parameterVM.EntityDescription = document.Name;
        //                parameterVM.EntityDetailRoute = "Document";
        //                parameterVM.EntityTypeDescription = "Document";
        //                break;
        //            case SystemValueTypes.EmployeeId:
        //                var employee = _employeeService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = employee.Id;
        //                parameterVM.EntityDescription = employee.Name;
        //                parameterVM.EntityDetailRoute = nameof(this.Employee);
        //                parameterVM.EntityTypeDescription = "Employee";
        //                break;
        //            case SystemValueTypes.IssueId:
        //                var issue = _issueService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = issue.Id;
        //                parameterVM.EntityDescription = issue.Reference;
        //                parameterVM.EntityDetailRoute = nameof(this.Issue);
        //                parameterVM.EntityTypeDescription = "Issue";
        //                break;
        //            case SystemValueTypes.IssueTypeId:
        //                var issueType = _issueTypeService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = issueType.Id;
        //                parameterVM.EntityDescription = issueType.Description;
        //                parameterVM.EntityDetailRoute = "IssueType";
        //                parameterVM.EntityTypeDescription = "Issue Type";
        //                break;
        //            case SystemValueTypes.MessageId:
        //                var message = _messageService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = message.Id;
        //                parameterVM.EntityDescription = message.Text;
        //                parameterVM.EntityDetailRoute = nameof(this.Message);
        //                parameterVM.EntityTypeDescription = "Message";
        //                break;
        //            case SystemValueTypes.MessageTypeId:
        //                var messageType = _messageTypeService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = messageType.Id;
        //                parameterVM.EntityDescription = messageType.Description;
        //                parameterVM.EntityDetailRoute = "MessageType";
        //                parameterVM.EntityTypeDescription = "Message Type";
        //                break;
        //            case SystemValueTypes.PropertyGroupId:
        //                var propertyGroup = _propertyGroupService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = propertyGroup.Id;
        //                parameterVM.EntityDescription = propertyGroup.Name;
        //                parameterVM.EntityDetailRoute = nameof(this.PropertyGroup);
        //                parameterVM.EntityTypeDescription = "Property Group";
        //                break;
        //            case SystemValueTypes.PropertyId:
        //                var property = _propertyService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = property.Id;
        //                parameterVM.EntityDescription = property.Address.ToSummary();
        //                parameterVM.EntityDetailRoute = nameof(this.Property);
        //                parameterVM.EntityTypeDescription = "Property";
        //                break;
        //            case SystemValueTypes.PropertyOwnerId:
        //                var propertyOwner = _propertyOwnerService.GetByIdAsync(parameter.Value.ToString()).Result;
        //                parameterVM.EntityId = propertyOwner.Id;
        //                parameterVM.EntityDescription = propertyOwner.Name;
        //                parameterVM.EntityDetailRoute = nameof(this.PropertyOwner);
        //                parameterVM.EntityTypeDescription = "Owner";
        //                break;
        //        }

        //        // Add VM parameter
        //        if (!String.IsNullOrEmpty(parameterVM.EntityId))
        //        {
        //            parameters.Add(parameterVM);
        //        }
        //    }

        //    return parameters;
        //}

        /// <summary>
        /// Action to display list of properties. All or filtered list    
        /// </summary>
        /// <param name="filterVM"></param>
        /// <returns></returns>
        public IActionResult AllPropertyList(PropertyFilterVM filterVM = null)
        {
            var propertyGroups = _propertyGroupService.GetAll().ToList();
            var propertyOwners = _propertyOwnerService.GetAll().ToList();
            
            var model = new PropertyListVM() 
            { 
                AllowCreate = true,
                HeaderText = "Properties",
                Filter = new PropertyFilterVM()
                {                   
                    PropertyGroupId = String.IsNullOrEmpty(filterVM.PropertyGroupId) ? EntityReference.None.Id : filterVM.PropertyGroupId,
                    PropertyGroupList = propertyGroups.Select(pg => new EntityReference()
                    {
                        Id = pg.Id,
                        Name = pg.Name
                    }).ToList(),
                    PropertyOwnerId = String.IsNullOrEmpty(filterVM.PropertyOwnerId) ? EntityReference.None.Id : filterVM.PropertyOwnerId,
                    PropertyOwnerList = propertyOwners.Select(po => new EntityReference()
                    {
                        Id = po.Id,
                        Name = po.Name
                    }).ToList()
                }
            };

            // Add None to lists
            model.Filter.PropertyGroupList.Insert(0, EntityReference.None);            
            model.Filter.PropertyOwnerList.Insert(0, EntityReference.None);          

            // Set property filter
            var propertyFilter = new PropertyFilter()
            {
                PropertyGroupIds = String.IsNullOrWhiteSpace(model.Filter.PropertyGroupId) || 
                                    model.Filter.PropertyGroupId.Equals(EntityReference.None.Id) ? 
                            new() : new() { model.Filter.PropertyGroupId },
                PropertyOwnerIds = String.IsNullOrWhiteSpace(model.Filter.PropertyOwnerId) || 
                                    model.Filter.PropertyOwnerId.Equals(EntityReference.None.Id) ?
                            new() : new() { model.Filter.PropertyOwnerId },
                PageNo = 1,
                PageItems = 10000000
            };

            // Get properties           
            var properties = _propertyService.GetByFilterAsync(propertyFilter).Result;

            model.Properties = properties.Select(p =>
            {
                var propertyGroup = propertyGroups.First(pg => pg.Id == p.GroupId);
                var propertyOwner = propertyOwners.First(po => po.Id == p.OwnerId);

                return new PropertyBasicVM()
                {
                    Id = p.Id,
                    AddressDescription = p.Address.ToSummary(),
                    PropertyGroupName = propertyGroup.Name,
                    PropertyGroupId = propertyGroup.Id,
                    PropertyOwnerName = propertyOwner.Name,
                    PropertyOwnerId = propertyOwner.Id,
                    AllowDelete = true
                };
            }).ToList();

            return View(model);
        }

        public IActionResult AllPropertyOwnerList()
        {
            var model = new PropertyOwnerListVM() 
            { 
                AllowCreate = true,
                HeaderText = "Property Owners" 
            };

            model.PropertyOwners = _propertyOwnerService.GetAll().Select(po =>
            {
                return new PropertyOwnerBasicVM()
                {
                    Id = po.Id,
                    Email = po.Email,
                    Name = po.Name,
                    AllowDelete = true
                };

                /*
                return new PropertyOwnerVM()
                {
                    Id = po.Id,
                    Email = po.Email,
                    Name = po.Name,
                    AllowDelete = true
                };
                */
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Returns view to add or update property owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult PropertyOwner(string? id)
        {            
            if (String.IsNullOrEmpty(id))   // New property owner
            {
                var model = new PropertyOwnerVM()
                {
                    AllowSave = true,
                    //AllowDelete = false,
                    HeaderText = "New Property Owner",
                    Name = "New",                    
                    Address = new AddressVM(),
                    DocumentList = new DocumentListVM()
                    {
                        Documents = new List<DocumentBasicVM>()  
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = false,
                        Properties = new List<PropertyBasicVM>()
                    },
                    MessageList = new MessageListVM()
                    {
                        Messages = new List<MessageBasicVM>()
                    }
                };

                return View(model);
            }
            else   // Display or edit property ownert
            {
                var propertyOwner = _propertyOwnerService.GetByIdAsync(id).Result;
                var messageTypes = _messageTypeService.GetAll();
                var properties = _propertyService.GetAll();

                var propertyGroups = _propertyGroupService.GetAll();

                var documents = propertyOwner.DocumentIds == null ?
                new List<DocumentBasicVM>() :
                propertyOwner.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
                .Select(document => new DocumentBasicVM()
                {
                    Id = document.Id,
                    Name = document.Name,
                    AllowDelete = true
                }).ToList();

                var model = new PropertyOwnerVM()
                {
                    AllowSave = true,
                    //AllowDelete = false,
                    HeaderText = "Property Owner",
                    Id = propertyOwner.Id,
                    Email = propertyOwner.Email,
                    Name = propertyOwner.Name,
                    Phone = propertyOwner.Phone,
                    Address = new AddressVM()
                    {
                        Line1 = propertyOwner.Address.Line1,
                        Line2 = propertyOwner.Address.Line2,
                        Town = propertyOwner.Address.Town,
                        County = propertyOwner.Address.County,
                        Postcode = propertyOwner.Address.Postcode
                    },
                    DocumentList = new DocumentListVM()
                    {
                        Documents = documents
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = true
                    },
                    MessageList = new MessageListVM()
                    {
                        
                    }
                };

                // Load properties
                model.PropertyList.Properties = _propertyService.GetByPropertyOwner(propertyOwner.Id).Result.Select(p =>
                {
                    var propertyGroup = propertyGroups.First(pg => pg.Id == p.GroupId);

                    return new PropertyBasicVM()
                    {
                        Id = p.Id,
                        AddressDescription = p.Address.ToSummary(),
                        PropertyGroupName = propertyGroup.Name,
                        PropertyGroupId = propertyGroup.Id,
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id,
                        AllowDelete = true                        
                    };
                }).ToList();

                // Load messages
                model.MessageList.Messages = _messageService.GetByPropertyOwner(propertyOwner.Id).Result.Select(m =>
                {
                    var messageType = messageTypes.First(mt => mt.Id == m.MessageTypeId);
                    var messageIssue = String.IsNullOrEmpty(m.IssueId) ? null : _issueService.GetByIdAsync(m.IssueId).Result;
                    var messageProperty = String.IsNullOrEmpty(m.PropertyId) ? null : properties.First(p => p.Id == m.PropertyId);

                    return new MessageBasicVM()
                    {
                        Id = m.Id,
                        IssueReference = (messageIssue == null ? "" : messageIssue.Reference),
                        PropertyId = m.PropertyId,
                        PropertyName = (messageProperty == null ? "" : messageProperty.Address.ToSummary()),
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id,
                        TypeDescription = messageType.Description,
                        AllowDelete = true
                    };
                }).ToList();

                return View(model);
            }
        }

        public IActionResult AllPropertyGroupList()
        {
            var model = new PropertyGroupListVM() 
            { 
                AllowCreate = true,
                HeaderText = "Property Groups" 
            };   // Default header

            model.PropertyGroups = _propertyGroupService.GetAll().Select(p =>
            {
                return new PropertyGroupBasicVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description, 
                    AllowDelete = true
                };

                //return new PropertyGroupVM()
                //{
                //    Id = p.Id,
                //    Name = p.Name,
                //    Description = p.Description
                //};
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Returns view to add or update property group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult PropertyGroup(string? id)
        {            
            if (String.IsNullOrEmpty(id))   // New property group
            {
                var model = new PropertyGroupVM()
                {
                    AllowSave = true,
                    //AllowDelete = false,
                    HeaderText = "New Property Group",
                    Name = "New",
                    Description = "New",
                    DocumentList = new DocumentListVM()
                    {
                        Documents = new List<DocumentBasicVM>()
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = false,
                        Issues = new List<IssueBasicVM>()
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = false,
                        Properties = new List<PropertyBasicVM>()
                    }
                };

                return View(model);
            }
            else     // Display or edit property group
            {
                var propertyGroup = _propertyGroupService.GetByIdAsync(id).Result;
                var propertyOwners = _propertyOwnerService.GetAll();

                var issueStatuses = _issueStatusService.GetAll();
                var issueTypes = _issueTypeService.GetAll();

                var documents = propertyGroup.DocumentIds == null ?
                      new List<DocumentBasicVM>() :
                      propertyGroup.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
                      .Select(document => new DocumentBasicVM()
                      {
                          Id = document.Id,
                          Name = document.Name,
                          AllowDelete = true
                      }).ToList();

                var model = new PropertyGroupVM()
                {
                    AllowSave = true,
                    //AllowDelete = false,
                    HeaderText = "Property Group",
                    Id = propertyGroup.Id,
                    Name = propertyGroup.Name,
                    Description = propertyGroup.Description,
                    DocumentList = new DocumentListVM()
                    {
                        Documents = documents
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = true
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = true
                    }
                };

                // Load properties               
                model.PropertyList.Properties = _propertyService.GetByPropertyGroup(propertyGroup.Id).Result.Select(p =>
                {
                    var propertyOwner = propertyOwners.First(po => po.Id == p.OwnerId);
                    return new PropertyBasicVM()
                    {
                        Id = p.Id,
                        AddressDescription = p.Address.ToSummary(),
                        PropertyGroupName = propertyGroup.Name,
                        PropertyGroupId = propertyGroup.Id,
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id,
                        AllowDelete = true
                    };
                }).ToList();

                // Load issues
                model.IssueList.Issues = _issueService.GetByPropertyGroup(propertyGroup.Id).Result.Select(i =>
                {
                    var issueStatus = issueStatuses.First(s => s.Id == i.StatusId);
                    var issueType = issueTypes.First(it => it.Id == i.TypeId);

                    return new IssueBasicVM()
                    {
                        Id = i.Id,
                        Reference = i.Reference,
                        Description = i.Description,
                        TypeDescription = issueType.Description,
                        PropertyOrBuilderDescription = propertyGroup.Name,
                        StatusDescription = issueStatus.Description,
                        PropertyGroupId = propertyGroup.Id,
                        AllowDelete = true
                    };
                }).ToList();                

                return View(model);
            }
        }
        
        /// <summary>
        /// Action to display list of audit events. All or filtered list    
        /// </summary>
        /// <param name="filterVM"></param>
        /// <returns></returns>
        public IActionResult AllAuditEventList(AuditEventFilterVM? filterVM = null)
        {            
            // Load data
            var auditEventTypes = _auditEventTypeService.GetAll().ToList();
            var properties = _propertyService.GetAll().ToList();
            var propertyGroups = _propertyGroupService.GetAll().ToList();
            var propertyOwners = _propertyOwnerService.GetAll().ToList();
            var systemValueTypes = _systemValueTypeService.GetAll().ToList();            

            // Set model
            var model = new AuditEventListVM()
            {
                HeaderText = "Audit Events",
                Filter = new AuditEventFilterVM()
                {
                    StartCreatedDateTime = filterVM.StartCreatedDateTime == DateTimeOffset.MinValue ?
                                    DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)) :
                                    filterVM.StartCreatedDateTime,
                    EndCreatedDateTime = filterVM.EndCreatedDateTime == DateTimeOffset.MinValue ?
                                    DateTimeOffset.UtcNow.AddDays(1) :
                                    filterVM.EndCreatedDateTime,
                    AuditEventTypeId = filterVM.AuditEventTypeId,
                    AuditEventTypeList = auditEventTypes.Select(aet => new EntityReference()
                    {
                        Id = aet.Id,
                        Name = aet.Description
                    }).ToList(),
                    PropertyId = String.IsNullOrEmpty(filterVM.PropertyId) ? EntityReference.None.Id : filterVM.PropertyId,
                    PropertyList = properties.Select(p => new EntityReference()
                    {
                        Id = p.Id,
                        Name = p.Address.ToSummary()
                    }).ToList(),
                    PropertyGroupId = String.IsNullOrEmpty(filterVM.PropertyGroupId) ? EntityReference.None.Id : filterVM.PropertyGroupId,
                    PropertyGroupList = propertyGroups.Select(pg => new EntityReference()
                    {
                        Id = pg.Id,
                        Name = pg.Name
                    }).ToList(),
                    PropertyOwnerId = String.IsNullOrEmpty(filterVM.PropertyOwnerId) ? EntityReference.None.Id : filterVM.PropertyOwnerId,
                    PropertyOwnerList = propertyOwners.Select(po => new EntityReference()
                    {
                        Id= po.Id,
                        Name = po.Name
                    }).ToList()
                }
            };

            // Set None for list options
            model.Filter.AuditEventTypeList.Insert(0, EntityReference.None);
            model.Filter.PropertyGroupList.Insert(0, EntityReference.None);
            model.Filter.PropertyList.Insert(0, EntityReference.None);
            model.Filter.PropertyOwnerList.Insert(0, EntityReference.None);

            // Set event filter            
            var auditEventFilter = new AuditEventFilter()
            {
                AuditEventTypeIds = String.IsNullOrEmpty(model.Filter.AuditEventTypeId) ||
                                        model.Filter.AuditEventTypeId.Equals(EntityReference.None.Id) ?
                                            new() : new() { model.Filter.AuditEventTypeId },
                PropertyGroupIds = String.IsNullOrEmpty(model.Filter.PropertyGroupId) || 
                                        model.Filter.PropertyGroupId.Equals(EntityReference.None.Id) ?
                                            new() : new() { model.Filter.PropertyGroupId },
                PropertyIds = String.IsNullOrEmpty(model.Filter.PropertyId) ||
                                        model.Filter.PropertyId.Equals(EntityReference.None.Id) ?
                                            new() : new() { model.Filter.PropertyId },
                PropertyOwnerIds = String.IsNullOrEmpty(model.Filter.PropertyOwnerId) || 
                                        model.Filter.PropertyOwnerId.Equals(EntityReference.None.Id) ?
                                            new() : new() { model.Filter.PropertyOwnerId },
                StartCreatedDateTime = model.Filter.StartCreatedDateTime,
                EndCreatedDateTime = model.Filter.EndCreatedDateTime,                
                PageNo = 1,
                PageItems = 10000000
            };            

            // Get audit events
            var auditEvents = _auditEventService.GetByFilterAsync(auditEventFilter).Result.ToList();

            model.AuditEvents = auditEvents.Select(ae =>
            {
                var auditEventType = auditEventTypes.First(et => et.Id == ae.EventTypeId);

                var auditEventBasic = new AuditEventBasicVM()
                {
                    Id = ae.Id,
                    EventTypeId = ae.EventTypeId,
                    EventTypeDescription = auditEventType.Description,
                    CreatedDateTime = ae.CreatedDateTime
                };

                // Add first parameter
                if (ae.Parameters.Any())
                {
                    auditEventBasic.FirstParameter = GetAuditEventParameterVM(ae.Parameters.First(), systemValueTypes);                    
                }
            
                return auditEventBasic;
            }).ToList();

            return View(model);
        }

        public IActionResult AllIssueTypeList()
        {
            var model = new IssueTypeListVM() { HeaderText = "Issue Types" };   // Default header

            model.IssueTypes = _issueTypeService.GetAll().Select(it =>
            {
                return new IssueTypeVM()
                {
                    Id = it.Id,
                    Description = it.Description
                };
            }).ToList();

            return View(model);
        }

        public IActionResult AllEmployeeList()
        {
            var model = new EmployeeListVM()
            {
                AllowCreate = true,
                HeaderText = "Employees"
            };

            model.Employees = _employeeService.GetAll().Select(e =>
            {
                return new EmployeeBasicVM()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Active = e.Active,
                    AllowDelete = true
                };
            }).ToList();

            return View(model);
        }

        public IActionResult AllIssueList(IssueFilterVM filterVM = null)     // (string? issueTypeId)
        {
            // Get properties (All properties/Specific group)
            var issueStatuses = _issueStatusService.GetAll();
            var issueTypes = _issueTypeService.GetAll();
            var properties = _propertyService.GetAll();
            var propertyGroups = _propertyGroupService.GetAll();

            // Set model
            var model = new IssueListVM() 
            {
                AllowCreate = true,
                HeaderText = "Issues",   // Default header
                Filter = new IssueFilterVM()
                {
                    Reference = filterVM.Reference,
                    IssueStatusId = String.IsNullOrEmpty(filterVM.IssueStatusId) ? EntityReference.None.Id : filterVM.IssueStatusId,
                    IssueStatusList = issueStatuses.Select(pg => new EntityReference()
                    {
                        Id = pg.Id,
                        Name = pg.Description
                    }).ToList(),
                    IssueTypeId = String.IsNullOrEmpty(filterVM.IssueTypeId) ? EntityReference.None.Id : filterVM.IssueTypeId,
                    IssueTypeList = issueTypes.Select(po => new EntityReference()
                    {
                        Id = po.Id,
                        Name = po.Description
                    }).ToList()
                }
            };

            // Add None to lists
            model.Filter.IssueStatusList.Insert(0, EntityReference.None);
            model.Filter.IssueTypeList.Insert(0, EntityReference.None);

            var issueFilter = new IssueFilter()
            {
                References = String.IsNullOrEmpty(model.Filter.Reference) ?
                                new() : new() { model.Filter.Reference },
                IssueStatusIds = String.IsNullOrWhiteSpace(model.Filter.IssueStatusId) ||
                                    model.Filter.IssueStatusId.Equals(EntityReference.None.Id) ?
                            new() : new() { model.Filter.IssueStatusId },
                IssueTypeIds = String.IsNullOrWhiteSpace(model.Filter.IssueTypeId) ||
                                    model.Filter.IssueTypeId.Equals(EntityReference.None.Id) ?
                            new() : new() { model.Filter.IssueTypeId },
                PageNo = 1,
                PageItems = 10000000
            };

            /*
            // Get issues (All issues/Issue type issues/Property issues)
            List<Issue> issues = null;
            if (!String.IsNullOrEmpty(issueTypeId)) issues = _issueService.GetByIssueType(issueTypeId).Result;            
            if (issues == null) issues = _issueService.GetAll().ToList();

            // Get issue type if set
            var issueTypeMain = String.IsNullOrEmpty(issueTypeId) ?
                        null :
                        issueTypes.First(it => it.Id == issueTypeId);
            if (issueTypeMain != null) model.HeaderText = $"Issue List : {issueTypeMain.Description}";
            */

            // Get issues
            var issues = _issueService.GetByFilterAsync(issueFilter).Result;
                       
            // Get issue models
            model.Issues = issues.Select(i =>
            {
                var issueType = issueTypes.First(it => it.Id == i.TypeId);
                var issueStatus = issueStatuses.First(ist => ist.Id == i.StatusId);
                var property = String.IsNullOrEmpty(i.PropertyId) ? null :
                        properties.First(p => p.Id == i.PropertyId);

                var propertyGroup = String.IsNullOrEmpty(i.PropertyGroupId) ? null :
                    propertyGroups.First(pg => pg.Id == i.PropertyGroupId);

                return new IssueBasicVM()
                {
                    Id = i.Id,
                    Reference = i.Reference,
                    TypeDescription = issueType.Description,
                    Description = i.Description,
                    PropertyOrBuilderDescription = (property == null ? propertyGroup.Name : property.Address.ToSummary()),
                    StatusDescription = issueStatus.Description,
                    PropertyId = property != null ? property.Id : String.Empty,
                    PropertyGroupId = propertyGroup != null ? propertyGroup.Id : String.Empty,
                    AllowDelete = true
                };
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Adds audit event
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private Task<AuditEvent> AddAuditEvent(AuditEventTypes eventType, params object[] values)
        {
            // Create audit event
            var auditEventType = _auditEventTypeService.GetByEnum(eventType).Result;
            var auditEvent = new AuditEvent()
            {
                EventTypeId = auditEventType.Id,
                Parameters = new List<AuditEventParameter>()
            };

            // Add parameters
            for (int index =0; index < values.Length; index = index + 2)
            {
                SystemValueTypes valueType = (SystemValueTypes)values[index];
                var systemValueType = _systemValueTypeService.GetByEnum(valueType).Result;

                auditEvent.Parameters.Add(new AuditEventParameter()
                {
                    SystemValueTypeId = systemValueType.Id,
                    Value  = values[index + 1]
                });
            }

            _auditEventService.AddAsync(auditEvent).Wait();

            return Task.FromResult(auditEvent);
        }

        /// <summary>
        /// Processes form to add or update message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditMessageForm(MessageVM message)
        {
            if (!ModelState.IsValid)
            {
                return View(message);
            }

            // Get message from DB if updating
            var messageDB = String.IsNullOrEmpty(message.Id) ? null : _messageService.GetByIdAsync(message.Id);
            if (!String.IsNullOrEmpty(message.Id) && messageDB == null)
            {
                throw new ArgumentException("Invalid message");
            }

            // Map VM to message
            var messageUpdate = _mapper.Map<Message>(message);
            //_messageService.UpdateAsync(messageUpdate).Wait();

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(message.Id) ? 
                        AuditEventTypes.MessageAdded : AuditEventTypes.MessageUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.MessageId, message.Id);

            // Display updated message
            return RedirectToAction(nameof(HomeController.Message), new { id = message.Id });
        }

        /// <summary>
        /// Processes form to add or update issue
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditIssueForm(IssueVM issue)
        {
            if (!ModelState.IsValid)
            {
                return View(issue);
            }

            // Get issue from DB if updating
            var issueDB = String.IsNullOrEmpty(issue.Id) ? null : _issueService.GetByIdAsync(issue.Id).Result;
            if (!String.IsNullOrEmpty(issue.Id) && issueDB == null)
            {
                throw new ArgumentException("Invalid issue");
            }

            // Map VM to issue
            var issueUpdated = _mapper.Map<Issue>(issue);
            //_issueService.UpdateAsync(issueUpdated).Wait();

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(issue.Id) ? 
                        AuditEventTypes.IssueAdded : AuditEventTypes.IssueUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.IssueId, issue.Id);

            // Display updated issue details
            return RedirectToAction(nameof(HomeController.Issue), new { id = issue.Id });
        }

        /// <summary>
        /// Processes form to add or update property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditPropertyForm(PropertyVM property)
        {
            if (!ModelState.IsValid)
            {
                return View(property);
            }

            // Get property from DB if updating
            var properyDB = String.IsNullOrEmpty(property.Id) ? null : _propertyService.GetByIdAsync(property.Id).Result;
            if (!String.IsNullOrEmpty(property.Id) && properyDB == null)
            {
                throw new ArgumentException("Invalid property");
            }

            // Map VM to property
            var propertyUpdated = _mapper.Map<Property>(property);
            //_propertyService.UpdateAsync(propertyUpdated).Wait();

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(property.Id) ? 
                        AuditEventTypes.PropertyAdded : AuditEventTypes.PropertyUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyId, property.Id);         

            // Display updated property details
            return RedirectToAction(nameof(HomeController.Property), new { id=property.Id } );            
        }

        /// <summary>
        /// Processes form to add or update property group
        /// </summary>
        /// <param name="propertyGroup"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditPropertyGroupForm(PropertyGroupVM propertyGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyGroup);
            }

            // Get property group from DB if updating
            var propertyGroupDB = String.IsNullOrEmpty(propertyGroup.Id) ? null : _propertyGroupService.GetByIdAsync(propertyGroup.Id).Result;
            if (!String.IsNullOrEmpty(propertyGroup.Id) && propertyGroupDB == null)
            {
                throw new ArgumentException("Invalid property group");
            }

            // Map VM to property group
            var propertyGroupUpdated = _mapper.Map<PropertyGroup>(propertyGroup);
            //_propertyGroupService.UpdateAsync(propertyGroupUpdated).Wait();

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(propertyGroup.Id) ? 
                        AuditEventTypes.PropertyGroupAdded : AuditEventTypes.PropertyGroupUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyGroupId, propertyGroup.Id);

            // Display updated property details
            return RedirectToAction(nameof(HomeController.PropertyGroup), new { id = propertyGroup.Id });
        }

        /// <summary>
        /// Processes form to add or update property owner
        /// </summary>
        /// <param name="propertyOwner"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditPropertyOwnerForm(PropertyOwnerVM propertyOwner)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyOwner);
            }

            // Get property owner from DB if updating
            var propertyOwnerDB = String.IsNullOrEmpty(propertyOwner.Id) ? null : _propertyOwnerService.GetByIdAsync(propertyOwner.Id).Result;
            if (!String.IsNullOrEmpty(propertyOwner.Id) && propertyOwnerDB == null)
            {
                throw new ArgumentException("Invalid property owner");
            }

            // Map VM to employee
            var propertyOwnerUpdated = _mapper.Map<PropertyOwner>(propertyOwner);
            //_propertyOwnerService.UpdateAsync(propertyOwnerUpdated).Wait();
            
            // Add audit event           
            var auditEventType = String.IsNullOrEmpty(propertyOwner.Id) ? 
                        AuditEventTypes.PropertyOwnerAdded : AuditEventTypes.PropertyOwnerUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyOwnerId, propertyOwner.Id);

            // Display updated details
            return RedirectToAction(nameof(HomeController.PropertyOwner), new { id=propertyOwner.Id });
        }

        /// <summary>
        /// Processes form to add or update employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IActionResult CreateEditEmployeeForm(EmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            // Get employee from DB if updating
            var employeeDB = String.IsNullOrEmpty(employee.Id) ? null : _employeeService.GetByIdAsync(employee.Id).Result;
            if (!String.IsNullOrEmpty(employee.Id) && employeeDB == null)
            {
                throw new ArgumentException("Invalid employee");
            }

            // Map VM to employee
            var employeeUpdate = _mapper.Map<PropertyOwner>(employee);
            //_employeeService.UpdateAsync(employeeUpdate).Wait();

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(employee.Id) ? 
                        AuditEventTypes.EmployeeAdded : AuditEventTypes.EmployeeUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.EmployeeId, employee.Id);          

            // Display updated employee details
            return RedirectToAction(nameof(HomeController.Employee), new { id = employee.Id });
        }

        /// <summary>
        /// Processes form to filter audit events
        /// </summary>
        /// <param name="auditEventList"></param>
        /// <returns></returns>
        public IActionResult FilterAuditEventsForm(AuditEventListVM auditEventList)
        {          
            return RedirectToAction(nameof(HomeController.AllAuditEventList), auditEventList.Filter);            
        }

        /// <summary>
        /// Processes form to reset filter for audit events
        /// </summary>
        /// <param name="auditEventList"></param>
        /// <returns></returns>
        public IActionResult ResetFilterAuditEventsForm()
        {
            return RedirectToAction(nameof(HomeController.AllAuditEventList));
        }

        /// <summary>
        /// Processes form to filter issues
        /// </summary>
        /// <param name="issueList"></param>
        /// <returns></returns>
        public IActionResult FilterIssuesForm(IssueListVM issueList)
        {
            return RedirectToAction(nameof(HomeController.AllIssueList), issueList.Filter);
        }

        /// <summary>
        /// Processes form to reset filter for issues
        /// </summary>
        /// <param name="issueList"></param>
        /// <returns></returns>
        public IActionResult ResetFilterIssuesForm()
        {
            return RedirectToAction(nameof(HomeController.AllIssueList));
        }

        /// <summary>
        /// Processes form to filter properties
        /// </summary>
        /// <param name="auditEventList"></param>
        /// <returns></returns>
        public IActionResult FilterPropertiesForm(PropertyListVM propertyList)
        {
            return RedirectToAction(nameof(HomeController.AllPropertyList), propertyList.Filter);
        }

        /// <summary>
        /// Processes form to reset filter for properties
        /// </summary>
        /// <param name="auditEventList"></param>
        /// <returns></returns>
        public IActionResult ResetFilterPropertiesForm()
        {
            return RedirectToAction(nameof(HomeController.AllPropertyList));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
