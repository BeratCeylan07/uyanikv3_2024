using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using uyanikv3.AppFilter;
using uyanikv3.customModels;
using uyanikv3.Models;
namespace uyanikv3.Controllers
{
    [AppFilterController]
    public class DenemeActionController : Controller
    {
        public int kutuphaneid { get; set; }
        public string MesajBaslik { get; set; }
        public string Mesaj { get; set; }
        public string MesajIcon { get; set; }

        [HttpPost]
        public JsonResult kitapcikAllSubeSet()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var Denemeler = context.Denemelers.Where(x => x.Yayin.KutuphaneId ==
                        (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s => new Denemeler()
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        Kategori = s.Kategori,
                        DenemeStoklars = s.DenemeStoklars,
                        Yayin = s.Yayin
                    }).ToList();
                    var seanslar = context.DenemeSeanslars
                        .Where(x => x.Deneme.Yayin.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID"))
                        .Select(s => new DenemeSeanslar()
                        {
                            Id = s.Id,
                            DenemeId = s.DenemeId,
                            Kontenjan = s.Kontenjan,
                            Saat = s.Saat,
                            Tarih = s.Tarih,
                            SeansUcret = s.SeansUcret,
                            Deneme = s.Deneme,
                            Durum = s.Durum
                        }).ToList();                           
                    var franchiseList = context.Kurumkutuphanelers.Where
                    (x => x.MerkezId == (int)HttpContext.Session.GetInt32("subeid")
                          && 
                          x.Id != (int)HttpContext.Session.GetInt32("kutuphaneID")).ToList();
                    foreach (var kutuphane in franchiseList)
                    {
                        foreach (var kitapcik in Denemeler)
                        {
                            var control = context.Denemelers.Where(x =>
                                x.Yayin.KutuphaneId == kutuphane.Id && x.DenemeBaslik == kitapcik.DenemeBaslik && x.Yayin.YayinBaslik == kitapcik.Yayin.YayinBaslik).Count();
                            if (control is 0)
                            {
                                int yayinid = context.Yayinlars.Where(x => 
                                    x.YayinBaslik == kitapcik.Yayin.YayinBaslik && 
                                    x.KutuphaneId == kutuphane.Id).Select(s => s.Id).First();
                                if (yayinid is 0)
                                {
                                    var yayin = new Yayinlar();
                                    yayin.YayinBaslik = kitapcik.Yayin.YayinBaslik;
                                    yayin.Logo = kitapcik.Yayin.Logo;
                                    yayin.KutuphaneId = (int)HttpContext.Session.GetInt32("kutuphaneID");
                                    context.Yayinlars.Add(yayin);
                                    context.SaveChanges();

                                    yayinid = context.Yayinlars.Where(x =>
                                            x.KutuphaneId == kutuphane.Id && x.YayinBaslik == kitapcik.Yayin.YayinBaslik)
                                        .Max(y => y.Id);
                                    
                                }
                                // ŞUBE İLK AÇILIŞINDA YKS / LGS GİBİ ANA KATEGORİLER OTOMATİK EKLENMELİ
                                int katid = context.Kategorilers.Where(x =>
                                    x.AltKategoriBaslik == kitapcik.Kategori.AltKategoriBaslik &&
                                    x.Kat.KutuphaneId == kutuphane.Id).Select(s => s.Id).First();
                                if (katid is 0)
                                {
                                    int anakatid = context.Anakategorilers
                                        .Where(x => x.AnaKategoriBaslik == kitapcik.Kategori.Kat.AnaKategoriBaslik && x.KutuphaneId == kutuphane.Id)
                                        .Select(s => s.Id).First();
                                    var altkategori = new Kategoriler();
                                    altkategori.KatId = anakatid;
                                    altkategori.AltKategoriBaslik = kitapcik.Kategori.AltKategoriBaslik;
                                    context.Kategorilers.Add(altkategori);
                                    context.SaveChanges();
                                    katid = context.Kategorilers.Where(x =>
                                        x.Kat.KutuphaneId == kutuphane.Id &&
                                        x.AltKategoriBaslik == kitapcik.Kategori.AltKategoriBaslik).Max(k => k.Id);
                                }
                                var yenideneme = new Denemeler
                                {
                                    DenemeBaslik = kitapcik.DenemeBaslik,
                                    YayinId = yayinid,
                                    KategoriId = katid,
                                    GirisFiyat = kitapcik.GirisFiyat
                                };
                                
                                context.Denemelers.Add(yenideneme);
                                context.SaveChanges();
                            }
                            
                        }
                        foreach (var seans in seanslar)
                        {
                            var seanscontrol = context.DenemeSeanslars.Where(x =>
                                x.Deneme.Yayin.KutuphaneId == kutuphane.Id &&
                                x.Deneme.DenemeBaslik == seans.Deneme.DenemeBaslik && x.Tarih.Date == seans.Tarih.Date && x.Saat == seans.Saat).Count();
                            if (seanscontrol is 0)
                            {
                                int denemeid = context.Denemelers
                                    .Where(x => x.Yayin.KutuphaneId == kutuphane.Id && x.DenemeBaslik == seans.Deneme.DenemeBaslik).Select(s => s.Id).First();
                                var yeniseans = new DenemeSeanslar();
                                yeniseans.DenemeId = denemeid;
                                yeniseans.Tarih = seans.Tarih;
                                yeniseans.Saat = seans.Saat;
                                yeniseans.Durum = seans.Durum;
                                yeniseans.Kontenjan = seans.Kontenjan;
                                yeniseans.SeansUcret = seans.SeansUcret;
                                context.DenemeSeanslars.Add(yeniseans);
                                context.SaveChanges();
                            }
                        }
                    }

                    return Json("İşlem Başarılı");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(ex.ToString());
            }
        }
        [HttpPost]
        public JsonResult DenemeList()
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
            using(var context = new U1626744Db60AContext())
            {
                try
                {
                    List<viewDenemelerModel> Denemeler = context.Denemelers.Where(x => x.Kategori.Kat.KutuphaneId == kutuphaneid).Select(s => new viewDenemelerModel
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        Kategori = s.Kategori,
                        YayinId = s.YayinId,
                        YayinBaslik = s.Yayin.YayinBaslik,
                        doluStok = context.SeansOgrSets.Where(x => x.Seans.DenemeId == s.Id && x.Durum != 0).Count(),
                        toplamStok = s.DenemeStoklars.Where(t => t.StokType == 1).Sum(a => a.Adet) - s.DenemeStoklars.Where(t => t.StokType == 2).Sum(a => a.Adet)
                    }).OrderBy(o => o.DenemeBaslik).ToList();
                    var jsonresult = JsonConvert.SerializeObject(Denemeler);
                    return Json(jsonresult);
                }
                catch (Exception ex)
                {
                    MesajBaslik = "Hata Oluştu";
                    JsonConvert.SerializeObject(MesajBaslik);
                    Mesaj = ex.Message;
                    JsonConvert.SerializeObject(Mesaj);
                    MesajIcon = "error";
                    JsonConvert.SerializeObject(MesajIcon);
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });
                }
            }
        }

        [HttpPost]
        public JsonResult kitapcikstoklist()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    List<viewDenemeStoklarModel> stoklist = context.DenemeStoklars
                        .Where(x => x.Deneme.Yayin.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s => new viewDenemeStoklarModel()
                        {
                            Id = s.Id,
                            DenemeId = s.DenemeId,
                            StokType = s.StokType,
                            Adet = s.Adet,
                            Tarih = s.Tarih,
                            TarihSTR = s.Tarih.ToShortDateString(),
                            denemeBaslik = s.Deneme.DenemeBaslik
                        }).OrderByDescending(o => o.Tarih).ToList();
                    var jsonresult = JsonConvert.SerializeObject(stoklist);
                    return Json(jsonresult);
                }
            }
            catch (Exception ex)
            {
                MesajBaslik = "Hata Oluştu";
                JsonConvert.SerializeObject(MesajBaslik);
                Mesaj = ex.Message;
                JsonConvert.SerializeObject(Mesaj);
                MesajIcon = "error";
                JsonConvert.SerializeObject(MesajIcon);
                return Json(new { MesajBaslik, Mesaj, MesajIcon });
            }
        }
        
        [HttpPost]
        public JsonResult kitapciklist()
        {
            kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
            using(var context = new U1626744Db60AContext())
            {
                try
                {
                    List<viewDenemelerModel> Denemeler = context.Denemelers.Where(x => x.Kategori.Kat.KutuphaneId == kutuphaneid).Select(s => new viewDenemelerModel
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        Kategori = s.Kategori,
                        YayinId = s.YayinId,
                        Yayin = s.Yayin,
                        doluStok = context.SeansOgrSets.Where(x => x.Seans.DenemeId == s.Id && x.Durum != 0).Count(),
                        toplamStok = s.DenemeStoklars.Where(t => t.StokType == 1).Sum(a => a.Adet) - s.DenemeStoklars.Where(t => t.StokType == 2).Sum(a => a.Adet)
                    }).OrderBy(o => o.DenemeBaslik).ToList();
                    var jsonresult = JsonConvert.SerializeObject(Denemeler);
                    return Json(jsonresult);
                }
                catch (Exception ex)
                {
                    MesajBaslik = "Hata Oluştu";
                    JsonConvert.SerializeObject(MesajBaslik);
                    Mesaj = ex.Message;
                    JsonConvert.SerializeObject(Mesaj);
                    MesajIcon = "error";
                    JsonConvert.SerializeObject(MesajIcon);
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });
                }
            }
        }

        [HttpPost]
        public JsonResult stokop(viewDenemeStoklarModel model, int type)
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var stokrow = context.DenemeStoklars.Where(x => x.Id == model.Id).Select(s =>
                        new DenemeStoklar()
                        {
                            Id = s.Id,
                            DenemeId = s.DenemeId,
                            Adet = s.Adet,
                            StokType = s.StokType,
                            Tarih = s.Tarih,
                        }).FirstOrDefault();
                    if (type == 1)
                    {
                        var newstokrow = new DenemeStoklar();
                        newstokrow.DenemeId = model.DenemeId;
                        newstokrow.Adet = model.Adet;
                        newstokrow.Tarih = DateTime.Now;
                        newstokrow.StokType = model.StokType;
                        context.DenemeStoklars.Add(newstokrow);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        JsonConvert.SerializeObject(MesajBaslik);
                        MesajIcon = "success";
                        JsonConvert.SerializeObject(MesajIcon);
                    }else if (type == 2)
                    {
                        stokrow.DenemeId = model.DenemeId;
                        stokrow.Adet = model.Adet;
                        stokrow.StokType = model.StokType;
                        context.Entry(stokrow).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        JsonConvert.SerializeObject(MesajBaslik);
                        MesajIcon = "success";
                        JsonConvert.SerializeObject(MesajIcon);

                    }else if (type == 3)
                    {
                        context.DenemeStoklars.Remove(stokrow);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı, Stok Kaydı Silindi";
                        JsonConvert.SerializeObject(MesajBaslik);
                        MesajIcon = "danger";
                        JsonConvert.SerializeObject(MesajIcon);
                    }
                }
            }
            catch (Exception ex)
            {       
                MesajBaslik = "Hata Oluştu";
                JsonConvert.SerializeObject(MesajBaslik);
                Mesaj = ex.Message;
                JsonConvert.SerializeObject(Mesaj);
                MesajIcon = "error";
                JsonConvert.SerializeObject(MesajIcon);
            }
            return Json(new { MesajBaslik, Mesaj, MesajIcon });

        }
        [HttpPost]
        public JsonResult DenemeOp(viewDenemelerModel model, int type)
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    var DenemeKontrol = context.Denemelers.Where(X => X.Id == model.Id || X.DenemeBaslik == model.DenemeBaslik).Select(s => new Denemeler
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        KategoriId = s.KategoriId,
                        YayinId = s.YayinId 
                    }).FirstOrDefault();
                    if (type == 1)
                    {
                        var Deneme = new Denemeler();
                        Deneme.DenemeBaslik = model.DenemeBaslik;
                        Deneme.KategoriId = model.KategoriId;
                        Deneme.GirisFiyat = model.GirisFiyat;
                        Deneme.YayinId = model.YayinId;
                        context.Denemelers.Add(Deneme);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Deneme Eklendi";
                        MesajIcon = "success";
                    }
                    else if (type == 2)
                    {
                        DenemeKontrol.DenemeBaslik = model.DenemeBaslik;
                        DenemeKontrol.GirisFiyat = model.GirisFiyat;
                        DenemeKontrol.KategoriId = model.KategoriId;
                        DenemeKontrol.YayinId = model.YayinId;
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Deneme Kitapçığı Güncellendi";
                        MesajIcon = "success";

                    }
                    else if (type == 3)
                    {
                        if (DenemeKontrol.DenemeSeanslars.Count == 0)
                        {
                            context.Denemelers.Remove(DenemeKontrol); 
                            context.SaveChanges();
                            MesajBaslik = "İşlem Başarılı";
                            Mesaj = "Deneme Kitapçığı Silindi";
                            MesajIcon = "success";
                        }
                        else
                        {
                            MesajBaslik = "İşlem Başarısız";
                            Mesaj = "Deneme Kitapçığına Tanımlı Deneme Sınavı Seansı Bulunmaktadır, Lütfen Kontrol Ederek Tekrar Deneyiniz";
                            MesajIcon = "error";
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MesajBaslik = "Hata Oluştu";
                Mesaj = ex.Message;
                MesajIcon = "error";
            }

            string baslik = JsonConvert.SerializeObject(MesajBaslik);
            string mesaj = JsonConvert.SerializeObject(Mesaj);
            string icon = JsonConvert.SerializeObject(MesajIcon);
            return Json(new { baslik, mesaj, icon });
        }

        [HttpPost]
        public JsonResult kitapcikBilgi(int kitapcikid)
        {
            using (var context = new U1626744Db60AContext())
            {
                try
                {
                    var kitapcik = context.Denemelers.Where(x => x.Id == kitapcikid).Select(s => new Denemeler()
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        KategoriId = s.KategoriId,
                        YayinId = s.YayinId
                    }).First();
                    var jsonresult = JsonConvert.SerializeObject(kitapcik);
                    return Json(jsonresult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        [HttpPost]
        public JsonResult kitapcikInfoUpdate(viewDenemelerModel model)
        {
            using (var context = new U1626744Db60AContext())
            {
                try
                {
                    var kitapcik = context.Denemelers.Where(x => x.Id == model.Id).Select(s => new Denemeler()
                    {
                        Id = s.Id,
                        DenemeBaslik = s.DenemeBaslik,
                        GirisFiyat = s.GirisFiyat,
                        KategoriId = s.KategoriId,
                        YayinId = s.YayinId
                    }).First();
                    if (kitapcik is not null)
                    {
                        kitapcik.YayinId = model.YayinId;
                        kitapcik.KategoriId = model.KategoriId;
                        kitapcik.GirisFiyat = model.GirisFiyat;
                        kitapcik.DenemeBaslik = model.DenemeBaslik;
                        context.Entry(kitapcik).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();

                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Kitapçık Güncellendi";
                        MesajIcon = "success";

                    }
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
    }
}
