using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class DenemeStoklar
{
    public int Id { get; set; }

    public int DenemeId { get; set; }

    public int StokType { get; set; }

    public int Adet { get; set; }

    public DateTime Tarih { get; set; }

    public virtual Denemeler Deneme { get; set; } = null!;
}
