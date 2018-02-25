using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class KeyValueFile : IModel
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }
        [BsonElement("Key")]
        public string Key { get; set; }
        [BsonElement("MimeType")]
        public string Mime { get; set; }
        [BsonElement("Value")]
        public byte[] Value { get; set; }
    }
}
