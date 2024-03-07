using Microsoft.AspNetCore.Mvc;
using uyanikv3.AppFilter;

namespace uyanikv3.Controllers;

[AppFilterController]
public class QRActionController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}