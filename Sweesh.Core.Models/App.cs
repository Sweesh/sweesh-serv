using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class App : IModel
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("AppName")]
        public string AppName { get; set; }
        [BsonElement("Files")]
        public File[] Files { get; set; }

        public App()
        {
            
        }
    }
}
