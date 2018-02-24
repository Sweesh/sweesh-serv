using MongoDB.Driver;
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
        private MongoClient client = null;
        private IMongoDatabase database = null;
        private IMongoCollection<T> collection = null;
        private string collectionName = null;

        public MongoClient Client => client ?? (client = CreateClient());
        public IMongoDatabase Database => database ?? (database = client.GetDatabase(connection.Database));
        public IMongoCollection<T> Collection => Database.GetCollection<T>(CollectionName);
        public MongoConnection Connection => connection;
        public string CollectionName => collectionName ?? (collectionName = typeof(T).Name.ToLower());

        public BaseAdapter(MongoConnection connection)
        {
            this.connection = connection;
        }

        private MongoClient CreateClient()
        {
            return new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(connection.Host, connection.Port)
            });
        }

        public async Task<T> Get(string id)
        {
            var results = await Collection.FindAsync(t => t.Id == id);
            return await results.FirstOrDefaultAsync();
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
