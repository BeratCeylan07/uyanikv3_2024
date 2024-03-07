using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Okullar
{
    public int Id { get; set; }

    public string OkulBaslik { get; set; } = null!;

    public int KutuphaneId { get; set; }

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;

    public virtual ICollection<Ogrenciler> Ogrencilers { get; set; } = new List<Ogrenciler>();
}
