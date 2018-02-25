using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sweesh.Core.Web.Controllers
{
    [Route("api/[controller]/[action]"), Authorize]
    public abstract class BaseController : Controller
    {
        
    }
}
