using System;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
	public class viewUyelikPaketIkramTanimlamalar
	{
        public int Id { get; set; }
        public int PaketId { get; set; }
        public string IkramBaslik { get; set; } = null!;
        public int Adet { get; set; }

        public virtual Kutuphaneuyelikpaketler PaketıDNavigation { get; set; } = null!;
    }
}

