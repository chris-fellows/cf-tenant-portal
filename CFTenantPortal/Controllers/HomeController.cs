using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using System.Diagnostics;

namespace CFTenantPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IIssueTypeService _issueTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyOwnerService _propertyOwnerService;
        private readonly IPropertyService _propertyService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                IIssueService issueService,
                IIssueTypeService issueTypeService,
                IPropertyGroupService propertyGroupService,
                IPropertyOwnerService propertyOwnerService,
                IPropertyService propertyService) 
        {
            _logger = logger;
            _issueService = issueService;
            _issueTypeService = issueTypeService;
            _propertyGroupService = propertyGroupService;
            _propertyOwnerService = propertyOwnerService;
            _propertyService = propertyService;
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
            var issue = _issueService.GetById(id).Result;
            var issueType = _issueTypeService.GetById(issue.IssueTypeId).Result;
            var property = String.IsNullOrEmpty(issue.PropertyId) ?
                           null :
                           _propertyService.GetById(issue.PropertyId).Result;

            var model = new IssueModel()
            {
                Id = issue.Id,
                Description = issue.Description,
                IssueTypeId = issue.IssueTypeId,
                IssueTypeDescription = issueType.Description,
                PropertyDescription = property == null ? "" : property.Address.ToSummary(),
                Status = issue.Status,
                StatusDescription = issue.Status.ToString(),     // TODO: Set correctly                                  
                IssueTypeList = _issueTypeService.GetAll().Result.Select(it =>
                {
                    return new IssueTypeModel()
                    {
                        Id = it.Id,
                        Description = it.Description
                    };
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Property(string id)
        {
            var property = _propertyService.GetById(id).Result;
            var propertyGroup = _propertyGroupService.GetById(property.GroupId).Result;
            var propertyOwner = _propertyOwnerService.GetById(property.OwnerId).Result;

            var issueTypes = _issueTypeService.GetAll().Result;

            var model = new PropertyModel2()
            {
                Id = property.Id,
                PropertyGroupName = propertyGroup.Name,
                AddressDescription = property.Address.ToSummary(),
                PropertyOwnerName = propertyOwner.Name                
            };

            // Load issues
            model.Issues = _issueService.GetByProperty(property.Id).Result.Select(i =>
            {
                var issueType = issueTypes.First(it => it.Id == i.IssueTypeId);

                return new IssueModel()
                {
                    Id = i.Id,
                    Description = i.Description, 
                    IssueTypeDescription = issueType.Description,
                    PropertyDescription = property.Address.ToSummary(),
                    StatusDescription = i.Status.ToString()     // TODO: Set properly
                };
            }).ToList();

            return View(model);
        }

        public IActionResult PropertyList(string? propertyGroupId, string? propertyOwnerId)
        {
            var model = new PropertyListModel() { HeaderText = "Property List" };   // Default header

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

            model.Properties = properties.Select(p =>
            {
                var propertyGroup = propertyGroups.First(pg => pg.Id == p.GroupId);
                var propertyOwner = propertyOwners.First(po => po.Id == p.OwnerId);

                return new PropertyModel()
                {
                    Id = p.Id,
                    AddressDescription = p.Address.ToSummary(),
                    PropertyGroupName = propertyGroup.Name,
                    PropertyOwnerName = propertyOwner.Name
                };
            }).ToList();

            return View(model);
        }

        public IActionResult PropertyOwnerList()
        {
            var model = new PropertyOwnerListModel() { HeaderText = "Property Owner List" };

            model.PropertyOwners = _propertyOwnerService.GetAll().Result.Select(po =>
            {
                return new PropertyOwnerModel()
                {
                    Id = po.Id,
                    Email = po.Email,
                    Name = po.Name
                };
            }).ToList();

            return View(model);
        }

        public IActionResult PropertyGroupList()
        {
            var model = new PropertyGroupListModel() { HeaderText = "Property Groups " };   // Default header

            model.PropertyGroups = _propertyGroupService.GetAll().Result.Select(p =>
            {
                return new PropertyGroupModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                };
            }).ToList();

            return View(model);
        }

        public IActionResult IssueTypeList()
        {
            var model = new IssueTypeListModel() { HeaderText = "Issue Types" };   // Default header

            model.IssueTypes = _issueTypeService.GetAll().Result.Select(it =>
            {
                return new IssueTypeModel()
                {
                    Id = it.Id,
                    Description = it.Description
                };
            }).ToList();

            return View(model);
        }

        public IActionResult IssueList(string? issueTypeId, string? propertyId)
        {
            var model = new IssueListModel() { HeaderText = "Issue List" };   // Default header

            // Get properties (All properties/Specific group)            
            var issueTypes = _issueTypeService.GetAll().Result;

            // Get issues (All issues/Issue type issues/Property issues)
            List<Issue> issues = null;
            if (!String.IsNullOrEmpty(issueTypeId)) issues = _issueService.GetByIssueType(issueTypeId).Result;
            if (!String.IsNullOrEmpty(propertyId)) issues = _issueService.GetByProperty(propertyId).Result;
            if (issues == null) issues = _issueService.GetAll().Result;

            // Get issue type if set
            var issueTypeMain = String.IsNullOrEmpty(issueTypeId) ?
                        null :
                        issueTypes.First(it => it.Id == issueTypeId);
            if (issueTypeMain != null) model.HeaderText = $"Issue List : {issueTypeMain.Description}";

            // Get property if set
            var propertyMain = String.IsNullOrEmpty(propertyId) ?
                        null :
                        _propertyService.GetById(propertyId).Result;                        
            if (propertyMain != null) model.HeaderText = $"Issue List : {propertyMain.Address.ToSummary()}";
           
            var properties = _propertyService.GetAll().Result;

            // Get issue models
            model.Issues = issues.Select(i =>
            {
                var issueType = issueTypes.First(it => it.Id == i.IssueTypeId);
                var property = String.IsNullOrEmpty(i.PropertyId) ? null :
                        properties.First(p => p.Id == i.PropertyId);

                return new IssueModel()
                {
                    Id = i.Id,
                    IssueTypeDescription = issueType.Description,
                    Description = i.Description,
                    PropertyDescription = (property == null ? "" : property.Address.ToSummary()),
                    StatusDescription = i.Status.ToString() // TODO: Set property
                };
            }).ToList();

            return View(model);
        }       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
