using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace uyanikv3.AppFilter;

public class OgrAppFilterController : Controller
{
    // GET
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? ogr = context.HttpContext.Session.GetInt32("ogrOturum");
        if (!ogr.HasValue)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "action","OgrLogin" }, {"controller","OgrAuth"}
            });
        }
        base.OnActionExecuting(context);
    }
}