using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Bogus;



namespace cs
{
    public static class Mock
    {
        
        static int orgId = 0;
        static int packageId = 0;

        public static Faker<Org> Orgs = new Faker<Org>()
            .RuleFor(e => e.OrgId, f => ++orgId)
            .RuleFor(e => e.Name, f => f.Name.FirstName())
        ;

        public static Faker<Package> Packages(InstanceContext ctx) {
            return new Faker<Package>()
            .RuleFor(e => e.PackageId, f =>  ++packageId)
            .RuleFor(e => e.Name, f => f.Hacker.Noun())
            .RuleFor(e => e.SourceRef, f => f.Internet.Url())
            .RuleFor(e => e.Org, (f, e) => {
                return ctx.Orgs.Find(f.Random.Number(1, orgId));
            })
            
        ;
        }

        public static Faker<Package> DependentPackages(InstanceContext ctx) {
            return new Faker<Package>()
            .RuleFor(e => e.PackageId, f =>  ++packageId)
            .RuleFor(e => e.Name, f => f.Hacker.Noun())
            .RuleFor(e => e.SourceRef, f => f.Internet.Url())
            .RuleFor(e => e.Org, (f, e) => {
                return ctx.Orgs.Find(f.Random.Number(1, orgId));
            })
            // .RuleFor(e => e.Dependencies, (f, e) => {
            //     var deps = ctx.Packages.Find(f.Random.Number(1, packageId));
            //     //return ctx.Orgs.Find(f.Random.Number(1, orgId));
            // })
            
            
        ;
        }

    }
}
