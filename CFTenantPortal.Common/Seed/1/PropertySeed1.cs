using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Seed1
{
    public class PropertySeed1 : IEntityList<Property>
    {
        private readonly IDocumentService _documentService;
        private readonly IPropertyFeatureTypeService _propertyFeatureTypeService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IPropertyOwnerService _propertyOwnerService;

        public PropertySeed1(IDocumentService documentService,
                            IPropertyFeatureTypeService propertyFeatureTypeService,
                            IPropertyGroupService propertyGroupService, 
                                IPropertyOwnerService propertyOwnerService)
        {
            _documentService = documentService;
            _propertyFeatureTypeService = propertyFeatureTypeService;
            _propertyGroupService = propertyGroupService;
            _propertyOwnerService = propertyOwnerService;
        }

        public Task<List<Property>> ReadAllAsync()
        {
            var entities = new List<Property>();

            var documents = _documentService.GetAll().ToList();
            var propertyFeatureTypes = _propertyFeatureTypeService.GetAll().ToList();
            var propertyGroups = _propertyGroupService.GetAll().ToList();
            var propertyOwners = _propertyOwnerService.GetAll().ToList();

            var document1 = documents[0];
            var document2 = documents[1];
            var document3 = documents[2];
            var document4 = documents[3];

            var propertyGroup1 = propertyGroups[0];
            var propertyGroup2 = propertyGroups[1];

            var propertyOwner1 = propertyOwners[0];
            var propertyOwner2 = propertyOwners[1];
            var propertyOwner3  = propertyOwners[2];
            var propertyOwner4 = propertyOwners[3];
            var propertyOwner5 = propertyOwners[4];

            var propertyFeatureType1 = propertyFeatureTypes[0];
            var propertyFeatureType2 = propertyFeatureTypes[1];
            var propertyFeatureType3 = propertyFeatureTypes[2];
            var propertyFeatureType4 = propertyFeatureTypes[3];

            entities.Add(new Property()
            {              
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                GroupId = propertyGroup1.Id,
                OwnerId = propertyOwner1.Id,
                DocumentIds = new List<string>() { document3.Id, document4.Id },
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            entities.Add(new Property()
            {             
                Address = new Address()
                {
                    Line1 = "101 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                GroupId = propertyGroup1.Id,
                OwnerId = propertyOwner2.Id,
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            entities.Add(new Property()
            {            
                Address = new Address()
                {
                    Line1 = "102 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                GroupId = propertyGroup1.Id,
                OwnerId = propertyOwner3.Id,
                DocumentIds = new List<string>() { document3.Id, document4.Id },
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            entities.Add(new Property()
            {                
                Address = new Address()
                {
                    Line1 = "50 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = propertyGroup2.Id,
                OwnerId = propertyOwner4.Id,
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            entities.Add(new Property()
            {             
                Address = new Address()
                {
                    Line1 = "51 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = propertyGroup2.Id,
                OwnerId = propertyOwner5.Id,
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            entities.Add(new Property()
            {             
                Address = new Address()
                {
                    Line1 = "52 Church Street",
                    County = "Berkshire",
                    Town = "Cookham",
                    Postcode = "SL3 9YT",
                },
                GroupId = propertyGroup2.Id,
                OwnerId = propertyOwner5.Id,   // Owns multiple properties
                FeatureTypeIds = new List<string>() { propertyFeatureType1.Id, propertyFeatureType3.Id }
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<Property> entities)
        {
            return Task.CompletedTask;
        }
    }
}
