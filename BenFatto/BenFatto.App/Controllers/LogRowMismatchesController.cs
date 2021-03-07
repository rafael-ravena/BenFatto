using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System;

namespace BenFatto.App.Controllers
{
    public class LogRowMismatchesController : BaseController
    {
        // GET: LogRowMismatches
        public async Task<IActionResult> Index(DTO.LogRowMismatch entity = null, long importId = 0, int page = 0)
        {
            if (null == entity)
                entity = new DTO.LogRowMismatch();
            string[] data = {
                    $"importId={HttpUtility.UrlEncode(importId.ToString())}",
                    $"exception={HttpUtility.UrlEncode(entity.ThrownException)}",
                    $"row={HttpUtility.UrlEncode(entity.Row)}",
                    $"page={HttpUtility.UrlEncode(page.ToString())}"
                };
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowMismatchesFilter}?{string.Join("&", data)}");
            return View(JsonConvert.DeserializeObject<List<DTO.LogRowMismatch>>(responseData));
        }
        // GET: LogRowMismatches/Detail/5
        public async Task<IActionResult> Detail(long? id, long importId, int page)
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
        // POST: LogRowMismatches/Edit/5?importId={0}&page={1}
        [HttpPost]
        public async Task<IActionResult> Edit(long importId, int page, [FromForm] DTO.LogRowMismatch entity)
        {
            try
            {
                string responseData = await ApiClientHelper.ExecutePostAsync($"{ApiClientHelper.LogRowMismatchesUrl}", JsonConvert.SerializeObject(entity, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    NullValueHandling = NullValueHandling.Ignore
                }));
                bool converted = bool.Parse(responseData);
                messageType = (converted ? "success" : "warning");
                message = $"Log row{ (converted ? "" : " not") } converted to Row.";
            }
            catch (Exception ex)
            {
                messageType = "error";
                message = $"Something went wrong: {ex.Message}";
            }
            finally
            {
                ((Controller)this).DisplayTempData(messageType, message);
            }
            return RedirectToAction("index", new { importId = importId, page = page });
        }
        // GET: LogRowMismatches/Delete/5?importId={0}&page={1}
        public async Task<IActionResult> Delete(long? id, long importId, int page)
        {
            if (id == null)
            {
                ((Controller)this).DisplayTempData("warning", "Sorry! We can't find this Log Error you are trying to delete! Maybe you've already deleted it, didn't you?");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            await ApiClientHelper.ExecuteDeleteAsync($"{ApiClientHelper.LogRowMismatchesUrl}/{id}");
            ((Controller)this).DisplayTempData("warning", $"Log row with error deleted successfuly.");
            return RedirectToAction("index", new { importId = importId, page = page });
        }

    }
}
