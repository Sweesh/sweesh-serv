using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Sweesh.Core.Web.Controllers
{
    using System.Security.Claims;
    using Abstract;
    using Abstract.Managers;
    using Configuration.Jwt;
    using Models;
    using Models.Request;

    public class AuthController : BaseController
    {
        private IUserAdapter userAdapter;
        private IHashManager hashManager;
        private IConfiguration configuration;

        public AuthController(IUserAdapter userAdapter, IHashManager hashManager, IConfiguration configuration)
        {
            this.userAdapter = userAdapter;
            this.hashManager = hashManager;
            this.configuration = configuration;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]AuthRequest login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                {
                    return StatusCode(400, new
                    {
                        Token = (string)null,
                        Message = "Invalid Username/Password"
                    });
                }

                var id = hashManager.BasicHash(login.Username);

                var user = await userAdapter.Get(id);

                if (user == null)
                {
                    return StatusCode(404, new
                    {
                        Token = (string)null,
                        Message = "Incorrect Username/Password"
                    });
                }

                if (!hashManager.VerifyHash(login.Password, user.Salt, user.Password))
                {
                    return StatusCode(401, new
                    {
                        Token = (string)null,
                        Message = "Incorrect Username/Password"
                    });
                }

                var expiration = int.Parse(configuration["Tokens:ExpirationOffset"]) * 60;

                var token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create(configuration["Tokens:Key"]))
                    .AddClaim(ClaimTypes.PrimarySid, user.Id)
                    .AddSubject(user.Username)
                    .AddIssuer(configuration["Tokens:Issuer"])
                    .AddAudience(configuration["Tokens:Issuer"])
                    .AddExpiry(expiration)
                    .Build();

                return Ok(new {
                    Token = token.Value,
                    Expires = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Token = (string)null,
                    Message = ex.Message
                });
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]AuthRequest register)
        {
            try
            {
                if (string.IsNullOrEmpty(register.Username) || string.IsNullOrEmpty(register.Password))
                {
                    return StatusCode(400, new
                    {
                        Worked = false,
                        Message = "Invalid Username/Password"
                    });
                }

                var id = hashManager.BasicHash(register.Username);
                var user = await userAdapter.Get(id);
                if (user != null)
                {
                    return StatusCode(403, new
                    {
                        Worked = false,
                        Message = "Username already exists, sign in?"
                    });
                }

                var salt = hashManager.GenerateSalt(42);
                await userAdapter.Insert(new User
                {
                    Id = id,
                    Username = register.Username,
                    Password = hashManager.PasswordHash(register.Password, salt),
                    Salt = salt
                });

                return Ok(new
                {
                    Worked = true,
                    Message = "Sweesh & Flick, you're now a member!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Worked = false,
                    Message = ex.Message,
                    Stack = ex
                });
            }
        }
    }
}
