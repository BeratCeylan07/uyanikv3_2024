using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tweetinvi.Security;
using uyanikv3.AppFilter;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers
{

public class OgrenciActionController : Controller
{
public int kutuphaneid { get; set; }
        public int auth { get; set; }
public string MesajBaslik { get; set; }
public string Mesaj { get; set; }
public string MesajIcon { get; set; }

[HttpPost]
public JsonResult ogrKatSet(int ogrid)
{
    kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
    using (var context = new U1626744Db60AContext())
    {
        try
        {
            int ogrkategori = context.Ogrencilers.Where(x => x.Id == ogrid).Select(s => s.KategoriId).First();

            var yayinlist = context.Yayinlars
                .Where(x => x.Denemelers.Where(x => x.Kategori.KatId == ogrkategori).Count() > 0).Select(s =>
                    new viewYayinlarModel()
                    {
                        Id = s.Id,
                        YayinBaslik = s.YayinBaslik,
                        seanslist = context.DenemeSeanslars.Where(a => a.Deneme.YayinId == s.Id && a.Tarih.Date >= DateTime.Now.Date).ToList()
                    }).OrderBy(o => o.YayinBaslik).ToList();
            yayinlist = yayinlist.Where(x => x.seanslist.Count() > 0).ToList();
            return Json(JsonConvert.SerializeObject(yayinlist));

        }
        catch (Exception ex)
        {
            return Json("Hata");
        }
    }
}

[HttpPost]
public JsonResult OgrenciOP(viewOgrencilerModel model, int? type)
{
    using (var context = new U1626744Db60AContext())
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            var ogrencikontrol = context.Ogrencilers.Where(x => x.Id == model.Id || x.Telefon == model.Telefon).Select(s => new Ogrenciler
            {
                Id = s.Id,
                Ad = s.Ad,
                Soyad = s.Soyad,
                Telefon = s.Telefon,
                Durum = s.Durum,
                KategoriId = s.KategoriId,
                OkulId = s.OkulId,
            }).FirstOrDefault();
            var ogrencikontrolup = context.Ogrencilers.Where(x => x.Id == model.Id || x.Telefon == model.Telefon).Select(s => new Ogrenciler
            {
                Id = s.Id,
                Ad = s.Ad,
                Soyad = s.Soyad,
                Telefon = s.Telefon,
                Durum = s.Durum,
                KategoriId = s.KategoriId,
                KutuphaneId = s.KutuphaneId,
                Sifre = s.Sifre,
                OkulId = s.OkulId,
            }).FirstOrDefault();
            if (type == 1 && ogrencikontrol == null)
            {
                var ogrenci = new Ogrenciler();
                ogrenci.Ad = model.Ad;
                ogrenci.Soyad = model.Soyad;
                ogrenci.Telefon = model.Telefon;
                ogrenci.Qrasc = passwordHash(qrBinary(model.Ad.ToUpper() + model.Soyad.ToUpper() + model.Telefon));
                string ps = RandomNumber(5).ToString();
                ogrenci.Sifre = passwordHash(ps);
                ogrenci.KategoriId = model.KategoriId;
                ogrenci.OkulId = 1;
                ogrenci.Durum = model.Durum;
                ogrenci.KutuphaneId = kutuphaneid;
                ogrenci.KutuphaneUye = model.KutuphaneUye;
                ogrenci.Ktarih = DateTime.Now.Date;
                context.Ogrencilers.Add(ogrenci);
                string wpmesaj = "Uyanık Kütüphane Ailesine Hoş Geldiniz.%0ASisteme erişim İçin telefon numaranız ile birlikte kullanabileceğiniz şifreniz: " + ps + "%0A";
                string ogrTelefon = "+9" + model.Telefon.ToString().Replace(" ", "");
                var wpLink = context.Wplinks.Where(x => x.KatId == model.KategoriId).Select(s => new Wplink
                {
                    Id = s.Id,
                    Link = s.Link,
                    LinkBaslik = s.LinkBaslik
                }).ToList();
                foreach (var item in wpLink)
                {
                    wpmesaj += "Whatsapp Grubu Katılım Bağlantıları:%0A" + item.LinkBaslik + " " + item.Link + "%0A";
                }
                MesajBaslik = "İşlem Başarılı";
                Mesaj = "<div class=\"container-fluid\">\r\n<div class=\"row\"><div class=\"col-md-12\"><b>Öğrenci Kaydı Eklendi</b></div><div class=\"col-md-12\"></div></div><div class=\"row\"><div class=\"col-md-12\"><a href=\"whatsapp://send?text="+wpmesaj+"&phone="+ogrTelefon+" target=\"_blank\" class=\"btn btn-success btn-block\" style=\"height: auto; margin-top:10px;\"><i class=\"fab fa-whatsapp fa-2x\"></i><br>Mesaj Gönder</a></div></div></div>";
                MesajIcon = "success";
                var _mesaj = JsonConvert.SerializeObject(Mesaj);
                var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);
                var _mesajicon = JsonConvert.SerializeObject(MesajIcon);
                context.SaveChanges();
                int ogrid = context.Ogrencilers.Where(x => x.Ad == model.Ad && x.Soyad == model.Soyad && x.Telefon == model.Telefon).Select(s => s.Id).First();
                return Json(new { _mesaj, _mesajbaslik, _mesajicon, ogrid });
            }
            else if (type == 2 && ogrencikontrolup != null)
            {
                ogrencikontrolup.Ad = model.Ad.ToUpper();
                ogrencikontrolup.Soyad = model.Soyad.ToUpper();
                ogrencikontrolup.Telefon = model.Telefon;
                ogrencikontrolup.KategoriId = model.KategoriId;
                ogrencikontrolup.OkulId = 1;
                MesajBaslik = "İşlem Başarılı";
                Mesaj = "Öğrenci Kaydı Güncellendi";
                MesajIcon = "success";
                context.Entry(ogrencikontrolup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                int ogrid = model.Id;
                var _mesaj = JsonConvert.SerializeObject(Mesaj);
                var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);
                var _mesajicon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { _mesaj, _mesajbaslik, _mesajicon,ogrid });
                
            }
            else if (type == 3)
            {
                context.SeansOgrSets.RemoveRange(context.SeansOgrSets.Where(x => x.OgrId == ogrencikontrol.Id).ToList());
                context.SaveChanges();
                var uyeikramhareketler = context.Uyegirisikramharekets.Where(x => x.UyeGiris.UyeId == model.Id).ToList();
                context.Uyegirisikramharekets.RemoveRange(uyeikramhareketler);
                context.SaveChanges();
                var uyegirishareketler = context.Uyegirishareketlers.Where(x => x.UyeId == model.Id).ToList();
                context.Uyegirishareketlers.RemoveRange(uyegirishareketler);
                context.SaveChanges();
                var uyepakettanimlamalar = context.Kutuphaneuyelikpakettanimlamalars.Where(x => x.OgrId == model.Id).ToList();
                context.Kutuphaneuyelikpakettanimlamalars.RemoveRange(uyepakettanimlamalar);
                context.SaveChanges();
                var bildirimlog = context.Ogrbildirimlogs.Where(a => a.OgrId == model.Id).ToList();
                context.Ogrbildirimlogs.RemoveRange(bildirimlog);
                context.SaveChanges();
                var ogrodemeler = context.OgrenciOdemelers.Where(x => x.OgrId == model.Id).ToList();
                context.OgrenciOdemelers.RemoveRange(ogrodemeler);
                context.SaveChanges();
                var ogrtokens = context.Ogrtokens.Where(x => x.OgrId == model.Id).ToList();
                context.Ogrtokens.RemoveRange(ogrtokens);
                context.SaveChanges();
                var ogrencisil = context.Ogrencilers.Where(x => x.Id == model.Id).First();
                context.Ogrencilers.RemoveRange(ogrencisil);
                context.SaveChanges();
                MesajBaslik = "İşlem Başarılı";
                Mesaj = ogrencikontrol.Ad + " " + ogrencikontrol.Soyad + " İsimli Öğrenci & Tüm Verileri Kalıcı Olarka Silindi";
                MesajIcon = "info";
                var _mesaj = JsonConvert.SerializeObject(Mesaj);
                var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);
                var _mesajicon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { _mesaj, _mesajbaslik, _mesajicon });
            }
            else if(ogrencikontrol != null)
            {
                MesajBaslik = "İşlem Başarısız";
                Mesaj = ogrencikontrol.Ad + " " + ogrencikontrol.Soyad + " İsimli Öğrenci Zaten Kayıtlıdır";
                MesajIcon = "info";
                var _mesaj = JsonConvert.SerializeObject(Mesaj);
                var _mesajbaslik = JsonConvert.SerializeObject(MesajBaslik);
                var _mesajicon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { _mesaj, _mesajbaslik, _mesajicon });

            }
            return null;
        }
        catch (Exception ex)
        {
                var _mesajbaslik = "Hata Oluştu";
                var _mesaj = JsonConvert.SerializeObject(ex.ToString() + " " + "(" + ex.Message + ")" );
                var _mesajicon = JsonConvert.SerializeObject(MesajIcon);
                return Json(new { _mesaj, _mesajbaslik, _mesajicon });
        }

    }
}

