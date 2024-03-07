using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using uyanikv3.Models;
using uyanikv3.customModels;
using Newtonsoft.Json;
using uyanikv3.AppFilter;

namespace uyanikv3.Controllers
{
    using Alias = IActionResult;

    [AppFilterController]
    public class YayinActionController : Controller
    {
        public int kutuphaneid { get; set; }
        public string MesajBaslik { get; set; }
        public string Mesaj { get; set; }
        public string MesajIcon { get; set; }
        private readonly IWebHostEnvironment _environment;
        public YayinActionController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpPost]
        public JsonResult yayinAllSubeSet()
        {
            try
            {
                using (var context = new U1626744Db60AContext())
                {
                    //Session daki mevcut kütüphanenin tüm yayınları
                    var yayinlist = context.Yayinlars.Where(x => x.KutuphaneId == (int)HttpContext.Session.GetInt32("kutuphaneID") 
                                                && 
                                                x.Kutuphane.MerkezId == (int)HttpContext.Session.GetInt32("subeid")).Select(s => new Yayinlar()
                    {
                        YayinBaslik = s.YayinBaslik,
                        KutuphaneId = s.KutuphaneId,
                        Logo = s.Logo,
                        Aciklama = s.Aciklama
                    }).ToList();
                    var franchiseList = context.Kurumkutuphanelers.Where
                         (x => x.MerkezId == (int)HttpContext.Session.GetInt32("subeid")
                          && 
                          x.Id != (int)HttpContext.Session.GetInt32("kutuphaneID")).ToList();
                    foreach (var kutuphane in franchiseList)
                    {
                        foreach (var yyn in yayinlist)
                        {
                            var control = context.Yayinlars
                                .Where(x => x.KutuphaneId == kutuphane.Id && x.YayinBaslik == yyn.YayinBaslik)
                                .Count();
                            if (control is 0)
                            {
                                var yayin = new Yayinlar();
                                yayin.YayinBaslik = yyn.YayinBaslik;
                                yayin.Logo = yyn.Logo;
                                yayin.KutuphaneId = kutuphane.Id;
                                context.Yayinlars.Add(yayin);
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
                throw;
            }
        }
        [HttpPost]
        public JsonResult YayinList(int type)
        {
            using(var context = new U1626744Db60AContext())
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

                try
                {
                    List<viewYayinlarModel> yayinlist = context.Yayinlars.Where(x => x.KutuphaneId == kutuphaneid).Select(s => new viewYayinlarModel
                    {
                        Id = s.Id,
                        YayinBaslik = s.YayinBaslik,
                        Aciklama = s.Aciklama,
                        Logo = s.Logo,
                        toplamDenemSeans = context.DenemeSeanslars.Where(k => k.Deneme.YayinId == s.Id).Count(),
                        toplamKitapcik =  context.Denemelers.Where(a => a.YayinId == s.Id).Count()
                    }).OrderBy(o => o.YayinBaslik).ToList();
                    if (type == 1)
                    {
                        var yayinforseans = yayinlist.Where(a => a.toplamDenemSeans > 0).ToList();
                        return Json(yayinforseans);

                    }
                    else
                    {
                        var jsonresult = JsonConvert.SerializeObject(yayinlist);
                        return Json(jsonresult);
                    }
                    
                }
                catch (Exception ex)
                {
                    Mesaj = ex.Message;
                    MesajBaslik = "Hata Oluştu";
                    MesajIcon = "error";
                    return Json(new { Mesaj, MesajBaslik, MesajIcon });
                }
            }
        }
        [HttpPost]
        public JsonResult yayinlistforogr(int katid)
        {
            using(var context = new U1626744Db60AContext())
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

                var yayinlist = context.Yayinlars.Where(x => x.Denemelers.Where(a => a.Kategori.KatId == katid).Count() > 0 && x.Denemelers.Select(s => s.DenemeSeanslars.Where(x => x.Deneme.KategoriId == katid && x.Tarih.Date >= DateTime.Now.Date).Count() > 0).FirstOrDefault() != null).Select(s => new viewYayinlarModel
                {
                    Id = s.Id,
                    Logo = s.Logo,
                    YayinBaslik = s.YayinBaslik,
                    toplamDenemSeans = context.DenemeSeanslars.Where(b => b.Tarih.Date >= DateTime.Now.Date && b.Deneme.YayinId == s.Id).Count(),
                    seanslist = context.DenemeSeanslars.Where(a => a.Deneme.YayinId == s.Id && a.Deneme.Kategori.Kat.Id == katid && a.Tarih.Date >= DateTime.Now.Date).ToList()
                }).OrderBy(o => o.YayinBaslik).ToList();
                try
                {
                    for (int i = 0; i <= yayinlist.Count(); i++)
                    {
                        if (yayinlist[i].seanslist.Count() == 0)
                        {
                            yayinlist.RemoveAt(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                }
                var jsonresult = JsonConvert.SerializeObject(yayinlist);
                return Json(jsonresult);
            }
        }

        [HttpPost]
        public JsonResult SeansCountControl(int yayinid)
        {
            try
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

                int ynid = Convert.ToInt32(yayinid);
                using (var context = new U1626744Db60AContext())
                {
                    int count = 0;
                    count = context.DenemeSeanslars
                        .Where(x => x.Deneme.YayinId == ynid && x.Tarih.Date >= DateTime.Now.Date).Count();
                    return Json(count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json("");
            }
            
        }
        [HttpPost]
        public JsonResult yayinlistforogrchange(int seansid)
        {
            using(var context = new U1626744Db60AContext())
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

                int katid = context.DenemeSeanslars.Where(x => x.Id == seansid).Select(s => s.Deneme.KategoriId).First();
                var yayinlist = context.Yayinlars.Where(x => x.Denemelers.Where(a => a.Kategori.KatId == katid).Count() > 0).Select(s => new viewYayinlarModel
                {
                    Id = s.Id,
                    Logo = s.Logo,
                    YayinBaslik = s.YayinBaslik
                }).OrderBy(o => o.YayinBaslik).ToList();
                var jsonresult = JsonConvert.SerializeObject(yayinlist);
                return Json(jsonresult);
            }
        }
        [HttpPost]
        public IActionResult yayinekle(viewYayinlarModel model)
        {

            try
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                string filename = model._Logo.FileName;
            
                if (model._Logo != null && model._Logo.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    Random rnd = new Random();
                    int num = rnd.Next();

                    var fileName = num.ToString() + Path.GetExtension(model._Logo.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model._Logo.CopyToAsync(stream);
                    }

                    using (var context = new U1626744Db60AContext())
                    {
                        var yayin = new Yayinlar();
                        yayin.KutuphaneId = kutuphaneid;
                        yayin.YayinBaslik = model.YayinBaslik;
                        yayin.Aciklama = model.Aciklama;
                        yayin.Logo = "/images/" + fileName;
                        context.Yayinlars.Add(yayin);
                        context.SaveChanges();
                    }

                }
                return Redirect("~/Home/Yayinlar");
            }
            catch (Exception ex)
            {
                return Redirect("~/Home/Yayinlar");
            }

        }
        [HttpPost]
        public IActionResult yayinDuzenle(viewYayinlarModel model)
        {
            try
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");
                String filename = "";
                if (model._Logo != null && model._Logo.Length > 0)
                {
                
                    Random rnd = new Random();
                    int num = rnd.Next();
                    if (model._Logo.ContentType == "image/jpeg" || model._Logo.ContentType == "image/jpg" || model._Logo.ContentType == "image/png")
                    {
                        String uploadfolder = Path.Combine(_environment.WebRootPath,"images");
                        filename = num + "_" + model._Logo.FileName;
                        String filepath = Path.Combine(uploadfolder, filename);
                        model._Logo.CopyTo(new FileStream(filepath,FileMode.Create));

                    }


                    using (var context = new U1626744Db60AContext())
                    {
                        var yayin = context.Yayinlars.First(x => x.Id == model.Id && x.KutuphaneId == kutuphaneid);
                        yayin.KutuphaneId = kutuphaneid;
                        yayin.YayinBaslik = model.YayinBaslik;
                        yayin.Aciklama = model.Aciklama;
                        yayin.Logo = "/images/" + filename;
                        context.Entry(yayin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();
                    }

                }
                return Redirect("~/Home/Yayinlar");
            }
            catch (Exception e)
            {
                return Redirect("~/Home/Yayinlar/" + e.ToString());
            }
        }
        public JsonResult YayinOp(viewYayinlarModel model, int type)
        {
            try
            {
                kutuphaneid = (int)HttpContext.Session.GetInt32("kutuphaneID");

                using(var context = new U1626744Db60AContext()) 
                {
                    var YayinKontrol = context.Yayinlars.Where(x => x.Id == model.Id || x.YayinBaslik == model.YayinBaslik).Select(s => new Yayinlar
                    {
                        Id = s.Id,
                        YayinBaslik = s.YayinBaslik,
                        Aciklama = s.Aciklama,
                        Denemelers = s.Denemelers,
                        Kutuphane = s.Kutuphane,
                        Logo = s.Logo,
                        KutuphaneId = s.KutuphaneId
                    }).FirstOrDefault();
                    if (type == 1)
                    {
                        var yayin = new Yayinlar();
                        yayin.KutuphaneId = kutuphaneid;
                        yayin.YayinBaslik = model.YayinBaslik;
                        yayin.Aciklama = model.Aciklama;
                        yayin.Logo = model.Logo;
                        context.Yayinlars.Add(yayin);
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Yayın Eklendi";
                        MesajIcon = "success";
                    }
                    else if (type == 2)
                    {
                        YayinKontrol.YayinBaslik = model.YayinBaslik;
                        YayinKontrol.Aciklama = model.Aciklama;
                        YayinKontrol.Logo = model.Logo;
                        context.SaveChanges();
                        MesajBaslik = "İşlem Başarılı";
                        Mesaj = "Yayın Güncellendi";
                        MesajIcon = "success";
                        
                    }
                    else if (type == 3)
                    {
                        if (YayinKontrol.Denemelers.Count() == 0)
                        {
                            context.Yayinlars.Remove(YayinKontrol);
                            Mesaj = "Yayın Silindi";
                            MesajBaslik = "İşlem Başarılı";
                            MesajIcon = "info";
                        }
                        else
                        {
                            MesajBaslik = "İşlem Başarısız";
                            Mesaj = "Yayına Ait Kitapçık Kayıtları Bulunmaktadır, İşlem İptal Edildi";
                            MesajIcon = "error";
                        }
                    }
                    return Json(new { MesajBaslik, Mesaj, MesajIcon });
                }
            }
            catch (Exception ex)
            {
                MesajIcon = "error";
                MesajBaslik = "Hata Oluştu";
                Mesaj = ex.Message;
                return Json(new { Mesaj, MesajIcon, MesajBaslik });  
            }
        }
    }
}
