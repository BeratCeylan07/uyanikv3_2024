using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewYayinlarModel
    {
        public int Id { get; set; }
        public string YayinBaslik { get; set; } = null!;
        public string? Aciklama { get; set; }
        public string? Logo { get; set; }
        public IFormFile? _Logo { get; set; }
        public int kategoriID {get; set;}
        public int toplamKitapcik { get; set; }
        public int toplamDenemSeans { get; set; }
        public List<DenemeSeanslar> seanslist { get; set; }
        public virtual ICollection<Denemeler> Denemelers { get; set; }


    }
}