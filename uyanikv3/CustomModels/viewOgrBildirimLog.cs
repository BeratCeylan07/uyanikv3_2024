using uyanikv3.Models;

namespace uyanikv3.customModels;

public class viewOgrBildirimLog
{
    public int Id { get; set; }

    public int OgrId { get; set; }

    public int BildirimId { get; set; }

    public DateTime LogTarih { get; set; }

    public virtual Bildirimler Bildirim { get; set; } = null!;

    public virtual Ogrenciler Ogr { get; set; } = null!;
}