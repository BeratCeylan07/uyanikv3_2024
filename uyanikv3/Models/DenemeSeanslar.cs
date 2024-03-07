using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class DenemeSeanslar
{
    public int Id { get; set; }

    public int DenemeId { get; set; }

    public DateTime Tarih { get; set; }

    public string Saat { get; set; } = null!;

    public int Durum { get; set; }

    public int Kontenjan { get; set; }

    public double SeansUcret { get; set; }

    public virtual Denemeler Deneme { get; set; } = null!;

    public virtual ICollection<SeansOgrSet> SeansOgrSets { get; set; } = new List<SeansOgrSet>();

    public virtual ICollection<SeansPaketTanim> SeansPaketTanims { get; set; } = new List<SeansPaketTanim>();

    public virtual ICollection<Seansbildirimlog> Seansbildirimlogs { get; set; } = new List<Seansbildirimlog>();
}
