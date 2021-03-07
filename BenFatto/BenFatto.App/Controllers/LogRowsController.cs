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
    public class LogRowsController : BaseController
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
            ViewBag.Methods = SettingsManager.Current.HttpMethods;
            return View(JsonConvert.DeserializeObject<List<DTO.LogRow>>(responseData));
        }
        public async Task<IActionResult> Detail(long? id, long importId, int page)
        {
            if (id == null)
                return NotFound();
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowUrl}/{id}");
            DTO.LogRow row = JsonConvert.DeserializeObject<DTO.LogRow>(responseData);
            if (row == null)
                return NotFound();
            return View(row);
        }
        public IActionResult Create(long importId = 0)
        {
            ViewBag.Methods = SettingsManager.Current.HttpMethods;
            ViewBag.UtcOffsetRange = SettingsManager.Current.UtcOffsetRange;
            return View(new DTO.LogRow { ImportId = importId });
        }
        [HttpPost]
        public async Task<IActionResult> Create(int importId, [FromForm] DTO.LogRow logRow)
        {
            try
            {
                string responseData = await ApiClientHelper.ExecutePostAsync($"{ApiClientHelper.LogRowUrl}", JsonConvert.SerializeObject(logRow, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    NullValueHandling = NullValueHandling.Ignore
                }));
                messageType = "success";
                message = $"Log Row successfuly added!";
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
            return RedirectToAction("Index", new { importId = importId });
        }
        public async Task<IActionResult> Edit(long? id, long importId, int page)
        {
            if (id == null)
                return NotFound();
            string responseData = await ApiClientHelper.ExecuteGetAsync($"{ApiClientHelper.LogRowUrl}/{id}");
            DTO.LogRow row = JsonConvert.DeserializeObject<DTO.LogRow>(responseData);
            if (row == null)
                return NotFound();
            ViewBag.Methods = SettingsManager.Current.HttpMethods;
            ViewBag.UtcOffsetRange = SettingsManager.Current.UtcOffsetRange;
            return View(row);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] DTO.LogRow logRow)
        {
            try
            {
                string responseData = await ApiClientHelper.ExecutePostAsync($"{ApiClientHelper.LogRowUrl}", JsonConvert.SerializeObject(logRow, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    NullValueHandling = NullValueHandling.Ignore
                }));
                messageType = "success";
                message = $"You have successfully edited this row!";
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
            return RedirectToAction("Index", new { importId = logRow.ImportId });
        }
        public async Task<IActionResult> Delete(long? id, long importId, int page)
        {
            if (id == null)
                return NotFound();
            await ApiClientHelper.ExecuteDeleteAsync($"{ApiClientHelper.LogRowUrl}/{id}");
            ((Controller)this).DisplayTempData("warning", $"Log row deleted successfuly.");
            return RedirectToAction("index", new { importId = importId, page = page });
        }
    }
}
