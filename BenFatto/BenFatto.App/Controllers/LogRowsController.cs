using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BenFatto.App.Controllers
{
    public class LogRowsController : Controller
    {
        // GET: LogRowMismatches
        public async Task<IActionResult> Index(DTO.LogRow entity = null, long importId = 0, int page = 0)
        {
            if (null == entity)
                entity = new DTO.LogRow();
            string[] data = {
                    $"importId={HttpUtility.UrlEncode(importId.ToString())}",
                    $"ipAddress={HttpUtility.UrlEncode(entity.IpAddress)}",
                    $"responseCode={HttpUtility.UrlEncode(entity.ResponseCode.ToString())}",
                    $"when={HttpUtility.UrlEncode(entity.Date.ToString())}",
                    $"userAgent={HttpUtility.UrlEncode(entity.UserAgent)}",
                    $"method={HttpUtility.UrlEncode(entity.Method.ToString())}",
                    $"page={HttpUtility.UrlEncode(page.ToString())}"
                };
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowFilter}?{string.Join("&", data)}");
            string userAgentsResponseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.UserAgentsUrl}");
            ViewBag.UserAgents = JsonConvert.DeserializeObject<List<DTO.UserAgent>>(userAgentsResponseData).Select(i => new SelectListItem { Text = i.Name, Value = i.Name });
            ViewBag.Methods = new List<SelectListItem> {
                new SelectListItem { Value = "0", Text="GET"},
                new SelectListItem { Value = "1", Text="PUT"},
                new SelectListItem { Value = "2", Text="POST"},
                new SelectListItem { Value = "3", Text="PATCH"},
                new SelectListItem { Value = "4", Text="DELETE"}
            };
            return View(JsonConvert.DeserializeObject<List<DTO.LogRow>>(responseData));
        }
        public IActionResult Create(long importId = 0)
        {
            ViewBag.Methods = new List<SelectListItem> {
                new SelectListItem { Value = "0", Text="GET"},
                new SelectListItem { Value = "1", Text="PUT"},
                new SelectListItem { Value = "2", Text="POST"},
                new SelectListItem { Value = "3", Text="PATCH"},
                new SelectListItem { Value = "4", Text="DELETE"}
            };
            return View(new DTO.LogRow());
        }
    }
}
