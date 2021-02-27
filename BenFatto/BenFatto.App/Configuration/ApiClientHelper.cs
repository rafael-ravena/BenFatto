using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
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
                //curl - X POST "https://localhost:44376/api/LogRowMismatches" - H  "accept: */*" - H  "Content-Type: text/json"
                client.DefaultRequestHeaders.Add("accept", "*/*");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent content = new StringContent(JsonConvert.SerializeObject(jsonData), Encoding.UTF8, "application/json");
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
    }
}
