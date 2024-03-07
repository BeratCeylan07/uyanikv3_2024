using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewGeneleKasa
    {
        public double toplamCiro { get; set; }
        public double toplamMaliyet { get; set; }
        public double toplamKar { get; set; }
        public DateTime baslangicTarih { get; set; }
        public DateTime bitisTarih { get; set; }
    }
}
