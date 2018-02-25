using System.Threading.Tasks;
using MongoDB.Driver;

namespace Sweesh.Core.Adapters
{
    using Abstract.Adapters;
    using Configuration.Models;
    using Models;

    public class KeyValueFileAdapter : BaseAdapter<KeyValueFile>, IKeyValueFileAdapter
    {
        public KeyValueFileAdapter(MongoConnection connection) : base(connection) {}

        public async Task<KeyValueFile> GetByKey(string key, string userid)
        {
            var results = await Collection.FindAsync(t => t.Key.ToLower() == key.ToLower() && userid == t.UserId);
            return results.FirstOrDefault();
        }
    }
}
