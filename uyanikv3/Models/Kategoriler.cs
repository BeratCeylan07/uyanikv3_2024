using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Kategoriler
{
    public int Id { get; set; }

    public string AltKategoriBaslik { get; set; } = null!;

    public string? Aciklama { get; set; }

    public int KatId { get; set; }

    public virtual ICollection<Denemeler> Denemelers { get; set; } = new List<Denemeler>();

    public virtual Anakategoriler Kat { get; set; } = null!;
}
