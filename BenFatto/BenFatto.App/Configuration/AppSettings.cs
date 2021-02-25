using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App.Configuration
{
    public class AppSettings
    {
        private static AppSettings _settings;
        public AppSettings(IConfiguration configuration)
        {
            BaseUrl = configuration.GetValue<string>("BaseUrl");
            FilesEndPoint = configuration.GetValue<string>("FilesEndPoint");
            ImportsEndPoint = configuration.GetValue<string>("ImportsEndPoint");
            LogRowMismatches = configuration.GetValue<string>("LogRowMismatches");
            LogRows = configuration.GetValue<string>("LogRows");
            UserAgents = configuration.GetValue<string>("UserAgents");
        }
        public static AppSettings Current
        {
            get
            {
                if (null == _settings)
                    _settings = GetCurrentSettings();
                return _settings;
            }
        }
        private static AppSettings GetCurrentSettings()
        {
            IConfigurationRoot configuration = Helper.GetConfiguration();
            return new AppSettings(configuration.GetSection("Settings"));
        }
        public string BaseUrl { get; set; }
        public string FilesEndPoint { get; set; }
        public string ImportsEndPoint { get; set; }
        public string LogRowMismatches { get; set; }
        public string LogRows { get; set; }
        public string UserAgents { get; set; }
    }
}
