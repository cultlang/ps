using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace cs.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(string username, string password)
        {
            using (var ctx = new InstanceContext())
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                var acc = new Account()
                {
                  Username = username,
                  Password = passwordHash,
                };
                ctx.Add<Account>(acc);
                ctx.SaveChanges();

                var payload = new Dictionary<string, object>{{ "accountId", acc }};

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();                
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, System.Environment.GetEnvironmentVariable("APP_SECRET"));

                return new JsonResult(new Dictionary<string,string>{{"token", token}});
            } 
        }
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            using (var ctx = new InstanceContext())
            {
                var acc = ctx.Accounts.Where(e => e.Username == username)
                    .FirstOrDefault();


                if(acc == null || !BCrypt.Net.BCrypt.Verify(password, acc.Password))
                {
                    return Unauthorized();
                }
                
                var payload = new Dictionary<string, object>{{ "accountId", acc }};

                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();                
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, System.Environment.GetEnvironmentVariable("APP_SECRET"));

                return new JsonResult(new Dictionary<string,string>{{"token", token}});
            } 
        }
    }
}
