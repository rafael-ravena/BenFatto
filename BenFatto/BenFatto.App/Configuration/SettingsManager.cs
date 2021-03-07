using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App
{
    public class SettingsManager
    {
        private static List<SelectListItem> utcOffset;
        private static SettingsManager _settings;
        public SettingsManager(IConfiguration configuration)
        {
            DateFormat = configuration.GetValue<string>("DateFormat");
            BrowserDateFormat = configuration.GetValue<string>("BrowserDateFormat");
            CultureInfo = CultureInfo.GetCultureInfo(configuration.GetValue<string>("DateCulture"));
            BaseUrl = configuration.GetValue<string>("BaseUrl");
            Files = configuration.GetValue<string>("Files");
            Imports = configuration.GetValue<string>("Imports");
            LogRowMismatches = configuration.GetValue<string>("LogRowMismatches");
            LogRows = configuration.GetValue<string>("LogRows");
            UserAgents = configuration.GetValue<string>("UserAgents");
            Filter = configuration.GetValue<string>("Filter");
            FilterPage = configuration.GetValue<string>("FilterPage");
            FilterPageSize = configuration.GetValue<string>("FilterPageSize");
            LogRowToObject = configuration.GetValue<string>("LogRowToObject");
            LogObjectToRow = configuration.GetValue<string>("LogObjectToRow");
            DefaultPassword = configuration.GetValue<string>("DefaultPassword");
        }
        public static SettingsManager Current
        {
            get
            {
                if (null == _settings)
                    _settings = GetCurrentSettings();
                return _settings;
            }
        }
        private static SettingsManager GetCurrentSettings()
        {
            IConfigurationRoot configuration = Helper.GetConfiguration();
            return new SettingsManager(configuration.GetSection("Settings"));
        }
        public string DateFormat { get; private set; }
        public string BrowserDateFormat { get; private set; }
        public CultureInfo CultureInfo { get; private set; }
        public string BaseUrl { get; private set; }
        public string Files { get; private set; }
        public string Imports { get; private set; }
        public string LogRowMismatches { get; private set; }
        public string LogRows { get; private set; }
        public string UserAgents { get; private set; }
        public string Filter { get; private set; }
        public string FilterPage { get; private set; }
        public string FilterPageSize { get; private set; }
        public string LogRowToObject { get; private set; }
        public string LogObjectToRow { get; private set; }
        public string DefaultPassword { get; private set; }
        public List<SelectListItem> UtcOffsetRange
        {
            get
            {
                if (null == utcOffset)
                {
                    utcOffset = new List<SelectListItem>();
                    for (int i = -12; i < 15; i++)
                    {
                        utcOffset.Add(new SelectListItem($"UTC {(i * 100).ToTimeZoneString()}", (i * 100).ToString()));
                    }
                }
                return utcOffset;

            }
        }
        public List<SelectListItem> HttpMethods
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Value = "0", Text="GET"},
                    new SelectListItem { Value = "1", Text="PUT"},
                    new SelectListItem { Value = "2", Text="POST"},
                    new SelectListItem { Value = "3", Text="PATCH"},
                    new SelectListItem { Value = "4", Text="DELETE"}
                };
            }
        }
    }
}
