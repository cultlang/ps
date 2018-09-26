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


namespace cs
{
    public class InstanceContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite("Data Source=data.db");
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Org>()
                .HasAlternateKey(c => c.Name)
                .HasName("AlternateKey_Name");

            modelBuilder.Entity<Package>()
            .Property<string>("DependencyCollection")
            .HasField("_dependencies");
        }

        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Package> Packages  { get; set; }
        public DbSet<Org> Orgs  { get; set; }
    }
}
