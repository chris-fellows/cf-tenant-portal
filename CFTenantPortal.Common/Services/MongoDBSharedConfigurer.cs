using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CFTenantPortal.Services
{
    public class MongoDBSharedConfigurer : ISharedDatabaseConfigurer
    {
        private readonly IDatabaseConfig _databaseConfig;

        private readonly IServiceProvider _serviceProvider;

        public MongoDBSharedConfigurer(IDatabaseConfig databaseConfig,
                        IServiceProvider serviceProvider)
        {
            _databaseConfig = databaseConfig;
            _serviceProvider = serviceProvider;
        }

        public async Task InitialiseAsync()
        {
            // Configure main DB
            var client = new MongoClient(_databaseConfig.ConnectionString);
            var database = client.GetDatabase(_databaseConfig.DatabaseName);            

            await InitialiseAccountTransactions(database);
            await InitialiseAccountTransactionTypes(database);
            await InitialiseAuditEvents(database);
            await InitialiseAuditEventTypes(database);
            await InitialiseDocuments(database);
            await InitialiseEmployees(database);
            await InitialiseIssues(database);
            await InitialiseIssueStatuses(database);
            await InitialiseIssueTypes(database);            
        }

        private async Task InitialiseAccountTransactions(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseAccountTransactionTypes(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseAuditEvents(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseAuditEventTypes(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseDocuments(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseEmployees(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseIssues(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseIssueStatuses(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }

        private async Task InitialiseIssueTypes(IMongoDatabase database)
        {
            //var collection = database.GetCollection<AccountTransaction>("account_transactions");
            //var indexDefinitionName = Builders<AccountTransaction>.IndexKeys.Ascending(x => x.Name);
            //await collection.Indexes.CreateOneAsync(new CreateIndexModel<AccountTransaction>(indexDefinitionName));
        }
    }
}
