using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Kutuphanetoken
{
    public int Id { get; set; }

    public int KutuphaneId { get; set; }

    public string Token { get; set; } = null!;

    public virtual ICollection<Kutuphanetoken> InverseKutuphane { get; set; } = new List<Kutuphanetoken>();

    public virtual Kutuphanetoken Kutuphane { get; set; } = null!;
}
