using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Yayinlar
{
    public int Id { get; set; }

    public string YayinBaslik { get; set; } = null!;

    public string? Aciklama { get; set; }

    public string? Logo { get; set; }

    public int KutuphaneId { get; set; }

    public virtual ICollection<Denemeler> Denemelers { get; set; } = new List<Denemeler>();

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;
}
