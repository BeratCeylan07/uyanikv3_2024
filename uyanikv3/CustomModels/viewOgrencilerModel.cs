using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewOgrencilerModel
    {
        public int Id { get; set; }
        public int KategoriId { get; set; }
        public int okulId { get; set; }
        public string Ad { get; set; } = null!;
        public string Soyad { get; set; } = null!;
        public string Telefon { get; set; } = null!;
        public DateTime Ktarih { get; set; }
        public string denemeAd { get; set; }
        public string KtarihSTR { get; set; }
        public string saat { get; set; }
        public string Sifre { get; set; } = null!;
        public int Durum { get; set; }
        public int seansid { get; set; }
        public int toplamSeans { get; set; }
        public string seansbaslik { get; set; }
        public string odemeDurumSTR { get; set; }
        public int? KutuphaneUye { get; set; }
        public int KutuphaneId { get; set; }
        public int snsetid { get; set; }
        public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;
        public string ogrtoken { get; set; }
        public virtual Anakategoriler Kategori { get; set; } = null!;
        public virtual DenemeSeanslar seans { get; set; } = null!;
        public virtual Okullar Okul { get; set; } = null!;
        
        public virtual ICollection<OgrenciOdemeler> OgrenciOdemelers { get; set; }
        public virtual ICollection<SeansOgrSet> SeansOgrSets { get; set; }
        public virtual ICollection<Kutuphaneuyelikpakettanimlamalar> Kutuphaneuyelikpakettanimlamalars { get; set; }
    }
}