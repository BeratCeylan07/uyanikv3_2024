using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Seansbildirimlog
{
    public int Id { get; set; }

    public int SeansId { get; set; }

    public int BildirimId { get; set; }

    public DateTime LogTarih { get; set; }

    public virtual Bildirimler Bildirim { get; set; } = null!;

    public virtual DenemeSeanslar Seans { get; set; } = null!;
}
