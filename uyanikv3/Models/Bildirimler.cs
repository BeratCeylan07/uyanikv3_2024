using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Bildirimler
{
    public int Id { get; set; }

    public int KutuphaneId { get; set; }

    public string Baslik { get; set; } = null!;

    public string Mesaj { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public int Durum { get; set; }

    public virtual ICollection<Ogrbildirimlog> Ogrbildirimlogs { get; set; } = new List<Ogrbildirimlog>();

    public virtual ICollection<Seansbildirimlog> Seansbildirimlogs { get; set; } = new List<Seansbildirimlog>();
}
