using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Seanspaketicerikler
{
    public int Id { get; set; }

    public int PaketId { get; set; }

    public string Baslik { get; set; } = null!;

    public int Gecerliliksuresi { get; set; }

    public string Aciklama { get; set; } = null!;

    public string Aciklama2 { get; set; } = null!;

    public string Picture { get; set; } = null!;
}
