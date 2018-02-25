using System.Threading.Tasks;

namespace Sweesh.Core.Abstract
{
    using Models;

    public interface IAppAdapter : IBaseAdapter<App>
    {
        Task<App> GetByName(string appname);
        Task AddFile(File file, string appid);
        Task DeleteFile(File file, string appid);
    }
}
