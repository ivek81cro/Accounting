using AccountingUI.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class BookAccountModel : ValidationBindableBase
    {
        private string _konto;
        [Required]
        public string Konto
        {
            get { return _konto; }
            set 
            { 
                SetProperty(ref _konto, value);
            }
        }

        private string _opis;
        [Required]
        public string Opis
        {
            get { return _opis; }
            set 
            { 
                SetProperty(ref _opis, value);
            }
        }
    }
}
