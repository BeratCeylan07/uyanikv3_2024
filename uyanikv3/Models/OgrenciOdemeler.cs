using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class OgrenciOdemeler
{
    public int Id { get; set; }

    public int OgrId { get; set; }

    public double Tutar { get; set; }

    public int Durum { get; set; }

    public DateTime Tarih { get; set; }

    public virtual Ogrenciler Ogr { get; set; } = null!;
}