[HttpPost]
public JsonResult ogrseanshatirlat(int ogrid, int? seanssetid)
{
    using (var context = new U1626744Db60AContext())
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        try
        {
            var ogrSet = context.Ogrencilers.Where(x => x.Id == ogrid).Select(s => new Ogrenciler()
            {
                Id = s.Id,
                Telefon = s.Telefon,
                Ad = s.Ad,
                Soyad = s.Soyad
            }).FirstOrDefault();
            List<viewSeansModel> seanslist;
            if (seanssetid is null)
            {
                seanslist  = context.SeansOgrSets.Where(x => x.OgrId == ogrid && x.Seans.Tarih.Date >= DateTime.Now.Date).Select(y => new viewSeansModel()
                {
                    Id = y.Id,
                    kitapcikBaslik = y.Seans.Deneme.DenemeBaslik,
                    yayinBaslik = y.Seans.Deneme.Yayin.YayinBaslik,
                    KategoriBaslik = y.Seans.Deneme.Kategori.AltKategoriBaslik,
                    TarihSTR = y.Seans.Tarih.ToString("dd.MM.yyyy - ddddd"),
                    Saat = y.Seans.Saat,
                    Tarih = y.Seans.Tarih,
                    Deneme = y.Seans.Deneme,
                    qrCodeforOgr = y.Qr,
                    ogrSeansKayitTarih = y.SeansKayitTarih.ToString("dd.MM.yyyy - ddddd"),
                    SeansUcret = y.Seans.SeansUcret
                }).OrderBy(o => o.Tarih).ToList();
            }
            else
            {
                seanslist  = context.SeansOgrSets.Where(x => x.OgrId == ogrid && x.Id == seanssetid).Select(y => new viewSeansModel()
                {
                    Id = y.Id,
                    kitapcikBaslik = y.Seans.Deneme.DenemeBaslik,
                    yayinBaslik = y.Seans.Deneme.Yayin.YayinBaslik,
                    KategoriBaslik = y.Seans.Deneme.Kategori.AltKategoriBaslik,
                    TarihSTR = y.Seans.Tarih.ToString("dd.MM.yyyy - ddddd"),
                    Saat = y.Seans.Saat,
                    Tarih = y.Seans.Tarih,
                    Deneme = y.Seans.Deneme,
                    qrCodeforOgr = y.Qr,
                    ogrSeansKayitTarih = y.SeansKayitTarih.ToString("dd.MM.yyyy - ddddd"),
                    SeansUcret = y.Seans.SeansUcret
                }).OrderBy(o => o.Tarih).ToList();
            }
            MesajBaslik = "İşlem Başarılı";
            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            MesajIcon = "success";
            string icon = JsonConvert.SerializeObject(MesajIcon);
            string ogrTelefon = "+90" + ogrSet.Telefon;
            string wpCustomMsg = "Sn. " + ogrSet.Ad.ToUpper() + " " + ogrSet.Soyad.ToUpper();
            var jsonresult = JsonConvert.SerializeObject(seanslist);
            return Json(new
            {
                icon, baslik, jsonresult, ogrTelefon,
                wpCustomMsg
            });
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            Mesaj = ex.ToString() + "(" + ex.Message + ")";
            string mesaj = JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            string icon = JsonConvert.SerializeObject(MesajIcon);
            return Json(new { mesaj, baslik, icon });
        }
    }
}

