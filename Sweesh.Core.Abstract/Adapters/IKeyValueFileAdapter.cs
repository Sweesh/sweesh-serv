using System.Threading.Tasks;

namespace Sweesh.Core.Abstract.Adapters
{
    using Models;

    public interface IKeyValueFileAdapter : IBaseAdapter<KeyValueFile>
    {
        Task<KeyValueFile> GetByKey(string key, string userid);
    }
}
