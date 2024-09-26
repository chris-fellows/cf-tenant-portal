using CFTenantPortal.Enums;
using CFTenantPortal.Export.CSV;
using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFUtilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using CFTenantPortal.Web.Models;
using CFUtilities.Utilities;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CFTenantPortal.Controllers
{
    /*
     * 
     * Q) If we wanted to add an "Export to CSV" button on the list components (E.g. Property list) then how would it work?

    A)
    - When we pass the model to the component then we need to set the filter properties. Currently they're only set
      from the "All Properties" component.
    - If we were displaying the page for a property and we wanted an Export button the issue list then we'd set
      PropertyVM.IssueListVM.Filter.PropertyId = PropertyVM.PropertyId. When the Export button is clicked then the
      issue list component would pass Model.Filter
    */
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
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IMessageTypeService _messageTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyOwnerService _propertyOwnerService;
        private readonly IPropertyService _propertyService;      
        private readonly IRequestInfoService _requestInfoService;
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
                ILoginService securityService,
                IMapper mapper,
                IMessageService messageService,
                IMessageTypeService messageTypeService,
                IPropertyGroupService propertyGroupService,
                IPropertyOwnerService propertyOwnerService,
                IPropertyService propertyService,      
                IRequestInfoService requestInfoService,
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
            _loginService = securityService;
            _mapper = mapper;
            _messageService = messageService;
            _messageTypeService = messageTypeService;
            _propertyGroupService = propertyGroupService;
            _propertyOwnerService = propertyOwnerService;
            _propertyService = propertyService;
            _requestInfoService = requestInfoService;
            _systemValueTypeService = systemValueTypeService;
        }

        //private bool IsLoggedIn
        //{
        //    get
        //    {
        //        return HttpContext.User.Identity != null &&
        //            HttpContext.User.Identity.IsAuthenticated;

        //        /*
        //        if (HttpContext.Session != null)
        //        {
        //            return HttpContext.Session.Keys.Contains("UserId");
        //        }
        //        return false;
        //        */
        //    }
        //}        

        public IActionResult Login()
        {
            return View();
        }
        
        /// <summary>
        /// Processes form to log out
        /// </summary>
        /// <returns></returns>
        public IActionResult LogoutForm()
        {
            var method = HttpContext.Request.Method;

            if (_requestInfoService.IsLoggedIn)
            {
                // Get employee or property owner
                var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var employee = _employeeService.GetByIdAsync(userId).Result;
                var propertyOwner = employee == null ? _propertyOwnerService.GetByIdAsync(userId).Result : null;

                // Set system value type for user (EmployeeId/PropertyOwnerId)
                SystemValueType userSystemValueType = new();
                if (employee != null)
                {
                    userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.EmployeeId).Result;
                }
                else if (propertyOwner != null)
                {
                    userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.PropertyOwnerId).Result;
                }

                // Sign out
                HttpContext.SignOutAsync().Wait();
                
                // Create UserLoggedOut audit event
                var auditEventType = _auditEventTypeService.GetByEnum(AuditEventTypes.UserLoggedOut).Result;
                if (auditEventType != null)
                {
                    var auditEvent = new AuditEvent()
                    {
                        EventTypeId = auditEventType.Id,                        
                        Parameters = new List<AuditEventParameter>()
                        {
                            new AuditEventParameter()
                            {
                                SystemValueTypeId = userSystemValueType.Id,
                                Value = userId
                            }
                        }
                    };

                    _auditEventService.AddAsync(auditEvent).Wait();
                }                             
            }
                        
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Processes form to log in
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public IActionResult LoginForm(LoginVM login)
        {
            var method = HttpContext.Request.Method;

            // Check if logged in            
            if (_requestInfoService.IsLoggedIn)
            {
                return RedirectToAction(nameof(Index));
            }            

            // https://stackoverflow.com/questions/73748488/passing-username-and-password-to-controller-method-asp-net-mvc
            // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
            if (ModelState.IsValid)
            {
                var authenticateResult = _loginService.AuthenticateAsync(login.Email, login.Password).Result;
                if (authenticateResult != null)
                {
                    var claims = new List<Claim>();
                    SystemValueType userSystemValueType = new();    // EmployeeId/PropertyOwnerId

                    if (authenticateResult is Employee)
                    {
                        userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.EmployeeId).Result;

                        var employee = (Employee)authenticateResult;
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(employee.Id)));
                        claims.Add(new Claim(ClaimTypes.Name, employee.Name));
                        if (employee.Roles != null)
                        {
                            claims.AddRange(employee.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));
                        }

                        /*
                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(employee.Id)),
                                new Claim(ClaimTypes.Name, employee.Name)
                        };
                        */
                        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        //{
                        //    IsPersistent = user.RememberLogin
                        //});
                        //return LocalRedirect(user.ReturnUrl);                     
                    }
                    else if (authenticateResult is PropertyOwner)
                    {
                        userSystemValueType = _systemValueTypeService.GetByEnum(SystemValueTypes.PropertyOwnerId).Result;

                        var propertyOwner = (PropertyOwner)authenticateResult;
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(propertyOwner.Id)));
                        claims.Add(new Claim(ClaimTypes.Name, propertyOwner.Name));
                        claims.Add(new Claim(ClaimTypes.Role, UserRoles.PropertyOwner.ToString())); // TODO: Add PropertyOwner.Roles
                        //if (propertyOwner.Roles != null)
                        //{
                        //    claims.AddRange(propertyOwner.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));
                        //}
                    }

                    // Create claims principle
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = true
                    }).Wait();

                    // Create UserLoggedIn audit event
                    var auditEventType = _auditEventTypeService.GetByEnum(AuditEventTypes.UserLoggedIn).Result;
                    if (auditEventType != null)
                    {
                        var auditEvent = new AuditEvent()
                        {
                            EventTypeId = auditEventType.Id,
                            Parameters = new List<AuditEventParameter>()
                                {
                                    new AuditEventParameter()
                                    {
                                        SystemValueTypeId = userSystemValueType.Id,
                                        Value = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value
                                    }
                                }
                        };

                        _auditEventService.AddAsync(auditEvent).Wait();
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Invalid email or password";
                }     
            }          

            return RedirectToAction(nameof(Login));
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
                        Documents = new List<DocumentBasicVM>(),
                        Filter = new DocumentFilterVM()                                  
                    },
                    EmployeeRefList = employees.Select(e =>
                    {
                        return _mapper.Map<EntityReference>(e);
                    }).ToList(),
                    IssueStatusId = issueStatus.Id,
                    IssueStatusRefList = issueStatuses.Select(i =>
                    {
                        return _mapper.Map<EntityReference>(i);
                    }).ToList(),
                    IssueTypeRefList = issueTypes.Select(i =>
                    {
                        return _mapper.Map<EntityReference>(i);
                    }).ToList(),
                    MessageList = new MessageListVM()
                    {
                        Messages= new List<MessageBasicVM>(),
                        Filter = new MessageFilterVM()
                    },
                    PropertyGroupRefList = propertyGroups.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);                        
                    }).ToList(),
                    PropertyRefList = properties.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);
                    }).ToList(),
                    PropertyOwnerRefList = propertyOwners.Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);
                    }).ToList(),
                };

                // Add none for optional
                model.EmployeeRefList.Insert(0, EntityReference.None);
                model.PropertyGroupRefList.Insert(0, EntityReference.None);
                model.PropertyRefList.Insert(0, EntityReference.None);                
                model.PropertyOwnerRefList.Insert(0, EntityReference.None);

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
                        Documents = documents,
                        Filter = new DocumentFilterVM()
                        {
                            IssueId = issue.Id
                        }
                    },
                    EmployeeRefList = employees.Select(e =>
                    {
                        return _mapper.Map<EntityReference>(e);                        
                    }).ToList(),
                    IssueStatusId = issue.StatusId,
                    IssueStatusRefList = issueStatuses.Select(i =>
                    {
                        return _mapper.Map<EntityReference>(i);                        
                    }).ToList(),
                    IssueTypeRefList = issueTypes.Select(i =>
                    {
                        return _mapper.Map<EntityReference>(i);
                    }).ToList(),
                    MessageList = new MessageListVM()
                    {
                        Filter = new MessageFilterVM()
                        {
                            IssueId = issue.Id
                        }
                    },
                    PropertyGroupRefList = propertyGroups.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);                        
                    }).ToList(),
                    PropertyRefList  = properties.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);                        
                    }).ToList(),
                    PropertyOwnerRefList = propertyOwners.Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);                    
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
                model.EmployeeRefList.Insert(0, EntityReference.None);
                model.PropertyGroupRefList.Insert(0, EntityReference.None);
                model.PropertyRefList.Insert(0, EntityReference.None);
                model.PropertyOwnerRefList.Insert(0, EntityReference.None);

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
                    DocumentList = new DocumentListVM() 
                    { 
                        Documents = new List<DocumentBasicVM>(),
                        Filter = new DocumentFilterVM()
                    },                  
                    //Issues = new List<IssueBasicVM>(),
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = false,
                        Issues = new List<IssueBasicVM>(),
                        Filter = new IssueFilterVM()                        
                    },
                    MessageList = new MessageListVM()
                    {
                        Messages = new List<MessageBasicVM>(),
                        Filter = new MessageFilterVM()
                    },
                    AccountTransactions = new List<AccountTransactionBasicVM>(),
                    PropertyGroupId = propertyGroups.OrderBy(pg => pg.Name).First().Id,
                    PropertyGroupRefList = propertyGroups.OrderBy(pg => pg.Name).Select(pg =>
                    {
                        return _mapper.Map<EntityReference>(pg);
                    }).ToList(),
                    PropertyOwnerId = propertyOwners.OrderBy(po => po.Name).First().Id,
                    PropertyOwnerRefList = propertyOwners.OrderBy(po => po.Name).Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);                        
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
                        Documents = documents,                                                 
                        Filter = new DocumentFilterVM()
                        {
                            PropertyId = property.Id
                        }
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = true,
                        Filter = new IssueFilterVM()
                        {
                            TestPropertyId = property.Id
                        }
                    },
                    MessageList = new MessageListVM()
                    {
                        Filter = new MessageFilterVM()
                        {
                            PropertyId = property.Id
                        }
                    },
                    PropertyGroupRefList = propertyGroups.OrderBy(pg => pg.Name).Select(pg =>
                    {
                        return _mapper.Map<EntityReference>(pg);                        
                    }).ToList(),
                    PropertyOwnerRefList = propertyOwners.OrderBy(po => po.Name).Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);                        ;
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
                    IssueRefList= new List<EntityReference>(),        
                    MessageTypeRefList = messageTypes.Select(mt =>
                    {
                        return _mapper.Map<EntityReference>(mt);                        
                    }).ToList(),
                    PropertyRefList = properties.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);
                    }).ToList(),
                    PropertyOwnerRefList = propertyOwners.Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);
                    }).ToList(),
                };

                // Add None for optional properties
                model.IssueRefList.Insert(0, EntityReference.None);
                model.PropertyRefList.Insert(0, EntityReference.None);
                model.PropertyOwnerRefList.Insert(0, EntityReference.None);

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
                    IssueRefList  = new List<EntityReference>(),
                    MessageTypeId = message.MessageTypeId,
                    MessageTypeRefList = messageTypes.Select(mt =>
                    {
                        return _mapper.Map<EntityReference>(mt);                        
                    }).ToList(),
                    PropertyId = message.PropertyId,
                    PropertyRefList = properties.Select(p =>
                    {
                        return _mapper.Map<EntityReference>(p);                        
                    }).ToList(),
                    PropertyOwnerId = message.PropertyOwnerId,
                    PropertyOwnerRefList = propertyOwners.Select(po =>
                    {
                        return _mapper.Map<EntityReference>(po);                        
                    }).ToList(),
                    Text = message.Text
                };

                // Add None for optional properties
                model.IssueRefList.Insert(0, EntityReference.None);
                model.PropertyRefList.Insert(0, EntityReference.None);
                model.PropertyOwnerRefList.Insert(0, EntityReference.None);

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
                    PropertyGroupRefList = propertyGroups.Select(pg => _mapper.Map<EntityReference>(pg)).ToList(),
                    PropertyOwnerId = String.IsNullOrEmpty(filterVM.PropertyOwnerId) ? EntityReference.None.Id : filterVM.PropertyOwnerId,
                    PropertyOwnerRefList = propertyOwners.Select(po => _mapper.Map<EntityReference>(po)).ToList()                    
                }
            };

            // Add None to lists
            model.Filter.PropertyGroupRefList.Insert(0, EntityReference.None);            
            model.Filter.PropertyOwnerRefList.Insert(0, EntityReference.None);          

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
                HeaderText = "Property Owners",
                Filter = new PropertyOwnerFilterVM()
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
                        Documents = new List<DocumentBasicVM>(),
                        Filter = new DocumentFilterVM()
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = false,
                        Properties = new List<PropertyBasicVM>()
                    },
                    MessageList = new MessageListVM()
                    {
                        Messages = new List<MessageBasicVM>(),
                        Filter = new MessageFilterVM()                        
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
                        Documents = documents,
                        Filter = new DocumentFilterVM()
                        {
                            PropertyOwnerId = propertyOwner.Id
                        }
                    },
                    PropertyList = new PropertyListVM()
                    {
                        AllowCreate = true
                    },
                    MessageList = new MessageListVM()
                    {
                        Filter = new MessageFilterVM()
                        {
                            PropertyOwnerId = propertyOwner.Id
                        }
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
                HeaderText = "Property Groups",
                Filter = new PropertyGroupFilterVM()
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
                        Documents = new List<DocumentBasicVM>(),
                        Filter = new DocumentFilterVM()
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = false,
                        Issues = new List<IssueBasicVM>(),
                        Filter = new IssueFilterVM()                       
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
                        Documents = documents,
                        Filter = new DocumentFilterVM()
                        {
                            PropertyGroupId = propertyGroup.Id
                        }
                    },
                    IssueList = new IssueListVM()
                    {
                        AllowCreate = true,
                        Filter = new IssueFilterVM()
                        {
                            TestPropertyGroupId = propertyGroup.Id
                        }
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
                    AuditEventTypeRefList = auditEventTypes.Select(aet => _mapper.Map<EntityReference>(aet)).ToList(),
                    PropertyId = String.IsNullOrEmpty(filterVM.PropertyId) ? EntityReference.None.Id : filterVM.PropertyId,
                    PropertyRefList = properties.Select(p => _mapper.Map<EntityReference>(p)).ToList(),
                    PropertyGroupId = String.IsNullOrEmpty(filterVM.PropertyGroupId) ? EntityReference.None.Id : filterVM.PropertyGroupId,
                    PropertyGroupRefList = propertyGroups.Select(pg => _mapper.Map<EntityReference>(pg)).ToList(),
                    PropertyOwnerId = String.IsNullOrEmpty(filterVM.PropertyOwnerId) ? EntityReference.None.Id : filterVM.PropertyOwnerId,
                    PropertyOwnerRefList = propertyOwners.Select(po => _mapper.Map<EntityReference>(po)).ToList()
                }
            };

            // Set None for list options
            model.Filter.AuditEventTypeRefList.Insert(0, EntityReference.None);
            model.Filter.PropertyGroupRefList.Insert(0, EntityReference.None);
            model.Filter.PropertyRefList.Insert(0, EntityReference.None);
            model.Filter.PropertyOwnerRefList.Insert(0, EntityReference.None);

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
            var model = new IssueTypeListVM() 
            { 
                HeaderText = "Issue Types"
            };   // Default header

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
                HeaderText = "Employees",
                Filter = new EmployeeFilterVM()
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
                    IssueStatusRefList = issueStatuses.Select(pg => _mapper.Map<EntityReference>(pg)).ToList(),
                    IssueTypeId = String.IsNullOrEmpty(filterVM.IssueTypeId) ? EntityReference.None.Id : filterVM.IssueTypeId,
                    IssueTypeRefList = issueTypes.Select(po => _mapper.Map<EntityReference>(po)).ToList()
                }
            };

            // Add None to lists
            model.Filter.IssueStatusRefList.Insert(0, EntityReference.None);
            model.Filter.IssueTypeRefList.Insert(0, EntityReference.None);

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

        ///// <summary>
        ///// Processes form to export properties
        ///// </summary>
        ///// <param name="propertyList"></param>
        ///// <param name="format">Export format</param>
        ///// <returns></returns>
        //public IActionResult ExportPropertiesV2Form(PropertyFilterVM filterX, string format)
        //{
        //    // Set property filter
        //    var filter = new PropertyFilter()
        //    {
        //        //PropertyGroupIds = String.IsNullOrWhiteSpace(propertyList.Filter.PropertyGroupId) ||
        //        //                    propertyList.Filter.PropertyGroupId.Equals(EntityReference.None.Id) ?
        //        //            new() : new() { propertyList.Filter.PropertyGroupId },
        //        //PropertyOwnerIds = String.IsNullOrWhiteSpace(propertyList.Filter.PropertyOwnerId) ||
        //        //                    propertyList.Filter.PropertyOwnerId.Equals(EntityReference.None.Id) ?
        //        //            new() : new() { propertyList.Filter.PropertyOwnerId },
        //        PageNo = 1,
        //        PageItems = 10000000
        //    };

        //    // Get properties           
        //    var properties = _propertyService.GetByFilterAsync(filter).Result;

        //    int xxx = 1000;

        //    switch (format)
        //    {
        //        case "CSV": return ExportPropertiesToCSV(properties);
        //    }

        //    throw new ArgumentException("Invalid format");
        //}

        /// <summary>
        /// Processes form to export audit events
        /// </summary>
        /// <param name="propertyList"></param>
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportAuditEventsForm(AuditEventFilterVM? filter = null)
        {
            // Set filter
            var auditEventFilter = new AuditEventFilter()
            { 
               AuditEventTypeIds = String.IsNullOrEmpty(filter.AuditEventTypeId) ||
                                          filter.AuditEventTypeId.Equals(EntityReference.None.Id) ?
                                            new() : new() { filter.AuditEventTypeId },
                PropertyGroupIds = String.IsNullOrEmpty(filter.PropertyGroupId) ||
                                        filter.PropertyGroupId.Equals(EntityReference.None.Id) ?
                                            new() : new() { filter.PropertyGroupId },
                PropertyIds = String.IsNullOrEmpty(filter.PropertyId) ||
                                        filter.PropertyId.Equals(EntityReference.None.Id) ?
                                            new() : new() { filter.PropertyId },
                PropertyOwnerIds = String.IsNullOrEmpty(filter.PropertyOwnerId) ||
                                        filter.PropertyOwnerId.Equals(EntityReference.None.Id) ?
                                            new() : new() { filter.PropertyOwnerId },
                StartCreatedDateTime = filter.StartCreatedDateTime,
                EndCreatedDateTime = filter.EndCreatedDateTime,                
                PageNo = 1,
                PageItems = 10000000
            };

            // Get audit events           
            var auditEvents = _auditEventService.GetByFilterAsync(auditEventFilter).Result;

            int xxx = 1000;

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportAuditEventsToCSV(auditEvents);
            }

            throw new ArgumentException("Invalid format");
        }


        /// <summary>
        /// Processes form to export documents
        /// </summary>        
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportDocumentsForm(DocumentFilterVM? filter = null)
        {
            // Get documents           
            var documents = _documentService.GetAll().ToList();

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportDocumentsToCSV(documents);
            }

            throw new ArgumentException("Invalid format");
        }


        /// <summary>
        /// Processes form to export messages
        /// </summary>        
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportMessageForm(MessageFilterVM? filter = null)
        {
            // Get messages           
            var messages = _messageService.GetAll().ToList();

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportMessagesToCSV(messages);
            }

            throw new ArgumentException("Invalid format");
        }

        /// <summary>
        /// Processes form to export properties
        /// </summary>
        /// <param name="propertyList"></param>
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportPropertiesForm(PropertyFilterVM? filter= null)
        {
            // Set property filter
            var propertyFilter = new PropertyFilter()
            {
                PropertyGroupIds = String.IsNullOrWhiteSpace(filter.PropertyGroupId) ||
                                    filter.PropertyGroupId.Equals(EntityReference.None.Id) ?
                            new() : new() { filter.PropertyGroupId },
                PropertyOwnerIds = String.IsNullOrWhiteSpace(filter.PropertyOwnerId) ||
                                     filter.PropertyOwnerId.Equals(EntityReference.None.Id) ?
                            new() : new() { filter.PropertyOwnerId },
                PageNo = 1,
                PageItems = 10000000
            };

            // Get properties           
            var properties = _propertyService.GetByFilterAsync(propertyFilter).Result;

            int xxx = 1000;

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportPropertiesToCSV(properties);
            }

            throw new ArgumentException("Invalid format");
        }

        public IActionResult ExportIssuesForm(IssueFilterVM? filter)
        {
            // Set issue filter
            var issueFilter = new IssueFilter()
            {
                References = String.IsNullOrEmpty(filter.Reference) ?
                                new() : new() { filter.Reference },
                IssueStatusIds = String.IsNullOrWhiteSpace(filter.IssueStatusId) ||
                                    filter.IssueStatusId.Equals(EntityReference.None.Id) ?
                            new() : new() { filter.IssueStatusId },
                IssueTypeIds = String.IsNullOrWhiteSpace(filter.IssueTypeId) ||
                                    filter.IssueTypeId.Equals(EntityReference.None.Id) ?
                            new() : new() { filter.IssueTypeId },
                PageNo = 1,
                PageItems = 10000000
            };

            // Get properties           
            var issues = _issueService.GetByFilterAsync(issueFilter).Result;

            int xxx = 1000;

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportIssuesToCSV(issues);
            }

            throw new ArgumentException("Invalid format");
        }     

        /// <summary>
        /// Processes form to export property groups
        /// </summary>        
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportPropertyGroupsForm(PropertyGroupFilterVM? filter = null)
        {
            // Get properties           
            var propertyGroups = _propertyGroupService.GetAll().ToList();

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportPropertyGroupsToCSV(propertyGroups);
            }

            throw new ArgumentException("Invalid format");
        }

        /// <summary>
        /// Processes form to export property owners
        /// </summary>        
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportPropertyOwnersForm(PropertyOwnerFilterVM? filter= null)
        {
            // Get property owners         
            var propertyOwners = _propertyOwnerService.GetAll().ToList();

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportPropertyOwnersToCSV(propertyOwners);
            }

            throw new ArgumentException("Invalid format");
        }

        /// <summary>
        /// Processes form to export employees
        /// </summary>        
        /// <param name="format">Export format</param>
        /// <returns></returns>
        public IActionResult ExportEmployeesForm(EmployeeFilterVM? filter = null)
        {
            // Get employees
            var employees = _employeeService.GetAll().ToList();

            const string format = "CSV";
            switch (format)
            {
                case "CSV": return ExportEmployeesToCSV(employees);
            }

            throw new ArgumentException("Invalid format");
        }

        private IActionResult ExportAuditEventsToCSV(List<AuditEvent> auditEvents)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new AuditEventCSVExport();
                export.WriteAsync(auditEvents, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"AuditEvents{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportDocumentsToCSV(List<Document> documents)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new DocumentCSVExport();
                export.WriteAsync(documents, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);
                return File(fileContent, "text/csv", $"Documents{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportMessagesToCSV(List<Message> messages)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new MessageCSVExport();
                export.WriteAsync(messages, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);
                return File(fileContent, "text/csv", $"Messages{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportEmployeesToCSV(List<Employee> employees)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new EmployeeCSVExport();
                export.WriteAsync(employees, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"Employees{exportSettings.DefaultExtension}.csv");
            }
        }

        private IActionResult ExportPropertyGroupsToCSV(List<PropertyGroup> propertyGroups)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new PropertyGroupCSVExport();
                export.WriteAsync(propertyGroups, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"PropertyGroups{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportPropertyOwnersToCSV(List<PropertyOwner> propertyOwners)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new PropertyOwnerCSVExport();
                export.WriteAsync(propertyOwners, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"PropertyOwners{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportPropertiesToCSV(List<Property> properties)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new PropertyCSVExport();
                export.WriteAsync(properties, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"Properties{exportSettings.DefaultExtension}");
            }
        }

        private IActionResult ExportIssuesToCSV(List<Issue> issues)
        {
            using (var session = new DisposableActionsSession())
            {
                // Set CSV settings
                var exportSettings = new CSVExportSettings()
                {
                    File = Path.GetTempFileName(),
                    ColumnDelimiter = SystemConfig.DefaultCSVExportSettings.ColumnDelimiter,
                    Encoding = SystemConfig.DefaultCSVExportSettings.Encoding,
                    DefaultExtension = SystemConfig.DefaultCSVExportSettings.DefaultExtension
                };
                session.AddAction(() => { IOUtilities.DeleteFiles(new[] { exportSettings.File }); });

                // Export
                var export = new IssueCSVExport();
                export.WriteAsync(issues, exportSettings).Wait();

                var fileContent = System.IO.File.ReadAllBytes(exportSettings.File);                
                return File(fileContent, "text/csv", $"Issues{exportSettings.DefaultExtension}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
