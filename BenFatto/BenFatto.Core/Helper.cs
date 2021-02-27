using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BenFatto
{
    public class Helper
    {
        #region [ Configuration Singleton ]

        private static IConfigurationBuilder _configurationBuilder;
        public static IConfigurationBuilder ConfigurationBuilder
        {
            get
            {
                if (null == _configurationBuilder)
                    _configurationBuilder = BuildConfiguration();
                return _configurationBuilder;
            }
        }
        private static IConfigurationBuilder BuildConfiguration()
        {
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
        }
        public static IConfigurationRoot GetConfiguration()
        {
            return ConfigurationBuilder.Build();

        }

        #endregion

        public static string GetChunk(ref string value, char searchValue)
        {
            int searchPos = value.IndexOf(searchValue);
            if (searchPos < 0)
                return string.Empty;
            string returnValue = value.Substring(0, searchPos);
            value = value[(searchPos + 1)..];
            return returnValue;
        }
    }
}
