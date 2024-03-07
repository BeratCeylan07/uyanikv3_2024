using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers;

public class OnKayitController : Controller
{
    public string msgTitle { get; set; }
    public string msg { get; set; }
    public string msgIcon { get; set; }

    public IActionResult OgrLogin()
    {
        HttpContext.Session.Clear();
        return View();
    }

    public JsonResult SubeList()
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                List<Kurumkutuphaneler> subeler = context.Kurumkutuphanelers.ToList();
                return Json(subeler);
            }
        }
        catch (Exception ex)
        {
            return Json(ex.ToString());
        }
    } 
    [HttpPost]
    public JsonResult ogrSeansGetir(viewOgrencilerModel model)
        {
            using (var db = new U1626744Db60AContext())
            {
                try
                {
                    string ad = HttpContext.Session.GetString("OgrAd");
                    string soyad = HttpContext.Session.GetString("OgrSoyad");
                    string tel = HttpContext.Session.GetString("OgrTel");
                    int ogrid = db.Ogrencilers.Where(x =>
                        x.Ad == ad && x.Soyad == soyad && x.Telefon == tel &&
                        x.KutuphaneId == model.KutuphaneId).Select(s => s.Id).FirstOrDefault();
                    List<viewSeansModel> seansList = db.DenemeSeanslars
                        .Where(a => a.Deneme.Yayin.KutuphaneId == model.KutuphaneId &&
                                    
                                    a.SeansOgrSets.Where(b => b.OgrId == ogrid && b.Durum == 0).Count() > 0).Select(x =>
                            new viewSeansModel
                            {
                                Id = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrid)
                                    .Select(s => s.Id).First(),
                                customseansid = x.Id,
                                TarihSTR = x.Tarih.ToShortDateString(),
                                Saat = x.Saat,
                                Durum = x.Durum,
                                SeansUcret = x.SeansUcret,
                                Kontenjan = x.Kontenjan,
                                GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Where(x => x.Durum != 0).Count(),
                                KayitliOgrenci = x.SeansOgrSets.Count(),
                                SeansGun = x.Tarih.ToString("dddd"),
                                Tarih = x.Tarih,
                                Deneme = x.Deneme,     
                                KategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik,
                                yayinBaslik = x.Deneme.Yayin.YayinBaslik,
                                kitapcikBaslik = x.Deneme.DenemeBaslik,
                                subeinfo = db.Kurumkutuphanelers.Where(q => q.Id == model.KutuphaneId).Select(a => a.KutuphaneBaslik).First(),
                                yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik,
                                yayinLogo = x.Deneme.Yayin.Logo,
                                kitapcikucret = x.SeansUcret,
                                ogrSeansKayitTarih = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrid)
                                    .Select(s => s.SeansKayitTarih.ToString("dd.MM.yyyy - dddd")).First(),
                                ogrseansDurum = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrid)
                                    .Select(s => s.Durum).First()
                            }).OrderByDescending(o => o.Tarih).ToList();
                    var paketlist = db.SeansPakets
                        .Where(x => x.KutuphaneId == model.KutuphaneId && x.SeansPaketTanims.Count() > 0).Select(s => new SeansPaket()
                        {
                            Id = s.Id,
                            PaketAdi = s.PaketAdi,
                            Adet = s.Adet,
                            SeansPaketTanims = s.SeansPaketTanims.Select(r => new SeansPaketTanim()
                            {
                                Id = r.Id,
                                Seansid = r.Seansid,
                                Paketid = r.Paketid
                            }).ToList(),
                            Fiyat = s.Fiyat
                        }).ToList();
                    var ucret = "";
                    int counter = 0;
                    if (paketlist.Count() == 0)
                    {
                        ucret = JsonConvert.SerializeObject(seansList.Sum(t => t.kitapcikucret));
                    }
                    else
                    {
                
                            foreach (var paket in paketlist)
                            {
                                foreach (var pakettanim in paket.SeansPaketTanims.ToList())
                                {
                                    foreach (var ogrseans in seansList.ToList())
                                    {
                                        if (pakettanim.Seansid == ogrseans.customseansid)
                                        {
                                            counter++;
                                        }
                                    }
                                 
                                }
                                if (paket.Adet == counter)
                                {
                                    ucret = JsonConvert.SerializeObject(paket.Fiyat);
                                    break;
                                }
                                else
                                {
                                    ucret = JsonConvert.SerializeObject(seansList.Sum(t => t.kitapcikucret));

                                }
                              
                            }
                        }  
              
                    

                  
                    var jsonSeansList = JsonConvert.SerializeObject(seansList);
                    return Json(new { jsonSeansList, ucret });
                }
                catch (Exception ex)
                {
                    msgTitle = "Uyarı";
                    msgIcon = "error";
                    msg = "Lütfen Şube Seçerek Tekrar Deneyiniz";
                    return Json(new {msgIcon,msgTitle,msg});
                }
            }
        }
    [HttpPost]
    public JsonResult ogrseanskayitsil(int id)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var seans = context.SeansOgrSets.First(x => x.Id == id);
                context.SeansOgrSets.Remove(seans);
                context.SaveChanges();
                msg = "Öğrenci Seans Kaydı Silindi";
                msgTitle = "İşlem Başarılı";
                msgIcon = "info";

            }
        }
        catch (Exception ex)
        {
            msg = "Hata Oluştu: " + ex.Message;
            msgTitle = "İşlem Başarısız";
            msgIcon = "error";
        }
        return Json(new { msg, msgTitle, msgIcon });
    }
        public IActionResult seansList(string Ad, string Soyad, string Telefon, int subeid)
        {
            using (var db = new U1626744Db60AContext())
            {        
                string _ad = HttpContext.Session.GetString("OgrAd");
                string _soyad = HttpContext.Session.GetString("OgrSoyad");
                string _tel = HttpContext.Session.GetString("OgrTel");
                var ogrcontrol = db.Ogrencilers.FirstOrDefault(x =>
                    x.Ad == _ad && x.Soyad == _soyad && x.Telefon == _tel && x.KutuphaneId == subeid);
               List<viewSeansModel> seansList = db.DenemeSeanslars
                    .Where(a => a.Deneme.Yayin.KutuphaneId == subeid && a.Durum != 4 && a.Tarih >= DateTime.Now.Date && a.Kontenjan > a.SeansOgrSets.Where(c => c.Durum != 0).Count()).Select(x =>
                        new viewSeansModel
                        {
                            Id = x.Id,
                            TarihSTR = x.Tarih.ToShortDateString(),
                            Saat = x.Saat,
                            Durum = x.Durum,
                            DenemeId = x.DenemeId,
                            SeansOgrSets = x.SeansOgrSets,
                            SeansUcret = x.SeansUcret,
                            Kontenjan = x.Kontenjan,
                            GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Count(),
                            KayitliOgrenci = x.SeansOgrSets.Count(),
                            SeansGun = x.Tarih.ToString("dddd"),
                            Tarih = x.Tarih,
                            Deneme = x.Deneme,
                            yayinBaslik = x.Deneme.Yayin.YayinBaslik,
                            kitapcikBaslik = x.Deneme.DenemeBaslik,
                            yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik,
                            yayinLogo = x.Deneme.Yayin.Logo
                        }).OrderBy(o => o.Tarih.Date).ToList();

               if (ogrcontrol is not null)
               {
                   var beforeseansofogr = db.DenemeSeanslars
                       .Where(x => x.SeansOgrSets.Where(x => x.OgrId == ogrcontrol.Id).Count() > 0).ToList();
                   foreach (var beforeseans in beforeseansofogr)
                   {
                       seansList = seansList.Where(x => x.DenemeId != beforeseans.DenemeId).ToList();
                   }
               }

               var jsonSeansList = JsonConvert.SerializeObject(seansList);
               
                var subeinfo = db.Kurumkutuphanelers.Where(x => x.Id == subeid).First();
                var iban = subeinfo.Iban;
                var kas = subeinfo.Adsoyad;
                var bank = subeinfo.Banka;
                var onkayitmsg = subeinfo.Onkayitexplanation;
                HttpContext.Session.SetString("kbibn",subeinfo.Iban ?? "");
                HttpContext.Session.SetString("kas",subeinfo.Adsoyad ?? "");
                HttpContext.Session.SetString("kBnk",subeinfo.Banka ?? "");
                HttpContext.Session.SetString("kutuphaneonkayitmsg",subeinfo.Onkayitexplanation ?? "");

                return Json(new { jsonSeansList, iban, kas, bank, onkayitmsg });
            }
        }

    [HttpPost]
    public IActionResult LoginAction(viewOgrencilerModel model)
    {   
        HttpContext.Session.Clear();;
        HttpContext.Session.SetInt32("Oturum",1);
        HttpContext.Session.SetString("OgrAd",model.Ad);
        HttpContext.Session.SetString("OgrSoyad",model.Soyad);
        HttpContext.Session.SetString("OgrTel",model.Telefon);
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("Oturum") is 1)
        {
            return View();
        }
        else
        {
            return RedirectToAction("OgrLogin");
        }    
    }

    public IActionResult OturumKapat()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public JsonResult SeansOnKayit(viewOgrencilerModel model)
    {
        string ad = HttpContext.Session.GetString("OgrAd");
        string soyad = HttpContext.Session.GetString("OgrSoyad");
        string tel = HttpContext.Session.GetString("OgrTel");
        try
        {
                        using (var context = new U1626744Db60AContext())
                        {
                                // 1) Daha Önce kaydı Var mı?
                                var ogrcontrol = context.Ogrencilers.FirstOrDefault(x =>
                                    x.Ad == ad && x.Soyad == soyad && x.Telefon == tel && x.KutuphaneId == model.KutuphaneId);
                                int ogrid = 0;
                                int katid = 0;
                                if (ogrcontrol is null)
                                {
                                    katid = context.DenemeSeanslars.Where(x => x.Id == model.seansid)
                                        .Select(s => s.Deneme.Kategori.KatId).FirstOrDefault();
                                    Ogrenciler ogr = new Ogrenciler
                                    {
                                        KutuphaneId = model.KutuphaneId,
                                        Ad = ad,
                                        Soyad = soyad,
                                        Telefon = tel,
                                        OkulId = 1,
                                        Ktarih = DateTime.Now,
                                        KategoriId = katid,
                                        Durum = 1,
                                        Sifre = "01234"
                                    };
                                    context.Ogrencilers.Add(ogr);
                                    context.SaveChanges();
                                    ogrid = context.Ogrencilers.Where(x =>
                                        x.Ad == ad && x.Soyad == soyad && x.Telefon == tel).Max(m => m.Id);
                                }
                                else
                                {
                                    ogrid = ogrcontrol.Id;
                                    katid = ogrcontrol.KategoriId;
                                }
                                // seans kaydını yap

                                var seansonkayit = new SeansOgrSet();
                                seansonkayit.SeansId = model.seansid;
                                seansonkayit.OgrId = ogrid;
                                seansonkayit.Durum = 0;
                                seansonkayit.SeansKayitTarih = DateTime.Now;
                                seansonkayit.Qr = Guid.NewGuid().ToString();
                                context.SeansOgrSets.Add(seansonkayit);
                                context.SaveChanges();
                                var seansinfo = context.DenemeSeanslars.Where(x => x.Id == model.seansid).Select(s => new DenemeSeanslar()
                                {
                                    Id = s.Id,
                                    Deneme = s.Deneme,
                                    Tarih = s.Tarih,
                                    Saat = s.Saat,
                                    SeansUcret = s.SeansUcret
                                }).FirstOrDefault();
                                var subeinfo = context.Kurumkutuphanelers.FirstOrDefault(x => x.Id == model.KutuphaneId);
                                msgTitle = "İşlem Başarılı";
                                msg = "Seçtiğiniz Seans İçin Ön Kaydınız Alınmıştır <br />  Seans Bilgileri <br /> Şube: " +  subeinfo.KutuphaneBaslik  + " Oturum: " + seansinfo.Deneme.DenemeBaslik + " Tarih: " + seansinfo.Tarih.ToString("dd.MM.yyyy - dddd") + " Saat: " + seansinfo.Saat + " <hr /> <strong>Seans Ücreti: " + seansinfo.SeansUcret + " TL</strong>";
                                msgIcon = "success";
                                return Json(new { msgTitle, msgIcon, msg });
                            }
        }
        catch (Exception ex)
        {
            msgTitle = "Hata";
            msg = ex.Message + " <br />" + ex.ToString();
            msgIcon = "error";
            return Json(new { msgTitle, msgIcon, msg });

        }
    }
}