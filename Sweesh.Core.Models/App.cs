using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class App : IModel
    {
        [BsonId]
        public string Id { get; set; }
        public string AppName { get; set; }
        public File[] Files { get; set; }

        public App()
        {
            
        }
    }
}
