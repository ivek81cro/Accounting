using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class BookIraHzzoModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private DateTime _datumPlacanja;
        public DateTime DatumPlacanja
        {
            get { return _datumPlacanja; }
            set { SetProperty(ref _datumPlacanja, value); }
        }

        private string _dokument;
        public string Dokument
        {
            get { return _dokument; }
            set { SetProperty(ref _dokument, value); }
        }

        private string _originalniBroj;
        public string OriginalniBroj
        {
            get { return _originalniBroj; }
            set { SetProperty(ref _originalniBroj, value); }
        }

        private DateTime _datumDokumenta;
        public DateTime DatumDokumenta
        {
            get { return _datumDokumenta; }
            set { SetProperty(ref _datumDokumenta, value); }
        }

        private string _program;
        public string Program
        {
            get { return _program; }
            set { SetProperty(ref _program, value); }
        }

        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private decimal _iznosRacuna;
        public decimal IznosRacuna
        {
            get { return _iznosRacuna; }
            set { SetProperty(ref _iznosRacuna, value); }
        }

        private decimal _placeniIznos;
        public decimal PlaceniIznos
        {
            get { return _placeniIznos; }
            set { SetProperty(ref _placeniIznos, value); }
        }

        private bool _povezan;
        public bool Povezan
        {
            get { return _povezan; }
            set { SetProperty(ref _povezan, value); }
        }
    }
}
