using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class LocoOrderModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _zaposlenikId;
        public int ZaposlenikId
        {
            get { return _zaposlenikId; }
            set { SetProperty(ref _zaposlenikId, value); }
        }

        private DateTime _datum;
        public DateTime Datum
        {
            get { return _datum; }
            set { SetProperty(ref _datum, value); }
        }

        private string _markaVozila;
        public string MarkaVozila
        {
            get { return _markaVozila; }
            set { SetProperty(ref _markaVozila, value); }
        }

        private string _registracija;
        public string Registracija
        {
            get { return _registracija; }
            set { SetProperty(ref _registracija, value); }
        }

        private int _pocetnoStanje;
        public int PocetnoStanje
        {
            get { return _pocetnoStanje; }
            set { SetProperty(ref _pocetnoStanje, value); }
        }

        private int _zavrsnoStanje;
        public int ZavrsnoStanje
        {
            get { return _zavrsnoStanje; }
            set { SetProperty(ref _zavrsnoStanje, value); }
        }

        private string _relacija;
        public string Relacija
        {
            get { return _relacija; }
            set { SetProperty(ref _relacija, value); }
        }

        private int _prijedeniKilometri;
        public int PrijedeniKilometri
        {
            get { return _prijedeniKilometri; }
            set { SetProperty(ref _prijedeniKilometri, value); }
        }

        private string _opis;
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private int _obracunId;
        public int ObracunId
        {
            get { return _obracunId; }
            set { SetProperty(ref _obracunId, value); }
        }
    }
}
