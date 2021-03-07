using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace BenFatto.App.Controllers
{
    public class ImportsController : BaseController
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
        // GET: Imports/Create
        public IActionResult Create()
        {
            return View(new DTO.File());
        }
        // GET: Imports/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DTO.File file)
        {
            try
            {
                byte[] content;
                using (MemoryStream stream = new MemoryStream())
                {
                    file.LogFile.CopyTo(stream);
                    content = stream.ToArray();
                }
                string response = await ApiClientHelper.ExecuteMultipartPostAsync($"{ApiClientHelper.FileUploadUrl}", content);
                DTO.Import import = JsonConvert.DeserializeObject<DTO.Import>(response);
                messageType = "success";
                message = $"File imported with {import.SuccessCount} rows successfuly processed, and {import.ErrorCount} rows with error.";
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
            return RedirectToAction("Index");
        }
    }
}
