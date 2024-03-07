using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Kutuphaneuyelikpakettanimlamalar
{
    public int Id { get; set; }

    public int OgrId { get; set; }

    public int PaketId { get; set; }

    public DateTime Tarih { get; set; }

    public DateTime BaslangicTarih { get; set; }

    public DateTime BitisTarih { get; set; }

    public virtual Ogrenciler Ogr { get; set; } = null!;

    public virtual Kutuphaneuyelikpaketler Paket { get; set; } = null!;
}
