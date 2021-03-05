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
        public string DateFormat { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public string BaseUrl { get; set; }
        public string Files { get; set; }
        public string Imports { get; set; }
        public string LogRowMismatches { get; set; }
        public string LogRows { get; set; }
        public string UserAgents { get; set; }
        public string Filter { get; set; }
        public string FilterPage { get; set; }
        public string FilterPageSize { get; set; }
        public string DefaultPassword { get; set; }
        public List<SelectListItem> UtcOffsetRange
        {
            get
            {
                if (null == utcOffset)
                {
                    utcOffset = new List<SelectListItem>();
                    for (int i = -12; i < 15; i++)
                    {
                        string utcValue = i.ToTimeZoneString();
                        utcOffset.Add(new SelectListItem($"UTC {utcValue}", utcValue));
                    }
                }
                return utcOffset;

            }
        }
    }
}
