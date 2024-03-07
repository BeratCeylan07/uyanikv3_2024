using System;
namespace uyanikv3.customModels
{
	public class pushNotification
	{
		public string bildirimBaslik { get; set; }
		public string bildirimMesaj { get; set; }
		public string token { get; set; }
		public int bildirimType { get; set; }
		public string bildirimImg { get; set; }
	}
}

