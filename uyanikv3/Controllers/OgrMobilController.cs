using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;
using uyanikv3.AppFilter;
using uyanikv3.customModels;
using uyanikv3.Models;

namespace uyanikv3.Controllers
{


    public class OgrMobilController : OgrAppFilterController
    {
        public string msgTitle { get; set; }
        public string msg { get; set; }
        public string msgIcon { get; set; }
        public int ogrNO { get; set; }
        public int franchiseID { get; set; }
        
        public int kutuphaneid { get; set; }
        public int auth { get; set; }
        public string MesajBaslik { get; set; }
        public string Mesaj { get; set; }
        public string MesajIcon { get; set; }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult profil()
        {
            return View();
        }
        public IActionResult guncelSeanslar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult onkayitTalep(int seansID)
        {
            using (var db = new U1626744Db60AContext())
            {
                // "2" ÖN KAYIT ANLAMINA GELİR
                try
                {
                    var seancuret = db.DenemeSeanslars.Where(x => x.Id == seansID).Select(s => s.SeansUcret).FirstOrDefault().ToString();
                    SeansOgrSet onKayit = new SeansOgrSet();
                    onKayit.SeansId = seansID;
                    onKayit.OgrId = (int)HttpContext.Session.GetInt32("ogrID");
                    onKayit.Durum = 0;
                    onKayit.Aciklama = "IBAN (ÖN KAYIT)";
                    onKayit.SeansKayitTarih = DateTime.Now.Date;
                    db.SeansOgrSets.Add(onKayit);
                    db.SaveChanges();
                    msgTitle = "İşlem Başarılı";
                    msg = "Seçtiğiniz Seans İçin Ön Kaydınız Alınmıştır <br /> <strong>Seans Ücreti: " + seancuret + " TL</strong> ";
                    msgIcon = "success";
                    return Json(new { msgTitle, msgIcon, msg });
                }
                catch (Exception ex)
                {
                    msgTitle = "İşlem Başarısız";
                    msg = "Lütfen Tekrar Deneyiniz";
                    msgIcon = "error";
                    return Json(new { msgTitle, msgIcon, msg });
                }
            }
        }

