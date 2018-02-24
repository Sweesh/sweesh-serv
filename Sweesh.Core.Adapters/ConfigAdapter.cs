using System;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Sweesh.Core.Models;

namespace Sweesh.Core.Adapters
{
    public class ConfigAdapter
    {
        internal IMongoCollection<Config> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sweesh.Core.Adapters.ConfigAdapter"/> class.
        /// </summary>
        public ConfigAdapter()
        {
            // Should be put in application initialization, BEFORE client created
            BsonClassMap.RegisterClassMap<Config>();

            // Call to be replaced with a DI access of a mongo client
            var Client = new MongoClient("barngang.co:27017");

            // Could also be converted with DI access
            var db = Client.GetDatabase("sweesh");

            // Could also be converted with DI access
            collection = db.GetCollection<Config>("config");
        }


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

            return collection.UpdateOneAsync(filter, update);
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

            return collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Gets the Config by app identifier.
        /// </summary>
        /// <returns>The by app identifier.</returns>
        /// <param name="config">Config.</param>
        public Task<Config> GetByAppId(Config config) {
            var filter = Builders<Config>.Filter.Eq(conf => conf.AppId, config.AppId);

            return collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the Config by the app name.
        /// </summary>
        /// <returns>The by app name.</returns>
        /// <param name="config">Config.</param>
        public Task GetByAppName(Config config) {
            var filter = Builders<Config>.Filter.Eq(conf => conf.AppName, config.AppName);

            return collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