[HttpPost]
public JsonResult ogrlistforseans(int ogrid, int seansid)
{
    try
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        using (var context = new U1626744Db60AContext())
        {
            List<int> kitapcikid = context.DenemeSeanslars.Where(x => x.Id == seansid).Select(s => s.DenemeId).ToList();
            List<DenemeSeanslar> seanslar = context.DenemeSeanslars
                .Where(x => x.Deneme.Yayin.KutuphaneId == kutuphaneid && x.Tarih.Date >= DateTime.Now.Date &&
                            x.Durum != 4).Select(s => new DenemeSeanslar()
                {
                    Id = s.Id,
                    Deneme = s.Deneme,
                    SeansUcret = s.SeansUcret,
                    Tarih = s.Tarih,
                    Kontenjan = s.Kontenjan,
                    Saat = s.Saat,
                    DenemeId = s.DenemeId
                }).ToList();
                var kayitolabilecegiseanslar = new List<DenemeSeanslar>();
                for (int i = 0; i <= kitapcikid.Count(); i++)
                {
                    int denemeid = kitapcikid[i];
                    kayitolabilecegiseanslar.AddRange(seanslar.Where(x => x.DenemeId != denemeid).ToList());
                }

                var jsonresult = JsonConvert.SerializeObject(kayitolabilecegiseanslar);
                return Json(jsonresult);
        }
    }
    catch (Exception ex)
    {
        return Json("");
    }
}
[HttpPost]
public JsonResult OgrBilgiGetir(int ogrid)
{
    using (var context = new U1626744Db60AContext())
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            var ogr = context.Ogrencilers.Where(x => x.Id == ogrid).Select(s => new viewOgrencilerModel
            {
                Id = s.Id,
                Ad = s.Ad.ToUpper(),
                Soyad = s.Soyad.ToUpper(),
                Telefon = s.Telefon.ToUpper(),
                KutuphaneUye = s.KutuphaneUye,
                Durum = s.Durum,
                Kategori = s.Kategori,
                KategoriId = s.KategoriId,
                KtarihSTR = s.Ktarih.ToString("dd.MM.yyyy"),
            }).First();
            var ogrseanslist = context.SeansOgrSets.Where(x => x.OgrId == ogrid).Select(s => new viewSeansModel
            {
                Id = s.Id,
                customseansid = s.Seans.Id,
                kitapcikBaslik = s.Seans.Deneme.DenemeBaslik.ToUpper(),
                KategoriBaslik = s.Seans.Deneme.Kategori.AltKategoriBaslik.ToUpper(),
                yayinBaslik = s.Seans.Deneme.Yayin.YayinBaslik.ToUpper(),
                Saat = s.Seans.Saat,
                TarihSTR = s.Seans.Tarih.ToString("dd.MM.yyyy / ddddd"),
                ogrseansDurum = s.Durum,
                Tarih = s.Seans.Tarih,
                Durum = s.Durum,
                ogrSeansKayitTarih = s.SeansKayitTarih.ToString("dd.MM.yyyy / dddd"),
            }).OrderByDescending(o => o.Tarih).ToList();
            int toplamseans = ogrseanslist.Count();
            var ogrbitenseansjsonresult = JsonConvert.SerializeObject(ogrseanslist.Where(x => x.Tarih.Date < DateTime.Now.Date).ToList());
            int toplambitenseans = ogrseanslist.Where(x => x.Tarih < DateTime.Now.Date).Count();
            var ogrguncelseansjsonresult = JsonConvert.SerializeObject(ogrseanslist.Where(x => x.Tarih.Date >= DateTime.Now.Date).ToList());
            int toplamguncelseans = ogrseanslist.Where(x => x.Tarih > DateTime.Now.Date).Count();
            var ogrbilgijsonresult = JsonConvert.SerializeObject(ogr);
            var toplamkitapcik = JsonConvert.SerializeObject(ogrseanslist.Where(x => x.Durum == 4).Count());
                    var alinankitapciklist = JsonConvert.SerializeObject(ogrseanslist.Where(x => x.Durum == 4).ToList());
            return Json(new { toplamkitapcik, alinankitapciklist, toplamseans, toplambitenseans, toplamguncelseans, ogrbitenseansjsonresult, ogrguncelseansjsonresult, ogrbilgijsonresult });
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.ToString() + "(" + ex.Message + ")";
            MesajIcon = "error";
            return Json(new { Mesaj, MesajBaslik, MesajIcon });
        }
    }
}
[HttpPost]
public JsonResult OgrenciList()
{
    using(var context = new U1626744Db60AContext())
    {
    try
        { 
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
        auth = (int)HttpContext.Session.GetInt32("adminAuth");
                
            List<viewOgrencilerModel> ogrlist = context.Ogrencilers.Where(x => x.KutuphaneId == kutuphaneid).Select(s => new viewOgrencilerModel
            {
                Id = s.Id,
                Ad = s.Ad.ToUpper(),
                Soyad = s.Soyad.ToUpper(),
                Telefon = s.Telefon.ToUpper(),
                Kategori = s.Kategori,
                KtarihSTR = s.Ktarih.ToString("dd.MM.yyyy"),
                Durum = s.Durum
            }).OrderBy(o => o.Ad).ToList();
            var jsonresult = JsonConvert.SerializeObject(ogrlist);
            
            return Json(new { jsonresult, auth });
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            Mesaj = ex.Message;
            MesajIcon = "error";
            return Json(new { MesajBaslik, Mesaj, MesajIcon });
        }
    }
}

