using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class PropertyGroupSeed1 : IEntityList<PropertyGroup>
    {
        private readonly IDocumentService _documentService;

        public PropertyGroupSeed1(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public Task<List<PropertyGroup>> ReadAllAsync()
        {
            var entities = new List<PropertyGroup>();

            var documents = _documentService.GetAll().ToList();

            var document1 = documents[0];
            var document2 = documents[1];

            entities.Add(new PropertyGroup()
            {             
                Name = "Building 1",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { document1.Id }
            });

            entities.Add(new PropertyGroup()
            {             
                Name = "Building 2",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { document1.Id }
            });

            entities.Add(new PropertyGroup()
            {             
                Name = "Building 3",
                Description = "Maidenhead, Berkshire",
                DocumentIds = new List<string>() { document1.Id }
            });

            entities.Add(new PropertyGroup()
            {             
                Name = "Building 4",
                Description = "Cookham, Berkshire",
                DocumentIds = new List<string>() { document2.Id }
            });

            entities.Add(new PropertyGroup()
            {             
                Name = "Building 5",
                Description = "Cookham, Berkshire",
                DocumentIds = new List<string>() { document2.Id }
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<PropertyGroup> entities)
        {
            return Task.CompletedTask;
        }
    }
}
