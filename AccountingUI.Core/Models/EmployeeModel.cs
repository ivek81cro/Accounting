using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class EmployeeModel : ValidationBindableBase
    {
        public int Id { get; set; }

        private string _oib;
        [Required]
        public string Oib
        {
            get { return _oib; }
            set { SetProperty(ref _oib, value); }
        }

        private string _ime;
        [Required]
        public string Ime
        {
            get { return _ime; }
            set { SetProperty(ref _ime, value); }
        }

        private string _prezime;
        [Required]
        public string Prezime
        {
            get { return _prezime; }
            set { SetProperty(ref _prezime, value); }
        }

        public string Ulica { get; set; }
        public string Broj { get; set; }
        private string _mjesto;
        public string Mjesto
        {
            get { return _mjesto; }
            set { SetProperty(ref _mjesto, value); }
        }
        public string Drzava { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        private string _iban;
        public string Iban
        {
            get { return _iban; }
            set { SetProperty(ref _iban, value); }
        }
        public string StrucnaSprema { get; set; }
        public string Zvanje { get; set; }
        public decimal Olaksica { get; set; }
        public DateTime? DatumDolaska { get; set; }
        private DateTime? _datumOdlaska;
        public DateTime? DatumOdlaska
        {
            get { return _datumOdlaska; }
            set { SetProperty(ref _datumOdlaska, value); }
        }

        public void Reset()
        {
            Id = 0;
            Oib = "";
            Ime = "";
            Prezime = "";
            Ulica = "";
            Broj = "";
            Mjesto = "";
            Drzava = "";
            Telefon = "";
            Email = "";
            Iban = "";
            StrucnaSprema="";
            Zvanje = "";
            Olaksica = 0;
            DatumDolaska = null;
            DatumDolaska = null;
        }
    }
}