        public IActionResult seansList()
        {
            using (var db = new U1626744Db60AContext())
            {
                ogrNO = (int)HttpContext.Session.GetInt32("ogrID");
                int ogrKategoriID = (int)HttpContext.Session.GetInt32("ogrKategoriID");
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                List<viewSeansModel> sntest = db.DenemeSeanslars.Where(x => x.Deneme.Yayin.KutuphaneId == kutuphaneid && x.SeansOgrSets.Where(n => n.OgrId == ogrNO).Count() == 0)
                    .Select(s => new viewSeansModel()
                    {
                        Id = s.Id
                    }).ToList();
                List<viewSeansModel> seansList = db.DenemeSeanslars
                    .Where(a => a.Deneme.Yayin.KutuphaneId == kutuphaneid && a.Durum != 4 && a.Deneme.Kategori.Kat.Id == ogrKategoriID && a.Tarih >= DateTime.Now.Date && a.SeansOgrSets.Where(n => n.OgrId == ogrNO).Count() == 0).Select(x =>
                        new viewSeansModel
                        {
                            Id = x.Id,
                            TarihSTR = x.Tarih.ToShortDateString(),
                            Saat = x.Saat,
                            Durum = x.Durum,
                            SeansUcret = x.SeansUcret,
                            Kontenjan = x.Kontenjan,
                            GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Count(),
                            KayitliOgrenci = x.SeansOgrSets.Count(),
                            SeansGun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)x.Tarih.DayOfWeek],
                            Tarih = x.Tarih,
                            Deneme = x.Deneme,
                            yayinBaslik = x.Deneme.Yayin.YayinBaslik,
                            kitapcikBaslik = x.Deneme.DenemeBaslik,
                            yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik,
                            yayinLogo = x.Deneme.Yayin.Logo
                        }).OrderByDescending(o => o.Tarih).ToList();
                var jsonSeansList = JsonConvert.SerializeObject(seansList);
                return Json(jsonSeansList);
            }
        }

        public IActionResult ogrSeanslar()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ogrseanskayitsil(int id)
        {
            ogrNO = (int)HttpContext.Session.GetInt32("ogrID");

            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var seans = context.SeansOgrSets.Where(x => x.Id == id && x.OgrId == ogrNO).First();
                    context.SeansOgrSets.Remove(seans);
                    context.SaveChanges();
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
        public IActionResult ogrSeansGetir()
        {
            using (var db = new U1626744Db60AContext())
            {
                ogrNO = (int)HttpContext.Session.GetInt32("ogrID");
                int ogrKategoriID = (int)HttpContext.Session.GetInt32("ogrKategoriID");
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                List<viewSeansModel> seansList = db.DenemeSeanslars
                    .Where(a => a.Deneme.Yayin.KutuphaneId == kutuphaneid && a.Deneme.Kategori.KatId == ogrKategoriID && a.Tarih.Date > DateTime.Now.Date &&
                                  a.SeansOgrSets.Where(b => b.OgrId == ogrNO).Count() > 0).Select(x => new viewSeansModel
                    {
                        Id = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrNO)
                            .Select(s => s.Id).First(),
                        TarihSTR = x.Tarih.ToShortDateString(),
                        Saat = x.Saat,
                        Durum = x.Durum,
                        SeansUcret = x.SeansUcret,
                        Kontenjan = x.Kontenjan,
                        GuncelKontenjan = x.Kontenjan - x.SeansOgrSets.Count(),
                        KayitliOgrenci = x.SeansOgrSets.Count(),
                        SeansGun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)x.Tarih.DayOfWeek],
                        Tarih = x.Tarih,
                        Deneme = x.Deneme,
                        yayinBaslik = x.Deneme.Yayin.YayinBaslik,
                        kitapcikBaslik = x.Deneme.DenemeBaslik,
                        yayinKategoriBaslik = x.Deneme.Kategori.AltKategoriBaslik,
                        yayinLogo = x.Deneme.Yayin.Logo,
                        kitapcikucret = x.SeansUcret,
                        ogrSeansKayitTarih = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrNO)
                        .Select(s => s.SeansKayitTarih.ToString("dd.MM.yyyy  dddd")).First(),
                        ogrseansDurum = x.SeansOgrSets.Where(u => u.SeansId == x.Id && u.OgrId == ogrNO)
                            .Select(s => s.Durum).First()
                    }).OrderByDescending(o => o.Tarih).ToList();
                var ucret = JsonConvert.SerializeObject(seansList.Sum(t => t.kitapcikucret));
                var jsonSeansList = JsonConvert.SerializeObject(seansList);
                return Json(new {jsonSeansList, ucret});
            }
        }

        public JsonResult uyeGirisHareketler(int? ogrid)
        {
            using (var db = new U1626744Db60AContext())
            {
                ogrNO = (int)HttpContext.Session.GetInt32("ogrID");
                List<viewUyeGirisHareketlerModel> girisHareketler = db.Uyegirishareketlers.Where(x => x.UyeId == ogrNO)
                    .Select(y => new viewUyeGirisHareketlerModel
                    {
                        Id = y.Id,
                        tarihStr = y.Tarih.ToShortDateString(),
                        Saat = y.Saat,
                        Tarih = y.Tarih,
                        gun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)y.Tarih.DayOfWeek]
                    }).OrderByDescending(o => o.Tarih).ToList();
                var jsonUyegirisList = JsonConvert.SerializeObject(girisHareketler);
                return Json(jsonUyegirisList);
            }
        }

        public JsonResult uyeGirisIkramControl()
        {
            using (var db = new U1626744Db60AContext())
            {
                var ikramListJSON = "";
                franchiseID = (int)HttpContext.Session.GetInt32("franchiseID");
                ogrNO = (int)HttpContext.Session.GetInt32("ogrID");
                Uyegirishareketler girisControl = db.Uyegirishareketlers
                    .Where(x => x.UyeId == ogrNO && x.Tarih.Date == DateTime.Now.Date).Select(s =>
                        new Uyegirishareketler
                        {
                            Id = s.Id,
                            Saat = s.Saat,
                            Tarih = s.Tarih,
                            Uyegirisikramharekets = s.Uyegirisikramharekets,
                            Uye = s.Uye,
                            UyeId = s.UyeId
                        }).FirstOrDefault();
                if (girisControl != null)
                {
                    int uyePaket = db.Kutuphaneuyelikpakettanimlamalars.Where(x => x.OgrId == ogrNO)
                        .Max(m => m.PaketId);

                    int sonGiris = db.Uyegirishareketlers.Where(a => a.UyeId == ogrNO).Max(m => m.Id);

                    List<Uyelikpaketikramtanimlamalar> alinanIkram = db.Uyegirisikramharekets
                        .Where(x => x.UyeGirisId == sonGiris).Select(s => new Uyelikpaketikramtanimlamalar
                        {
                            Id = s.IkramId,
                            IkramBaslik = s.Ikram.IkramBaslik,
                        }).ToList();
                    List<Uyelikpaketikramtanimlamalar> ikramKume = db.Uyelikpaketikramtanimlamalars
                        .Where(x => x.PaketId == uyePaket).Select(s => new Uyelikpaketikramtanimlamalar
                        {
                            Id = s.Id,
                            IkramBaslik = s.IkramBaslik
                        }).ToList();
                    List<Uyelikpaketikramtanimlamalar> sonuc = db.Uyelikpaketikramtanimlamalars
                        .Where(x => x.Id != alinanIkram.Select(s => s.Id).FirstOrDefault()).ToList();
                    ikramListJSON = JsonConvert.SerializeObject(sonuc);
                    return Json(new { ikramListJSON });

                }

                ikramListJSON = JsonConvert.SerializeObject("");

                return Json(new { ikramListJSON });
            }
        }

        public JsonResult uyelikBilgilerim()
        {
            using (var db = new U1626744Db60AContext())
            {
                ogrNO = (int)HttpContext.Session.GetInt32("ogrID");

                int sonPaket = db.Kutuphaneuyelikpakettanimlamalars.Where(x => x.OgrId == ogrNO)
                    .OrderByDescending(o => o.Id).Select(s => s.PaketId).First();
                var paket = db.Kutuphaneuyelikpakettanimlamalars.Where(x => x.OgrId == ogrNO)
                    .OrderByDescending(o => o.Id).Select(s => new viewKutuphaneUyePaketTanimlamalarModel
                    {
                        Id = s.Id,
                        BaslangicTarih = s.BaslangicTarih,
                        BitisTarih = s.BitisTarih,
                        Paket = s.Paket,
                        Ogr = s.Ogr,
                        Tarih = s.Tarih
                    }).First();
                DateTime d1 = paket.BaslangicTarih.Date;
                DateTime d2 = paket.BitisTarih.Date;
                int toplamGiris = db.Uyegirishareketlers
                    .Where(x => x.UyeId == ogrNO && x.Tarih.Date >= d1 && x.Tarih.Date <= d2).Count();
                viewUyelikPaketInfo paketBilgiler = new viewUyelikPaketInfo();
                paketBilgiler.baslangicTarih = paket.BaslangicTarih.ToShortDateString();
                paketBilgiler.bitisTarih = paket.BitisTarih.ToShortDateString();
                paketBilgiler.gecerlilikGun = paket.Paket.GecerlilikToplamGun.ToString() + " Gün";
                paketBilgiler.toplamGirisHak = paket.Paket.ToplamGirisHak.ToString() + " Gün";
                ;
                paketBilgiler.paketAd = paket.Paket.PaketBaslik;
                paketBilgiler.ucret = paket.Paket.Ucret.ToString() + " TL";
                paketBilgiler.kalanGirisHak = Convert.ToString(paket.Paket.ToplamGirisHak - toplamGiris) + " Gün";
                paketBilgiler.tanimliIkramlar = db.Uyelikpaketikramtanimlamalars.Where(a => a.PaketId == sonPaket)
                    .Select(s => new viewUyelikPaketIkramTanimlamalar
                    {
                        Id = s.Id,
                        IkramBaslik = s.IkramBaslik,
                        Adet = s.Adet
                    }).ToList();
                var jsonPaketBilgilerim = JsonConvert.SerializeObject(paketBilgiler);
                return Json(new { jsonPaketBilgilerim });
            }

        }

    }
}