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
            var properties = _propertyService.GetAll();
            var propertyGroups = _propertyGroupService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();

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

        /// <summary>
        /// Returns view to add or update property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Property(string id)
        {
            var propertyGroups = _propertyGroupService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();

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

            var propertyGroups = _propertyGroupService.GetAll();
            var propertyOwners = _propertyOwnerService.GetAll();
            var properties = _propertyService.GetAll();

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

            model.PropertyOwners = _propertyOwnerService.GetAll().Select(po =>
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
                var propertyOwner = _propertyOwnerService.GetByIdAsync(id).Result;
                var messageTypes = _messageTypeService.GetAll();

                var propertyGroups = _propertyGroupService.GetAll();

                var documents = propertyOwner.DocumentIds == null ?
                new List<DocumentBasicVM>() :
                propertyOwner.DocumentIds.Select(documentId => _documentService.GetByIdAsync(documentId).Result)
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

            model.PropertyGroups = _propertyGroupService.GetAll().Select(p =>
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
            var model = new EmployeeListVM();

            model.Employees = _employeeService.GetAll().Select(e =>
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
            var issueStatuses = _issueStatusService.GetAll();
            var issueTypes = _issueTypeService.GetAll();

            var propertyGroups = _propertyGroupService.GetAll();

            // Get issues (All issues/Issue type issues/Property issues)
            List<Issue> issues = null;
            if (!String.IsNullOrEmpty(issueTypeId)) issues = _issueService.GetByIssueType(issueTypeId).Result;
            //if (!String.IsNullOrEmpty(propertyId)) issues = _issueService.GetByProperty(propertyId).Result;
            if (issues == null) issues = _issueService.GetAll().ToList();

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
           
            var properties = _propertyService.GetAll();

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
