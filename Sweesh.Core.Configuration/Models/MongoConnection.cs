using Microsoft.Extensions.Configuration;

namespace Sweesh.Core.Configuration.Models
{
    public class MongoConnection
    {
        private const string BaseConfig = "MongoConnection:";

        public string Host => config[BaseConfig + "Host"];
        public int Port => int.Parse(config[BaseConfig + "Port"]);
        public string User => config[BaseConfig + "User"];
        public string Pass => config[BaseConfig + "Pass"];

        private IConfiguration config;
        public MongoConnection(IConfiguration config)
        {
            this.config = config;
        }
    }
}
