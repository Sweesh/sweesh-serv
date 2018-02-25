using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Sweesh.Core.Adapters
{
    using Abstract;
    using Configuration.Models;
    using Models;

    public abstract class BaseAdapter<T> : IBaseAdapter<T>
        where T : IModel
    {
        private MongoConnection connection = null;
        private IMongoCollection<T> collection = null;
        private string collectionName = null;

        public IMongoCollection<T> Collection => collection ?? (collection = GetCollection());
        public string CollectionName => collectionName ?? (collectionName = typeof(T).Name.ToLower());

        public BaseAdapter(MongoConnection connection)
        {
            this.connection = connection;
        }

        private IMongoCollection<T> GetCollection()
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(connection.Host, connection.Port),
                UseSsl = false
            };

            var client = new MongoClient(settings);
            var db = client.GetDatabase(connection.Database);
            return db.GetCollection<T>(CollectionName);
        }

        public async Task<T> Get(string id)
        {
            var results = await Collection.FindAsync(t => t.Id == id);
            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> Get()
        {
            var results = await Collection.FindAsync(t => true);
            return results.ToEnumerable();
        }

        public Task Insert(T item)
        {
            return Collection.InsertOneAsync(item);

        }

        public Task Insert(params T[] item)
        {
            return Collection.InsertManyAsync(item);
        }

        public Task Delete(string id)
        {
            return Collection.DeleteOneAsync(t => t.Id == id);
        }

        public Task Delete(params string[] ids)
        {
            return Collection.DeleteManyAsync(t => ids.Any(a => a == t.Id));
        }
    }
}
