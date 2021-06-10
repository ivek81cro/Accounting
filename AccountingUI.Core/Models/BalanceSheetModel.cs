using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class BalanceSheetModel : ValidationBindableBase
    {
        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private string _konto;
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

        private decimal _stanje;
        public decimal Stanje
        {
            get { return _stanje; }
            set { SetProperty(ref _stanje, value); }
        }
    }
}
