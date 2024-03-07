using System;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using uyanikv3.Models;

namespace uyanikv3.customModels
{
    public class viewRaporSorguSonucModel
    {
        public int Id { get; set; }
        public string RaporAy { get; set; } = null!;
        public DateTime Tarih { get; set; }
        public virtual string D1 { get; set; }
        public virtual string D2 { get; set; }
        public virtual string raporTarih { get; set; }
        public double toplamciro { get; set; }
        public double toplamkayit { get; set; }
        public List<viewOgrencilerModel> kayitliogrlist { get; set;}
        public List<viewSeansModel> seansList { get; set; }
        public List<Denemeler> kitapciklist { get; set; }
        public List<viewDenemeStoklarModel> stokGirislerList { get; set; }
        public List<viewDenemeStoklarModel> stokCikislarList { get; set; }


    }
}

