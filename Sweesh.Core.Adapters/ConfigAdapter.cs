using System.Threading.Tasks;
using MongoDB.Driver;

namespace Sweesh.Core.Adapters
{
    using Abstract;
    using Configuration.Models;
    using Models;

    public class ConfigAdapter : BaseAdapter<Config>, IConfigAdapter
    {
        public ConfigAdapter(MongoConnection connection) : base(connection) { }

        /// <summary>
        /// Update the specified config, only updating the AppId and AppName
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="config">Config.</param>
        public Task Update(Config config)
        {
            var filterBuilder = Builders<Config>.Filter;
            var filter = filterBuilder.Eq(con => con.Id, config.Id);

            var updateBuilder = Builders<Config>.Update;
            var update = updateBuilder.Set(con => con.AppId, config.AppId)
                                      .Set(con => con.AppName, config.AppName);

            return Collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Adds the config item to the specified Config based on Id
        /// </summary>
        /// <returns>The config item.</returns>
        /// <param name="item">Item.</param>
        /// <param name="configId">Config identifier.</param>
        public Task AddConfigItem(ConfigItem item, string configId)
        {
            var filter = Builders<Config>.Filter.Eq(conf => conf.Id, configId);
            var update = Builders<Config>.Update.AddToSet(confItems => confItems.Configs, item);

            return Collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Gets the Config by app identifier.
        /// </summary>
        /// <returns>The by app identifier.</returns>
        /// <param name="config">Config.</param>
        public async Task<Config> GetByAppId(Config config)
        {
            var filter = Builders<Config>.Filter.Eq(conf => conf.AppId, config.AppId);

            var results = await Collection.FindAsync(filter);
            return await results.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the Config by the app name.
        /// </summary>
        /// <returns>The by app name.</returns>
        /// <param name="config">Config.</param>
        public Task<Config> GetByAppName(Config config)
        {
            var filter = Builders<Config>.Filter.Eq(conf => conf.AppName.ToLower(), config.AppName.ToLower());

            return Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the app by the name
        /// </summary>
        /// <returns>The name of the app</returns>
        /// <param name="appname">Appname.</param>
        public async Task<Config> GetByAppName(string appname, string userid)
        {
            return (await Collection.FindAsync(t => t.AppName.ToLower() == appname.ToLower() && t.UserId == userid)).FirstOrDefault();
        }
    }
}
