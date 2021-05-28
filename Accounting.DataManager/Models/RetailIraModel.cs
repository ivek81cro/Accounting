using System;

namespace Accounting.DataManager.Models
{
    public class RetailIraModel
    {
        public int Id { get; set; }
        public int RedniBroj { get; set; }
        public DateTime? Datum { get; set; }
        public decimal Stopa { get; set; }
        public decimal NaplacenaVrijednost { get; set; }
        public decimal PoreznaOsnovica { get; set; }
        public decimal NettoRuc { get; set; }
        public decimal Pdv { get; set; }
        public decimal NabavnaVrijednost { get; set; }
        public decimal StornoMarze { get; set; }
        public decimal StornoPdv { get; set; }
        public decimal MaloprodajnaVrijednost { get; set; }
        public bool Knjizen { get; set; }
        public int TemeljnicaId { get; set; }
    }
}
