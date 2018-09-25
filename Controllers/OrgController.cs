using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cs.Controllers
{
    [Route("api/orgs")]
    [ApiController]
    public class OrgController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Package>> GetOrgs(string filter = null)
        {
            using (var ctx = new InstanceContext())
            {
                return new JsonResult(
                    ctx.Orgs.Where(e => (filter == null) || e.Name.Contains(filter))
                        .ToList()
                );            
            } 
        }
    }
}
