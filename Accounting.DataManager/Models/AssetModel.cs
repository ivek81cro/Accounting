using System;

namespace Accounting.DataManager.Models
{
    public class AssetModel
    {

        public int Id { get; set; }
        public string Naziv { get; set; }
        public DateTime? DatumNabave { get; set; }
        public decimal Kolicina { get; set; }
        public string Lokacija { get; set; }
        public int InvBroj { get; set; }
        public string Dobavljac { get; set; }
        public string Dokument { get; set; }
        public DateTime? DatumUporabe { get; set; }
        public decimal NabavnaVrijednost { get; set; }
        public string Skupina { get; set; }
        public decimal VijekTrajanja { get; set; }
        public decimal StopaOtpisa { get; set; }
        public string SintetickiKonto { get; set; }
        public string KontoOtpisa { get; set; }
        public decimal IznosOtpisa { get; set; }
        public decimal SadasnjaVrijednost { get; set; }
        public DateTime? DatumRashodovanja { get; set; }
        public string VrstaPoTrajanju { get; set; }
    }
}
