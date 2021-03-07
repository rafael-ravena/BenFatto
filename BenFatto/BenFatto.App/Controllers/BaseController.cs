using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        internal string messageType;
        internal string message;
        public BaseController() : base()
        {
            messageType = string.Empty;
            message = string.Empty;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
