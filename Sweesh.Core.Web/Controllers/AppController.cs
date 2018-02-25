using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Sweesh.Core.Web.Controllers
{
    using Abstract;
    using Abstract.Managers;
    using Models;

    public class AppController : BaseController
    {
        private IAppAdapter appAdapter;
        private IAppManager appManager;

        public AppController(IAppManager appManager, IAppAdapter appAdapter)
        {
            this.appManager = appManager;
            this.appAdapter = appAdapter;
        }

        
        [HttpGet, Authorize]
        public async Task<IActionResult> Get(string appname)
        {
            var resp = await appManager.AppResponses(appname, UserId);

            return StatusCode(resp.StatusCode, resp);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post([FromBody]App item)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return StatusCode(401, new
                    {
                        Message = "You must be logged in"
                    });
                }

                item.Id = Guid.NewGuid().ToString();
                item.CreatorId = UserId;
                await appAdapter.Insert(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
