using System;
namespace uyanikv3.customModels
{
	public class viewUyelikPaketInfo
	{
		public string paketAd { get; set; }
		public string ucret { get; set; }
		public string gecerlilikGun { get; set; }
		public string toplamGirisHak { get; set; }
		public string baslangicTarih { get; set; }
		public string bitisTarih { get; set; }
		public string kalanGirisHak { get; set; }
		public bool paketStatus { get; set; }
		public List<viewUyelikPaketIkramTanimlamalar> tanimliIkramlar { get; set; }
	}
}

