using Microsoft.AspNetCore.Mvc;
using uyanikv3.Models;
using uyanikv3.customModels;
using Newtonsoft.Json;
using uyanikv3.AppFilter;

namespace uyanikv3.Controllers
{
    [AppFilterController]

    public class KategoriActionController : Controller
    {
        public int kutuphaneid { get; set; }
        public string MesajBaslik { get; set; }
        public string Mesaj { get; set; }
        public string MesajIcon { get; set; }
                [HttpPost]
        public JsonResult kategoriAllsubeSet()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var kategoriler = context.Anakategorilers
                        .Where(x => x.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s =>
                            new Anakategoriler()
                            {
                                AnaKategoriBaslik = s.AnaKategoriBaslik,
                                Kategorilers = s.Kategorilers.Select(u => new Kategoriler()
                                {
                                    AltKategoriBaslik = u.AltKategoriBaslik,
                                    KatId = u.KatId,
                                    Aciklama = u.Aciklama
                                }).ToList(),
                                KutuphaneId = s.KutuphaneId
                            }).ToList();
                    var franchiseList = context.Kurumkutuphanelers.Where
                    (x => x.MerkezId == (int)HttpContext.Session.GetInt32("subeid")
                          && 
                          x.Id != (int)HttpContext.Session.GetInt32("kutuphaneID")).ToList();
                    foreach (var kutuphane in franchiseList)
                    {
                        foreach (var kat in kategoriler)
                        {
                            var control = context.Anakategorilers.Where(x => x.AnaKategoriBaslik == kat.AnaKategoriBaslik).Count();
                            if (control is 0)
                            {
                                var newkat = new Anakategoriler
                                {   
                                    AnaKategoriBaslik = kat.AnaKategoriBaslik,
                                    KutuphaneId = kutuphane.Id,
                                    Kategorilers = kat.Kategorilers
                                };
                                context.Anakategorilers.Add(newkat);
                                context.SaveChanges();
                            }
                        }
                    }
                }

