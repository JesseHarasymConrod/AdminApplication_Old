using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace AdminApplication.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class ZipFileController : ControllerBase
    {
        [HttpGet("{fileName}")]
        public async Task<IActionResult> DownloadSpecificZip(string fileName)
        {
            string currentDir = Directory.GetCurrentDirectory() + @"\wwwroot\upgrades\";

            fileName += ".zip";

            var filePath = Path.Combine(currentDir, fileName);

            var contentType = "application/zip";

            HttpContext.Response.ContentType = contentType;

            var zippedFile = new FileContentResult(System.IO.File.ReadAllBytes(filePath), contentType);

            return zippedFile;

        }
    }
}
