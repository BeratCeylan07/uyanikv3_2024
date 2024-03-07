using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
	public class viewKurumKutuphaneler
	{
        public int Id { get; set; }
        public string KutuphaneBaslik { get; set; } = null!;
        public string? Il { get; set; }
        public string? Ilce { get; set; }
        public string? Adres { get; set; }
        public string? Tel { get; set; }
        public string? Tel2 { get; set; }
        public string? Eposta { get; set; }

        public virtual ICollection<Kategoriler> Kategorilers { get; set; }
        public virtual ICollection<Kutuphaneuyelikpaketler> Kutuphaneuyelikpaketlers { get; set; }
        public virtual ICollection<Ogrenciler> Ogrencilers { get; set; }
        public virtual ICollection<Okullar> Okullars { get; set; }
        public virtual ICollection<Raporlar> Raporlars { get; set; }
        public virtual ICollection<Systmuser> Systmusers { get; set; }
        public virtual ICollection<Yayinlar> Yayinlars { get; set; }
    }
}

