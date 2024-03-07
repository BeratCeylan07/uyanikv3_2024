using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Kurumkutuphaneler
{
    public int Id { get; set; }

    public int? MerkezId { get; set; }

    public string KutuphaneBaslik { get; set; } = null!;

    public string? Il { get; set; }

    public string? Ilce { get; set; }

    public string? Adres { get; set; }

    public string? Tel { get; set; }

    public string? Tel2 { get; set; }

    public string? Eposta { get; set; }

    public string? Banka { get; set; }

    public string? Iban { get; set; }

    public string? Adsoyad { get; set; }

    public string? Onkayitexplanation { get; set; }

    public virtual ICollection<Anakategoriler> Anakategorilers { get; set; } = new List<Anakategoriler>();

    public virtual ICollection<Kutuphaneuyelikpaketler> Kutuphaneuyelikpaketlers { get; set; } = new List<Kutuphaneuyelikpaketler>();

    public virtual Merkezsubeler? Merkez { get; set; }

    public virtual ICollection<Ogrenciler> Ogrencilers { get; set; } = new List<Ogrenciler>();

    public virtual ICollection<Okullar> Okullars { get; set; } = new List<Okullar>();

    public virtual ICollection<Raporlar> Raporlars { get; set; } = new List<Raporlar>();

    public virtual ICollection<SeansPaket> SeansPakets { get; set; } = new List<SeansPaket>();

    public virtual ICollection<Yayinlar> Yayinlars { get; set; } = new List<Yayinlar>();
}
