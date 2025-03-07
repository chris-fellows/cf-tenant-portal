//using CFTenantPortal.Models;
//using CFTenantPortal.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MongoDB.Driver;

//namespace CFTenantPortal.Services
//{
//    public class MongoDBContentTemplateService : MongoDBBaseService<ContentTemplate>, IContentTemplateService
//    {
//        public MongoDBContentTemplateService(IDatabaseConfig databaseConfig) : base(databaseConfig, "content_templates")
//        {

//        }

//        public async Task<ContentTemplate?> GetByIdAsync(string id)
//        {
//            return await _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
//        }
//    }
//}
