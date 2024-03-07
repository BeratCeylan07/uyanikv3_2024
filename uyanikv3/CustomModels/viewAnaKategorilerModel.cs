using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
	public class viewAnaKategorilerModel
	{
        public int Id { get; set; }
        public int KutuphaneId { get; set; }
        public string AnaKategoriBaslik { get; set; }

        public virtual ICollection<viewAltKategorilerModel> Kategorilers { get; set; }
    }
}

