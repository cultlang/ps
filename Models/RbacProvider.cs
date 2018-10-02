using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json ;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;


namespace cs
{
    public class RbacProvider
    {
        public InstanceContext Db {get; private set;}
        public Account User {get; set;}

        public RbacProvider(InstanceContext db, HttpContext http)
        {
            Db = db;
            try
            {
                var token = http.Request.Headers
                .Where(e => e.Key == "Authorization")
                .First()
                .Value.ToString()
                .Replace("Bearer ", "");

                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                
                var json = decoder.Decode(token, Environment.GetEnvironmentVariable("APP_SECRET"), verify: true);
                
                var accountId = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)["accountId"];
                User = db.Accounts
                        .Where(e => e.AccountId == int.Parse(accountId))
                        .First();
            }
            catch (TokenExpiredException)
            {
                User = null;
            }
            catch (SignatureVerificationException)
            {
                User = null;
            }
        }

        public bool HasOrgRole(RBACRole role, Org org)
        {

        }
    }
}
