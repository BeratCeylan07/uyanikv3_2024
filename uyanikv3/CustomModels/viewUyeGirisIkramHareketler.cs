using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
	public class viewUyeGirisIkramHareketler
	{
        public int Id { get; set; }
        public int UyeGirisId { get; set; }
        public int IkramId { get; set; }
        public int Durum { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; } = null!;
        public int Adet { get; set; }

        public virtual Uyelikpaketikramtanimlamalar Ikram { get; set; } = null!;
        public virtual Uyegirishareketler UyeGiris { get; set; } = null!;
        public List<Uyelikpaketikramtanimlamalar> ikramList { get; set; }
    }
}

