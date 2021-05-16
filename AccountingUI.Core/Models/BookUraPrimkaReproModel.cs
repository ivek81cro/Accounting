using System;

namespace AccountingUI.Core.Models
{
    public class BookUraPrimkaReproModel
    {
        public int Id { get; set; }
        public DateTime DatumKnjizenja { get; set; }
        public int BrojPrimke { get; set; }
        public bool Storno { get; set; }
        public decimal NettoNabavnaVrijednost { get; set; }
        public string NazivDobavljaca { get; set; }
        public string BrojRacuna { get; set; }
        public decimal FakturnaVrijednost { get; set; }
        public DateTime DatumRacuna { get; set; }
        public bool Otpremnica { get; set; }
        public DateTime DospijecePlacanja { get; set; }
        public decimal NabavnaVrijednost { get; set; }
        public decimal Rabat { get; set; }
        public decimal Pretporez { get; set; }
        public decimal VeleprodajniRabat { get; set; }
        public decimal CassaSconto { get; set; }
        public string PorezniBroj { get; set; }
        public int BrojUKnjiziUra { get; set; }
        public bool Knjizen { get; set; }
    }
}
