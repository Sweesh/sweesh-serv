using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class Config : IModel
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }
        [BsonElement("AppId")]
        public string AppId { get; set; }
        [BsonElement("AppName")]
        public string AppName { get; set; }
        [BsonElement("Configs")]
        public ConfigItem[] Configs { get; set; }


        public Config()
        {
            
        }
    }
}
