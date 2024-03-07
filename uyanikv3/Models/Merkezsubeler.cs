using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Merkezsubeler
{
    public int Id { get; set; }

    public string MerkezSubeAdi { get; set; } = null!;

    public string Il { get; set; } = null!;

    public virtual ICollection<Kurumkutuphaneler> Kurumkutuphanelers { get; set; } = new List<Kurumkutuphaneler>();

    public virtual ICollection<Systmuser> Systmusers { get; set; } = new List<Systmuser>();
}
