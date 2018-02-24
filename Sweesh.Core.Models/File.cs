using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class File
    {
        [BsonElement("FileName")]
        public string FileName { get; set; }
        [BsonElement("PossiblePaths")]
        public string[] PossiblePaths { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("OS")]
        public string OS { get; set; }

        public File() 
        {
            
        }
    }
}