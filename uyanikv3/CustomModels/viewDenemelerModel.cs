using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewDenemelerModel
    {
        public int Id { get; set; }
        public int YayinId { get; set; }
        public int KategoriId { get; set; }
        public string DenemeBaslik { get; set; } = null!;
        public string YayinBaslik { get; set; }
        public string KategoriBaslik { get; set; }
        public double GirisFiyat { get; set; }
        public double seansUcret { get; set; }
        public int toplamStok { get; set; }
        public int doluStok { get; set; }
        public int kalanStok { get; set; }
        public double toplamMaliyet { get; set; }
        public double hedefKar { get; set; }
        public double toplamKar { get; set; }
        public double birimKar { get; set; }
        public string tarih { get; set; }
        public string saat { get; set; }
        public int toplamDenemeSeans { get; set; }
        public virtual Kategoriler Kategori { get; set; } = null!;
        public virtual Yayinlar Yayin { get; set; } = null!;
        public virtual ICollection<DenemeSeanslar> DenemeSeanslars { get; set; }
        public virtual ICollection<DenemeStoklar> DenemeStoklars { get; set; }
        public virtual ICollection<Yayinlar> yayinList { get; set; }
        public virtual ICollection<Kategoriler> kategoriList { get; set; }

    }
}