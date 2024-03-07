using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Uyegirisikramhareket
{
    public int Id { get; set; }

    public int UyeGirisId { get; set; }

    public int IkramId { get; set; }

    public int Durum { get; set; }

    public DateTime Tarih { get; set; }

    public string Saat { get; set; } = null!;

    public int Adet { get; set; }

    public virtual Uyelikpaketikramtanimlamalar Ikram { get; set; } = null!;

    public virtual Uyegirishareketler UyeGiris { get; set; } = null!;
}
