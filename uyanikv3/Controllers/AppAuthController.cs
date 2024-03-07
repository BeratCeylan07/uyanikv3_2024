using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers
{ 
    public class AppAuthController : Controller
    {
        public static string userToken { get; set; }
        public string msgTitle { get; set; }
        public string msg { get; set; }
        public string msgIcon { get; set; }
        public int uyeOturum { get; set; }
        public int kutuphaneid { get; set; }
        
        public IActionResult Index(string token)
        {
            if (HttpContext.Session.GetInt32("adminAuth") == 1)
            {
                return Redirect("~/Home/Index/");
            }
            else
            {
                userToken = token;
                return View();
            }
        }
        public JsonResult franchiseList()
        {
            using (var context = new U1626744Db60AContext())
            {
                var franchise = context.Merkezsubelers.Select(s => new Merkezsubeler()
                
                {
                    Id = s.Id,
                    MerkezSubeAdi = s.MerkezSubeAdi,
                    Il = s.Il
                }).OrderBy(o => o.Id).ToList();
                var jsonFranchiseList = JsonConvert.SerializeObject(franchise);
                return Json(jsonFranchiseList);
            }
        }
        [HttpPost]
        public JsonResult adminLogin(string userName, string pass)
        {
            using (var db = new U1626744Db60AContext())
                {
                var admnUsr = db.Systmusers.Where(x => x.KAd == userName && x.SubeId == Convert.ToInt32(HttpContext.Session.GetInt32("subeid"))).Select(s => new Systmuser()
            {
                Id = s.Id,
                SubeId = s.SubeId,
                KAd = s.KAd,
                KPass = s.KPass,
                Auth = s.Auth,
                Subsubeuser = s.Subsubeuser,
                Sube = db.Merkezsubelers.Where(x => x.Id == s.SubeId).First(),
            }).FirstOrDefault();
                
            if (admnUsr != null)
            {
                ViewBag.tenantname = admnUsr.Sube.MerkezSubeAdi;
                if (admnUsr.KPass == pass)
                {
                    Kurumkutuphaneler firstsube;

                    if (admnUsr.Subsubeuser == 1)
                    {
                        HttpContext.Session.SetInt32("subsubeuser",1);
                        var subsube = db.UserSubsubeSets.Where(x => x.Userid == admnUsr.Id).First();
                        
                        HttpContext.Session.SetInt32("kutuphaneID",subsube.Subsubeid);
                        HttpContext.Session.SetString("kutuphaneIDSTR",subsube.Subsubeid.ToString());
                        firstsube = db.Kurumkutuphanelers.Where(x => x.Id == subsube.Subsubeid).First();
                    }
                    else
                    {
                        
                        HttpContext.Session.SetInt32("kutuphaneID",
                            db.Kurumkutuphanelers.Where(k => k.MerkezId == admnUsr.SubeId).Select(s => s.Id).First());
                        HttpContext.Session.SetString("kutuphaneIDSTR",
                            db.Kurumkutuphanelers.Where(k => k.MerkezId == admnUsr.SubeId).Select(s => s.Id).First().ToString());
                        firstsube = db.Kurumkutuphanelers.Where(x => x.MerkezId == admnUsr.SubeId).First();
                    }

                    HttpContext.Session.SetInt32("Id", admnUsr.Id);
                    HttpContext.Session.SetInt32("subeid", admnUsr.SubeId);
                    HttpContext.Session.SetString("AdminId", admnUsr.Id.ToString() ?? "");
                    HttpContext.Session.SetString("SubeAd",admnUsr.Sube.MerkezSubeAdi ?? "");
                    HttpContext.Session.SetString("subeil",admnUsr.Sube.Il ?? "");
                    HttpContext.Session.SetInt32("adminAuth", admnUsr.Auth);
                    HttpContext.Session.SetString("SubeAd",firstsube.KutuphaneBaslik ?? "");
                    HttpContext.Session.SetString("subeil",firstsube.Il ?? "");
                    HttpContext.Session.SetString("subeilce",firstsube.Ilce ?? "");
                    HttpContext.Session.SetString("subeadres",firstsube.Adres ?? "");
                    HttpContext.Session.SetString("subebank",firstsube.Banka ?? "");
                    HttpContext.Session.SetString("subebankadsoyad",firstsube.Adsoyad ?? "");
                    HttpContext.Session.SetString("subebankiban",firstsube.Iban ?? "");
                    HttpContext.Session.SetString("subeonkayitaciklama",firstsube.Onkayitexplanation ?? "");

                    
                    
                    
                    
                    HttpContext.Session.SetString("Hata", "");
                    msgIcon = "success";
                    msgTitle = "Giriş İşlemi Başarılı";
                    msg = "Lütfen Bekleyin, Yönlendiriliyorsunuz";
                    uyeOturum = 1;
                    return Json(new {uyeOturum, msgTitle, msgIcon, msg});                
                }
                else
                {
                    int franchParameter = (int)HttpContext.Session.GetInt32("kutuphaneID");
                    uyeOturum = 0;
                    msgIcon = "warning";
                    msgTitle = "Kullanıcı Bulunamadı";
                    msg = "Giriş Bilgileri Hatalı";
                    return Json(new {uyeOturum, msgTitle, msgIcon, msg});
                }
            }
            else
            {
                int franchParameter = (int)HttpContext.Session.GetInt32("kutuphaneID");
                                        uyeOturum = 0;
                    msgIcon = "warning";
                    msgTitle = "Kullanıcı Bulunamadı";
                    msg = "Giriş Bilgileri Hatalı";
                    return Json(new {uyeOturum, msgTitle, msgIcon, msg});            
            }
            }
        }
        
        public IActionResult Login(int id)
        {
            if (id is not 0)
            {
                
                if (HttpContext.Session.GetInt32("kutuphaneID") > 0)
                {
                    return View();
                }
                else
                {
                    try
                    {
                        using (var context = new U1626744Db60AContext())
                        {
                            int? _id = id;
                            HttpContext.Session.SetInt32("subeid", id);
                            ViewBag.kutuphane = context.Merkezsubelers.Where(x => x.Id == id).Select(s => s.MerkezSubeAdi).First();
                            ViewBag.francID = id;
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");

                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}