                return Json("İşlem Başarılı");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(ex.ToString());
            }
        }
        public JsonResult KategoriList()
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
            using (var context = new U1626744Db60AContext())
            {
                List<viewAnaKategorilerModel> kategori = context.Anakategorilers.Where(x => x.KutuphaneId == kutuphaneid).Select(s => new viewAnaKategorilerModel
                {
                    Id = s.Id,
                    AnaKategoriBaslik = s.AnaKategoriBaslik,
                    Kategorilers = s.Kategorilers.Select(c => new viewAltKategorilerModel()
                    {
                        Id = c.Id,
                        AltKategoriBaslik = c.AltKategoriBaslik,
                        toplamKitapcik = c.Denemelers.Count(),
                        toplamSeans = c.Denemelers.Select(d => d.DenemeSeanslars.Count()).First()
                    }).OrderBy(o => o.AltKategoriBaslik).ToList()
                }).OrderBy(o => o.AnaKategoriBaslik).ToList();
                var jsonresult = JsonConvert.SerializeObject(kategori);
                return Json(jsonresult);
            }
        }

        [HttpPost]
        public JsonResult WhatsappLinkList(int katid)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    List<Wplink> Link = context.Wplinks.Where(x => x.Kat.KutuphaneId == kutuphaneid && x.KatId == katid)
                        .Select(s => new Wplink()
                        {
                            Id = s.Id,
                            LinkBaslik = s.LinkBaslik,
                            Link = s.Link
                        }).OrderBy(o => o.LinkBaslik).ToList();
                    return Json(JsonConvert.SerializeObject(Link));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.SerializeObject("Hata :" + ex.ToString() + " <br> Hata Mesajı: " + ex.Message));
            }
        }

        [HttpPost]
        public JsonResult wplinkadd(Wplink model)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    Wplink wp = new Wplink();
                    wp.KatId = model.KatId;
                    wp.LinkBaslik = model.LinkBaslik;
                    wp.Link = model.Link;
                    context.Wplinks.Add(wp);
                    context.SaveChanges();
                    MesajBaslik = "İşlem Başarılı";
                    MesajBaslik = JsonConvert.SerializeObject(MesajBaslik);
                    Mesaj = "Whatsapp Linki İlgili Kategoriye Tanımlandı";
                    Mesaj = JsonConvert.SerializeObject(Mesaj);
                    MesajIcon = "success";
                    MesajIcon = JsonConvert.SerializeObject(MesajIcon);
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.SerializeObject("Hata :" + ex.ToString() + " <br> Hata Mesajı: " + ex.Message));
            }
        }

        [HttpPost]
        public JsonResult wplinkremove(int id)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var wplink = context.Wplinks.Where(x => x.Id == id).First();
                    context.Wplinks.Remove(wplink);
                    context.SaveChanges();
                    MesajBaslik = "İşlem Başarılı";
                    MesajBaslik = JsonConvert.SerializeObject(MesajBaslik);
                    Mesaj = "Whatsapp Linki Silindi";
                    Mesaj = JsonConvert.SerializeObject(Mesaj);
                    MesajIcon = "success";
                    MesajIcon = JsonConvert.SerializeObject(MesajIcon);
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.SerializeObject("Hata :" + ex.ToString() + " <br> Hata Mesajı: " + ex.Message));
            }
        }
        [HttpPost]
        public JsonResult AltKategoriList(int? type, int katid)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            using (var context = new U1626744Db60AContext())
            {
               
                var jsonresult = "";
                List<viewAltKategorilerModel> kategori = context.Kategorilers.Where(x => x.Kat.KutuphaneId == kutuphaneid && x.Kat.Id == katid).Select(s => new viewAltKategorilerModel
                {
                    Id = s.Id,
                    AltKategoriBaslik = s.AltKategoriBaslik,
                    toplamSeans = context.Denemelers.Where(a => a.KategoriId == s.Id).Select(s => s.DenemeSeanslars.Count()).First(),
                    KatId = s.KatId,
                    
                }).OrderBy(o => o.AltKategoriBaslik).ToList();
              
                jsonresult = JsonConvert.SerializeObject(kategori);
                return Json(jsonresult);
            }
        }
        [HttpPost]
        public JsonResult AnaKategoriOP(viewAnaKategorilerModel model, int? type)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            try
            {
                using ( var context = new U1626744Db60AContext())
                {
                    var KategoriKontrol = context.Anakategorilers.Where(x => x.KutuphaneId == kutuphaneid && x.AnaKategoriBaslik == model.AnaKategoriBaslik).Select(s => new Anakategoriler
                    {
                        Id = s.Id,
                        AnaKategoriBaslik = s.AnaKategoriBaslik,
                        Kategorilers = s.Kategorilers
                    }).FirstOrDefault();
                    if (type == 1 && KategoriKontrol == null)
                    {
                        var YeniKategori = new Anakategoriler();
                        YeniKategori.AnaKategoriBaslik = model.AnaKategoriBaslik;
                        YeniKategori.KutuphaneId = kutuphaneid;
                        context.Anakategorilers.Add(YeniKategori);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Kategori Eklendi";
                        MesajIcon = "success";
                    }
                    else if (type == 2)
                    {
                        KategoriKontrol.AnaKategoriBaslik = model.AnaKategoriBaslik;
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Kategori Güncellendi";
                        MesajIcon = "success";
                    }
                    else if (type == 3)
                    {
                        if (KategoriKontrol.Kategorilers.Count() == 0)
                        {
                            context.Anakategorilers.Remove(KategoriKontrol);
                            context.SaveChanges();
                            MesajBaslik = "İşlem Başarılı";
                            Mesaj = "Kategori SİLİNDİ";
                            MesajIcon = "info";
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MesajIcon = "error";
                MesajBaslik = "Hata Oluştu";
                Mesaj = ex.Message;
            }
            return Json(new { Mesaj, MesajIcon, MesajBaslik });
        }
        public JsonResult AltKategoriOp(viewAltKategorilerModel model, int? type)
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var AltKategoriKontrol = context.Kategorilers.Where(x => x.Kat.KutuphaneId == kutuphaneid && x.AltKategoriBaslik == model.AltKategoriBaslik).Select(s => new Kategoriler
                    {
                        Id = s.Id,
                        AltKategoriBaslik = s.AltKategoriBaslik,
                        Denemelers = s.Denemelers
                    }).FirstOrDefault();
                    if (type == 1 && AltKategoriKontrol == null)
                    {
                        var YeniAltKategori = new Kategoriler();
                        YeniAltKategori.AltKategoriBaslik = model.AltKategoriBaslik;
                        YeniAltKategori.KatId = model.KatId;
                        context.Kategorilers.Add(YeniAltKategori);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Alt Kategori Eklendi";
                        MesajIcon = "success";
                    }
                    else if (type == 2)
                    {
                        AltKategoriKontrol.AltKategoriBaslik = model.AltKategoriBaslik;
                        AltKategoriKontrol.KatId = model.KatId;
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Alt Kategori Güncellendi";
                        MesajIcon = "success";
                    }
                    else if (type == 3)
                    {
                        if (AltKategoriKontrol.Denemelers.Count() == 0)
                        {
                            context.Kategorilers.Remove(AltKategoriKontrol);
                            context.SaveChanges();
                            MesajBaslik = "İşlem Başarılı";
                            Mesaj = "Alt Kategori SİLİNDİ";
                            MesajIcon = "info";
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MesajIcon = "error";
                MesajBaslik = "Hata Oluştu";
                Mesaj = ex.Message;
            }
            return Json(new { Mesaj, MesajIcon, MesajBaslik });
        }
    }
}
