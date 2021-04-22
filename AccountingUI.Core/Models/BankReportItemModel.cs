using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class BankReportItemModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public int IdIzvod { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        
        private string _konto;
        [Required]
        public string Konto
        {
            get { return _konto; }
            set { SetProperty(ref _konto, value); }
        }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public string Strana { get; set; }
    }
}
