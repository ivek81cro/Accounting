using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class CashRegisterModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _redniBroj;
        public int RedniBroj
        {
            get { return _redniBroj; }
            set { SetProperty(ref _redniBroj, value); }
        }

        private DateTime? _datum;
        public DateTime? Datum
        {
            get { return _datum; }
            set { SetProperty(ref _datum, value); }
        }

        private decimal _gotovina;
        public decimal Gotovina
        {
            get { return _gotovina; }
            set { SetProperty(ref _gotovina, value); }
        }

        private decimal _kreditneKartice;
        public decimal KreditneKartice
        {
            get { return _kreditneKartice; }
            set { SetProperty(ref _kreditneKartice, value); }
        }

        private decimal _sveukupno;
        public decimal Sveukupno
        {
            get { return _sveukupno; }
            set { SetProperty(ref _sveukupno, value); }
        }

        private decimal _iznosSudjelovanja;
        public decimal IznosSudjelovanja
        {
            get { return _iznosSudjelovanja; }
            set { SetProperty(ref _iznosSudjelovanja, value); }
        }

        private bool _knjizen;
        public bool Knjizen
        {
            get { return _knjizen; }
            set { SetProperty(ref _knjizen, value); }
        }
        public int TemeljnicaId { get; set; }
    }
}
