using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class JoppdEmployeeModel : ValidationBindableBase
    {
        private string _oib;
        [Required]
        public string Oib
        {
            get { return _oib; }
            set { SetProperty(ref _oib, value); }
        }

        private string _sifraPrebivalista;
        [Required]
        public string SifraPrebivalista
        {
            get { return _sifraPrebivalista; }
            set { SetProperty(ref _sifraPrebivalista, value); }
        }

        private string _sifraOpcineRada;
        [Required]
        public string SifraOpcineRada
        {
            get { return _sifraOpcineRada; }
            set { SetProperty(ref _sifraOpcineRada, value); }
        }

        private string _oznakaStjecatelja;
        [Required]
        public string OznakaStjecatelja
        {
            get { return _oznakaStjecatelja; }
            set { SetProperty(ref _oznakaStjecatelja, value); }
        }

        private string _oznakaPrimitka;
        [Required]
        public string OznakaPrimitka
        {
            get { return _oznakaPrimitka; }
            set { SetProperty(ref _oznakaPrimitka, value); }
        }

        private string _dodatniMio;
        [Required]
        public string DodatniMio
        {
            get { return _dodatniMio; }
            set { SetProperty(ref _dodatniMio, value); }
        }

        private string _obvezaInvaliditet;
        [Required]
        public string ObvezaInvaliditet
        {
            get { return _obvezaInvaliditet; }
            set { SetProperty(ref _obvezaInvaliditet, value); }
        }

        private string _prviZadnjiMjesec;
        [Required]
        public string PrviZadnjiMjesec
        {
            get { return _prviZadnjiMjesec; }
            set { SetProperty(ref _prviZadnjiMjesec, value); }
        }

        private string _punoNepunoRadnoVrijeme;
        [Required]
        public string PunoNepunoRadnoVrijeme
        {
            get { return _punoNepunoRadnoVrijeme; }
            set { SetProperty(ref _punoNepunoRadnoVrijeme, value); }
        }

        private string _nacinIsplate;
        [Required]
        public string NacinIsplate
        {
            get { return _nacinIsplate; }
            set { SetProperty(ref _nacinIsplate, value); }
        }
    }
}
