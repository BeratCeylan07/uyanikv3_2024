using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tweetinvi.Security;
using uyanikv3.customModels;
using uyanikv3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace uyanikv3.Controllers
{
    public class OgrAuthController : Controller
    {
        public static string userToken { get; set; }
        public int kutuphaneid { get; set; }
        public string msgTitle { get; set; }
        public string msg { get; set; }
        public string msgIcon { get; set; }
        public string ps { get; set; }
        public int ogrID { get; set; }
        public int uyeOturum { get; set; }
        
        public IActionResult OgrLogin(string token)
        {
            if (HttpContext.Session.GetInt32("ogrOturum") == 1)
            {
                return Redirect("~/OgrMobil/Index/");
            }

            userToken = token;
            return View();
        }
        public JsonResult okulList()
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using(var db = new U1626744Db60AContext())
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                List<viewOkulmodel> okulList = db.Okullars.Where(x => x.KutuphaneId == kutuphaneid).Select(s => new viewOkulmodel
                {
                    Id = s.Id,
                    KutuphaneId = s.KutuphaneId,
                    OkulBaslik = s.OkulBaslik
                }).ToList();
                var jsonOkulList = JsonConvert.SerializeObject(okulList);
                List<viewAnaKategorilerModel> kategoriList = db.Anakategorilers.Where(x => x.KutuphaneId == kutuphaneid).Select(s => new viewAnaKategorilerModel
                {
                    Id = s.Id,
                    KutuphaneId = s.KutuphaneId,
                    AnaKategoriBaslik = s.AnaKategoriBaslik
                }).ToList();
                var jsonKategoriList = JsonConvert.SerializeObject(kategoriList);
                return Json(new { jsonKategoriList, jsonOkulList });
            }
        }
        public static string RandomNumber(int size)
        {
            var random = new Random();
            var result = String.Empty;
            for (int i = 0; i < size; i++)
            {
                result += random.Next(0, 10);
            }
            return result;
        }
        public void bildirimGonder(string mesaj, string baslik, string token)
        {
            
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
                        Title = baslik,
                        Body = mesaj
                    }
                };

                string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            }
            catch (Exception ex)
            {

            }
        }
        [HttpPost]
        public JsonResult ogrOnKayit(viewOgrencilerModel model)
        {
            using(var db = new U1626744Db60AContext())
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                if (model.Telefon.Length < 11)
                {
                    while (model.Telefon.Length < 11)
                    {
                        model.Telefon = "0" + model.Telefon;
                    }
                }
                var onkayitControl = db.Ogrencilers.Where(x => x.Telefon == model.Telefon && x.Ad == model.Ad && x.Soyad == model.Soyad && x.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID")).FirstOrDefault();
                if (onkayitControl != null)
                {
                    var ogrSet = db.Ogrencilers.Where(x => x.Id == onkayitControl.Id).Select(s => new Ogrenciler
                    {
                        Id = s.Id,
                        Ad = s.Ad,
                        Soyad = s.Soyad,
                        Telefon = s.Telefon,
                        Sifre = s.Sifre,
                        Qrasc = s.Qrasc,
                        Ogrtokens = s.Ogrtokens,
                        Kutuphane = s.Kutuphane,
                        KutuphaneUye = s.KutuphaneUye,
                        KategoriId = s.KategoriId,
                        Kutuphaneuyelikpakettanimlamalars = s.Kutuphaneuyelikpakettanimlamalars
                    }).FirstOrDefault();
                    HttpContext.Session.SetInt32("ogrOturum", 1);
                    HttpContext.Session.SetInt32("ogrID", ogrSet.Id);
                    HttpContext.Session.SetInt32("ogrKategoriID", ogrSet.KategoriId);
                    HttpContext.Session.SetString("ogrAd", ogrSet.Ad);
                    HttpContext.Session.SetString("ogrSoyad", ogrSet.Soyad);
                    HttpContext.Session.SetString("ogrTelefon", ogrSet.Telefon);
                    HttpContext.Session.SetString("ogrQRSet", ogrSet.Qrasc ?? "");
                    msgIcon = "info";
                    msgTitle = "Hoş Geldiniz";
                    msg = "Lütfen Bekleyin Yönlendiriliyorsunuz";
                    return Json(new { msgTitle, msg, msgIcon });
                }
                else
                {
                    ps = RandomNumber(5).ToString();

                    Ogrenciler onKayit = new Ogrenciler();
                    onKayit.Ad = model.Ad;
                    onKayit.Soyad = model.Soyad;
                    onKayit.Telefon = model.Telefon;
                    if (model.okulId == 0)
                    {
                        onKayit.OkulId = db.Okullars.Where(x => x.KutuphaneId == kutuphaneid).Select(s => s.Id).First();
                    }
                    else
                    {
                        onKayit.OkulId = model.okulId;

                    }
                    onKayit.KategoriId = model.KategoriId;
                    onKayit.KutuphaneId = kutuphaneid;
                    onKayit.Sifre = passwordHash(ps);
                    onKayit.Durum = 1;
                    onKayit.Ktarih = DateTime.Now;
                    db.Ogrencilers.Add(onKayit);
                    db.SaveChanges();
                    ogrID = db.Ogrencilers.Where(x => x.Ad == model.Ad && x.Soyad == model.Soyad && x.Telefon == model.Telefon && x.KutuphaneId == kutuphaneid).Select(s => s.Id).First();
                    Ogrtoken token = new Ogrtoken();
                    token.OgrId = ogrID;
                    token.Token = userToken ?? "token";
                    db.Ogrtokens.Add(token);
                    db.SaveChanges();
                    var ogrSet = db.Ogrencilers.Where(x => x.Id == ogrID).Select(s => new Ogrenciler
                    {
                        Id = s.Id,
                        Ad = s.Ad,
                        Soyad = s.Soyad,
                        Telefon = s.Telefon,
                        Sifre = s.Sifre,
                        Qrasc = s.Qrasc,
                        Ogrtokens = s.Ogrtokens,
                        Kutuphane = s.Kutuphane,
                        KutuphaneUye = s.KutuphaneUye,
                        KategoriId = s.KategoriId,
                        Kutuphaneuyelikpakettanimlamalars = s.Kutuphaneuyelikpakettanimlamalars
                    }).FirstOrDefault();
                    HttpContext.Session.SetInt32("ogrOturum", 1);
                    HttpContext.Session.SetInt32("ogrID", ogrSet.Id);
                    HttpContext.Session.SetInt32("ogrKategoriID", ogrSet.KategoriId);
                    HttpContext.Session.SetString("ogrAd", ogrSet.Ad);
                    HttpContext.Session.SetString("ogrSoyad", ogrSet.Soyad);
                    HttpContext.Session.SetString("ogrTelefon", ogrSet.Telefon);
                    HttpContext.Session.SetString("ogrQRSet", ogrSet.Qrasc ?? "");
                    msgIcon = "success";
                    msgTitle = "İşlem Başarılı";
                    string bildirimBaslik = "Uyanık Kütüphane Ailesine Hoş Geldiniz";
                    string bildirimmesaj = "Sisteme erişim İçin Ceptelefonu Numaranız İle Kullanacağınız Şifreniz: " + ps;
                    msg = "Lütfen Bekleyin Yönlendiriliyorsunuz";
                    bildirimGonder(bildirimmesaj, bildirimBaslik, userToken);
                    return Json(new { msgTitle, msg, msgIcon });
                }
            }
        }
        public IActionResult OgrSignIn(string? id)
        {
            try
            {
                using (var db = new U1626744Db60AContext())
                {
                    HttpContext.Session.SetInt32("kutuphaneID", Convert.ToInt32(id));
                    Kurumkutuphaneler kutuphaneinfo = db.Kurumkutuphanelers.Where(x => x.Id == Convert.ToInt32(id))
                        .Select(s => new Kurumkutuphaneler()
                        {
                            Id = s.Id,
                            KutuphaneBaslik = s.KutuphaneBaslik,
                            Adsoyad = s.Adsoyad,
                            Banka = s.Banka,
                            Iban = s.Iban,
                            Tel = s.Tel,
                            Onkayitexplanation = s.Onkayitexplanation
                        }).First();
                   HttpContext.Session.SetInt32("kutuphaneID", kutuphaneinfo.Id);
                   HttpContext.Session.SetString("kBnk", kutuphaneinfo.Banka ?? "");
                   HttpContext.Session.SetString("kbibn", kutuphaneinfo.Iban ?? "");
                   HttpContext.Session.SetString("kas", kutuphaneinfo.Adsoyad ?? "");
                   HttpContext.Session.SetString("kutuphaneAd", kutuphaneinfo.KutuphaneBaslik ?? "");
                   HttpContext.Session.SetString("kutuphaneTel", kutuphaneinfo.Tel ?? "");
                   HttpContext.Session.SetString("kutuphaneonkayitmsg", kutuphaneinfo.Onkayitexplanation ?? "");
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public JsonResult franchiseList()
        {
            using(var db = new U1626744Db60AContext())
            {
                List<viewKurumKutuphaneler> franchise = db.Kurumkutuphanelers.Select(s => new viewKurumKutuphaneler
                {
                    Id = s.Id,
                    KutuphaneBaslik = s.KutuphaneBaslik,
                    Il = s.Il,
                    Ilce = s.Ilce,
                    Adres = s.Adres,
                }).OrderBy(o => o.Id).ToList();
                var jsonFranchiseList = JsonConvert.SerializeObject(franchise);
                return Json(jsonFranchiseList);
            }
        }
        [HttpPost]
        public JsonResult signIn(string userName, string pass, int franchiseID)
        {
           using(var db = new U1626744Db60AContext())
            {
                try
                {
                    var ogrSet = db.Ogrencilers.Where(x => x.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") && x.Telefon == userName).Select(s => new Ogrenciler
                    {
                        Id = s.Id,
                        Ad = s.Ad,
                        Soyad = s.Soyad,
                        Telefon = s.Telefon,
                        Sifre = s.Sifre,
                        Qrasc = s.Qrasc,
                        Ogrtokens = s.Ogrtokens,
                        Kutuphane = s.Kutuphane,
                        KutuphaneUye = s.KutuphaneUye,
                        KategoriId = s.KategoriId,
                        Kutuphaneuyelikpakettanimlamalars = s.Kutuphaneuyelikpakettanimlamalars
                    }).FirstOrDefault();
                    if (ogrSet != null)
                    {
                        if (ogrSet.Sifre == passwordHash(pass))
                        {
                            HttpContext.Session.SetInt32("ogrOturum", 1);
                            HttpContext.Session.SetInt32("ogrID", ogrSet.Id);
                            HttpContext.Session.SetInt32("ogrKategoriID", ogrSet.KategoriId);
                            HttpContext.Session.SetString("ogrAd", ogrSet.Ad);
                            HttpContext.Session.SetString("ogrSoyad", ogrSet.Soyad);
                            HttpContext.Session.SetString("ogrTelefon", ogrSet.Telefon);
                            HttpContext.Session.SetString("ogrQRSet", ogrSet.Qrasc ?? "");
                            msgIcon = "success";
                            msgTitle = "Giriş İşlemi Başarılı";
                            msg = "Lütfen Bekleyin, Yönlendiriliyorsunuz";
                            uyeOturum = 1;
                            if (ogrSet.KutuphaneUye == 2)
                            {
                                HttpContext.Session.SetInt32("kutuphaneUye", 1);
                            }
                            else
                            {
                                HttpContext.Session.SetInt32("kutuphaneUye", 0);
                            }
                            if (ogrSet.Ogrtokens.Where(t => t.Token == userToken).Count() == 0)
                            {
                                if (userToken != null)
                                {
                                    Ogrtoken tokenSet = new Ogrtoken();
                                    tokenSet.Token = userToken;
                                    tokenSet.OgrId = ogrSet.Id;
                                    db.Ogrtokens.Add(tokenSet);
                                    db.SaveChanges();
                                }

                            }
                        }
                        else
                        {
                            uyeOturum = 0;
                            msgIcon = "warning";
                            msg = "Lütfen Şifrenizi Kontrol Ederek Tekrar Deneyiniz";
                            msgTitle = "Hatalı Şifre";
                            return Json(new { uyeOturum, msgTitle, msgIcon, msg });
                        }

                    }
                    else
                    {
                        uyeOturum = 0;

                        msgIcon = "warning";
                        msgTitle = "Kullanıcı Bulunamadı";
                        msg = "Lütfen Cep Telefonu Veya Şifrenizi Kontrol Ederek Tekrar Deneyiniz, Telefon Numaranızın Başında 0 Bulunduğundan Emin Olunuz";
                        return Json(new { uyeOturum, msgTitle, msgIcon, msg });
                    }

                }
                catch (Exception ex)
                {
                    uyeOturum = 0;
                    msgIcon = "warning";
                    msgTitle = "Kullanıcı Bulunamadı";
                    msg = "Lütfen Cep Telefonu Veya Şifrenizi Kontrol Ederek Tekrar Deneyiniz, Telefon Numaranızın Başında 0 Bulunduğundan Emin Olunuz";
                    return Json(new { uyeOturum, msgTitle, msgIcon, msg });
                }
                return Json(new { uyeOturum, msgTitle, msgIcon, msg });
            }

        }
        public IActionResult signOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("OgrLogin");
        }
        public static string passwordHash(string _password)
        {
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            byte[] password_bytes = Encoding.ASCII.GetBytes(_password);
            byte[] encrypted_bytes = SHA1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }
    }
}


