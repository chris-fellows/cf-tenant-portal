using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class PropertyOwnerSeed1 : IEntityList<PropertyOwner>
    {
        private readonly IDocumentService _documentService;

        public PropertyOwnerSeed1(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public Task<List<PropertyOwner>> ReadAllAsync()
        {
            var entities = new List<PropertyOwner>();


            var documents = _documentService.GetAll().ToList();

            var document1 = documents[0];
            var document2 = documents[1];
            var document3 = documents[2];

            entities.Add(new PropertyOwner()
            {              
                Name = "Owner 1",
                Email = "owner1@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                DocumentIds = new List<string>() { document2.Id, document3.Id }
            });

            entities.Add(new PropertyOwner()
            {             
                Name = "Owner 2",
                Email = "owner2@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                },
                DocumentIds = new List<string>() { document2.Id, document3.Id }
            });

            entities.Add(new PropertyOwner()
            {                             
                Name = "Owner 3",
                Email = "owner3@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            entities.Add(new PropertyOwner()
            {              
                Name = "Owner 4",
                Email = "owner4@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            entities.Add(new PropertyOwner()
            {               
                Name = "Owner 5",
                Email = "owner5@myproperty.com",
                Phone = "1234567890",
                Address = new Address()
                {
                    Line1 = "100 High Street",
                    County = "Berkshire",
                    Town = "Maidenhead",
                    Postcode = "SL1 8AX",
                }
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<PropertyOwner> entities)
        {
            return Task.CompletedTask;
        }
    }
}
