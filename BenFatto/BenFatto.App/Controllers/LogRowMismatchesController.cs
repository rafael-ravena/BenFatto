using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BenFatto.App.Controllers
{
    public class LogRowMismatchesController : Controller
    {
        // GET: LogRowMismatches
        public async Task<IActionResult> Index(DTO.LogRowMismatch entity = null, int page = 0)
        {
            if (null == entity)
                entity = new DTO.LogRowMismatch();
            string[] data = {
                    $"importId={HttpUtility.UrlEncode(entity.ImportId.ToString())}",
                    $"exception={HttpUtility.UrlEncode(entity.ThrownException)}",
                    $"row={HttpUtility.UrlEncode(entity.Row)}",
                    $"page={HttpUtility.UrlEncode(page.ToString())}"
                };
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowMismatchesFilter}?{string.Join("&", data)}");
            return View(JsonConvert.DeserializeObject<List<DTO.LogRowMismatch>>(responseData));
        }
        // GET: LogRowMismatches/Detail/5
        public async Task<IActionResult> Detail(long? id)
        {
            if (id == null)
                return NotFound();
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowMismatchesUrl}/{id}");
            DTO.LogRowMismatch row = JsonConvert.DeserializeObject<DTO.LogRowMismatch>(responseData);
            if (row == null)
                return NotFound();
            return View(row);
        }
        // GET: LogRowMismatches/Edit/5
        public async Task<IActionResult> Edit(long? id, long importId, int page)
        {
            if (id == null)
                return NotFound();
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowMismatchesUrl}/{id}");
            DTO.LogRowMismatch row = JsonConvert.DeserializeObject<DTO.LogRowMismatch>(responseData);
            if (row == null)
                return NotFound();
            return View(row);
        }
        // GET: LogRowMismatches/Detail/5
        [HttpPost]
        public async Task<IActionResult> Edit(long importId, int page, [FromForm] DTO.LogRowMismatch entity)
        {
            string responseData = await ApiClientHelper.ExecutePostAsync($"{ApiClientHelper.LogRowMismatchesUrl}", JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            }));
            return RedirectToAction("index", new { importId = importId, page = page });
        }
        // GET: LogRowMismatches/Detail/5
        public async Task<IActionResult> Delete(long? id, long importId, int page)
        {
            if (id == null)
                return NotFound();
            await ApiClientHelper.ExecuteDeleteAsync($"{ApiClientHelper.LogRowMismatchesUrl}/{id}");
            return RedirectToAction("index", new { importId = importId, page = page });
        }

    }
}
