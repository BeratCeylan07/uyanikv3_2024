using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewSeansModel
    {
        public int Id { get; set; }
        public int customseansid { get; set; }
        public int DenemeId { get; set; }
        public int yayinId {get; set;}
        public DateTime Tarih { get; set; }
        public string TarihSTR { get; set; }
        public int? guncelKitapcik { get; set; }
        public string qrCodeforOgr { get; set; }
        public string Saat { get; set; } = null!;
        public string SeansGun { get; set; }
        public int Durum { get; set; }
        public int Kontenjan { get; set; }
        public int GuncelKontenjan { get; set; }
        public int KayitliOgrenci { get; set; }
        public int onkayitliOgrenci { get; set; }
        public int kitapcikID { get; set; }
        public int KatilimSaglayanToplam { get; set; }
        public int KitapcikAlanToplam { get; set; }
        public int YoklamaBekleyenToplam { get; set; }
        public string KategoriBaslik { get; set; } = null!;
        public double SeansUcret { get; set; }
        public double kitapcikucret { get; set; }
        public string yayinBaslik { get; set; }
        public string? yayinLogo {get; set;}
        public string subeinfo { get; set; }
        public string yayinKategoriBaslik { get; set; } = null!;
        public string kitapcikBaslik { get; set; }
        public int actionType { get; set; }
        public virtual Denemeler Deneme { get; set; } = null!;
        public double toplamKazanc { get; set; }
        public string ogrSeansKayitTarih { get; set; }  
        public double hedefciro { get; set; }
        public virtual ICollection<SeansOgrSet> SeansOgrSets { get; set; }
        public virtual int? seansogrsetKontrol { get; set; }
        public int ogrseansDurum { get; set; }
        public virtual List<viewOgrencilerModel> kayitliOgrList { get; set; }
        public virtual List<viewOgrencilerModel> katilimsaglayanlist { get; set; }
        public virtual List<viewOgrencilerModel> kitapcikalanlist { get; set; }
        public virtual List<viewOgrencilerModel> yoklamabekleyen { get; set; }
        public virtual List<viewOgrencilerModel> onkayitliogr { get; set; }
        public virtual List<Ogrenciler> onKayitliOgrList { get; set; }
        public virtual List<viewDenemelerModel> kitapcikList { get; set; }
        public virtual List<viewAltKategorilerModel> AltkategoriList { get; set; }
    }
}