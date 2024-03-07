using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewKutuphaneUyePaketTanimlamalarModel
    {
        public int Id { get; set; }
        public int OgrId { get; set; }
        public int PaketId { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime BitisTarih { get; set; }
        public string TarihSTR { get; set; }
        public string BaslangicTarihSTR { get; set; }
        public string BitisTarihSTR { get; set; }
        public virtual Ogrenciler Ogr { get; set; } = null!;
        public int toplamGiris { get; set; }
        public int kalangun { get; set; }
        public int bugun { get; set; }
        public virtual Kutuphaneuyelikpaketler Paket { get; set; } = null!;
    }
}

