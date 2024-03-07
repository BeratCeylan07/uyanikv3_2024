using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers;

public class BildirimActionController : Controller
{
    public static string userToken { get; set; }
    public string msgTitle { get; set; }
    public string msg { get; set; }
    public string msgIcon { get; set; }
    public int uyeOturum { get; set; }
    public int kutuphaneId { get; set; }
    
    [HttpPost]
    public JsonResult BildirimList()
    {
        kutuphaneId = (int)HttpContext.Session.GetInt32("kutuphaneID");
        using (var context = new U1626744Db60AContext())
        {
            try
            {
                List<viewOgrBildirimLog> ogrlog = context.Ogrbildirimlogs.Where(x => x.Ogr.KutuphaneId == kutuphaneId)
                    .Select(s => new viewOgrBildirimLog()
                    {
                        Id = s.Id,
                        Bildirim = s.Bildirim,
                        LogTarih = s.LogTarih,
                        Ogr = s.Ogr
                    }).OrderByDescending(o => o.LogTarih).ToList();
                return Json(JsonConvert.SerializeObject(ogrlog));
            }
            catch (Exception ex)
            {
                msgIcon = "error";
                msg = "Hata Oluştu: Lütfen Sistem Yöneticiniz İle İletişime Geçiniz. Hata kodu: " + ex.Message + " " + ex
                    .ToString();
                msgTitle = "Hata Oluştu";
                var title = msgTitle;
                var msg_ = msg;
                var msgicon_ = msgIcon;
                return Json(new { title, msg_, msgicon_ });
            }
        }
    }

    [HttpPost]
    public void BildirimPost(string title, string msgcontent, string token)
    {
        kutuphaneId = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var App = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }
            var registrationToken = token;

            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "myData", "1337" },
                },
                Token = registrationToken,
                    
                Notification = new Notification()
                {
                    Title = title,
                    Body = msgcontent
                }
            };
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}