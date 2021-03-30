using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class PayrollSupplementModel : ValidationBindableBase
    {
        public string Sifra { get; set; }
        public string Naziv { get; set; }
    }
}
