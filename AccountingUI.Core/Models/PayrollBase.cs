using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class PayrollBase : ValidationBindableBase
    {
        private decimal _bruto;
        public decimal Bruto
        {
            get { return _bruto; }
            set { SetProperty(ref _bruto, value); }
        }

        private decimal _mio1;
        public decimal Mio1
        {
            get { return _mio1; }
            set { SetProperty(ref _mio1, value); }
        }

        private decimal _mio2;
        public decimal Mio2
        {
            get { return _mio2; }
            set { SetProperty(ref _mio2, value); }
        }

        private decimal _dohodak;
        public decimal Dohodak
        {
            get { return _dohodak; }
            set { SetProperty(ref _dohodak, value); }
        }

        private decimal _odbitak;
        public decimal Odbitak
        {
            get { return _odbitak; }
            set { SetProperty(ref _odbitak, value); }
        }

        private decimal _poreznaOsnovica;
        public decimal PoreznaOsnovica
        {
            get { return _poreznaOsnovica; }
            set { SetProperty(ref _poreznaOsnovica, value); }
        }

        private decimal _poreznaStopa1;
        public decimal PoreznaStopa1
        {
            get { return _poreznaStopa1; }
            set { SetProperty(ref _poreznaStopa1, value); }
        }

        private decimal _poreznaStopa2;
        public decimal PoreznaStopa2
        {
            get { return _poreznaStopa2; }
            set { SetProperty(ref _poreznaStopa2, value); }
        }

        private decimal _ukupnoPorez;
        public decimal UkupnoPorez
        {
            get { return _ukupnoPorez; }
            set { SetProperty(ref _ukupnoPorez, value); }
        }

        private decimal _prirez;
        public decimal Prirez
        {
            get { return _prirez; }
            set { SetProperty(ref _prirez, value); }
        }

        private decimal _ukupnoPorezPrirez;
        public decimal UkupnoPorezPrirez
        {
            get { return _ukupnoPorezPrirez; }
            set { SetProperty(ref _ukupnoPorezPrirez, value); }
        }

        private decimal _neto;
        public decimal Neto
        {
            get { return _neto; }
            set { SetProperty(ref _neto, value); }
        }

        private decimal _doprinosZdravstvo;
        public decimal DoprinosZdravstvo
        {
            get { return _doprinosZdravstvo; }
            set { SetProperty(ref _doprinosZdravstvo, value); }
        }
    }
}
