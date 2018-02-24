using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Sweesh.Core.Models;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Sweesh.Core.Adapters
{
    public class UserAdapter
    {
        internal IMongoCollection<User> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sweesh.Core.Adapters.UserAdapter"/> class.
        /// </summary>
        public UserAdapter()
        {
            // Should be put in application initialization, BEFORE client created
            BsonClassMap.RegisterClassMap<User>();

            // Call to be replaced with a DI access of a mongo client
            var Client = new MongoClient("barngang.co:27017");

            // Could also be converted with DI access
            var db = Client.GetDatabase("sweesh");

            // Could also be converted with DI access
            collection = db.GetCollection<User>("user");
        }


        /// <summary>
        /// Update the specified user.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="user">User.</param>
        public Task Update(User user) 
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = filterBuilder.Eq(u => u.Id, user.Id);

            var updateBuilder = Builders<User>.Update;
            var update = updateBuilder.Set(u => u.Password, user.Password)
                                      .Set(u => u.Salt, user.Salt);

            return collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Update the specified hashedUsername, password and salt.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="hashedUsername">Hashed username.</param>
        /// <param name="password">Password.</param>
        /// <param name="salt">Salt.</param>
        public Task Update(string hashedUsername, string password, string salt)
        {
            return Update(new User
            {
                Password = password,
                Salt = salt,
                Id = hashedUsername
            });
        }
    }
}
