using System.Threading.Tasks;

namespace Sweesh.Core.Abstract
{
    using Models;

    public interface IUserAdapter : IBaseAdapter<User>
    {
        Task Update(User user);
        Task Update(string hashedUsername, string password, string salt);
    }
}
