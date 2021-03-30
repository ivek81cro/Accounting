namespace AccountingUI.Core.Models
{
    public class PayrollSupplementEmployeeModel : PayrollSupplementModel
    {
        //Base properties: Sifra, Naziv
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _oib;
        public string Oib
        {
            get { return _oib; }
            set { SetProperty(ref _oib, value); }
        }

        private decimal _iznos;
        public decimal Iznos
        {
            get { return _iznos; }
            set { SetProperty(ref _iznos, value); }
        }
    }
}
