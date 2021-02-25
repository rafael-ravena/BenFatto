using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogRowMismatchesController : ControllerBase
    {
        [HttpGet("importId/exception/row/page/size/")]
        public IEnumerable<Model.LogRowMismatch> Get(long importId, string exception, string row, int page, int size)
        {
            //Model.LogRowMismatch.ImportId
            //Model.LogRowMismatch.IpAddress
            //Model.LogRowMismatch.ResponseCode
            //Model.LogRowMismatch.Date
            //Model.LogRowMismatch.UserAgent
            //Model.LogRowMismatch.Method
            Model.ClfContext context = new Model.ClfContext();
            Model.LogRowMismatch logRowMismatch = new Model.LogRowMismatch
            {
                ImportId = importId,
                ThrownException = exception,
                Row = row
            };
            return new Service.LogRowMismatchService(context).Filter(logRowMismatch, page, size);
        }
        [HttpGet("importId/exception/row/page/")]
        public IEnumerable<Model.LogRowMismatch> Get(long importId, string exception, string row, int page)
        {
            return Get(importId, exception, row, page, AppSettings.Current.PageSize);
        }
        [HttpGet("importId/exception/row/")]
        public IEnumerable<Model.LogRowMismatch> Get(long importId, string exception, string row)
        {
            return Get(importId, exception, row, 0);
        }
        [HttpPost]
        public void Post([FromBody] Model.LogRowMismatch entity)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.LogRowMismatchService(context).InsertOrUpdate(entity);
        }
        [HttpGet("{id}")]
        public Model.LogRowMismatch Get(long id)
        {
            Model.ClfContext context = new Model.ClfContext();
            return new Service.LogRowMismatchService(context).Get(id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.LogRowMismatchService(context).Delete(id);
        }
    }
}
