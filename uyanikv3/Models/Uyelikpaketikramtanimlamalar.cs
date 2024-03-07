using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Uyelikpaketikramtanimlamalar
{
    public int Id { get; set; }

    public int PaketId { get; set; }

    public string IkramBaslik { get; set; } = null!;

    public int Adet { get; set; }

    public virtual Kutuphaneuyelikpaketler Paket { get; set; } = null!;

    public virtual ICollection<Uyegirisikramhareket> Uyegirisikramharekets { get; set; } = new List<Uyegirisikramhareket>();
}
