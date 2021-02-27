using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF
{
    public class ClfAppSettings
    {
        private static ClfAppSettings _settings;
        public ClfAppSettings(IConfiguration configuration)
        {
            BatchSize = configuration.GetValue<int>("BatchSize");
            AutoFlush = configuration.GetValue<bool>("AutoFlush");
            DateFormat = configuration.GetValue<string>("DateFormat");
            CultureInfo = CultureInfo.GetCultureInfo(configuration.GetValue<string>("DateCulture"));
            PageSize = configuration.GetValue<int>("PageSize");
        }
        public static ClfAppSettings Current
        {
            get
            {
                if (null == _settings)
                    _settings = GetCurrentSettings();
                return _settings;
            }
        }
        private static ClfAppSettings GetCurrentSettings()
        {
            IConfigurationRoot configuration = Helper.GetConfiguration();
            return new ClfAppSettings(configuration.GetSection("Settings"));
        }
        public int BatchSize { get; set; }
        public bool AutoFlush { get; set; }
        public string DateFormat { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public int PageSize { get; set; }
    }
}
