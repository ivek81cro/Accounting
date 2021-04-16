using System;

namespace Accounting.DataManager.Models
{
    public class BookUraPrimkaModel
    {
        public int Id { get; set; }
        public DateTime DatumKnjizenja { get; set; }
        public int BrojPrimke { get; set; }
        public bool Storno { get; set; }
        public decimal MaloprodajnaVrijednost { get; set; }
        public string NazivDobavljaca { get; set; }
        public string BrojRacuna { get; set; }
        public DateTime DatumRacuna { get; set; }
        public bool Otpremnica { get; set; }
        public DateTime DospijecePlacanja { get; set; }
        public decimal FakturnaVrijednost { get; set; }
        public decimal MaloprodajnaMarza { get; set; }
        public decimal IznosPdv { get; set; }
        public decimal VrijednostBezPoreza { get; set; }
        public decimal NabavnaVrijednost { get; set; }
        public decimal MaloprodajniRabat { get; set; }
        public decimal NettoNabavnaVrijednost { get; set; }
        public decimal Pretporez { get; set; }
        public decimal VeleprodajniRabat { get; set; }
        public decimal CassaSconto { get; set; }
        public decimal NettoRuc { get; set; }
        public decimal PovratnaNaknada { get; set; }
        public string PorezniBroj { get; set; }
        public int BrojUKnjiziUra { get; set; }
    }
}
