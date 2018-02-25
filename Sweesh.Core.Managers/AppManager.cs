using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sweesh.Core.Managers
{
    using Abstract;
    using Abstract.Managers;
    using Models.Response;

    public class AppManager : IAppManager
    {
        private IAppAdapter appAdapter;
        private IConfigAdapter configAdapter;

        public AppManager(IAppAdapter appAdapter, IConfigAdapter configAdapter)
        {
            this.appAdapter = appAdapter;
            this.configAdapter = configAdapter;
        }

        public async Task<ResponseContainer<AppResponse[]>> AppResponses(string appname, string userid)
        {
            try
            {
                var items = new List<AppResponse>();

                if (string.IsNullOrEmpty(appname) || string.IsNullOrEmpty(userid))
                {
                    return new ResponseContainer<AppResponse[]>("Invalid request", 400);
                }

                var conf = await configAdapter.GetByAppName(appname, userid);
                if (conf == null)
                {
                    return new ResponseContainer<AppResponse[]>("Could not find configuration", 404);
                }

                var app = await appAdapter.GetByName(appname);

                foreach(var con in conf.Configs)
                {
                    var ap = app?.Files?.Where(t => t.FileName.ToLower() == con.ConfigName.ToLower()).FirstOrDefault();

                    items.Add(new AppResponse(con, ap));
                }


                return new ResponseContainer<AppResponse[]>(items.ToArray());
            }
            catch (Exception ex)
            {
                return new ResponseContainer<AppResponse[]>(ex.Message, 500);
            }
        }
    }
}