[HttpPost]
public JsonResult ogrnewpassword(int ogrid)
{
    using (var context = new U1626744Db60AContext())
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        try
        {
            var ogr = context.Ogrencilers.Where(x => x.Id == ogrid).First();
            string newpassword = RandomNumber(5);
            ogr.Sifre = passwordHash(newpassword);
            context.Entry(ogr).State = EntityState.Modified;
            context.SaveChanges();
            MesajBaslik = "İşlem Başarılı";
            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            string wpmsg = "Cep telefonu numaranız ile kullanabileceğiniz şifreniz: " + newpassword;
            Mesaj = "Öğrenci Şifresi Güncellendi. <hr /><a href=\"whatsapp://send?text="+ wpmsg + "&phone=+9"+ogr.Telefon+"\" class=\"btn btn-success\" target=\"_blank\">Whatsapp üzerinden İlet</a>";

            string mesaj = JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "success";
            string icon = JsonConvert.SerializeObject(MesajIcon);
            return Json(new { baslik, mesaj, icon });
        }
        catch (Exception ex)
        {
            MesajBaslik = "Hata Oluştu";
            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            Mesaj = ex.Message;
            string mesaj = JsonConvert.SerializeObject(Mesaj);
            MesajIcon = "error";
            string icon = JsonConvert.SerializeObject(MesajBaslik);
            return Json(new { baslik, mesaj, icon });
        }

    }
}
[HttpPost]
public static string passwordHash(string _password)
{
    SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
    byte[] password_bytes = Encoding.ASCII.GetBytes(_password);
    byte[] encrypted_bytes = SHA1.ComputeHash(password_bytes);
    return Convert.ToBase64String(encrypted_bytes);
}
public string qrBinary(string input)
{
    StringBuilder sb = new StringBuilder();
    foreach (var L in input.ToCharArray())
    {
        sb.Append(Convert.ToString(L, 2).PadLeft(8, '0'));
    }
    return sb.ToString();
}
public JsonResult allStudentsQRSet()
{
    using(var context = new U1626744Db60AContext())
    {
        kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

        List<Ogrenciler> ogrlist = context.Ogrencilers.Where(a => a.KutuphaneId == kutuphaneid).ToList();

        foreach (var x in ogrlist.ToList())
        {
            x.Qrasc = passwordHash(qrBinary(x.Ad.ToUpper() + x.Soyad.ToUpper() + x.Telefon));
        }
        context.SaveChanges();
        return Json("Ok");
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
}
}
