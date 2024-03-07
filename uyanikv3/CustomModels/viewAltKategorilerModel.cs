using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewAltKategorilerModel
    {

        public int Id { get; set; }
        public string AltKategoriBaslik { get; set; } = null!;
        public string? Aciklama { get; set; }
        public int KatId { get; set; }


        public string KategoriBaslik { get; set; } = null!;
        public int? toplamKitapcik { get; set; }
        public int? toplamSeans { get; set; }

        public int yayinid { get; set; }
        public virtual List<DenemeSeanslar> seansList { get; set; }
        public virtual ICollection<Denemeler> Denemelers { get; set; }
        public virtual ICollection<Ogrenciler> Ogrencilers { get; set; }
        public virtual Anakategoriler Kat { get; set; } = null!;

    }
}