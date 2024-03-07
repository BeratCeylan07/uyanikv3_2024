using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Wplink
{
    public int Id { get; set; }

    public int KatId { get; set; }

    public string LinkBaslik { get; set; } = null!;

    public string Link { get; set; } = null!;

    public virtual Anakategoriler Kat { get; set; } = null!;
}
