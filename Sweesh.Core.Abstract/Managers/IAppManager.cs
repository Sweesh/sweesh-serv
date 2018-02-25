using System.Threading.Tasks;

namespace Sweesh.Core.Abstract.Managers
{
    using Models.Response;

    public interface IAppManager
    {
        Task<ResponseContainer<AppResponse[]>> AppResponses(string appname, string userid);
    }
}
