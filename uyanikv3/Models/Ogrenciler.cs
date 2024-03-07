using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Ogrenciler
{
    public int Id { get; set; }

    public int KategoriId { get; set; }

    public int OkulId { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string Telefon { get; set; } = null!;

    public DateTime Ktarih { get; set; }

    public string Sifre { get; set; } = null!;

    public int Durum { get; set; }

    public string? Qrasc { get; set; }

    public int? KutuphaneUye { get; set; }

    public int KutuphaneId { get; set; }

    public virtual Anakategoriler Kategori { get; set; } = null!;

    public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;

    public virtual ICollection<Kutuphaneuyelikpakettanimlamalar> Kutuphaneuyelikpakettanimlamalars { get; set; } = new List<Kutuphaneuyelikpakettanimlamalar>();

    public virtual ICollection<Ogrbildirimlog> Ogrbildirimlogs { get; set; } = new List<Ogrbildirimlog>();

    public virtual ICollection<OgrenciOdemeler> OgrenciOdemelers { get; set; } = new List<OgrenciOdemeler>();

    public virtual ICollection<Ogrtoken> Ogrtokens { get; set; } = new List<Ogrtoken>();

    public virtual Okullar Okul { get; set; } = null!;

    public virtual ICollection<SeansOgrSet> SeansOgrSets { get; set; } = new List<SeansOgrSet>();

    public virtual ICollection<Uyegirishareketler> Uyegirishareketlers { get; set; } = new List<Uyegirishareketler>();
}
