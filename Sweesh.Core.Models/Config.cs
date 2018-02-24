using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class Config
    {
        [BsonId]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AppId { get; set; }
        public string AppName { get; set; }
        public ConfigItem[] Configs { get; set; }


        public Config()
        {
            
        }
    }
}
