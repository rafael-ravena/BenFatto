using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App.Configuration
{
    public class DbConfiguration
    {
        private static DbConfiguration _settings;
        public DbConfiguration(IConfiguration configuration)
        {
            Port = configuration.GetValue<int>("Port");
            Host = configuration.GetValue<string>("Host");
            DB = configuration.GetValue<string>("Database");
            UID = configuration.GetValue<string>("Username");
            PWD = configuration.GetValue<string>("Password");
        }
        public static DbConfiguration Current
        {
            get
            {
                if (null == _settings)
                    _settings = GetCurrentSettings();
                return _settings;
            }
        }
        private static DbConfiguration GetCurrentSettings()
        {
            IConfigurationRoot configuration = Helper.GetConfiguration();
            return new DbConfiguration(configuration.GetSection("DataBaseConfiguration"));
        }
        public int Port { get; set; }
        public string Host { get; set; }
        public string DB { get; set; }
        public string UID { get; set; }
        public string PWD { get; set; }
        public string ConnectionString
        {
            get
            {
                return $"Port={Port};Host={Host};Database={DB};Username={UID};Password={PWD};Persist Security Info=True";
            }
        }
    }
}
