using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Sweesh.Core.Models
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public User() 
        {
            
        }
    }
}
