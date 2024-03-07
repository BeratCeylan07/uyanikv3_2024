using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Raporlar
{
    public int Id { get; set; }

    public string RaporAy { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public DateTime D1 { get; set; }

    public DateTime D2 { get; set; }

    public double ToplamMasraf { get; set; }

    public double ToplamCiro { get; set; }

    public double HedefKar { get; set; }

    public double ToplamKar { get; set; }

    public double HedefKarToplamKarFark { get; set; }

    public double IskartaDenemeZarar { get; set; }

    public int ToplamStokGirisler { get; set; }

    public int ToplamStokCikislar { get; set; }

    public int ToplamDenemeSeans { get; set; }

    public int ToplamSinavaGirenOgr { get; set; }

    public string SampiyonDeneme { get; set; } = null!;

    public string KotuDeneme { get; set; } = null!;

    public int KutuphaneId { get; set; }

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;
}
