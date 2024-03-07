using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using uyanikv3.AppFilter;
using uyanikv3.customModels;
using uyanikv3.Models;
using Denemeler = uyanikv3.Models.Denemeler;

namespace uyanikv3.Controllers
{
    [AppFilterController]
    public class HomeController : Controller
    {
        // dotnet ef dbcontext scaffold "Server=94.73.145.160;Database=u1626744_db60A;User=u1626744_user60A;Password=x@9Qe1-x3H2_Q-nW;" Pomelo.EntityFrameworkCore.MySql -o Models -f
        
        public int kutuphaneid { get; set; }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Seans()
        {
            return View();
        }
        public IActionResult Ogrenci()
        {
            return View();
        }
        public IActionResult Kitapciklar()
        {
            return View();
        }
        public IActionResult Yayinlar()
        {
            return View();
        }

        public IActionResult Kategoriler()
        {
            return View();
        }

        public IActionResult Rapor()
        {
            return View();
        }
        public IActionResult KutuphaneUyelik()
        {
            return View();
        }

        [HttpPost]
        public JsonResult franchiseList()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    if (HttpContext.Session.GetInt32("subsubeuser") == 1)
                    {
                        var franchiseList = context.Kurumkutuphanelers
                            .Where(x => x.Id == (int)HttpContext.Session.GetInt32("kutuphaneID"))
                            .ToList();      
                        return Json(JsonConvert.SerializeObject(franchiseList));

                    }
                    else
                    {
                        var franchiseList = context.Kurumkutuphanelers
                            .Where(x => x.MerkezId == (int)HttpContext.Session.GetInt32("subeid")).OrderBy(o => o.Id)
                            .ToList();
                        return Json(JsonConvert.SerializeObject(franchiseList));

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
        public IActionResult FranchiseChange(int kutuphaneid)
        {
            try
            {
                HttpContext.Session.SetInt32("kutuphaneID", kutuphaneid);
                HttpContext.Session.SetString("kutuphaneIDSTR", kutuphaneid.ToString());
                using (var context = new U1626744Db60AContext())
                {
                    var kutuphane = context.Kurumkutuphanelers.Where(x => x.Id == kutuphaneid).First();
                    HttpContext.Session.SetString("SubeAd",kutuphane.KutuphaneBaslik ?? "");
                    HttpContext.Session.SetString("subeil",kutuphane.Il ?? "");
                    HttpContext.Session.SetString("subeilce",kutuphane.Ilce ?? "");
                    HttpContext.Session.SetString("subeadres",kutuphane.Adres ?? "");
                    HttpContext.Session.SetString("subebank",kutuphane.Banka ?? "");
                    HttpContext.Session.SetString("subebankadsoyad",kutuphane.Adsoyad ?? "");
                    HttpContext.Session.SetString("subebankiban",kutuphane.Iban ?? "");
                    HttpContext.Session.SetString("subeonkayitaciklama",kutuphane.Onkayitexplanation ?? "");
                }

                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(ex.ToString());
            }
        }

        [HttpPost]
        public JsonResult OnKayitKontrol()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    int onkayit = context.SeansOgrSets.Where(x =>
                        x.Ogr.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") &&
                        x.SeansKayitTarih.Date >= DateTime.Now.Date.AddDays(-10) && x.SeansKayitTarih.Date <= DateTime.Now.Date && x.Durum == 0).Count();
                    return Json(onkayit);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("~/AppAuth/Index");
        }
        class dashboard {
            public string gunluk_kayit_toplam { get; set; }

            public string gunluk_ciro_toplam { get; set; }

            public string gunluk_iban_toplam { get; set; }

            public string gunluk_nakit_toplam { get; set; }

            public List<viewSeansOgrSetModel> seanslist { get; set; }
            public List<viewDenemelerModel> kalankitapcik { get; set; }
            
            public List<viewSeansModel> gunlukSeansDurumList { get; set; }

            public string gunluk_sinav_toplam_kisi { get; set; }
        }

        [HttpPost]
        public JsonResult sessioncontrol()
        {
            string result = "Session Active";
            return Json(JsonConvert.SerializeObject(result));
            
        }

