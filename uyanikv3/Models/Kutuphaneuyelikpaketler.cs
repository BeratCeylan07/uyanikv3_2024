using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Kutuphaneuyelikpaketler
{
    public int Id { get; set; }

    public string PaketBaslik { get; set; } = null!;

    public double Ucret { get; set; }

    public int GecerlilikToplamGun { get; set; }

    public int ToplamGirisHak { get; set; }

    public int PaketDurum { get; set; }

    public int KutuphaneId { get; set; }

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;

    public virtual ICollection<Kutuphaneuyelikpakettanimlamalar> Kutuphaneuyelikpakettanimlamalars { get; set; } = new List<Kutuphaneuyelikpakettanimlamalar>();

    public virtual ICollection<Uyelikpaketikramtanimlamalar> Uyelikpaketikramtanimlamalars { get; set; } = new List<Uyelikpaketikramtanimlamalar>();
}
