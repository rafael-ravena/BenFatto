using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BenFatto.App.Controllers
{
    public class ImportsController : Controller
    {
        // GET: Imports
        public async Task<IActionResult> Index(DTO.Import entity = null)
        {
            if (null == entity)
                entity = new DTO.Import();
            string[] data = {
                    $"success={HttpUtility.UrlEncode(entity.SuccessCount.ToString())}",
                    $"error={HttpUtility.UrlEncode(entity.ErrorCount.ToString())}",
                    $"fileName={HttpUtility.UrlEncode(entity.FileName)}",
                    $"when={HttpUtility.UrlEncode(entity.When.ToString("MM-dd-yyyy hh:mm:ss"))}"
                };
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.ImportsFilter}?{string.Join("&", data)}");
            return View(JsonConvert.DeserializeObject<List<DTO.Import>>(responseData));
        }
        // GET: Imports/Detail/5
        public async Task<IActionResult> Detail(long? id)
        {
            if (id == null)
                return NotFound();
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.ImportsUrl}/{id}");
            DTO.Import import = JsonConvert.DeserializeObject<DTO.Import>(responseData);
            if (import == null)
                return NotFound();
            return View(import);
        }

    }
}
