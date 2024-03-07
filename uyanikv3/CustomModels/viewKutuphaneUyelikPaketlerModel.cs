using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewKutuphaneUyelikPaketlerModel
    {
        public int Id { get; set; }
        public string PaketBaslik { get; set; } = null!;
        public double Ucret { get; set; }
        public int GecerlilikToplamGun { get; set; }
        public int ToplamGirisHak { get; set; }
        public int PaketDurum { get; set; }

        public virtual ICollection<Kutuphaneuyelikpakettanimlamalar> Kutuphaneuyelikpakettanimlamalars { get; set; }
        public virtual ICollection<Uyelikpaketikramtanimlamalar> Uyelikpaketikramtanimlamalars { get; set; }
        public virtual List<Ogrenciler> kayitliOgrList { get; set; }
        public virtual List<Uyelikpaketikramtanimlamalar> paketIkramlar { get; set; }
    }
}

