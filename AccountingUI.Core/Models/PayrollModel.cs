namespace AccountingUI.Core.Models
{
    public class PayrollModel : PayrollBase
    {
        private string _oib;
        public string Oib
        {
            get { return _oib; }
            set { SetProperty(ref _oib, value); }
        }

        private string _ime;
        public string Ime
        {
            get { return _ime; }
            set { SetProperty(ref _ime, value); }
        }

        private string _prezime;
        public string Prezime
        {
            get { return _prezime; }
            set { SetProperty(ref _prezime, value); }
        }

        private bool _samoPrviStupMio;
        public bool SamoPrviStupMio
        {
            get { return _samoPrviStupMio; }
            set { SetProperty(ref _samoPrviStupMio, value); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }
    }
}
