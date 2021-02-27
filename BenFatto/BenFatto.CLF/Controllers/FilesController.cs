using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BenFatto.CLF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public void Post([FromBody] IFormFile file, int userId)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "ProcessedFiles");
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string uploadedFile = Path.Combine(dir, fileName);
            using (FileStream stream = new FileStream(uploadedFile, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Service.FileProcessor processor = new Service.FileProcessor(uploadedFile, userId);
            Model.ClfContext context = new Model.ClfContext();
            processor.ProcessFile(context);
        }
    }
}
