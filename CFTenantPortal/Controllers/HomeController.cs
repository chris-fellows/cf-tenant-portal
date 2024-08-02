using CFTenantPortal.Enums;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Abstractions;
using System.Diagnostics;
using System.Net.WebSockets;

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
        public IActionResult Issue(string id)
        {
            var employees = _employeeService.GetAll().Result;
            var issueStatuses = _issueStatusService.GetAll().Result;
            var issueTypes = _issueTypeService.GetAll().Result;
            var properties = _propertyService.GetAll().Result;
            var propertyGroups = _propertyGroupService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;

            var entityReferenceNone = EntityReference.None;
            
            if (String.IsNullOrEmpty(id))   // Create issue
            {                              
                var issueStatus = issueStatuses.First();
                var issueType = issueTypes.First();
                var property = properties.First();
                var createdByEmployee = employees.First();
                var createdByPropertyOwner = propertyOwners.First();

                var model = new IssueVM()
                {
                    HeaderText = "New Issue",
                    Reference = Guid.NewGuid().ToString(),
                    Description = "New",
                    TypeId = issueType.Id,
                    PropertyId = entityReferenceNone.Id,
                    PropertyGroupId = entityReferenceNone.Id,
                    CreatedEmployeeId = entityReferenceNone.Id,
                    CreatedPropertyOwnerId = entityReferenceNone.Id,     
                    Documents = new List<DocumentBasicVM>(),
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
                var issue = _issueService.GetById(id).Result;          

                var issueType = _issueTypeService.GetById(issue.TypeId).Result;
                var property = String.IsNullOrEmpty(issue.PropertyId) ?
                               null :
                               _propertyService.GetById(issue.PropertyId).Result;

                var documents = issue.DocumentIds == null ?
                           new List<DocumentBasicVM>() :
                           issue.DocumentIds.Select(documentId => _documentService.GetById(documentId).Result)
                           .Select(document => new DocumentBasicVM()
                           {
                               Id = document.Id,
                               Name = document.Name
                           }).ToList();
                           
                var model = new IssueVM()
                {
                    HeaderText = "Issue",
                    Id = issue.Id,
                    Reference = issue.Reference,
                    Description = issue.Description,
                    TypeId = issue.TypeId,
                    PropertyId = issue.PropertyId,
                    PropertyGroupId = issue.PropertyGroupId,
                    CreatedEmployeeId = issue.CreatedEmployeeId,
                    CreatedPropertyOwnerId = issue.CreatedPropertyOwnerId,                    
                    Documents = documents,                   
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

                // Add none for optional
                model.EmployeeList.Insert(0, EntityReference.None);
                model.PropertyGroupList.Insert(0, EntityReference.None);
                model.PropertyList.Insert(0, EntityReference.None);
                model.PropertyOwnerList.Insert(0, EntityReference.None);

                return View(model);
            }
        }

        public IActionResult Property(string id)
        {
            var propertyGroups = _propertyGroupService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;

            if (String.IsNullOrEmpty(id))   // New property
            {              
                var model = new PropertyVM()
                {
                    HeaderText = "New Property",
                    Address = new AddressVM()
                    {

                    },
                    Documents = new List<DocumentBasicVM>(),
                    Issues = new List<IssueBasicVM>(),
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
                var property = _propertyService.GetById(id).Result;
                var propertyGroup = _propertyGroupService.GetById(property.GroupId).Result;
                var propertyOwner = _propertyOwnerService.GetById(property.OwnerId).Result;

                var accountTransactionTypes = _accountTransactionTypeService.GetAll().Result;

                var issueStatuses = _issueStatusService.GetAll().Result;
                var issueTypes = _issueTypeService.GetAll().Result;

                var documents = property.DocumentIds == null ?
                   new List<DocumentBasicVM>() :
                   property.DocumentIds.Select(documentId => _documentService.GetById(documentId).Result)
                   .Select(document => new DocumentBasicVM()
                   {
                       Id = document.Id,
                       Name = document.Name
                   }).ToList();

                // TODO: Use auto mapping
                var model = new PropertyVM()
                {
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
                    Documents = documents,
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
                model.Issues = _issueService.GetByProperty(property.Id).Result.Select(i =>
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
                        PropertyId = property.Id
                    };
                }).ToList();

                return View(model);
            }
        }

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
                var employee = _employeeService.GetById(id).Result;


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

        public IActionResult Message(string id)
        {
            var messageTypes = _messageTypeService.GetAll().Result;
            var properties = _propertyService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;

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
                var message = _messageService.GetById(id).Result;

                var documents = message.DocumentIds == null ?
                       new List<DocumentBasicVM>() :
                       message.DocumentIds.Select(documentId => _documentService.GetById(documentId).Result)
                       .Select(document => new DocumentBasicVM()
                       {
                           Id = document.Id,
                           Name = document.Name
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

        public IActionResult AllPropertyList()  //string? propertyGroupId, string? propertyOwnerId)
        {
            var model = new PropertyListVM() { HeaderText = "Property List" };   // Default header

            /*
            // TODO: Make this more efficient
            var propertyGroups = _propertyGroupService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;

            // Get properties (All properties/Specific group/Specific owner)
            List<Property> properties = null;            
            if (!String.IsNullOrEmpty(propertyGroupId))   // Properties by group
            {
                properties = _propertyService.GetByPropertyGroup(propertyGroupId).Result;
                var propertyGroupMain = propertyGroups.First(pg => pg.Id == propertyGroupId);
                model.HeaderText = $"Property List : {propertyGroupMain.Name}";
            }
            else if (!String.IsNullOrEmpty(propertyOwnerId))  // Properties by owner
            {
                properties = _propertyService.GetAll().Result.Where(p => p.OwnerId == propertyOwnerId).ToList();
                var propertyOwnerMain = propertyOwners.First(po => po.Id == propertyOwnerId);
                model.HeaderText = $"Property List : {propertyOwnerMain.Name}";
            }
            else    // All properties
            {
                properties = _propertyService.GetAll().Result;
            } 
            */

            var propertyGroups = _propertyGroupService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;
            var properties = _propertyService.GetAll().Result;

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
                    PropertyOwnerId = propertyOwner.Id                    
                };
            }).ToList();

            return View(model);
        }

        public IActionResult AllPropertyOwnerList()
        {
            var model = new PropertyOwnerListVM() { HeaderText = "Property Owner List" };

            model.PropertyOwners = _propertyOwnerService.GetAll().Result.Select(po =>
            {
                return new PropertyOwnerVM()
                {
                    Id = po.Id,
                    Email = po.Email,
                    Name = po.Name
                };
            }).ToList();

            return View(model);
        }

        public IActionResult PropertyOwner(string? id)
        {
            if (String.IsNullOrEmpty(id))   // New property owner
            {
                var model = new PropertyOwnerVM()
                {                    
                    HeaderText = "New Property Owner",
                    Name = "New",                    
                    Address = new AddressVM(),
                    Documents = new List<DocumentBasicVM>(),
                    Properties = new List<PropertyBasicVM>(),
                    Messages = new List<MessageBasicVM>()
                };

                return View(model);
            }
            else   // Display or edit property ownert
            {
                var propertyOwner = _propertyOwnerService.GetById(id).Result;
                var messageTypes = _messageTypeService.GetAll().Result;

                var propertyGroups = _propertyGroupService.GetAll().Result;

                var documents = propertyOwner.DocumentIds == null ?
                new List<DocumentBasicVM>() :
                propertyOwner.DocumentIds.Select(documentId => _documentService.GetById(documentId).Result)
                .Select(document => new DocumentBasicVM()
                {
                    Id = document.Id,
                    Name = document.Name
                }).ToList();

                var model = new PropertyOwnerVM()
                {
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
                    Documents = documents
                };

                // Load properties
                model.Properties = _propertyService.GetByPropertyOwner(propertyOwner.Id).Result.Select(p =>
                {
                    var propertyGroup = propertyGroups.First(pg => pg.Id == p.GroupId);

                    return new PropertyBasicVM()
                    {
                        Id = p.Id,
                        AddressDescription = p.Address.ToSummary(),
                        PropertyGroupName = propertyGroup.Name,
                        PropertyGroupId = propertyGroup.Id,
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id
                    };
                }).ToList();

                // Load messages
                model.Messages = _messageService.GetByPropertyOwner(propertyOwner.Id).Result.Select(m =>
                {
                    var messageType = messageTypes.First(mt => mt.Id == m.MessageTypeId);

                    return new MessageBasicVM()
                    {
                        Id = m.Id,
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id,
                        TypeDescription = messageType.Description                        
                    };
                }).ToList();

                return View(model);
            }
        }

        public IActionResult AllPropertyGroupList()
        {
            var model = new PropertyGroupListVM() { HeaderText = "Property Groups " };   // Default header

            model.PropertyGroups = _propertyGroupService.GetAll().Result.Select(p =>
            {
                return new PropertyGroupVM()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                };
            }).ToList();

            return View(model);
        }

        public IActionResult PropertyGroup(string? id)
        {            
            if (String.IsNullOrEmpty(id))   // New property group
            {
                var model = new PropertyGroupVM()
                {
                    HeaderText = "New Property Group",
                    Name = "New",
                    Description = "New",
                    Documents = new List<DocumentBasicVM>(),
                    Issues = new List<IssueBasicVM>(),
                    Properties = new List<PropertyBasicVM>()
                };

                return View(model);
            }
            else     // Display or edit property group
            {
                var propertyGroup = _propertyGroupService.GetById(id).Result;
                var propertyOwners = _propertyOwnerService.GetAll().Result;

                var issueStatuses = _issueStatusService.GetAll().Result;
                var issueTypes = _issueTypeService.GetAll().Result;

                var documents = propertyGroup.DocumentIds == null ?
                      new List<DocumentBasicVM>() :
                      propertyGroup.DocumentIds.Select(documentId => _documentService.GetById(documentId).Result)
                      .Select(document => new DocumentBasicVM()
                      {
                          Id = document.Id,
                          Name = document.Name
                      }).ToList();

                var model = new PropertyGroupVM()
                {
                    HeaderText = "Property Group",
                    Id = propertyGroup.Id,
                    Name = propertyGroup.Name,
                    Description = propertyGroup.Description,
                    Documents = documents
                };

                // Load properties               
                model.Properties = _propertyService.GetByPropertyGroup(propertyGroup.Id).Result.Select(p =>
                {
                    var propertyOwner = propertyOwners.First(po => po.Id == p.OwnerId);
                    return new PropertyBasicVM()
                    {
                        Id = p.Id,
                        AddressDescription = p.Address.ToSummary(),
                        PropertyGroupName = propertyGroup.Name,
                        PropertyGroupId = propertyGroup.Id,
                        PropertyOwnerName = propertyOwner.Name,
                        PropertyOwnerId = propertyOwner.Id
                    };
                }).ToList();

                // Load issues
                model.Issues = _issueService.GetByPropertyGroup(propertyGroup.Id).Result.Select(i =>
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
                        PropertyGroupId = propertyGroup.Id
                    };
                }).ToList();                

                return View(model);
            }
        }

        public IActionResult AllIssueTypeList()
        {
            var model = new IssueTypeListVM() { HeaderText = "Issue Types" };   // Default header

            model.IssueTypes = _issueTypeService.GetAll().Result.Select(it =>
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
            var model = new EmployeeListVM();

            model.Employees = _employeeService.GetAll().Result.Select(e =>
            {
                return new EmployeeBasicVM()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Active = e.Active
                };
            }).ToList();

            return View(model);
        }

        public IActionResult AllIssueList(string? issueTypeId)  //, string? propertyId)
        {
            var model = new IssueListVM() { HeaderText = "Issue List" };   // Default header

            // Get properties (All properties/Specific group)
            var issueStatuses = _issueStatusService.GetAll().Result;
            var issueTypes = _issueTypeService.GetAll().Result;

            var propertyGroups = _propertyGroupService.GetAll().Result;

            // Get issues (All issues/Issue type issues/Property issues)
            List<Issue> issues = null;
            if (!String.IsNullOrEmpty(issueTypeId)) issues = _issueService.GetByIssueType(issueTypeId).Result;
            //if (!String.IsNullOrEmpty(propertyId)) issues = _issueService.GetByProperty(propertyId).Result;
            if (issues == null) issues = _issueService.GetAll().Result;

            // Get issue type if set
            var issueTypeMain = String.IsNullOrEmpty(issueTypeId) ?
                        null :
                        issueTypes.First(it => it.Id == issueTypeId);
            if (issueTypeMain != null) model.HeaderText = $"Issue List : {issueTypeMain.Description}";

            //// Get property if set
            //var propertyMain = String.IsNullOrEmpty(propertyId) ?
            //            null :
            //            _propertyService.GetById(propertyId).Result;                        
            //if (propertyMain != null) model.HeaderText = $"Issue List : {propertyMain.Address.ToSummary()}";
           
            var properties = _propertyService.GetAll().Result;

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
                    PropertyGroupId = propertyGroup != null ? propertyGroup.Id : String.Empty
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

            _auditEventService.Add(auditEvent).Wait();

            return Task.FromResult(auditEvent);
        }

        public IActionResult CreateEditMessageForm(MessageVM message)
        {
            // TODO: Save message

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(message.Id) ? 
                        AuditEventTypes.MessageAdded : AuditEventTypes.MessageUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.MessageId, message.Id);

            // Display updated message
            return RedirectToAction(nameof(HomeController.Message), new { id = message.Id });
        }

        public IActionResult CreateEditIssueForm(IssueVM issue)
        {
            // TODO: Save issue

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(issue.Id) ? 
                        AuditEventTypes.IssueAdded : AuditEventTypes.IssueUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.IssueId, issue.Id);

            // Display updated issue details
            return RedirectToAction(nameof(HomeController.Issue), new { id = issue.Id });
        }
      
        public IActionResult CreateEditPropertyForm(PropertyVM property)
        {
            // TODO: Save property

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(property.Id) ? 
                        AuditEventTypes.PropertyAdded : AuditEventTypes.PropertyUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyId, property.Id);         

            // Display updated property details
            return RedirectToAction(nameof(HomeController.Property), new { id=property.Id } );            
        }

        public IActionResult CreateEditPropertyGroupForm(PropertyGroupVM propertyGroup)
        {
            // TODO: Save property group

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(propertyGroup.Id) ? 
                        AuditEventTypes.PropertyGroupAdded : AuditEventTypes.PropertyGroupUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyGroupId, propertyGroup.Id);

            // Display updated property details
            return RedirectToAction(nameof(HomeController.PropertyGroup), new { id = propertyGroup.Id });
        }

        public IActionResult CreateEditPropertyOwnerForm(PropertyOwnerVM propertyOwner)
        {
            // TODO: Save property owner

            // Add audit event           
            var auditEventType = String.IsNullOrEmpty(propertyOwner.Id) ? 
                        AuditEventTypes.PropertyOwnerAdded : AuditEventTypes.PropertyOwnerUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.PropertyOwnerId, propertyOwner.Id);

            // Display updated details
            return RedirectToAction(nameof(HomeController.PropertyOwner), new { id=propertyOwner.Id });
        }

        public IActionResult CreateEditEmployeeForm(EmployeeVM employee)
        {
            // TODO: Save employee

            // Add audit event
            var auditEventType = String.IsNullOrEmpty(employee.Id) ? 
                        AuditEventTypes.EmployeeAdded : AuditEventTypes.EmployeeUpdated;
            var auditEvent = AddAuditEvent(auditEventType, SystemValueTypes.EmployeeId, employee.Id);          

            // Display updated employee details
            return RedirectToAction(nameof(HomeController.Employee), new { id = employee.Id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
