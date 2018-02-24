using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sweesh.Core.Abstract
{
    using Models;

    public interface IBaseAdapter<T>
        where T: IModel
    {
        Task<T> Get(string id);
        Task<IEnumerable<T>> Get();
        Task Insert(T item);
        Task Insert(params T[] item);
        Task Delete(string id);
        Task Delete(params string[] ids);
    }
}
