using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class BankReportItemModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public int IdIzvod { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Konto { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public string Strana { get; set; }
    }
}
