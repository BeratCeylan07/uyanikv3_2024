using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class SeansPaketTanim
{
    public int Id { get; set; }

    public int Paketid { get; set; }

    public int Seansid { get; set; }

    public virtual SeansPaket Paket { get; set; } = null!;

    public virtual DenemeSeanslar Seans { get; set; } = null!;
}
