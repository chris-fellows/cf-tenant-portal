using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;

namespace CFTenantPortal.Services
{
    public class MongoDBDocumentService : MongoDBBaseService<Document>, IDocumentService
    {
        public MongoDBDocumentService(IDatabaseConfig databaseConfig) : base(databaseConfig, "documents")
        {

        }

        public Task<Document?> GetByIdAsync(string id)
        {
            return _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        //public Task<AccountTransaction?> GetByNameAsync(string name)
        //{
        //    return _entities.Find(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public Task DeleteByIdAsync(string id)
        {
            return _entities.DeleteOneAsync(id);
        }

        //public Task<List<Document>> GetAll()
        //{
        //    return Task.FromResult(GetAllInternal());
        //}

        //public Task<Document> GetById(string id)
        //{
        //    return Task.FromResult(GetAllInternal().FirstOrDefault(it => it.Id == id));
        //}

        //public Task Update(Document document)
        //{
        //    return Task.CompletedTask;
        //}

        //private List<Document> GetAllInternal()
        //{
        //    var documents = new List<Document>();

        //    documents.Add(new Document()
        //    {
        //        Id = "1",
        //        Name = "Test1.pdf",
        //        Content = new byte[100]
        //    });

        //    documents.Add(new Document()
        //    {
        //        Id = "2",
        //        Name = "Test2.pdf",
        //        Content = new byte[100]
        //    });

        //    documents.Add(new Document()
        //    {
        //        Id = "3",
        //        Name = "Test3.pdf",
        //        Content = new byte[100]
        //    });

        //    documents.Add(new Document()
        //    {
        //        Id = "4",
        //        Name = "Test4.pdf",
        //        Content = new byte[100]
        //    });

        //    documents.Add(new Document()
        //    {
        //        Id = "5",
        //        Name = "Test5.pdf",
        //        Content = new byte[100]
        //    });

        //    return documents;
        //}
    }
}
