using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cs.Controllers
{
    [Route("api/packages")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        [HttpGet("{org}")]
        public ActionResult<IEnumerable<Package>> GetPackagesForOrg(string org, string filter = null)
        {
            using (var ctx = new InstanceContext())
            {
                return new JsonResult(
                    ctx.Packages
                        .Include(e => e.Org)
                        .Where(e => org == "all" || e.Org.Name == org)
                        .Where(e => (filter == null) || e.Name.Contains(filter))
                        .ToList()
                );            
            } 
        }
        [HttpGet("{org}/{name}")]
        public ActionResult<Package> GetPackageById(string org, string name)
        {
            using (var ctx = new InstanceContext())
            {
                return new JsonResult(
                    ctx.Packages
                    .Include(e => e.Org)
                    .Where(e => e.Org.Name == org)
                    .Where(e => e.Name == name)
                        .FirstOrDefault()
                );            
            } 
        }

        [HttpGet("{org}/{name}/artifacts")]
        public ActionResult<IEnumerable<Artifact>> GetArtifactsForPackage(string org, string name)
        {
            using (var ctx = new InstanceContext())
            {
                return new JsonResult(
                    ctx.Packages
                        .Include(p => p.Artifacts)
                        .Include(e => e.Org)
                        .Where(e => e.Org.Name == org)
                        .Where(e => e.Name == name)
                        .FirstOrDefault()
                        .Artifacts
                );            
            } 
        }

        [HttpGet("{org}/{name}/artifacts/{version}")]
        public ActionResult<Artifact> GetArtifactByVersion(string org, string name, string version)
        {
            using (var ctx = new InstanceContext())
            {
                return new JsonResult(
                    ctx.Packages
                        .Include(p => p.Artifacts)
                        .Include(e => e.Org)
                        .Where(e => e.Org.Name == org)
                        .Where(e => e.Name == name)
                        .FirstOrDefault()
                        .Artifacts
                        .Where(e => e.Version == version)
                        .FirstOrDefault()
                );            
            } 
        }
        [HttpGet("{org}/{name}/artifacts/{version}/resolve")]
        public RedirectResult ResolveArtifact(string org, string name, string version)
        {
            using (var ctx = new InstanceContext())
            {
                var res = ctx.Packages
                        .Include(p => p.Artifacts)
                        .Include(e => e.Org)
                        .Where(e => e.Org.Name == org)
                        .Where(e => e.Name == name)
                        .FirstOrDefault()
                        .Artifacts
                        .Where(e => e.Version == version)
                        .FirstOrDefault();
                return Redirect(res.Url);
            } 
        }
    }
}
