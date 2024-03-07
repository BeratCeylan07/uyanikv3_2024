using Microsoft.AspNetCore.Mvc;

namespace uyanikv3.Controllers;

public class QRViewController : Controller
{
    // GET
    public IActionResult Index(string qrcode)
    {
        if (qrcode is null)
        {
            return RedirectPermanent("https://uyanik.com.tr");
        }
        else
        {
            ViewBag.QRCODE = qrcode;
            return View();
        }
    }
}