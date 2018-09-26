using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bogus;



namespace cs
{
    public static class Mock
    {
        
        static int orgId = 0;
        static int packageId = 0;
        static int artifactId = 0;

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
            });
        }

        public static Faker<Package> DependentPackages(InstanceContext ctx) {
            return new Faker<Package>()
            .RuleFor(e => e.PackageId, f =>  ++packageId)
            .RuleFor(e => e.Name, f => f.Hacker.Noun())
            .RuleFor(e => e.SourceRef, f => f.Internet.Url())
            .RuleFor(e => e.Org, (f, e) => {
                return ctx.Orgs.Find(f.Random.Number(1, orgId));
            })
            .RuleFor(e => e.Dependencies, (f, e) => {
                var pcks = ctx.Packages
                    .Include(g => g.Org);

                var rnd = new Random();

                var semver = "v0.1.0";
                return pcks.OrderBy(x => rnd.Next()).Take(5)
                    .Select(g => String.Format("{0}/{1}:{2}", g.Org.Name, g.Name, semver))
                    .ToArray();
            });
        }

        public static Faker<Artifact> Artifact(InstanceContext ctx, Package p, string version) {
            return new Faker<Artifact>()
            .RuleFor(e => e.ArtifactId, f =>  ++artifactId)
            .RuleFor(e => e.Package, f => p)
            .RuleFor(e => e.Url, f => f.Internet.Url())
            .RuleFor(e => e.Version, f=> version);
        }
    }
}
