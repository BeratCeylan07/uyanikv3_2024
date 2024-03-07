using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewDenemeStoklarModel
    {
        public int Id { get; set; }
        public int DenemeId { get; set; }
        public int StokType { get; set; }
        public int Adet { get; set; }
        public double denemeAdetFiyat { get; set; }
        public string denemeBaslik { get; set; }
        public string yayinBaslik { get; set; }
        public string TarihSTR { get; set; }
        public DateTime Tarih { get; set; }
        public virtual Denemeler Deneme { get; set; } = null!;
    }
}