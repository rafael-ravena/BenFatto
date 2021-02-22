using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Model
{
    public enum LogRowMethod
    {
        [Description("Get")]
        GET = 0,
        [Description("Put")]
        PUT = 1,
        [Description("Post")]
        POST = 2,
        [Description("Patch")]
        PATCH = 3,
        [Description("Delete")]
        DELETE = 4
    }
}
