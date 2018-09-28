using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace cs.Controllers
{
    [Route("api/artifacts")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpGet("{org}/{package}/{version}/{filename}")]
        public IActionResult GetArtifact(string org, string package, string version, string filename)
        {
            using (var ctx = new InstanceContext())
            {
                var stream = System.IO.File.Open($"static/{org}/{package}/{org}_{package}_{version}.zip", FileMode.Open);

                if(stream == null)
                    return NotFound();

                return File(stream, "application/octet-stream");           
            } 
        }

        [HttpPost("{org}/{package}/{version}")]
        public async Task<IActionResult> UploadArtifact(string org, string package, string version, IFormFile file)
        {
            using (var ctx = new InstanceContext())
            {
                Package pkg;
                try
                {
                  pkg = ctx.Packages
                  .Include(e => e.Org)
                  .Where(e => e.Org.Name == org)
                  .Where(e => e.Name == package)
                  .FirstOrDefault();
                }
                catch
                {
                  return NotFound("No Such Package");
                }
                
                try
                {
                  var exits = ctx.Artifacts
                  .Where(e => e.Package.PackageId == pkg.PackageId && e.Version == version)
                  .First();

                  return BadRequest("Artifact Already Exists");
                }
                catch(Exception e)
                {
                  var art = new Artifact { 
                    Package = pkg,
                    Version = version,
                    Url = $"http://localhost:5000/api/artifacts/{org}/{package}/{org}_{package}_{version}.zip"
                  };
                  ctx.Artifacts.Add(art);
                  ctx.SaveChanges();

                  Directory.CreateDirectory($"static/{org}/{package}");

                  using (var stream = new FileStream($"static/{org}/{package}/{org}_{package}_{version}.zip", FileMode.Create))
                  {
                      await file.CopyToAsync(stream);
                      return Ok();
                  }
                }
            } 
        }

        [HttpPut("{org}/{package}/{version}")]
        public async Task<IActionResult> PatchArtifact(string org, string package, string version, IFormFile f)
        {
            using (var ctx = new InstanceContext())
            {
                Package pkg;
                try
                {
                  pkg = ctx.Packages
                  .Include(e => e.Org)
                  .Where(e => e.Org.Name == org)
                  .Where(e => e.Name == package)
                  .FirstOrDefault();
                }
                catch
                {
                  return NotFound("No Such Package");
                }
                
                try
                {
                  var exits = ctx.Artifacts
                  .Where(e => e.Package.PackageId == pkg.PackageId && e.Version == version)
                  .First();

                  var art = new Artifact { 
                    Package = pkg,
                    Version = version,
                    Url = $"http://localhost:5000/api/artifacts/{org}/{package}/{org}_{package}_{version}.zip"
                  };
                  ctx.Artifacts.Add(art);
                  ctx.SaveChanges();

                  Directory.CreateDirectory($"static/{org}/{package}");

                  using (var stream = new FileStream($"static/{org}/{package}/{org}_{package}_{version}.zip", FileMode.OpenOrCreate))
                  {
                      await f.CopyToAsync(stream);
                      return Ok();
                  }
                }
                catch(Exception e)
                {
                  return BadRequest("Artifact Does Not Exist");
                }
            } 
        }
    }
}
