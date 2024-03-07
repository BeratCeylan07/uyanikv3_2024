using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uyanikv3.AppFilter;
using uyanikv3.Models;

namespace uyanikv3.Controllers;

[AppFilterController]

public class SubeActionController : Controller
{
    [HttpPost]
    public JsonResult subeinfoupdate(Kurumkutuphaneler model)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var kutuphaneinfo = context.Kurumkutuphanelers
                    .Where(x => x.Id == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s =>
                        new Kurumkutuphaneler()
                        {
                            Id = s.Id,
                            KutuphaneBaslik = s.KutuphaneBaslik,
                            Il = s.Il,
                            Ilce = s.Ilce,
                            Adres = s.Adres,
                            Banka = s.Banka,
                            Iban = s.Iban,
                            Adsoyad = s.Adsoyad,
                            Onkayitexplanation = s.Onkayitexplanation
                        }).First();
                kutuphaneinfo.KutuphaneBaslik = model.KutuphaneBaslik;
                kutuphaneinfo.Il = model.Il;
                kutuphaneinfo.Ilce = model.Ilce;
                kutuphaneinfo.Adres = model.Adres;
                context.Entry(kutuphaneinfo).State = EntityState.Modified;
                context.SaveChanges();

                HttpContext.Session.SetInt32("kutuphaneID", kutuphaneinfo.Id);
                HttpContext.Session.SetString("AdminId", kutuphaneinfo.Id.ToString());
                HttpContext.Session.SetString("SubeAd",kutuphaneinfo.KutuphaneBaslik);
                HttpContext.Session.SetString("subeil",kutuphaneinfo.Il);
                HttpContext.Session.SetString("subeilce",kutuphaneinfo.Ilce);
                HttpContext.Session.SetString("subeadres",kutuphaneinfo.Adres);
                HttpContext.Session.SetString("subebank",kutuphaneinfo.Banka);
                HttpContext.Session.SetString("subebankadsoyad",kutuphaneinfo.Adsoyad);
                HttpContext.Session.SetString("subebankiban",kutuphaneinfo.Iban);
                HttpContext.Session.SetString("subeonkayitaciklama",kutuphaneinfo.Onkayitexplanation);
                return Json("İşlem Başarılı");
            }
        }
        catch (Exception ex)
        {
            return Json("Hata: " + ex);
        }
    }
    [HttpPost]
    public JsonResult subebankainfoupdate(Kurumkutuphaneler model)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var kutuphaneinfo = context.Kurumkutuphanelers
                    .Where(x => x.Id == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s =>
                        new Kurumkutuphaneler()
                        {
                            Id = s.Id,
                            KutuphaneBaslik = s.KutuphaneBaslik,
                            Il = s.Il,
                            Ilce = s.Ilce,
                            Adres = s.Adres,
                            Banka = s.Banka,
                            Iban = s.Iban,
                            Adsoyad = s.Adsoyad,
                            Onkayitexplanation = s.Onkayitexplanation
                        }).First();
                kutuphaneinfo.Banka = model.Banka;
                kutuphaneinfo.Adsoyad = model.Adsoyad;
                kutuphaneinfo.Iban = model.Iban;
                context.Entry(kutuphaneinfo).State = EntityState.Modified;
                context.SaveChanges();
                HttpContext.Session.SetInt32("kutuphaneID", kutuphaneinfo.Id);
                HttpContext.Session.SetString("AdminId", kutuphaneinfo.Id.ToString());
                HttpContext.Session.SetString("SubeAd",kutuphaneinfo.KutuphaneBaslik);
                HttpContext.Session.SetString("subeil",kutuphaneinfo.Il);
                HttpContext.Session.SetString("subeilce",kutuphaneinfo.Ilce);
                HttpContext.Session.SetString("subeadres",kutuphaneinfo.Adres);
                HttpContext.Session.SetString("subebank",kutuphaneinfo.Banka);
                HttpContext.Session.SetString("subebankadsoyad",kutuphaneinfo.Adsoyad);
                HttpContext.Session.SetString("subebankiban",kutuphaneinfo.Iban);
                HttpContext.Session.SetString("subeonkayitaciklama",kutuphaneinfo.Onkayitexplanation);
                return Json("İşlem Başarılı");
            }
        }
        catch (Exception ex)
        {
            return Json("Hata: " + ex);
        }
    }
    [HttpPost]
    public JsonResult subeonkayitmesajupdate(Kurumkutuphaneler model)
    {
        try
        {
            using (var context = new U1626744Db60AContext())
            {
                var kutuphaneinfo = context.Kurumkutuphanelers
                    .Where(x => x.Id == (int)HttpContext.Session.GetInt32("kutuphaneID")).Select(s =>
                        new Kurumkutuphaneler()
                        {
                            Id = s.Id,
                            MerkezId = (int)HttpContext.Session.GetInt32("subeid"),
                            KutuphaneBaslik = s.KutuphaneBaslik,
                            Il = s.Il,
                            Ilce = s.Ilce,
                            Adres = s.Adres,
                            Banka = s.Banka,
                            Iban = s.Iban,
                            Adsoyad = s.Adsoyad,
                            Onkayitexplanation = s.Onkayitexplanation
                        }).First();
                kutuphaneinfo.Onkayitexplanation = model.Onkayitexplanation;
                context.Entry(kutuphaneinfo).State = EntityState.Modified;
                context.SaveChanges();
                HttpContext.Session.SetInt32("kutuphaneID", kutuphaneinfo.Id);
                HttpContext.Session.SetString("AdminId", kutuphaneinfo.Id.ToString());
                HttpContext.Session.SetString("SubeAd",kutuphaneinfo.KutuphaneBaslik);
                HttpContext.Session.SetString("subeil",kutuphaneinfo.Il);
                HttpContext.Session.SetString("subeilce",kutuphaneinfo.Ilce);
                HttpContext.Session.SetString("subeadres",kutuphaneinfo.Adres);
                HttpContext.Session.SetString("subebank",kutuphaneinfo.Banka);
                HttpContext.Session.SetString("subebankadsoyad",kutuphaneinfo.Adsoyad);
                HttpContext.Session.SetString("subebankiban",kutuphaneinfo.Iban);
                HttpContext.Session.SetString("subeonkayitaciklama",kutuphaneinfo.Onkayitexplanation);
                return Json("İşlem Başarılı");
            }
        }
        catch (Exception ex)
        {
            return Json("Hata: " + ex);
        }
    }
}