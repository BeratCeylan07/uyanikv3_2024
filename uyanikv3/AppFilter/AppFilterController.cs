using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace uyanikv3.AppFilter
{
    public class AppFilterController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? userID = context.HttpContext.Session.GetInt32("Id");
            if (!userID.HasValue)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action","login" }, {"controller","AppAuth"}
                });
            }

            base.OnActionExecuting(context);
        }
    }
}

