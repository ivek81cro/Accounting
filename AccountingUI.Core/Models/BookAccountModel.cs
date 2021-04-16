using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class BookAccountModel : ValidationBindableBase
    {
        private string _konto;
        public string Konto
        {
            get { return _konto; }
            set { SetProperty(ref _konto, value); }
        }

        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }
    }
}
