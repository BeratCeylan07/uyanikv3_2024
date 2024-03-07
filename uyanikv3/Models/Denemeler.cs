using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Denemeler
{
    public int Id { get; set; }

    public int YayinId { get; set; }

    public int KategoriId { get; set; }

    public string DenemeBaslik { get; set; } = null!;

    public double GirisFiyat { get; set; }

    public virtual ICollection<DenemeSeanslar> DenemeSeanslars { get; set; } = new List<DenemeSeanslar>();

    public virtual ICollection<DenemeStoklar> DenemeStoklars { get; set; } = new List<DenemeStoklar>();

    public virtual Kategoriler Kategori { get; set; } = null!;

    public virtual Yayinlar Yayin { get; set; } = null!;
}
