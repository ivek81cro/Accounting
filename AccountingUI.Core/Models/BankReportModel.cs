using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class BankReportModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public int RedniBroj { get; set; }
        public DateTime DatumIzvoda { get; set; }
        public decimal SumaPotrazna { get; set; }
        public decimal SumaDugovna { get; set; }
        public decimal StanjePrethodnogIzvoda { get; set; }
        public decimal NovoStanje { get; set; }
        public bool Knjizen { get; set; }
    }
}
