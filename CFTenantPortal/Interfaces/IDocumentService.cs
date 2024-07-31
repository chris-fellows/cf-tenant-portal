using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IDocumentService
    {
        Task<List<Document>> GetAll();

        Task<Document> GetById(string id);
        
        Task Update(Document document);
    }
}