        public IActionResult Sube()
        {
            return View();
        }
        [HttpPost]
        public JsonResult rapor(viewRaporSorguSonucModel model)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    DateTime SorguStart = DateTime.Parse(model.D1);
                    DateTime SorguFinish = DateTime.Parse(model.D2);
                    int seanskayittoplam = context.SeansOgrSets.Where(x =>
                        x.Seans.Tarih.Date >= SorguStart.Date && x.Seans.Tarih.Date <= SorguFinish.Date).Count();

                    var seanslist = context.DenemeSeanslars
                        .Where(x => x.Tarih.Date >= SorguStart.Date && x.Tarih.Date <= SorguFinish.Date).Select(s =>
                            new viewSeansModel()
                            {
                                Id = s.Id,
                                SeansUcret = s.SeansUcret,
                                TarihSTR = s.Tarih.ToString("dd.MM.yyyy - dddd") + " / " + s.Saat,
                                kitapcikBaslik = s.Deneme.DenemeBaslik,
                                Kontenjan = context.DenemeStoklars.Where(k => k.DenemeId == s.DenemeId && k.StokType == 1).Sum(t => t.Adet),
                                toplamKazanc =
                                    context.SeansOgrSets.Where(q => q.SeansId == s.Id && q.Durum != 0).Count() *
                                    s.SeansUcret,
                                KitapcikAlanToplam = s.SeansOgrSets.Where(r => r.Durum == 4).Count(),
                                KayitliOgrenci =  context.SeansOgrSets.Where(q => q.SeansId == s.Id && q.Durum != 0).Count()
                            }).ToList();
                    var denemelist = context.Denemelers.Where(x => x.DenemeSeanslars.Where(a => a.Tarih.Date >= SorguStart.Date && a.Tarih.Date <= SorguFinish.Date).Count() > 0).Select(s => new Denemeler()
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        Kategori = s.Kategori,
                        Yayin = s.Yayin,
                        DenemeStoklars = s.DenemeStoklars
                    }).ToList();
                    double kayittoplam = seanslist.Sum(t => t.KayitliOgrenci);
                    double toplamkazanc = seanslist.Sum(t => t.toplamKazanc);
                    viewRaporSorguSonucModel sonuc = new viewRaporSorguSonucModel();
                    sonuc.kitapciklist = denemelist;
                    sonuc.toplamciro = toplamkazanc;
                    sonuc.toplamkayit = kayittoplam;
                    sonuc.seansList = seanslist;
                    var jsonresult = JsonConvert.SerializeObject(sonuc, new JsonSerializerSettings() {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

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
        public JsonResult gunluk_dashboard()
        {
            try
            {
                using(var context = new U1626744Db60AContext())
                {
                    List<viewSeansOgrSetModel> seans_kayitlar = context.SeansOgrSets.Where(x => x.Ogr.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") && x.Durum != 0 && x.SeansKayitTarih.Date == DateTime.Now.Date).Select(s => new viewSeansOgrSetModel
                    {
                        Id = s.Id,
                        SeansInfo = s.Seans,
                        Aciklama = s.Aciklama,
                        Durum = s.Durum,
                        Ogr = s.Ogr,
                        SeansKayitTarih = s.SeansKayitTarih,
                        OgrId = s.OgrId,
                        SeansId = s.SeansId,
                    }).OrderBy(o => o.SeansInfo.Deneme.DenemeBaslik).ToList();
                    var seansbuguntoplam = context.DenemeSeanslars.Where(x =>
                        x.Deneme.Kategori.Kat.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") &&
                        x.SeansOgrSets.Where(x => x.SeansKayitTarih.Date == DateTime.Now.Date && x.Durum != 0).Count() > 0).Select(s =>
                        new viewSeansModel()
                        {
                            Id = s.Id,
                            TarihSTR = s.Tarih.ToString("dd.MM.yyyy"),
                            Saat = s.Saat,
                            kitapcikBaslik = s.Deneme.DenemeBaslik,
                            yayinBaslik = s.Deneme.Yayin.YayinBaslik,
                            KategoriBaslik = s.Deneme.Kategori.AltKategoriBaslik,
                            KayitliOgrenci = s.SeansOgrSets.Where(x => x.SeansKayitTarih.Date == DateTime.Now.Date && x.Durum != 0)
                                .Count()
                        }).ToList();
                    var gunluk_kayit_toplam = seans_kayitlar.Count() + " ADET";
                    var gunluk_ciro_toplam = seans_kayitlar.Sum(t => t.SeansInfo.SeansUcret) + " TL";
                    var gunluk_iban_toplam = seans_kayitlar.Where(x => x.Aciklama != null && x.Aciklama.Contains("IBAN")).Sum(t => t.SeansInfo.SeansUcret) + " TL" ?? " 0 TL";
                    var gunluk_nakit_toplam = seans_kayitlar.Where(x => x.Aciklama != null && x.Aciklama.Contains("NAKIT")).Sum(t => t.SeansInfo.SeansUcret) + " TL" ?? " 0 TL";
                    
                    List<viewSeansOgrSetModel> bugunku_seanslar = context.SeansOgrSets.Where(x => x.Ogr.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") && x.Durum != 0 && x.Seans.Tarih.Date == DateTime.Now.Date).Select(s => new viewSeansOgrSetModel
                    {
                        Id = s.Id,
                        Aciklama = s.Aciklama,
                        kategoriBaslik = s.Seans.Deneme.Kategori.AltKategoriBaslik,
                        kayitUcret = s.Seans.SeansUcret,
                        Durum = s.Durum,
                        denemeAd = s.Seans.Deneme.DenemeBaslik,
                        Ogr = s.Ogr,
                        SeansId = s.SeansId,
                        yayinAd = s.Seans.Deneme.Yayin.YayinBaslik,
                        SeansKayitTarihSTR = s.SeansKayitTarih.ToString("dd.MM.yyyy - dddd")
                    }).OrderBy(o => o.yayinAd).ToList();
                  
                    dashboard dsh = new dashboard();
                    dsh.gunluk_kayit_toplam = gunluk_kayit_toplam;
                    dsh.gunluk_ciro_toplam = gunluk_ciro_toplam;
                    dsh.gunluk_nakit_toplam = gunluk_nakit_toplam;
                    dsh.gunluk_iban_toplam = gunluk_iban_toplam;
                    dsh.kalankitapcik =  context.Denemelers.Where(x => x.DenemeSeanslars.Where(y =>
                        y.Tarih.Date >= DateTime.Now.Date && x.Kategori.Kat.KutuphaneId ==
                        (int)HttpContext.Session.GetInt32("kutuphaneID") && y.DenemeId == x.Id).Count() > 0).Select(s =>
                        new viewDenemelerModel()
                        {
                            Id = s.Id,
                            YayinBaslik = s.Yayin.YayinBaslik,
                            KategoriBaslik = s.Kategori.AltKategoriBaslik,
                            DenemeBaslik = s.DenemeBaslik,
                            doluStok = context.SeansOgrSets.Where(c => c.Seans.DenemeId == s.Id && c.Durum != 0).Count(),
                            toplamStok =
                                s.DenemeStoklars.Where(t => t.StokType == 1 && t.DenemeId == s.Id).Sum(p => p.Adet) -
                                s.DenemeStoklars.Where(t => t.StokType == 2 && t.DenemeId == s.Id).Sum(p => p.Adet),
                            kalanStok =
                                s.DenemeStoklars.Where(t => t.StokType == 1 && t.DenemeId == s.Id).Sum(p => p.Adet) -
                                s.DenemeStoklars.Where(t => t.StokType == 2 && t.DenemeId == s.Id).Sum(p => p.Adet) -
                                context.SeansOgrSets.Where(c => c.Seans.DenemeId == s.Id && c.Durum != 0).Count()
                        }).OrderByDescending(o => o.doluStok).ToList();   

                   
                       
                    dsh.seanslist = bugunku_seanslar.OrderBy(o => o.Ogr.Ad.Trim()).ToList();
                    dsh.gunluk_sinav_toplam_kisi = bugunku_seanslar.Count().ToString() + " Kişi";
                    dsh.gunlukSeansDurumList = seansbuguntoplam;
                    JsonConvert.SerializeObject(dsh);

                    return Json(dsh);
                }
            }
            catch (Exception ex)
            {
                return Json("Hata Oluştu");
            }
        }

    }
}