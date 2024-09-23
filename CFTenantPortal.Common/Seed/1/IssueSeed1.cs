using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class IssueSeed1 : IEntityList<Issue>
    {
        private readonly IDocumentService _documentService;
        private readonly IIssueStatusService _issueStatusService;
        private readonly IIssueTypeService _issueTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyService _propertyService;

        public IssueSeed1(IDocumentService documentService,
            IIssueStatusService issueStatusService,
            IIssueTypeService issueTypeService,
            IPropertyGroupService propertyGroupService,
            IPropertyService propertyService)
        {
            _documentService = documentService;
            _issueStatusService = issueStatusService;
            _issueTypeService = issueTypeService; 
            _propertyGroupService = propertyGroupService;
            _propertyService = propertyService;
        }

        public Task<List<Issue>> ReadAllAsync()
        {
            var entities = new List<Issue>();

            var documents = _documentService.GetAll().ToList();
            var issueStatuses = _issueStatusService.GetAll().ToList();
            var issueTypes = _issueTypeService.GetAll().ToList();
            var propertyGroups = _propertyGroupService.GetAll().ToList();
            var properties = _propertyService.GetAll().ToList();

            var document1 = documents[0];
            var document2 = documents[1];
            var document3 = documents[2];
            var document4 = documents[3];

            var issueStatusNew = issueStatuses.First(s => s.Description.Equals("New"));
            var issueStatusProcessing = issueStatuses.First(s => s.Description.Equals("Processing"));
            var issueStatusCompleted = issueStatuses.First(s => s.Description.Equals("Completed"));
            var issueStatusCancelled = issueStatuses.First(s => s.Description.Equals("Cancelled"));

            var issueType1 = issueTypes[0];
            var issueType2 = issueTypes[1];

            var property1 = properties[0];
            var property2 = properties[1];
            var property3 = properties[2];

            var propertyGroup1 = propertyGroups[0];
            var propertyGroup2 = propertyGroups[1];

            entities.Add(new Issue()
            {              
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 1",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType1.Id,
                PropertyId = property1.Id,
                StatusId = issueStatusNew.Id,
                DocumentIds = new List<string>() { document1.Id, document2.Id }
            });

            entities.Add(new Issue()
            {             
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 2",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType1.Id,
                PropertyId = property1.Id,
                StatusId = issueStatusNew.Id
            });

            entities.Add(new Issue()
            {             
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 3",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType2.Id,
                PropertyId = property2.Id,
                StatusId = issueStatusNew.Id,
                DocumentIds = new List<string>() { document3.Id }
            });

            entities.Add(new Issue()
            {                
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 4",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType2.Id,
                PropertyId = property3.Id,
                StatusId = issueStatusNew.Id
            });

            entities.Add(new Issue()
            {             
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 5 for property group",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType2.Id,
                PropertyGroupId = propertyGroup1.Id,
                StatusId = issueStatusProcessing.Id,
                DocumentIds = new List<string>() { document4.Id }
            });

            entities.Add(new Issue()
            {                
                Reference = Guid.NewGuid().ToString(),
                Description = "Issue 6 for property group",
                CreatedDateTime = DateTime.Now,
                TypeId = issueType2.Id,
                PropertyGroupId = propertyGroup2.Id,
                StatusId = issueStatusCancelled.Id
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<Issue> entities)
        {
            return Task.CompletedTask;
        }
    }
}
