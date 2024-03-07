using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Anakategoriler
{
    public int Id { get; set; }

    public int KutuphaneId { get; set; }

    public string AnaKategoriBaslik { get; set; } = null!;

    public virtual ICollection<Kategoriler> Kategorilers { get; set; } = new List<Kategoriler>();

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;

    public virtual ICollection<Ogrenciler> Ogrencilers { get; set; } = new List<Ogrenciler>();

    public virtual ICollection<Wplink> Wplinks { get; set; } = new List<Wplink>();
}
