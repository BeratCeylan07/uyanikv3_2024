using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using uyanikv3.Models;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.AspNetCore.Builder.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using uyanikv3.AppFilter;
using uyanikv3.customModels;
using System;
using QRCoder;
using static System.Net.Mime.MediaTypeNames;

namespace uyanikv3.Controllers
{
[AppFilterController]
// OGR YOKLAMA: 4 İSE KİTAPÇIK ALDI, 2 İSE KATILIM SAĞLADI
public class SeansActionController : Controller
{
    public int kutuphaneid { get; set; }
    public int auth { get; set; }
    public string MesajBaslik { get; set; }
    public string Mesaj { get; set; }
    public string MesajIcon { get; set; }

    [HttpPost]
    public JsonResult seanspaketadd(SeansPaket model)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var paket = new SeansPaket();
                paket.KutuphaneId = (int)HttpContext.Session.GetInt32("kutuphaneID");
                paket.PaketAdi = model.PaketAdi;
                paket.Fiyat = model.Fiyat;
                paket.Adet = model.Adet;

                context.SeansPakets.Add(paket);
                context.SaveChanges();

                MesajBaslik = "İşlem Başarılı";
                Mesaj = "Seans Paketi Tanımlandı";
                MesajIcon = "success";

                return Json(new { MesajBaslik, Mesaj, MesajIcon });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public JsonResult SeansPaketDetay(int paketid)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seanspaketset = context.SeansPakets.Where(x => x.Id == paketid).Select(s => new SeansPaket()
                {
                    Id = s.Id,
                    PaketAdi = s.PaketAdi,
                    SeansPaketTanims = s.SeansPaketTanims.Select(b => new SeansPaketTanim()
                    {
                        Id = b.Id,
                        Seans = context.DenemeSeanslars.Where(l => l.Id == b.Seansid).Select(c => new DenemeSeanslar()
                        {
                            Id = c.Id,
                            Deneme = context.Denemelers.Where(d => d.Id == c.DenemeId).Select(e => new Denemeler()
                            {
                                Id = e.Id,
                                DenemeBaslik = e.DenemeBaslik,
                                Yayin = e.Yayin,
                                Kategori = e.Kategori
                            }).First(),
                       Tarih = c.Tarih,
                       Saat = c.Saat,
                       SeansUcret = c.SeansUcret,
                        }).First()
                    }).ToList(),
                    Adet = s.Adet,
                    Fiyat = s.Fiyat
                }).First();
                var jsonresult = JsonConvert.SerializeObject(seanspaketset);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    [HttpPost]
    public JsonResult seansaddinpaket(int seansid, int paketid)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var control = context.SeansPaketTanims.Where
                    (x => x.Paketid == paketid &&
                          x.Seansid == seansid).Count();
                if (control is 0)
                {
                    var paketseanstanim = new SeansPaketTanim();
                    paketseanstanim.Seansid = seansid;
                    paketseanstanim.Paketid = paketid;
                    context.SeansPaketTanims.Add(paketseanstanim);
                    context.SaveChanges();
                    MesajBaslik = "İşlem Başarılı";
                    Mesaj = "Pakete Seans Tanımlandı";
                    MesajIcon = "success";
                }
                else
                {
                    MesajBaslik = "İşlem Başarısız";
                    Mesaj = "Bu Seans Pakete Zaten Tanımlı";
                    MesajIcon = "warning";
                }
                return Json(new { MesajBaslik, Mesaj, MesajIcon });

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Json(ex.ToString());
        }
    }
    public JsonResult seanslitfornonpaket(int paketid)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seanslist = context.DenemeSeanslars
                    .Where(x => x.SeansPaketTanims.Where(x => x.Paketid == paketid).Count() <= 0 &&
                                x.Deneme.Yayin.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") &&
                                x.Deneme.DenemeStoklars.Count() > 0).Select(s => new viewSeansModel()
                    {
                        Id = s.Id,
                        kitapcikBaslik = s.Deneme.DenemeBaslik,
                        yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                        KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                        TarihSTR = s.Tarih.ToString("dd.MM.yyyy - dddd"),
                        Saat = s.Saat,
                        SeansUcret = s.SeansUcret,
                    }).ToList();
                var jsonresult = JsonConvert.SerializeObject(seanslist);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Json(ex.ToString());
        }
    }
    public JsonResult seansPaketList()
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seanspaketlist = context.SeansPakets
                    .Where(x => x.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s =>
                        new SeansPaket()
                        {
                            Id = s.Id,
                            PaketAdi = s.PaketAdi,
                            Fiyat = s.Fiyat,
                            Adet = s.Adet,
                            SeansPaketTanims = s.SeansPaketTanims
                        }).ToList();
                var jsonresult = JsonConvert.SerializeObject(seanspaketlist);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Json(ex.ToString());
        }
    }
    [HttpPost]
    public JsonResult SeansList()
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                List<viewSeansModel> snlist = context.DenemeSeanslars.Where(a => a.Deneme.Kategori.Kat.KutuphaneId == kutuphaneid && a.Tarih.Date >= DateTime.Now.Date).Select(x => new viewSeansModel
                {
                    Id = x.Id,
                    TarihSTR = x.Tarih.ToShortDateString(),
                    Saat = x.Saat,
                    Durum = x.Durum,
                    SeansUcret = x.SeansUcret,
                    yayinId = x.Deneme.YayinId,
                    DenemeId = x.DenemeId,
                    Kontenjan = x.Kontenjan,
                    GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Where(c => c.Durum != 0).Count(),
                    KayitliOgrenci = x.SeansOgrSets.Count(),
                    onkayitliOgrenci = x.SeansOgrSets.Where(q => q.Durum == 0).Count(),
                    SeansGun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)x.Tarih.DayOfWeek],
                    Tarih = x.Tarih,
                    Deneme = x.Deneme,
                    yayinBaslik = x.Deneme.Yayin.YayinBaslik.ToUpper(),
                    kitapcikBaslik = x.Deneme.DenemeBaslik.ToUpper(),
                    yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik.ToUpper()
                }).OrderBy(o => o.Tarih.Date).ToList();
                var jsonresult = JsonConvert.SerializeObject(snlist);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);

            Mesaj = ex.Message;
            var _mesaj = JsonConvert.SerializeObject(Mesaj);
            JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            var _MesajIcon = MesajIcon;
            JsonConvert.SerializeObject(MesajIcon);
            return Json(new { _mesajbaslik, _mesaj, _MesajIcon });
        }
    }
    [HttpPost]
    public JsonResult SeansListAfter()
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                List<viewSeansModel> snlist = context.DenemeSeanslars.Where(a => a.Deneme.Kategori.Kat.KutuphaneId == kutuphaneid && a.Tarih.Date < DateTime.Now.Date).Select(x => new viewSeansModel
                {
                    Id = x.Id,
                    TarihSTR = x.Tarih.ToShortDateString(),
                    Saat = x.Saat,
                    Durum = x.Durum,
                    SeansUcret = x.SeansUcret,
                    yayinId = x.Deneme.YayinId,
                    Kontenjan = x.Kontenjan,
                    GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Count(),
                    KayitliOgrenci = x.SeansOgrSets.Count(),
                    SeansGun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)x.Tarih.DayOfWeek],
                    Tarih = x.Tarih,
                    Deneme = x.Deneme,
                    yayinBaslik = x.Deneme.Yayin.YayinBaslik.ToUpper(),
                    kitapcikBaslik = x.Deneme.DenemeBaslik.ToUpper(),
                    yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik.ToUpper()
                }).OrderByDescending(o => o.Tarih).ToList();
                var jsonresult = JsonConvert.SerializeObject(snlist);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);

            Mesaj = ex.Message;
            var _mesaj = JsonConvert.SerializeObject(Mesaj);
            JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            var _MesajIcon = MesajIcon;
            JsonConvert.SerializeObject(MesajIcon);
            return Json(new { _mesajbaslik, _mesaj, _MesajIcon });
        }
    }
    [HttpPost]
    public JsonResult OgrSeansYoklama(int ogrid, int seanskayitno, int type,string qrcode)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
    
        try
        {
            var seanskontrol = new SeansOgrSet();
            using (var context = new U1626744Db60AContext())
            {
                
                if (qrcode is null)
                {
                    
                    seanskontrol = context.SeansOgrSets.Where(x => x.Id == seanskayitno && x.OgrId == ogrid).Select(s => new SeansOgrSet
                    {
                        Id = s.Id,
                        OgrId = s.OgrId,
                        SeansId = s.SeansId,
                        Durum = s.Durum,
                        Seans = s.Seans,
                        Ogr = s.Ogr,
                        SeansKayitTarih = s.SeansKayitTarih,
                        Aciklama = s.Aciklama,
                        Qr = s.Qr
                    }).First();
                }
                else
                {
                    seanskontrol = context.SeansOgrSets.Where(x => x.Qr == qrcode).Select(s => new SeansOgrSet
                    {
                        Id = s.Id,
                        OgrId = s.OgrId,
                        Seans = context.DenemeSeanslars.Where(c => c.Id == s.SeansId).Select(r => new DenemeSeanslar()
                        {
                            Id = r.Id,
                            Deneme = r.Deneme,
                            Tarih = r.Tarih,
                            Saat = r.Saat
                        }).First(),
                        SeansId = s.SeansId,
                        Ogr = s.Ogr,
                        Durum = s.Durum,
                        SeansKayitTarih = s.SeansKayitTarih,
                        Aciklama = s.Aciklama,
                        Qr = s.Qr
                    }).First();
                }
                if (seanskontrol != null)
                {

                    var sninf = context.DenemeSeanslars
                        .Where(x => x.Id == x.SeansOgrSets.Where(x => x.Id == seanskayitno || x.Qr == qrcode).Select(s => s.SeansId)
                            .First()).Select(s => new viewSeansModel()
                        {
                            Id = s.Id,
                            yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                            KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                            kitapcikBaslik = s.Deneme.DenemeBaslik,
                            TarihSTR = s.Tarih.ToString("dd.MM.yyyy - dddd"),
                            Saat = s.Saat,
                            qrCodeforOgr = context.SeansOgrSets.Where(x => x.Id == seanskayitno).Select(s => s.Qr).First()
                        }).First();

                    
                    if (qrcode is not null)
                    {
                        
                        if (seanskontrol.Durum != 1 || seanskontrol.Durum == 0)
                        {
                            MesajBaslik = "Uyarı";
                            Mesaj = "Bu Seans Kaydı İçin Daha Önce Yoklama Alınmış";
                            MesajIcon = "warning";
                            return Json(new { MesajBaslik, Mesaj, MesajIcon });
                        }
                        else
                        {
                            MesajBaslik = "İşlem Başarılı";
                            Mesaj = "Yoklama Alındı <hr /> Seans: "+seanskontrol.Seans.Deneme.DenemeBaslik + " " + seanskontrol.Seans.Tarih.ToShortDateString() + " / " + seanskontrol.Seans.Saat + "<hr />" + seanskontrol.Ogr.Ad + " " + seanskontrol.Ogr.Soyad;
                            MesajIcon = "success";
                            seanskontrol.Durum = type;
                            context.Entry(seanskontrol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        var ogrinf = context.Ogrencilers.Where(x => x.Id == ogrid).Select(s => new Ogrenciler()
                        {
                            Id = s.Id,
                            Ad = s.Ad,
                            Soyad = s.Soyad,
                            Telefon = s.Telefon
                    
                        }).First();
                        string mesaj = "";
                        if (type == 1)
                        {
                            mesaj = "Ön Kayıt Talebi Onaylandı";
                        }
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = mesaj;
                        MesajIcon = "success";
                        seanskontrol.Durum = type;
                        context.Entry(seanskontrol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();
                        return Json(new { MesajBaslik, Mesaj, MesajIcon,ogrinf,sninf });

                    }
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });


                  
                }
                else
                {
                    MesajBaslik = "İşlem Başarısız";
                    var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);

                    Mesaj = "Seans Bulunamadı";
                    var _mesaj = JsonConvert.SerializeObject(Mesaj);
                    JsonConvert.SerializeObject(Mesaj);
                    MesajIcon = "error";
                    var _MesajIcon = MesajIcon;
                    JsonConvert.SerializeObject(MesajIcon);
                    return Json(new { _mesajbaslik, _mesaj, _MesajIcon });
                }
            }

        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);

            Mesaj = ex.Message + " HATA KODU BURDA";
            var _mesaj = JsonConvert.SerializeObject(Mesaj);
            JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            var _MesajIcon = MesajIcon;
            JsonConvert.SerializeObject(MesajIcon);
            return Json(new { _mesajbaslik, _mesaj, _MesajIcon });
        }
    }
    [HttpPost]
    public JsonResult seansbilgigetir(int? seansid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seans = context.DenemeSeanslars.Where(X => X.Deneme.Kategori.Kat.KutuphaneId == kutuphaneid && X.Id == seansid).Select(S => new viewSeansModel
                {
                    Id = S.Id,
                    kitapcikBaslik = S.Deneme.DenemeBaslik,
                    KategoriBaslik = S.Deneme.Kategori.Kat.AnaKategoriBaslik + "/" + S.Deneme.Kategori.AltKategoriBaslik,
                    yayinBaslik = S.Deneme.Yayin.YayinBaslik,
                    Kontenjan = S.Kontenjan,
                    GuncelKontenjan = S.Kontenjan - S.SeansOgrSets.Where(x => x.Durum == 2 || x.Durum == 4 || x.Durum == 1).Count(),
                    TarihSTR = S.Tarih.ToShortDateString() + " - " + S.Tarih.ToString("dddd") + " / " + S.Saat,
                    Tarih = S.Tarih,
                    Saat = S.Saat,
                    DenemeId = S.DenemeId,
                    Durum = S.Durum,
                    SeansUcret = S.SeansUcret,
                    yayinLogo = S.Deneme.Yayin.Logo,
                    KitapcikAlanToplam = S.SeansOgrSets.Where(x => x.Durum == 4).Count(),
                    KatilimSaglayanToplam = S.SeansOgrSets.Where(x => x.Durum == 2).Count(),
                    KayitliOgrenci = S.SeansOgrSets.Where(x => x.Durum != 0).Count()
                }).OrderBy(o => o.Tarih).First();
                var jsonresult = JsonConvert.SerializeObject(seans);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    [HttpPost]
    public JsonResult seansbildirimlist(int seansid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var bildirimlogs = context.Seansbildirimlogs.Where(x => x.SeansId == seansid).Select(s =>
                    new Seansbildirimlog()
                    {
                        Id = s.Id,
                        Seans = s.Seans,
                        LogTarih = s.LogTarih,
                        Bildirim = s.Bildirim
                    }).OrderByDescending(o => o.LogTarih).ToList();
                var jsonresult = JsonConvert.SerializeObject(bildirimlogs);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);

            Mesaj = ex.Message.ToString();
            var _mesaj = JsonConvert.SerializeObject(Mesaj);
            JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            var _MesajIcon = MesajIcon;
            JsonConvert.SerializeObject(MesajIcon);
            return Json(new { _mesajbaslik, _mesaj, _MesajIcon });
        }
    }


    public JsonResult ogrlistfornonseans(int kitapcikid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                int seanstoplam = context.DenemeSeanslars.Where(x => x.DenemeId == kitapcikid).Count();
                int kategoriId = context.DenemeSeanslars.Where(x => x.DenemeId == kitapcikid)
                    .Select(s => s.Deneme.Kategori.KatId).FirstOrDefault();
                var seanscontrol = context.SeansOgrSets.Where(x => x.Seans.DenemeId == kitapcikid).Select(s => new viewSeansOgrSetModel()
                {
                    Id = s.Id,
                    OgrId = s.OgrId,
                    SeansInfo = s.Seans,
                    SeansId = s.SeansId,
                    katid = s.Seans.Deneme.Kategori.Kat.Id
                }).ToList();
                var ogrlist = context.Ogrencilers.Where(x => x.KutuphaneId == kutuphaneid && x.KategoriId == kategoriId && x.SeansOgrSets.Where(x => x.Seans.DenemeId == kitapcikid).Count() <= 0 && x.Ktarih.Date.Year >= DateTime.Now.Date.AddYears(-1).Year).Select(s => new viewOgrencilerModel()
                {
                    Id = s.Id,
                    Ad = s.Ad,
                    Soyad = s.Soyad,
                    Telefon = s.Telefon,
                    Kategori = s.Kategori,
                    KategoriId = s.KategoriId
                }).ToList();

                var jsondata = JsonConvert.SerializeObject(ogrlist.OrderBy(o => o.Ad).ToList());
                return Json(jsondata);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }

    [HttpPost]
    public JsonResult ogrlistforseans(int? seansid, int? param)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        auth = (int)HttpContext.Session.GetInt32("adminAuth");
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                if (param == 1)
                {
                    var ogrlist = context.SeansOgrSets.Where(a => a.Ogr.KutuphaneId == kutuphaneid).Select(s => new viewOgrencilerModel
                    {
                        Id = s.Ogr.Id,
                        snsetid = s.Id,
                        seansid = context.SeansOgrSets.Where(x => x.Id == s.Id).Select(b => b.SeansId).First(),
                        Ad = s.Ogr.Ad.ToUpper(),
                        Soyad = s.Ogr.Soyad.ToUpper(),
                        Telefon = s.Ogr.Telefon,
                        Durum = s.Durum,
                        Ktarih = s.SeansKayitTarih,
                        KtarihSTR = s.SeansKayitTarih.ToString("dd.MM.yyyy - dddd")
                    }).OrderBy(o => o.Ad).ToList();
                    var jsonresult = JsonConvert.SerializeObject(ogrlist);
                    return Json(new { jsonresult, auth });
                }
                else
                {
                    var ogrlist = context.SeansOgrSets.Where(a => a.SeansId == seansid).Select(s => new viewOgrencilerModel
                    {
                        Id = s.Ogr.Id,
                        snsetid = s.Id,
                        seansid = context.SeansOgrSets.Where(x => x.Id == s.Id).Select(b => b.SeansId).First(),
                        Ad = s.Ogr.Ad.ToUpper(),
                        Soyad = s.Ogr.Soyad.ToUpper(),
                        Telefon = s.Ogr.Telefon,
                        Durum = s.Durum,
                        Ktarih = s.SeansKayitTarih,
                        KtarihSTR = s.SeansKayitTarih.ToString("dd.MM.yyyy - dddd")
                    }).OrderBy(o => o.Ad).ToList();
                    var jsonresult = JsonConvert.SerializeObject(ogrlist);
                    return Json(new { jsonresult, auth });
                }

            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.Message + " (" + ex.ToString() + ")";
            MesajIcon = "error";
            return Json(new { MesajBaslik, Mesaj, MesajIcon });
        }
    }
    [HttpPost]
    public JsonResult onKayitList()
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var onkayitlist = context.SeansOgrSets.Where(x => x.Ogr.KutuphaneId == kutuphaneid && x.Durum == 0).Select(s => new viewSeansOgrSetModel
                {
                    Id = s.Id,
                    Ogr = s.Ogr,
                    SeansId = s.SeansId,
                    Durum = s.Durum,
                    yayinAd = s.Seans.Deneme.Yayin.YayinBaslik,
                    kategoriBaslik = s.Seans.Deneme.Kategori.AltKategoriBaslik,
                    seansTarih = s.Seans.Tarih.ToString("dd.MM.yyyy") + " " + s.Seans.Saat,
                    tarihSaat = s.SeansKayitTarih.ToString("dd.MM.yyyy"),
                    denemeAd = s.Seans.Deneme.DenemeBaslik,
                    kayitUcret = s.Seans.SeansUcret,
                    SeansKayitTarih = s.SeansKayitTarih
                }).OrderByDescending(o => o.SeansKayitTarih).ToList();
                var jsonresult = JsonConvert.SerializeObject(onkayitlist);
                var gunlukkayitlist = context.SeansOgrSets.Where(x => x.Ogr.KutuphaneId == kutuphaneid && x.Durum != 0 && x.Seans.Tarih.Date == DateTime.Now.Date).Select(s => new viewSeansOgrSetModel
                {
                    Id = s.Id,
                    Ogr = s.Ogr,
                    SeansId = s.SeansId,
                    Durum = s.Durum,
                    yayinAd = s.Seans.Deneme.Yayin.YayinBaslik,
                    kategoriBaslik = s.Seans.Deneme.Kategori.AltKategoriBaslik,
                    seansTarih = s.Seans.Tarih.ToString("dd.MM.yyyy") + " " + s.Seans.Saat,
                    tarihSaat = s.SeansKayitTarih.ToString("dd.MM.yyyy"),
                    denemeAd = s.Seans.Deneme.DenemeBaslik,
                    kayitUcret = s.Seans.SeansUcret,
                    SeansKayitTarih = s.SeansKayitTarih
                }).OrderByDescending(o => o.SeansKayitTarih).ToList();
                var jsonresult2 = JsonConvert.SerializeObject(gunlukkayitlist);
                return Json(new { jsonresult, jsonresult2 });
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [HttpPost]
    public JsonResult seanskayitaktifpasif(int seansid, int type)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seans = context.DenemeSeanslars.Where(x => x.Id == seansid).Select(s  => new DenemeSeanslar()
                {
                    Id = s.Id,
                    DenemeId = s.DenemeId,
                    Tarih = s.Tarih,
                    Saat = s.Saat,
                    SeansUcret = s.SeansUcret,
                    Kontenjan = s.Kontenjan,
                    Durum = type
                }).First();
                context.Entry(seans).State = EntityState.Modified;
                context.SaveChanges();
                string durumstr = "";
                if (type == 1)
                {
                    durumstr = "Açıldı";
                }
                else if (type == 4)
                {
                    durumstr = "Kapatıldı";
                }

                MesajIcon = "info";
                MesajBaslik = "İşlem Başarılı";
                string baslik = JsonConvert.SerializeObject(MesajBaslik);
                Mesaj = "Seans Kayıt Alımına " + durumstr;
                string mesaj = JsonConvert.SerializeObject(Mesaj);
                string icon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { baslik, mesaj, icon });
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            Mesaj = ex.Message;
            string mesaj = JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            string icon = JsonConvert.SerializeObject(MesajIcon);
            return Json(new { baslik, mesaj, icon });
        }
    }
    [HttpPost]
    public JsonResult SeansListForOgr(int? ogrid, int? yayinid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                int kategoriID = context.Ogrencilers.Where(x => x.Id == ogrid).Select(s => s.KategoriId).First();
                var seanslist = context.DenemeSeanslars.Where(x => x.Deneme.Kategori.KatId == kategoriID && x.Deneme.YayinId == yayinid && x.Kontenjan > x.SeansOgrSets.Where(a => a.Durum == 1).Count() && x.SeansOgrSets.Where(b => b.OgrId == ogrid && b.Seans.DenemeId == x.DenemeId).Count() <= 0 && x.Tarih.Date >= DateTime.Now.Date).Select(s => new viewSeansModel
                {
                    Id = s.Id,
                    Deneme = s.Deneme,
                    DenemeId = s.DenemeId,
                    Kontenjan = s.Kontenjan,
                    kitapcikBaslik = s.Deneme.DenemeBaslik,
                    KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                    yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                    GuncelKontenjan = s.Kontenjan - s.SeansOgrSets.Where(x => x.Durum == 1).Count(),
                    TarihSTR = s.Tarih.ToString("dd.MM.yyyy") + " / " + s.Tarih.ToString("dddd"),
                    Saat = s.Saat,
                    Tarih = s.Tarih,
                    SeansOgrSets = s.SeansOgrSets.OrderBy(o => o.Ogr.Ad).ToList()
                }).OrderBy(o => o.Tarih).ToList();
                var jsonresult = JsonConvert.SerializeObject(seanslist);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.ToString() + " (" + ex.Message + ")";
            MesajIcon = "error";
            return Json(new { Mesaj, MesajBaslik, MesajIcon });
        }
    }

    [HttpPost]
    public JsonResult seansiptal(int seansid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seans = context.DenemeSeanslars.Where(x => x.Id == seansid).Select(s => new DenemeSeanslar()
                {
                    Id = s.Id,
                }).First();
                var seansbildirim = context.Seansbildirimlogs.Where(x => x.SeansId == seansid).ToList();
                context.Seansbildirimlogs.RemoveRange(seansbildirim);
                context.SaveChanges();
                var ogrlist = context.SeansOgrSets.Where(x => x.SeansId == seansid).ToList();
                context.SeansOgrSets.RemoveRange(ogrlist);
                context.SaveChanges();
                context.DenemeSeanslars.RemoveRange(seans);
                context.SaveChanges();

                MesajBaslik = "İşlem Başarılı";
                Mesaj = "Deneme Sınavı Seansı Silinmiş Olup, Kayıtlı Tüm Öğrencilerin Sınav Kayıtları İptal Edilmiştir";
                MesajIcon = "success";

            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = "Hata Kodu: " + ex.Message;
            MesajIcon = "error";
        }
        string baslik = JsonConvert.SerializeObject(MesajBaslik);
        string mesaj = JsonConvert.SerializeObject(Mesaj);
        string icon = JsonConvert.SerializeObject(MesajIcon);
        return Json(new { baslik, mesaj, icon });
    }

    [HttpPost]
    public JsonResult seansayarset(viewSeansModel model)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seans = context.DenemeSeanslars.Where(x => x.Id == model.Id).Select(s => new DenemeSeanslar()
                {
                    Id = s.Id,
                    Tarih = s.Tarih,
                    Saat = s.Saat,
                    SeansUcret = s.SeansUcret,
                    Kontenjan = s.Kontenjan,
                    DenemeId = s.DenemeId
                }).First();
                seans.Tarih = model.Tarih;
                seans.Saat = model.Saat;
                seans.SeansUcret = model.SeansUcret;
                seans.Kontenjan = model.Kontenjan;
                seans.DenemeId = model.DenemeId;
                context.Entry(seans).State = EntityState.Modified;
                context.SaveChanges();
                MesajBaslik = "İşlem Başarılı";
                Mesaj = "Seans Bilgileri Güncellendi";
                MesajIcon = "success";
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = "Hata Kodu: " + ex.Message;
            MesajIcon = "error";
        }
        string baslik = JsonConvert.SerializeObject(MesajBaslik);
        string mesaj = JsonConvert.SerializeObject(Mesaj);
        string icon = JsonConvert.SerializeObject(MesajIcon);
        return Json(new { baslik, mesaj, icon });
    }

    [HttpPost]
    public JsonResult seansonkayitlist(int seansid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seansonkayit = context.SeansOgrSets.Where(x => x.SeansId == seansid && x.Durum == 0).Select(s =>
                    new viewOgrencilerModel()
                    {
                        Id = s.Ogr.Id,
                        Ad = s.Ogr.Ad,
                        Soyad = s.Ogr.Soyad,
                        Telefon = s.Ogr.Telefon,
                        Ktarih = s.SeansKayitTarih,
                        KtarihSTR = s.SeansKayitTarih.ToString("dddd.MM.yyyy - ddddd")
                    }).ToList();

                var jsonresult = JsonConvert.SerializeObject(seansonkayit);
                return Json(jsonresult);
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "İşlem Başarısız";
            Mesaj = "Hata Kodu: " + ex.Message;
            MesajIcon = "error";

            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            string mesaj = JsonConvert.SerializeObject(Mesaj);
            string icon = JsonConvert.SerializeObject(MesajIcon);
            return Json(new { baslik, mesaj, icon });
        }
    }

           [HttpPost]
    public JsonResult seanschange(int snsetid, int yeniseansid)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        using (U1626744Db60AContext context = new U1626744Db60AContext())
        {
            try
            {
                SeansOgrSet getseansset = context.SeansOgrSets.Where(x => x.Id == snsetid).First();
                DenemeSeanslar eskiseans = context.DenemeSeanslars.Where(x => x.Id == x.SeansOgrSets.FirstOrDefault(a => a.Id == snsetid).SeansId).Select(s => new DenemeSeanslar()
                {
                    Id = s.Id,
                    Tarih = s.Tarih,
                    Saat = s.Saat,
                    Kontenjan = s.Kontenjan,
                    SeansOgrSets = s.SeansOgrSets.ToList(),
                    SeansUcret = s.SeansUcret,
                    DenemeId = s.DenemeId,
                    Deneme = s.Deneme,
                    Durum = s.Durum
                }).First();
                string eskiseansinfo = eskiseans.Deneme.DenemeBaslik + " " +
                                       eskiseans.Tarih.ToString("dd.MM.yyyy - dddd") + " / " + eskiseans.Saat;
                DenemeSeanslar yeniseans = context.DenemeSeanslars.Where(x => x.Id == yeniseansid).Select(s => new DenemeSeanslar()
                {
                    Id = s.Id,
                    Tarih = s.Tarih,
                    Saat = s.Saat,
                    Kontenjan = s.Kontenjan,
                    SeansOgrSets = s.SeansOgrSets.ToList(),
                    SeansUcret = s.SeansUcret,
                    DenemeId = s.DenemeId,
                    Deneme = s.Deneme,
                    Durum = s.Durum
                }).First();
                string yeniseansinfo = yeniseans.Deneme.DenemeBaslik + " " +
                                       yeniseans.Tarih.ToString("dd.MM.yyyy - dddd") + " / " + yeniseans.Saat;
                var ogrenci = context.Ogrencilers.FirstOrDefault(x => x.Id == getseansset.OgrId);
                int seanstoplamogr = yeniseans.SeansOgrSets.Where(x => x.Durum != 0).Count();
                int toplamkontenjan = yeniseans.Kontenjan;
                if (toplamkontenjan > seanstoplamogr)
                {
                    getseansset.SeansId = yeniseansid;
                    context.Entry(getseansset).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    MesajBaslik = "İşlem Başarılı";
                    Mesaj = "Seans Değişikliği Yapıldı";
                    MesajIcon = "success";

                }
                else
                {
                    MesajBaslik = "İşlem Başarısız";
                    Mesaj = "Seçilen Seans İçin Boş Kontenjan Kalmamıştır.";
                    MesajIcon = "warning";
                }
                string baslik = JsonConvert.SerializeObject(MesajBaslik);
                string mesaj = JsonConvert.SerializeObject(Mesaj);
                string icon = JsonConvert.SerializeObject(MesajIcon);
                string wpCustomMsg = "whatsapp://send?text=";
                wpCustomMsg += "Sn. " + ogrenci.Ad.ToUpper() + " " + ogrenci.Soyad.ToUpper() + "%0A";
                wpCustomMsg += "Seans Kaydınız " + eskiseansinfo + " Oturumunda -> " + yeniseansinfo +
                               " Oturumuna Aktarılmıştır %0A https://uyaniksistem.com/QRView/index?qrcode="+getseansset.Qr;
                wpCustomMsg += "&phone=+9" + ogrenci.Telefon;
                return Json(new { baslik, mesaj, icon, wpCustomMsg });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

    [HttpPost]
    public JsonResult SeansAction(viewSeansModel model, int? type)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                try
                {
                    int denemeID = model.DenemeId;
                    DateTime seansTarih = model.Tarih;
                    int seansKontenjan = context.DenemeStoklars
                        .Where(x => x.DenemeId == model.DenemeId && x.StokType == 1).Sum(t => t.Adet) - context
                        .DenemeStoklars
                        .Where(x => x.DenemeId == model.DenemeId && x.StokType == 2).Sum(t => t.Adet);
                    string saat = model.Saat;
                    if (type == 1)
                    {
                        DenemeSeanslar seans = new DenemeSeanslar();
                        seans.DenemeId = denemeID;
                        seans.Tarih = seansTarih;
                        seans.Kontenjan = model.Kontenjan;
                        seans.Saat = saat;
                        seans.SeansUcret = model.SeansUcret;
                        seans.Durum = 1;
                        context.DenemeSeanslars.Add(seans);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Seans Oluşturuldu";
                        MesajIcon = "success";

                    }
                    else if (type == 2)
                    {
                        var seansUp = context.DenemeSeanslars.Where(x => x.Id == model.Id).First();
                        seansUp.DenemeId = denemeID;
                        seansUp.Kontenjan = model.Kontenjan;
                        seansUp.Tarih = seansTarih;
                        seansUp.Kontenjan = model.Kontenjan;
                        seansUp.Saat = saat;
                        seansUp.SeansUcret = model.SeansUcret;
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Seans Bilgileri Güncellendi";
                        MesajIcon = "success";
                        context.SaveChanges();
                    }
                    else if (type == 0)
                    {
                        List<SeansOgrSet> ogrOfSeans = context.SeansOgrSets.Where(x => x.SeansId == model.Id).ToList();
                        context.SeansOgrSets.RemoveRange(ogrOfSeans);
                        context.SaveChanges();
                        DenemeSeanslar seansCancel = context.DenemeSeanslars.Where(x => x.Id == model.Id).First();
                        context.DenemeSeanslars.Remove(seansCancel);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Seans İptal Edilmiş Olup, Bu Seansa Bağlı Tüm Öğrenci Kayıtları İptal Edilmiştir.(Bu İşlem Geri Alınamamaktadır)";
                        MesajIcon = "success";
                    }
                }
                catch (Exception ex)
                {
                    MesajBaslik = "İşlem Başarısız";
                    Mesaj = ex.Message;
                    MesajIcon = "error";
                }

                string title = JsonConvert.SerializeObject(MesajBaslik);
                string msg = JsonConvert.SerializeObject(Mesaj);
                string icon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { title, msg, icon });
            }
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.ToString();
            MesajIcon = "error";
            return Json(new { Mesaj, MesajBaslik, MesajIcon });
        }

    }

    [HttpPost]
    public JsonResult ogrseanskayitsil(int id)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seanskayit = context.SeansOgrSets.First(x => x.Id == id);
                context.SeansOgrSets.Remove(seanskayit);
                context.SaveChanges();
                var seans = context.DenemeSeanslars.FirstOrDefault();
                Mesaj = "Öğrenci Seans Kaydı Silindi";
                MesajBaslik = "İşlem Başarılı";
                MesajIcon = "info";

            }
        }
        catch (Exception ex)
        {
            Mesaj = "Hata Oluştu: " + ex.Message;
            MesajBaslik = "İşlem Başarısız";
            MesajIcon = "error";
        }
        string msg = JsonConvert.SerializeObject(Mesaj);
        string title = JsonConvert.SerializeObject(MesajBaslik);
        string icon = JsonConvert.SerializeObject(MesajIcon);
        return Json(new { msg, title, icon });
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
    public JsonResult multipushnotification(int type, int seansid, int kategoriid, string baslik, string mesaj)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using (var context = new U1626744Db60AContext())
            {
                var seans = new DenemeSeanslar();
                var kategori = new List<Anakategoriler>();
                var ogrlist = new List<Ogrenciler>();
                string token = "";
                if (type == 1) // Kategori bazlı toplu bildirim
                {
                    using (var contexte = new U1626744Db60AContext())
                    {
                        kategori = context.Anakategorilers.Where(x => x.Id == kategoriid).Select(s =>
                            new Anakategoriler()
                            {
                                Id = s.Id,
                                AnaKategoriBaslik = s.AnaKategoriBaslik
                            }).ToList();
                        for (int i = 0; i <= kategori.Count(); i++)
                        {
                            ogrlist = context.Ogrencilers
                                .Where(x => x.KutuphaneId == kutuphaneid && x.KategoriId == kategoriid).Select(s =>
                                    new Ogrenciler()
                                    {
                                        Id = s.Id,
                                        Ad = s.Ad,
                                        Soyad = s.Soyad,
                                        Ogrtokens = s.Ogrtokens
                                    }).OrderBy(o => o.Ad).ToList();
                            token = ogrlist.Select(s => s.Ogrtokens.Select(s => s.Token).First()).First();

                            foreach (var ogr in ogrlist)
                            {
                                bildirimGonder(mesaj, baslik, token, 2, ogr.Id);
                                Bildirimler bildirim = new Bildirimler();
                                bildirim.Baslik = baslik;
                                bildirim.Mesaj = mesaj;
                                bildirim.Tarih = DateTime.Now;
                                bildirim.KutuphaneId = kutuphaneid;
                                context.Bildirimlers.Add(bildirim);
                                context.SaveChanges();
                                int bildirimno = context.Bildirimlers.Max(m => m.Id);
                                Ogrbildirimlog opnslog = new Ogrbildirimlog();
                                opnslog.BildirimId = bildirimno;
                                opnslog.OgrId = ogr.Id;
                                opnslog.LogTarih = DateTime.Now;
                                context.Ogrbildirimlogs.Add(opnslog);
                                context.SaveChanges();
                                MesajBaslik = "İşlem Başarılı";
                                Mesaj = kategori[0].AnaKategoriBaslik +
                                        " Kategorisine tanımlı tüm kişiler için bildirim gönderildi";
                                MesajIcon = "success";
                            }
                        }
                    }
                }
                else if (type == 2) // Seans bazlı toplu bildirim
                {
                    seans = context.DenemeSeanslars.Where(x => x.Id == seansid).Select(s => new DenemeSeanslar()
                    {
                        Id = s.Id,
                        SeansUcret = s.SeansUcret,
                        Tarih = s.Tarih,
                        Saat = s.Saat

                    }).First();
                    var seansogrlist = context.SeansOgrSets.Where(a => a.SeansId == seans.Id).Select(b => new viewOgrencilerModel()
                    {
                        Id = b.Ogr.Id,
                        Ad = b.Ogr.Ad,
                        Soyad = b.Ogr.Soyad,
                        Telefon = b.Ogr.Telefon,
                        ogrtoken = b.Ogr.Ogrtokens.Where(x => x.OgrId == b.Ogr.Id).Select(s => s.Token).First(),
                    }).ToList();
                    string tokenn = "";
                    foreach (var ogr in seansogrlist)
                    {
                        tokenn = ogr.ogrtoken;

                        Bildirimler bildirim = new Bildirimler();
                        bildirim.Baslik = baslik;
                        bildirim.Mesaj = mesaj;
                        bildirim.Tarih = DateTime.Now;
                        bildirim.KutuphaneId = kutuphaneid;
                        context.Bildirimlers.Add(bildirim);
                        context.SaveChanges();
                        int bildirimno = context.Bildirimlers.Max(m => m.Id);
                        Seansbildirimlog snpnslog = new Seansbildirimlog();
                        snpnslog.BildirimId = bildirimno;
                        snpnslog.SeansId = seans.Id;
                        snpnslog.LogTarih = DateTime.Now;
                        context.Seansbildirimlogs.Add(snpnslog);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Toplu bildirim mesajı gönderilmiştir.";
                        MesajIcon = "success";
                        try
                        {
                            if (tokenn != null)
                            {
                                bildirimGonder(mesaj, baslik, tokenn, 3, ogr.Id);
                            }
                        }
                        catch
                        {

                        }

                    }
                }
            }
            return Json("İşlem Yapılmadı");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    [HttpPost]
    public JsonResult yoklamalistforogr(int ogrid, int parameter)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using (var context = new U1626744Db60AContext())
            {
                List<viewSeansOgrSetModel> list = context.SeansOgrSets
                    .Where(x => x.OgrId == ogrid && x.Durum == parameter).Select(s => new viewSeansOgrSetModel()
                    {
                        Id = s.Id,
                        denemeAd = s.Seans.Deneme.DenemeBaslik,
                        tarihSaat = s.SeansKayitTarih.ToString("dd.MM.yyyy - dddd"),
                        seansTarih = s.Seans.Tarih.ToString("dd.MM.yyyy - dddd")
                    }).OrderBy(o => o.denemeAd).ToList();
                return Json(JsonConvert.SerializeObject(list));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public string nwguidtext { get; set; }
    [HttpPost]
    public IActionResult ogrSeansKayitMulti([FromBody] string[] l)
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {

            using (var context = new U1626744Db60AContext())
            {

                int ogrID = Convert.ToInt32(l[0]);
                int lCount = l.Count();
                if (lCount != 1)
                {
                    var ogrSet = context.Ogrencilers.Where(x => x.Id == ogrID).Select(y => new Ogrenciler
                    {
                        Id = y.Id,
                        Ad = y.Ad.ToUpper(),
                        Soyad = y.Soyad.ToUpper(),
                        Telefon = y.Telefon,
                        Ogrtokens = y.Ogrtokens,
                        Qrasc = y.Qrasc
                    }).First();
                    string toplamTutar = "";
                    string wp1 = "";
                    string wp2 = "";
                    var basarisizseanslar = new List<viewSeansModel>();
                    var basariliseanslar = new List<viewSeansModel>();
                    for (int i = 1; i <= lCount - 1; i++)
                    {
                        nwguidtext = Guid.NewGuid().ToString();
                        int seans_set_id = Convert.ToInt32(l[i]);
                        int kitapcikid = 0;
                        // 480 - 224
                        DenemeSeanslar seansinn = context.DenemeSeanslars.Where(x => x.Id == seans_set_id).Select(s => new DenemeSeanslar
                        {
                            Id = s.Id,
                            DenemeId = s.DenemeId,
                            Deneme = s.Deneme

                        }).First();
                        kitapcikid = seansinn.DenemeId;
                        var ogrSeansKontrol = context.SeansOgrSets.Where(x => x.SeansId == seans_set_id && x.OgrId == ogrID && x.Seans.DenemeId == kitapcikid).Select(a => new viewSeansOgrSetModel
                        {
                            Id = a.Id,
                            OgrId = a.OgrId,
                            SeansId = a.SeansId,
                            Durum = a.Durum,
                            SeansKayitTarih = a.SeansKayitTarih
                        }).FirstOrDefault();
                        int seansKontenjanBaslangic = 0;
                        seansKontenjanBaslangic = context.DenemeSeanslars.Where(x => x.Id == seans_set_id).Select(s => s.Kontenjan).FirstOrDefault();
                        int seansToplamKayit = 0;
                        seansToplamKayit = context.SeansOgrSets.Where(x => x.SeansId == seans_set_id).Count();
                        var seansSet = context.DenemeSeanslars.Where(x => x.Id == seans_set_id).Select(y => new viewSeansModel
                        {
                            Id = y.Id,
                            kitapcikBaslik = y.Deneme.DenemeBaslik.ToUpper(),
                            SeansUcret = y.SeansUcret,
                            Durum = y.Durum,
                            Saat = y.Saat,
                            Kontenjan = y.Kontenjan,
                            Tarih = y.Tarih,
                            yayinBaslik = y.Deneme.Yayin.YayinBaslik.ToUpper(),
                            KategoriBaslik = y.Deneme.Kategori.AltKategoriBaslik.ToUpper(),
                            TarihSTR = y.Tarih.ToShortDateString(),
                            SeansGun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)y.Tarih.DayOfWeek]
                        }).FirstOrDefault();
                        if (seansKontenjanBaslangic > seansToplamKayit)
                        {

                            if (ogrSeansKontrol == null)
                            {
                                List<SeansOgrSet> toplam = context.SeansOgrSets.Where(x => x.OgrId == ogrID).Select(s => new SeansOgrSet
                                {
                                    Id = s.Id,
                                    OgrId = s.OgrId,
                                    SeansId = s.Seans.DenemeId,

                                }).ToList();

                                SeansOgrSet toplam2 = toplam.Where(y => y.SeansId == kitapcikid).FirstOrDefault();
                                if (toplam2 != null)
                                {
                                    context.SeansOgrSets.Remove(toplam2);
                                    context.SaveChanges();
                                    basarisizseanslar.Add(new viewSeansModel()
                                    {
                                        Id = seansSet.Id,
                                        kitapcikBaslik = seansSet.kitapcikBaslik,
                                        KategoriBaslik = seansSet.KategoriBaslik,
                                        Tarih = seansSet.Tarih,
                                        TarihSTR = seansSet.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                        Saat = seansSet.Saat,
                                        yayinBaslik = seansSet.yayinBaslik,
                                        SeansUcret = seansSet.SeansUcret
                                    });
                                }
                                else
                                {
                                    if (false)
                                    {
                                        basarisizseanslar.Add(new viewSeansModel()
                                        {
                                            Id = seansSet.Id,
                                            kitapcikBaslik = seansSet.kitapcikBaslik,
                                            KategoriBaslik = seansSet.KategoriBaslik,
                                            TarihSTR = seansSet.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                            Saat = seansSet.Saat,
                                            yayinBaslik = seansSet.yayinBaslik,
                                            SeansUcret = seansSet.SeansUcret
                                        });

                                    }
                                    else
                                    {
                                        var toplamkitapcik = context.DenemeStoklars.Where(x => x.DenemeId == kitapcikid).Select(s => new DenemeStoklar {
                                        Id = s.Id,
                                        StokType = s.StokType,
                                        Adet = s.Adet,
                                        DenemeId = s.DenemeId})
            .ToList();
                                        int girisstok = context.DenemeStoklars.Where(x => x.DenemeId == kitapcikid && x.StokType == 1).Select(s => new DenemeStoklar
                                        {
                                            Id = s.Id,
                                            StokType = s.StokType,
                                            Adet = s.Adet,
                                            DenemeId = s.DenemeId
                                        }).Sum(t => t.Adet);
                                        int cikisstok = context.DenemeStoklars.Where(x => x.DenemeId == kitapcikid && x.StokType == 2).Select(s => new DenemeStoklar
                                        {
                                            Id = s.Id,
                                            StokType = s.StokType,
                                            Adet = s.Adet,
                                            DenemeId = s.DenemeId
                                        }).Sum(t => t.Adet);

                                        int toplamstok = girisstok - cikisstok - seansToplamKayit;

                                        if (toplamstok > 0)
                                        {

                                            string ogrToken = ogrSet.Ogrtokens.Select(s => s.Token).FirstOrDefault();
                                            string bildirimMesaj = seansSet.yayinBaslik + " - " + seansSet.KategoriBaslik + " - " + seansSet.kitapcikBaslik + " - " + seansSet.TarihSTR + " / " + seansSet.SeansGun + " / " + seansSet.Saat;
                                            SeansOgrSet ogrSeansKayit = new SeansOgrSet();
                                            ogrSeansKayit.OgrId = ogrSet.Id;
                                            ogrSeansKayit.SeansId = seans_set_id;
                                            ogrSeansKayit.SeansKayitTarih = DateTime.Now.Date;
                                            ogrSeansKayit.Durum = 1;
                                            ogrSeansKayit.Qr = nwguidtext;
                                            context.SeansOgrSets.Add(ogrSeansKayit);
                                            context.SaveChanges();
                                            basariliseanslar.Add(context.DenemeSeanslars.Where(x => x.Id == seans_set_id)
                                                .Select(s => new viewSeansModel()
                                                {
                                                    Id = s.Id,
                                                    kitapcikBaslik = s.Deneme.DenemeBaslik,
                                                    yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                                                    KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                                                    SeansUcret = s.SeansUcret,
                                                    Tarih = s.Tarih,
                                                    TarihSTR = s.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                                    Saat = s.Saat,
                                                    qrCodeforOgr = nwguidtext
                                                }).First());
                                            seansSet.Kontenjan = toplamstok - 1;
                                          
                                            double seansUcret = context.DenemeSeanslars.Where(s => s.Id == seans_set_id).Select(u => u.SeansUcret).FirstOrDefault();
                                            try { bildirimGonder(bildirimMesaj, "Seans Kaydınız Alındı", ogrToken, 2, ogrSet.Id); } catch { continue; }
                                            OgrenciOdemeler ogrOdeme = new OgrenciOdemeler();
                                            ogrOdeme.OgrId = ogrSet.Id;
                                            ogrOdeme.Tarih = DateTime.Now;
                                            ogrOdeme.Tutar = seansUcret;
                                            ogrOdeme.Durum = 1;
                                            context.OgrenciOdemelers.Add(ogrOdeme);
                                            context.SaveChanges();
                                            var seansinfo = context.DenemeSeanslars.Where(x => x.Id == seans_set_id).Select(
                                                s => new viewSeansModel
                                                {
                                                    Id = s.Id,
                                                    DenemeId = s.DenemeId,
                                                    yayinId = s.Deneme.YayinId,
                                                    Tarih = s.Tarih,
                                                    TarihSTR = s.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                                    Saat = s.Saat,
                                                    Durum = s.Durum,
                                                    KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                                                    SeansUcret = s.SeansUcret,
                                                    yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                                                    kitapcikBaslik = s.Deneme.DenemeBaslik
                                                }).First();


                                        }
                                        else
                                        {
                                            basarisizseanslar.Add(new viewSeansModel()
                                            {
                                                Id = seansSet.Id,
                                                kitapcikBaslik = seansSet.kitapcikBaslik,
                                                KategoriBaslik = seansSet.KategoriBaslik,
                                                TarihSTR = seansSet.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                                Saat = seansSet.Saat,
                                                yayinBaslik = seansSet.yayinBaslik,
                                                SeansUcret = seansSet.SeansUcret
                                            });
                                            MesajBaslik = "İşlem İptal Edildi";
                                            Mesaj = "Öğrenci Bu Deneme Sınavına Daha Önceden Kayıt Olmuştur";
                                            MesajIcon = "warning";

                                        }
                                    }

                                }





                            }
                            else
                            {
                                basarisizseanslar.Add(new viewSeansModel()
                                {
                                    Id = seansSet.Id,
                                    kitapcikBaslik = seansSet.kitapcikBaslik,
                                    KategoriBaslik = seansSet.KategoriBaslik,
                                    TarihSTR = seansSet.Tarih.ToString("dd.MM.yyyy - ddddd"),
                                    Saat = seansSet.Saat,
                                    yayinBaslik = seansSet.yayinBaslik,
                                    SeansUcret = seansSet.SeansUcret
                                });
                                MesajBaslik = "İşlem İptal Edildi";
                                Mesaj = "Öğrenci Bu Deneme Sınavına Daha Önceden Kayıt Olmuştur";
                                MesajIcon = "warning";

                            }
                        }
                        else
                        {
                            var seanssonuc = context.DenemeSeanslars.Where(x => x.Id == seans_set_id).Select(s => new DenemeSeanslar()
                            {
                                Id = s.Id,
                                Durum = s.Durum,
                                Deneme = context.Denemelers.Where(a => a.Id == s.Deneme.Id).Select(c => new Denemeler()
                                {
                                    Id = c.Id,
                                    Yayin = c.Yayin,
                                    DenemeBaslik = c.DenemeBaslik,
                                    Kategori = c.Kategori
                                }).First(),
                                Tarih = s.Tarih,
                                Kontenjan = s.Kontenjan,
                                SeansUcret = s.SeansUcret,
                                Saat = s.Saat,
                                Seansbildirimlogs = s.Seansbildirimlogs,
                                SeansOgrSets = s.SeansOgrSets,
                                DenemeId = s.DenemeId
                            }).FirstOrDefault();
                            seanssonuc.Durum = 2;
                            context.Entry(seanssonuc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            context.SaveChanges();

                            basarisizseanslar.Add(new viewSeansModel
                            {
                                Id = seanssonuc.Id,
                                kitapcikBaslik = seanssonuc.Deneme.DenemeBaslik,
                                KategoriBaslik = seanssonuc.Deneme.Kategori.AltKategoriBaslik,
                                yayinBaslik = seanssonuc.Deneme.Yayin.YayinBaslik,
                                TarihSTR = seanssonuc.Tarih.ToString("dd.MM.yyyy dddd"),
                                Saat = seanssonuc.Saat,
                                Tarih = seanssonuc.Tarih,
                                Kontenjan = seanssonuc.Kontenjan,
                                Durum = seanssonuc.Durum
                                
                            });
                            MesajBaslik = "İşlem İptal Edildi";
                            Mesaj = "Bu Deneme Sınavı İçin Boş Kontenjan Kalmamıştır";
                            MesajIcon = "warning";

                        }

                    }

                    MesajBaslik = "İşlem Başarılı";
                    JsonConvert.SerializeObject(MesajBaslik);
                    MesajIcon = "success";
                    JsonConvert.SerializeObject(MesajIcon);
                    wp1 = "https://wa.me/9" + ogrSet.Telefon + "?text=Sn.%20" + ogrSet.Ad.ToUpper() + " " + ogrSet.Soyad.ToUpper() + "%0A";
                    wp2 = "%0ASeansları%20İçin%20Kaydınız%20Alınmıştır";
                    string ogrTelefon = "+9" + ogrSet.Telefon;
                  
                    var basarilidenemeler = JsonConvert.SerializeObject(basariliseanslar);
                    string basariliseansstr = "";
                    basariliseansstr += ogrSet.Ad + " - " + ogrSet.Soyad + "\n";
                    foreach (var item in basariliseanslar)
                    {
                        basariliseansstr += item.kitapcikBaslik + " - " + item.TarihSTR + " - " + item.Saat + "\n";
                    }
                   
                    string wpCustomMsg = "";
                    wpCustomMsg += "Sn. " + ogrSet.Ad.ToUpper() + " " + ogrSet.Soyad.ToUpper();
                    

                    var basarisizdenemeler = JsonConvert.SerializeObject(basarisizseanslar);

                    toplamTutar = Convert.ToString(basariliseanslar.Sum(t => t.SeansUcret));
                    return Json(new
                    {
                        MesajBaslik,
                        MesajIcon,
                        wp1,
                        wp2,
                        basarilidenemeler,
                        basarisizdenemeler,
                        toplamTutar,
                        ogrTelefon,
                        wpCustomMsg,
                        nwguidtext
                    });
                }
                else
                {
                    MesajBaslik = "İşlem Başarısız";
                    Mesaj = "Lütfen Seans Seçimi Yapınız";
                    MesajIcon = "error";
                    return Json(new { Mesaj, MesajBaslik, MesajIcon });

                }
            }
        }
        catch (Exception ex)
        {

            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.Message;
            MesajIcon = "error";
        }
        return Json(new { Mesaj, MesajBaslik, MesajIcon });

    }

    [HttpPost]
    public JsonResult odemeupdateafterogrSeansKayitMulti(int id, string odeme)
    {
        try
        {
            using(var context = new U1626744Db60AContext())
            {
                List<SeansOgrSet> seanskayit = context.SeansOgrSets.Where(x => x.OgrId == id).ToList();
                seanskayit.ForEach(f => f.Aciklama = odeme);
                context.UpdateRange(seanskayit);
                context.SaveChanges();
                return Json(true);
            }
        }
        catch (Exception ex)
        {
            return Json(false);
        }
    }
}
}