using System;

namespace AccountingUI.Core.Models
{
    public class PayrollCalculationModel : PayrollModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }

        private int _accountingId;
        public int AccountingId
        {
            get { return _accountingId; }
            set { SetProperty(ref _accountingId, value); }
        }

        public void ResetMoneyValues()
        {
            Bruto = 0;
            Mio1 = 0;
            Mio2 = 0;
            Dohodak = 0;
            DoprinosZdravstvo = 0;
            Neto = 0;
            Odbitak = 0;
            PoreznaOsnovica = 0;
            PoreznaStopa1 = 0;
            PoreznaStopa2 = 0;
            Prirez = 0;
            UkupnoPorez = 0;
            UkupnoPorezPrirez = 0;            
        }
    }
}
