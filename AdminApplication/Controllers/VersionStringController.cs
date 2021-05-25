using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AdminApplication.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace AdminApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionStringController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public VersionStringController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ContentResult Get()
        {
            IHeaderDictionary clientHeaders = HttpContext.Request.Headers;
            var versionString = clientHeaders["Version"];

            ContentResult upgradeResult = GetUpgradeVersion(versionString);


            return upgradeResult;
        }

        private ContentResult GetUpgradeVersion(string versionString)
        {
            var versionQuery = 
                from m in _context.Upgrade
                orderby m.CreationDate descending  
                select new { m.FileName, m.Version };

            string messageString = "";
            var versionResults = versionQuery.ToList();

            if (versionResults.Count > 0)
            {
                var serverVersion = new Version(versionResults[0].Version);
                var clientVersion = new Version(versionString);

                var versionCompareResult = serverVersion.CompareTo(clientVersion);

                if (versionCompareResult > 0)
                {
                    messageString = versionResults[0].FileName + "," + versionResults[0].Version;
                }
                else
                {
                    messageString = "Client is up to date";
                }
            }
            else
            {
                messageString = "No Version file found";
            }

            return Content(messageString);
        }
    }
}
