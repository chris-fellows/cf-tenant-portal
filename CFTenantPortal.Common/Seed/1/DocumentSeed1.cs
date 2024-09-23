using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Seed1
{
    public class DocumentSeed1 : IEntityList<Document>
    {
        public Task<List<Document>> ReadAllAsync()
        {
            var entities = new List<Document>();

            entities.Add(new Document()
            {             
                Name = "Test1.pdf",
                Content = new byte[100]
            });

            entities.Add(new Document()
            {             
                Name = "Test2.pdf",
                Content = new byte[100]
            });

            entities.Add(new Document()
            {                
                Name = "Test3.pdf",
                Content = new byte[100]
            });

            entities.Add(new Document()
            {             
                Name = "Test4.pdf",
                Content = new byte[100]
            });

            entities.Add(new Document()
            {             
                Name = "Test5.pdf",
                Content = new byte[100]
            });

            return Task.FromResult(entities);
        }

        public Task WriteAllAsync(List<Document> entities)
        {
            return Task.CompletedTask;
        }
    }
}
