using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class User : IModel
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Username")]
        public string Username { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonElement("Salt")]
        public string Salt { get; set; }

        public User() 
        {
            
        }
    }
}
