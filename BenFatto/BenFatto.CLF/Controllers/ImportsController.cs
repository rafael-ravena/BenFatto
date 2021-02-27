using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BenFatto.CLF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportsController : ControllerBase
    {
        [HttpGet("filter/")]
        public IEnumerable<Model.Import> Filter(int success, int error, string fileName, DateTime when)
        {
            //Model.Import.When
            //Model.Import.SuccessCount
            //Model.Import.ErrorCount 
            //Model.Import.FileName
            Model.Import filter = new Model.Import
            {
                ErrorCount = error,
                SuccessCount = success,
                FileName = fileName,
                When = when
            };
            Model.ClfContext context = new Model.ClfContext();
            return new Service.ImportService(context).Filter(filter);
        }
        [HttpPost]
        public void Post([FromBody] Model.Import import)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.ImportService(context).InsertOrUpdate(import);
        }
        [HttpGet("{id}")]
        public Model.Import Get(long id)
        {
            Model.ClfContext context = new Model.ClfContext();
            return new Service.ImportService(context).Get(id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.ImportService(context).Delete(id);
        }

    }
}
