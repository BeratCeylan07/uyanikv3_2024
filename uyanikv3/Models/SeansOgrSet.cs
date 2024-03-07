using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class SeansOgrSet
{
    public int Id { get; set; }

    public int OgrId { get; set; }

    public int SeansId { get; set; }

    public DateTime SeansKayitTarih { get; set; }

    public string? Aciklama { get; set; }

    public int Durum { get; set; }

    public string? Qr { get; set; }

    public virtual Ogrenciler Ogr { get; set; } = null!;

    public virtual DenemeSeanslar Seans { get; set; } = null!;
}
