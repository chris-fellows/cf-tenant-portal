using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using CFTenantPortal.Services;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult PropertyList(string? propertyGroupId)
        {
            //return View();
            //

            // TODO: Make this more efficient
            var propertyGroups = _propertyGroupService.GetAll().Result;
            var propertyOwners = _propertyOwnerService.GetAll().Result;

            // Get properties (All properties/Specific group)
            var properties = String.IsNullOrEmpty(propertyGroupId) ?
                    _propertyService.GetAll().Result :
                    _propertyService.GetByPropertyGroup(propertyGroupId).Result;

            var propertyModels = properties.Select(p =>
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
            });

            return View(propertyModels);
        }

        public IActionResult PropertyGroupList()
        {                           
            var propertyGroups = _propertyGroupService.GetAll().Result.Select(p =>
            {
                return new PropertyGroupModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                };
            });

            return View(propertyGroups);
        }

        public IActionResult IssueList(string? propertyId)
        {
            var model = new IssueListModel() { HeaderText = "Issue List" };   // Default header

            // Get properties (All properties/Specific group)            
            var issueTypes = _issueTypeService.GetAll().Result;

            // Get issues (All issues/Property issues)
            var issues = String.IsNullOrEmpty(propertyId) ?
                        _issueService.GetAll().Result :
                        _issueService.GetByProperty(propertyId).Result;

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
