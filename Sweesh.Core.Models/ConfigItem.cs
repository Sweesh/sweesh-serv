using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class ConfigItem
    {
        [BsonElement("ConfigName")]
        public string ConfigName { get; set; }
        [BsonElement("Raw")]
        public Byte[] Raw { get; set; }
        [BsonElement("Destination")]
        public string Destination { get; set; }

        public ConfigItem()
        {
        }
    }
}
