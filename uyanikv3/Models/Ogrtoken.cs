using System;
using System.Collections.Generic;

namespace uyanikv3.Models;

public partial class Ogrtoken
{
    public int Id { get; set; }

    public int OgrId { get; set; }

    public string Token { get; set; } = null!;

    public virtual Ogrenciler Ogr { get; set; } = null!;
}
