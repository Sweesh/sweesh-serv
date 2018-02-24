using MongoDB.Driver;
using System.Threading.Tasks;

namespace Sweesh.Core.Adapters
{
    using Abstract;
    using Configuration.Models;
    using Models;

    public class UserAdapter : BaseAdapter<User>, IUserAdapter
    {
        public UserAdapter(MongoConnection connection) : base(connection) { }

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

            return Collection.UpdateOneAsync(filter, update);
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
