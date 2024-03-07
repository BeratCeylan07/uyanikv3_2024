using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Systmuser
{
    public int Id { get; set; }

    public string KAd { get; set; } = null!;

    public string KPass { get; set; } = null!;

    public int Auth { get; set; }

    public int SubeId { get; set; }

    public int Subsubeuser { get; set; }

    public virtual Merkezsubeler Sube { get; set; } = null!;
}
