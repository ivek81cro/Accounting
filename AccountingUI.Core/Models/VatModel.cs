using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class VatModel : ValidationBindableBase
    {
        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private decimal _iraOsnovica0;
        public decimal IraOsnovica0
        {
            get { return _iraOsnovica0; }
            set { SetProperty(ref _iraOsnovica0, value); }
        }

        private decimal _iraOsnovica5;
        public decimal IraOsnovica5
        {
            get { return _iraOsnovica5; }
            set { SetProperty(ref _iraOsnovica5, value); }
        }

        private decimal _iraOsnovica10;
        public decimal IraOsnovica10
        {
            get { return _iraOsnovica10; }
            set { SetProperty(ref _iraOsnovica10, value); }
        }

        private decimal _iraOsnovica13;
        public decimal IraOsnovica13
        {
            get { return _iraOsnovica13; }
            set { SetProperty(ref _iraOsnovica13, value); }
        }

        private decimal _iraOsnovica25;
        public decimal IraOsnovica25
        {
            get { return _iraOsnovica25; }
            set { SetProperty(ref _iraOsnovica25, value); }
        }

        private decimal _pdv5;
        public decimal Pdv5
        {
            get { return _pdv5; }
            set { SetProperty(ref _pdv5, value); }
        }

        private decimal _pdv10;
        public decimal Pdv10
        {
            get { return _pdv10; }
            set { SetProperty(ref _pdv10, value); }
        }

        private decimal _pdv13;
        public decimal Pdv13
        {
            get { return _pdv13; }
            set { SetProperty(ref _pdv13, value); }
        }

        private decimal _pdv25;
        public decimal Pdv25
        {
            get { return _pdv25; }
            set { SetProperty(ref _pdv25, value); }
        }

        private decimal _uraOsnovica0;
        public decimal UraOsnovica0
        {
            get { return _uraOsnovica0; }
            set { SetProperty(ref _uraOsnovica0, value); }
        }

        private decimal _uraOsnovica5;
        public decimal UraOsnovica5
        {
            get { return _uraOsnovica5; }
            set { SetProperty(ref _uraOsnovica5, value); }
        }

        private decimal _uraOsnovica10;
        public decimal UraOsnovica10
        {
            get { return _uraOsnovica10; }
            set { SetProperty(ref _uraOsnovica10, value); }
        }

        private decimal _uraOsnovica13;
        public decimal UraOsnovica13
        {
            get { return _uraOsnovica13; }
            set { SetProperty(ref _uraOsnovica13, value); }
        }

        private decimal _uraOsnovica25;
        public decimal UraOsnovica25
        {
            get { return _uraOsnovica25; }
            set { SetProperty(ref _uraOsnovica25, value); }
        }

        private decimal _pretporezT5;
        public decimal PretporezT5
        {
            get { return _pretporezT5; }
            set { SetProperty(ref _pretporezT5, value); }
        }

        private decimal _pretporezT10;
        public decimal PretporezT10
        {
            get { return _pretporezT10; }
            set { SetProperty(ref _pretporezT10, value); }
        }

        private decimal _pretporezT13;
        public decimal PretporezT13
        {
            get { return _pretporezT13; }
            set { SetProperty(ref _pretporezT13, value); }
        }

        private decimal _pretporezT25;
        public decimal PretporezT25
        {
            get { return _pretporezT25; }
            set { SetProperty(ref _pretporezT25, value); }
        }

        private decimal _neoporezivo;
        public decimal Neoporezivo
        {
            get { return _neoporezivo; }
            set { SetProperty(ref _neoporezivo, value); }
        }
    }
}
