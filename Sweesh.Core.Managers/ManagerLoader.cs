using MongoDB.Bson.Serialization;

namespace Sweesh.Core.Managers
{
    using Configuration;
    using Models;

    public static class ManagerLoader
    {
        /// <summary>
        /// Has to be present otherwise structure map won't pick up the libs.
        /// </summary>
        /// <returns>The managers.</returns>
        /// <param name="setup">Setup.</param>
        public static Setup LoadManagers(this Setup setup)
        {
            BsonClassMap.RegisterClassMap<User>();
            BsonClassMap.RegisterClassMap<App>();
            BsonClassMap.RegisterClassMap<Config>();
            return setup;
        }
    }
}
