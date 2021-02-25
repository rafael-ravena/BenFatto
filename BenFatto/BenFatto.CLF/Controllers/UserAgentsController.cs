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
    public class UserAgentsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<DTO.UserAgent> Get()
        {
            IEnumerable<DTO.UserAgent> collection;
            Model.ClfContext context = new Model.ClfContext();
            collection = new Service.LogRowService(context).GetDistinctUserAgents();
            return collection;
        }

    }
}
