using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewOkulmodel
    {
        public int Id { get; set; }
        public string OkulBaslik { get; set; } = null!;
        public int KutuphaneId { get; set; }
        public int toplamOgrenci { get; set; }
        public virtual Kurumkutuphaneler Kutuphane { get; set; } = null!;
        public virtual ICollection<Ogrenciler> Ogrencilers { get; set; }
    }
}

