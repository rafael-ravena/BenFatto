﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BenFatto.CLF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogRowsController : ControllerBase
    {
        [HttpGet("filter/page/size/")]
        public IEnumerable<Model.LogRow> Get(long importId, string ipAddress, short responseCode, DateTime when, string userAgent, string method, int page, int size)
        {
            //Model.LogRow.ImportId
            //Model.LogRow.IpAddress
            //Model.LogRow.ResponseCode
            //Model.LogRow.Date
            //Model.LogRow.UserAgent
            //Model.LogRow.Method
            Model.ClfContext context = new Model.ClfContext();
            Model.LogRow logRow = new Model.LogRow
            {
                ImportId = importId,
                IpAddress = ipAddress,
                ResponseCode = responseCode,
                Date = when,
                UserAgent = userAgent
            };
            if (!string.IsNullOrEmpty(method))
                logRow.SetMethodByName(method);
            return new Service.LogRowService(context).Filter(logRow, page, size);
        }
        [HttpGet("filter/page/")]
        public IEnumerable<Model.LogRow> Get(long importId, string ipAddress, short responseCode, DateTime when, string userAgent, string method, int page)
        {
            return Get(importId, ipAddress, responseCode, when, userAgent, method, page, ClfAppSettings.Current.PageSize);
        }
        [HttpGet("filter/")]
        public IEnumerable<Model.LogRow> Get(long importId, string ipAddress, short responseCode, DateTime when, string userAgent, string method)
        {
            return Get(importId, ipAddress, responseCode, when, userAgent, method, 0);
        }
        [HttpPost]
        public void Post([FromBody] Model.LogRow entity)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.LogRowService(context).InsertOrUpdate(entity);
        }
        [HttpPost("get-row-text/")]
        public string GetRowText([FromBody] Model.LogRow data)
        {
            return data.ToString();
        }
        [HttpPost("get-row-object/")]
        public Model.LogRow GetRowObject([FromBody] DTO.LogRowData data)
        {
            return Model.LogRow.Parse(data.Row, data.ImportId, data.RowNumber);
        }
        [HttpGet("{id}")]
        public Model.LogRow Get(long id)
        {
            Model.ClfContext context = new Model.ClfContext();
            return new Service.LogRowService(context).Get(id);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Model.ClfContext context = new Model.ClfContext();
            new Service.LogRowService(context).Delete(id);
        }
    }
}
