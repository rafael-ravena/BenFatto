using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Configuration
{
    public class DataBaseConfiguration
    {
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
