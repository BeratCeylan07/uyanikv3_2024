using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Uyegirishareketler
{
    public int Id { get; set; }

    public int UyeId { get; set; }

    public DateTime Tarih { get; set; }

    public string Saat { get; set; } = null!;

    public virtual Ogrenciler Uye { get; set; } = null!;

    public virtual ICollection<Uyegirisikramhareket> Uyegirisikramharekets { get; set; } = new List<Uyegirisikramhareket>();
}
