using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sweesh.Core.Web.Controllers
{
    [Route("api/[controller]/[action]"), Authorize]
    public abstract class BaseController : Controller
    {
        public string IPAddress => Request.HttpContext.Connection.RemoteIpAddress.ToString();

        public string UserAgent => Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "";

        public string UserId => GetClaim(ClaimTypes.PrimarySid, out string userid) ? userid : null;

        public string Username => GetClaim(ClaimTypes.Email, out string email) ? email : null;

        //Don't use this for access control, use the [Authorize(Roles = "")] attribute
        public string[] Roles => GetClaims<string>(ClaimTypes.Role);

        private bool GetClaim<T>(string name, out T claim)
            where T : IConvertible
        {
            var strClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == name)?.Value;
            if (strClaim == null)
            {
                claim = default(T);
                return false;
            }
            try
            {
                claim = (T)Convert.ChangeType(strClaim, typeof(T));
                return true;
            }
            catch
            {
                claim = default(T);
                return false;
            }
        }

        private T[] GetClaims<T>(string name)
            where T : IConvertible
        {
            var typedClaims = new List<T>();
            foreach (var claim in User.FindAll(name))
            {
                try
                {
                    typedClaims.Add((T)Convert.ChangeType(claim, typeof(T)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return typedClaims.ToArray();
        }
    }
}
