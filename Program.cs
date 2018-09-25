using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace cs
{
    public class Program
    {
       public static void Main(string[] args)
        {
            using (var ctx = new InstanceContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                for (int i = 0; i < 10; ++i)
                    ctx.Orgs.Add(Mock.Orgs.Generate());
                ctx.SaveChanges();
            }
            
            using (var ctx = new InstanceContext())
            {
                for (int i = 0; i < 100; ++i)
                    ctx.Packages.Add(Mock.Packages(ctx).Generate());

                ctx.SaveChanges();
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
