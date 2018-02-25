using System.Threading.Tasks;
using MongoDB.Driver;

namespace Sweesh.Core.Adapters
{
    using Abstract;
    using Configuration.Models;
    using Models;

    public class AppAdapter : BaseAdapter<App>, IAppAdapter
    {
        public AppAdapter(MongoConnection connection) : base(connection)
        {
        }

        public async Task<App> GetByName(string appname)
        {
            return (await Collection.FindAsync(t => t.AppName.ToLower() == appname.ToLower())).FirstOrDefault();
        }

        public Task AddFile(File file, string appId) 
        {
            var filter = Builders<App>.Filter.Eq(app => app.Id, appId);
            var update = Builders<App>.Update.AddToSet(app => app.Files, file);

            return Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteFile(File file, string appId)
        {
            var builder = Builders<App>.Filter;
            var filter = builder.And(builder.And(builder.Eq("Files.FileName", file.FileName),
                                                  builder.Eq("Files.OS", file.OS)),
                                     builder.Eq(app => app.Id, appId));

            return Collection.DeleteOneAsync(filter);
        }
    }
}
