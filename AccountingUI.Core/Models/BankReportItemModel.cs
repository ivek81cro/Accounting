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
        
        private decimal _dugovna;
        public decimal Dugovna
        {
            get { return _dugovna; }
            set { SetProperty(ref _dugovna, value); }
        }

        private decimal _potrazna;
        public decimal Potrazna
        {
            get { return _potrazna; }
            set { SetProperty(ref _potrazna, value); }
        }

        public string Strana { get; set; }
    }
}
