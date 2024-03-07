using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class SeansPaket
{
    public int Id { get; set; }

    public int KutuphaneId { get; set; }

    public string PaketAdi { get; set; } = null!;

    public double Fiyat { get; set; }

    public int Adet { get; set; }

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;

    public virtual ICollection<SeansPaketTanim> SeansPaketTanims { get; set; } = new List<SeansPaketTanim>();
}
