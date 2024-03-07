using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewSeansOgrSetModel
    {
        public int Id { get; set; }
        public int OgrId { get; set; }
        public int SeansId { get; set; }
        
        public int katid { get; set; }
        public DateTime SeansKayitTarih { get; set; }
        public string SeansKayitTarihSTR { get; set; }
        public string? Aciklama { get; set; }
        public string kategoriBaslik { get; set; }
        public int Durum { get; set; }
        public double kayitUcret { get; set; }
        public int ucretOdemeDurum { get; set; }
        public string[] multipleSeansID { get; set; }
        public string denemeAd { get; set; }
        public string yayinAd { get; set; }
        public string tarihSaat { get; set; }
        public string seansTarih { get; set; }
        public virtual Ogrenciler Ogr { get; set; } = null!;
        public virtual DenemeSeanslar SeansInfo { get; set; }
        public virtual List<DenemeSeanslar> Seans { get; set; } = null!;
        public virtual List<viewSeansModel> Seanss { get; set; }

    }
}