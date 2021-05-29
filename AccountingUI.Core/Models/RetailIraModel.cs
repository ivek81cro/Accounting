using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class RetailIraModel : ValidationBindableBase
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
        private bool _knjizen;
        public bool Knjizen
        {
            get { return _knjizen; }
            set { SetProperty(ref _knjizen, value); }
        }
        public int TemeljnicaId { get; set; }
    }
}
