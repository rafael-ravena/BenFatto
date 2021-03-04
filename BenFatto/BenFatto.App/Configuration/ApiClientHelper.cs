using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BenFatto.App
{
    public class ApiClientHelper
    {
        public static async Task<string> ExecuteGetAsync(string url)
        {
            string responseData;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                    responseData = await response.Content.ReadAsStringAsync();
            }
            return responseData;
        }
        public static async Task<string> ExecutePostAsync(string url, string jsonData)
        {
            string responseData;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("accept", "*/*");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using (HttpResponseMessage httpResponse = await client.PostAsync(url, content))
                    responseData = await httpResponse.Content.ReadAsStringAsync();
            }
            return responseData;
        }
        public static async Task ExecuteDeleteAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(url);
            }
        }
        public static async Task ExecuteMultipartPostAsync(string url, byte[] file)
        {
            using(HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(file)), "file", $"{DateTime.Now:yyyyMMdd-HHmmssfff}.log");
                    HttpResponseMessage httpResponse = await client.PostAsync(url, content);
                }
            }
        }
        public static string ImportsFilter
        {
            get
            {
                return $"{ImportsUrl}{SettingsManager.Current.Filter}";
            }
        }
        public static string ImportsUrl
        {
            get
            {
                return $"{SettingsManager.Current.BaseUrl}{SettingsManager.Current.Imports}";
            }
        }
        public static string LogRowMismatchesFilter
        {
            get
            {
                return $"{LogRowMismatchesUrl}{SettingsManager.Current.Filter}";
            }
        }
        public static string LogRowMismatchesUrl
        {
            get
            {
                return $"{SettingsManager.Current.BaseUrl}{SettingsManager.Current.LogRowMismatches}";
            }
        }
        public static string LogRowFilter
        {
            get
            {
                return $"{LogRowUrl}{SettingsManager.Current.Filter}";
            }
        }
        public static string LogRowUrl
        {
            get
            {
                return $"{SettingsManager.Current.BaseUrl}{SettingsManager.Current.LogRows}";
            }
        }
        public static string UserAgentsUrl
        {
            get
            {
                return $"{SettingsManager.Current.BaseUrl}{SettingsManager.Current.UserAgents}";
            }
        }
        public static string FileUploadUrl
        {
            get
            {
                return $"{SettingsManager.Current.BaseUrl}{SettingsManager.Current.Files}";
            }
        }
    }
}
