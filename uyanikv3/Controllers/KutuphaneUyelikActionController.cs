using System.Runtime.InteropServices.JavaScript;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers;

public class KutuphaneUyelikActionController : Controller
{
    public int kutuphaneid { get; set; } = 1;
    public string MesajBaslik { get; set; }
    public string Mesaj { get; set; }
    public string MesajIcon { get; set; }
    

    [HttpPost]
    public JsonResult uyelikPaketTanimla(int ogrid, int paketid)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var uyelikcontrol = context.Kutuphaneuyelikpakettanimlamalars
                    .Where(x => x.OgrId == ogrid && x.BitisTarih.Date > DateTime.Now.Date).ToList();
                int count = uyelikcontrol.Count();
                if (count == 0)
                {
                    var paketinfo = context.Kutuphaneuyelikpaketlers.Where(x => x.Id == paketid).Select(s =>
                        new Kutuphaneuyelikpaketler()
                        {
                            Id = s.Id,
                            GecerlilikToplamGun = s.GecerlilikToplamGun,
                            ToplamGirisHak = s.ToplamGirisHak,
                            Ucret = s.Ucret,
                            PaketDurum = s.PaketDurum,
                            PaketBaslik = s.PaketBaslik
                        }).First();
                    Kutuphaneuyelikpakettanimlamalar yeni_tanim = new Kutuphaneuyelikpakettanimlamalar();
                    yeni_tanim.PaketId = paketid;
                    yeni_tanim.OgrId = ogrid;
                    yeni_tanim.BaslangicTarih = DateTime.Now.Date;
                    yeni_tanim.BitisTarih = DateTime.Now.AddDays(paketinfo.GecerlilikToplamGun).Date;
                    yeni_tanim.Tarih = DateTime.Now;
                    
                    context.Kutuphaneuyelikpakettanimlamalars.Add(yeni_tanim);
                    context.SaveChanges();
                    MesajBaslik = "Başarılı";
                    Mesaj = "Üyelik Tanımı Yapıldı";
                    MesajIcon = "success";
                    JsonConvert.SerializeObject(Mesaj);
                    JsonConvert.SerializeObject(MesajBaslik);
                    JsonConvert.SerializeObject(MesajIcon);
                    return Json(new
                    {
                        Mesaj,MesajBaslik,MesajIcon
                    });
                }
                else
                {
                    MesajBaslik = "İşlem Başarısız";
                    Mesaj = "Üyelik Tanımı";
                    MesajIcon = "success";
                    JsonConvert.SerializeObject(Mesaj);
                    JsonConvert.SerializeObject(MesajBaslik);
                    JsonConvert.SerializeObject(MesajIcon);
                    return Json(new
                    {
                        Mesaj,MesajBaslik,MesajIcon
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    [HttpPost]
    public JsonResult GirisCikisAction(int uyeid, DateTime? tarih, int parameter)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var girisControl = context.Uyegirishareketlers
                    .Where(x => x.UyeId == uyeid && x.Uye.KutuphaneId == kutuphaneid).ToList();
                int count = girisControl.Where(x => x.Tarih.Date == DateTime.Now.Date).Count();
                if (count == 0)
                {
                    int girisCount = girisControl.Count();
                    string token = context.Ogrtokens.Where(x => x.OgrId == uyeid).Select(s => s.Token).First();
                    var paket = context.Kutuphaneuyelikpakettanimlamalars.Where(x => x.OgrId == uyeid).Select(s =>
                        new Kutuphaneuyelikpakettanimlamalar()
                        {
                            Id = s.Id,
                            Paket = s.Paket,
                            Tarih = s.Tarih,
                            BaslangicTarih = s.BaslangicTarih,
                            BitisTarih = s.BitisTarih,
                            PaketId = s.PaketId
                        }).OrderByDescending(o => o.Id).First();
                    int paketSinir = paket.Paket.ToplamGirisHak;    
                    
                    if (paketSinir > girisCount && paket.BitisTarih.Date >= DateTime.Now.Date)
                    {
                        Uyegirishareketler giris = new Uyegirishareketler();
                        giris.Tarih = tarih ?? DateTime.Now.Date;
                        giris.Saat = DateTime.Now.ToString("hh:mm");
                        giris.UyeId = uyeid;
                        context.Uyegirishareketlers.Add(giris);
                        context.SaveChanges();
                        girisCount = context.Uyegirishareketlers
                            .Where(x => x.Tarih.Date >= paket.BaslangicTarih.Date && x.UyeId == uyeid).Count();
                        MesajBaslik = "Başarılı";
                        Mesaj = "Üye Girişi Yapıldı. Kalan Giriş Hakkınız: " + Convert.ToString(paketSinir - girisCount);
                        MesajIcon = "success";
                        bildirimGonder(Mesaj, MesajBaslik, token, 2, uyeid);
                        JsonConvert.SerializeObject(Mesaj);
                        JsonConvert.SerializeObject(MesajBaslik);
                        JsonConvert.SerializeObject(MesajIcon);
                        return Json(new
                        {
                            Mesaj,MesajBaslik,MesajIcon
                        });
                    }
                    else
                    {
                        MesajBaslik = "İşlem İptal Edildi";
                        Mesaj = "Kütüphane Girişi Başarısız. İlgili Üyenin Giriş Hakkı Kalmamıştır";
                        MesajIcon = "warning";
                        JsonConvert.SerializeObject(Mesaj);
                        JsonConvert.SerializeObject(MesajBaslik);
                        JsonConvert.SerializeObject(MesajIcon);
                        return Json(new
                        {
                            Mesaj,MesajBaslik,MesajIcon
                        });
                    }
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    [HttpPost]
    public JsonResult pakettanimla(viewKutuphaneUyelikPaketlerModel model)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var paketkontrol = context.Kutuphaneuyelikpaketlers.Where(x => x.PaketBaslik == model.PaketBaslik)
                    .FirstOrDefault();
                if (paketkontrol == null)
                {
                    Kutuphaneuyelikpaketler paket = new Kutuphaneuyelikpaketler();
                    paket.KutuphaneId = kutuphaneid;
                    paket.PaketBaslik = model.PaketBaslik;
                    paket.PaketDurum = 1;
                    paket.Ucret = model.Ucret;
                    paket.ToplamGirisHak = model.ToplamGirisHak;
                    paket.GecerlilikToplamGun = model.GecerlilikToplamGun;
                    context.Kutuphaneuyelikpaketlers.Add(paket);
                    context.SaveChanges();
                    return Json("");
                }
                return Json("");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    public void bildirimGonder(string mesaj, string baslik, string token, int? bildirimType, int SorOID)
    {
        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID"); 
            
                if (FirebaseApp.DefaultInstance == null)
                {
                    var App = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile("uyanik-kutuphane-antalya-firebase.json")
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

                using (var context = new U1626744Db60AContext())
                {
                    Bildirimler bildirim = new Bildirimler();
                    bildirim.Baslik = baslik;
                    bildirim.Mesaj = mesaj;
                    bildirim.KutuphaneId = kutuphaneid;
                    context.Bildirimlers.Add(bildirim);
                    context.SaveChanges();
                    if (bildirimType != null)
                    {
                        int lastPnsID = context.Bildirimlers.Where(x => x.KutuphaneId == kutuphaneid)
                            .Max(m => m.Id);
                        if (bildirimType == 2)
                        {
                            Ogrbildirimlog ogrPnsLog = new Ogrbildirimlog();
                            ogrPnsLog.BildirimId = lastPnsID;
                            ogrPnsLog.OgrId = SorOID;
                            ogrPnsLog.LogTarih = DateTime.Now;
                            context.Ogrbildirimlogs.Add(ogrPnsLog);
                            context.SaveChanges();
                        }
                        else if (bildirimType == 3)
                        {
                            Seansbildirimlog snPnsLog = new Seansbildirimlog();
                            snPnsLog.BildirimId = lastPnsID;
                            snPnsLog.SeansId = SorOID;
                            snPnsLog.LogTarih = DateTime.Now;
                            context.Seansbildirimlogs.Add(snPnsLog);
                            context.SaveChanges();
                        }
                    }
                }

        }
        catch (Exception ex)
        {
            return;
        }

    }
    

    [HttpPost]
    public JsonResult GirisIptal(int girisID)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var giris = context.Uyegirishareketlers.Where(x => x.Id == girisID).First();
                context.Uyegirishareketlers.Remove(giris);
                context.SaveChanges();
                MesajBaslik = "İşlem Başarılı";
                Mesaj = "Üyenin Kütüphane Girişi İptal Edildi";
                MesajIcon = "info";
                JsonConvert.SerializeObject(Mesaj);
                JsonConvert.SerializeObject(MesajBaslik);
                JsonConvert.SerializeObject(MesajIcon);
                return Json(new
                {
                    Mesaj,MesajBaslik,MesajIcon
                });
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "warning";
            Mesaj = "Hata Oluştu: " + ex.Message;
            MesajIcon = "error";
            JsonConvert.SerializeObject(Mesaj);
            JsonConvert.SerializeObject(MesajBaslik);
            JsonConvert.SerializeObject(MesajIcon);
            return Json(new
            {
                Mesaj,MesajBaslik,MesajIcon
            });
        }
    }
    
    [HttpPost]
    public JsonResult uyelikPaketlist()
    {
        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
            using (var context = new U1626744Db60AContext())
            {
                List<Kutuphaneuyelikpaketler> paketlist = context.Kutuphaneuyelikpaketlers
                    .Where(x => x.KutuphaneId == kutuphaneid).Select(s => new Kutuphaneuyelikpaketler()
                    {
                        Id = s.Id,
                        PaketBaslik = s.PaketBaslik,
                        PaketDurum = s.PaketDurum,
                        ToplamGirisHak = s.ToplamGirisHak,
                        GecerlilikToplamGun = s.GecerlilikToplamGun,
                        Kutuphane = s.Kutuphane,
                        
                    }).ToList();
                var result = JsonConvert.SerializeObject(paketlist);
                return Json(result);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpPost]
    
    [HttpPost]
    public JsonResult uyeGirisCikisList(int ogrid)
    {
        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using (var context = new U1626744Db60AContext())
            {
                List<viewUyeGirisHareketlerModel> girisCikis = context.Uyegirishareketlers
                    .Where(x => x.Uye.KutuphaneId == kutuphaneid && x.UyeId == ogrid).Select(s => new viewUyeGirisHareketlerModel()
                    {
                        Id = s.Id,
                        UyeId = s.UyeId,
                        Uye = s.Uye,
                        tarihStr = s.Tarih.ToString("dd.MM.yyyy - dddd"),
                        Saat = s.Saat,
                        Tarih = s.Tarih
                    }).OrderByDescending(o => o.Tarih).ToList();
                return Json(JsonConvert.SerializeObject(girisCikis));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    [HttpPost]
    public JsonResult UyeList()
    {
        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using (var context = new U1626744Db60AContext())
            {
                List<viewKutuphaneUyePaketTanimlamalarModel> uyelist = context.Kutuphaneuyelikpakettanimlamalars
                    .Where(x => x.Paket.KutuphaneId == kutuphaneid).Select(s =>
                        new viewKutuphaneUyePaketTanimlamalarModel
                        {
                            Id = s.Id,
                            Ogr = s.Ogr,
                            OgrId = s.OgrId,
                            Paket = s.Paket,
                            TarihSTR = s.Tarih.ToString("dd.MM.yyyy"),
                            BaslangicTarihSTR = s.BaslangicTarih.ToString("dd.MM.yyyy"),
                            BitisTarihSTR = s.BitisTarih.ToString("dd.MM.yyyy"),
                            BitisTarih = s.BitisTarih,
                            bugun = context.Uyegirishareketlers.Where(a => a.UyeId == s.OgrId && a.Tarih.Date == DateTime.Now.Date).Count(),
                            toplamGiris = context.Uyegirishareketlers.Where(x => x.UyeId == s.Ogr.Id && x.Tarih.Date >= s.Tarih.Date).Count(),
                        }).OrderByDescending(o => o.BitisTarih).ToList();

                List<viewOgrencilerModel> drpuyelist = context.Ogrencilers
                .Where(x => x.KutuphaneId == kutuphaneid && x.KutuphaneUye == 1).Select(s =>
                    new viewOgrencilerModel()
                    {
                        Id = s.Id,
                        Ad = s.Ad,
                        Soyad = s.Soyad,
                        Telefon = s.Telefon,
                        ogrtoken = s.Ogrtokens.Select(s => s.Token).First(),
                        KtarihSTR = s.Ktarih.ToString("dd.MM.yy / dddd")
                    }).OrderBy(o => o.Ad).ToList();
                var _uyelist = JsonConvert.SerializeObject(uyelist);
                var _drpuyelist = JsonConvert.SerializeObject(drpuyelist);
                return Json(new { _uyelist, _drpuyelist });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}