using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewUyeGirisHareketlerModel
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; } = null!;
        public string tarihStr { get; set; }
        public string gun { get; set; }
        public virtual Ogrenciler Uye { get; set; } = null!;
        public string AdSoyad { get; set; }
        public int typ { get; set; }
        public string ogrQR { get; set; }
    }
}

