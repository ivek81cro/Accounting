using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class AccountBalanceModel : ValidationBindableBase
    {
        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private string _dokument;
        public string Dokument
        {
            get { return _dokument; }
            set { SetProperty(ref _dokument, value); }
        }

        private DateTime _datum;
        public DateTime Datum
        {
            get { return _datum; }
            set { SetProperty(ref _datum, value); }
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
