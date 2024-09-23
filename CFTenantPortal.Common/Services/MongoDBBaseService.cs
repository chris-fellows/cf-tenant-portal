using CFTenantPortal.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Services
{
    public abstract class MongoDBBaseService<TEntityType>
    {
        //protected MongoClient? _client;
        //protected IMongoCollection<TEntityType> _entities;
        protected readonly Lazy<Tuple<MongoClient, IMongoCollection<TEntityType>>>? _lazy;

        protected MongoClient _client => _lazy!.Value.Item1;
        protected IMongoCollection<TEntityType> _entities => _lazy!.Value.Item2;

        public MongoDBBaseService(IDatabaseConfig databaseConfig, string collectionName)
        {
            // Set properties to initialise on first use
            _lazy = new Lazy<Tuple<MongoClient, IMongoCollection<TEntityType>>>(() => Initialise(databaseConfig, collectionName));

            //_client = new MongoClient(databaseConfig.ConnectionString);
            //var database = _client.GetDatabase(databaseConfig.DatabaseName);
            //_entities = database.GetCollection<TEntityType>(collectionName);
        }

        /// <summary>
        /// Initialises Mongo properties
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        protected Tuple<MongoClient, IMongoCollection<TEntityType>> Initialise(IDatabaseConfig databaseConfig, string collectionName)
        {
            var client = new MongoClient(databaseConfig.ConnectionString);
            var database = client.GetDatabase(databaseConfig.DatabaseName);
            var collection = database.GetCollection<TEntityType>(collectionName);
            return new Tuple<MongoClient, IMongoCollection<TEntityType>>(client, collection);
        }

        public async Task ImportAsync(IEntityList<TEntityType> entityList)
        {
            var entities = entityList.ReadAllAsync().Result;
            if (!entities.Any()) return;

            using (var session = await _client.StartSessionAsync())
            {                
                    try
                    {
                        session.StartTransaction();
                        await _entities.InsertManyAsync(entities);
                        await session.CommitTransactionAsync();
                    }
                    catch (Exception exception)
                    {
                        await session.AbortTransactionAsync();
                        throw exception;
                    }                
            }
        }

        public Task ExportAsync(IEntityList<TEntityType> eventTypeList)
        {
            eventTypeList.WriteAllAsync(GetAll().ToList());
            return Task.CompletedTask;
        }

        public IEnumerable<TEntityType> GetAll()
        {
            return _entities.Find(x => true).ToEnumerable();
        }

        //public Task<TEntityType?> GetByIdAsync(string id)
        //{
        //    return _entities.Find(x => x.Id == id).FirstOrDefaultAsync();
        //}

        //public Task<TEntityType?> GetByNameAsync(string name)
        //{
        //    return _entities.Find(x => x.Name == name).FirstOrDefaultAsync();
        //}

        public Task<TEntityType> AddAsync(TEntityType eventType)
        {
            _entities.InsertOneAsync(eventType);
            return Task.FromResult(eventType);
        }

        public Task<TEntityType> UpdateAsync(TEntityType eventType)
        {            
            //_entities.UpdateOneAsync(eventType);
            return Task.FromResult(eventType);
        }

        public async Task DeleteAllAsync()
        {
            await _entities.DeleteManyAsync(Builders<TEntityType>.Filter.Empty);
        }

        public Task DeleteByIdAsync(string id)
        {
            return _entities.DeleteOneAsync(id);
        }
    }
}
