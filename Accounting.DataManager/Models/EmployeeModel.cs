using System;

namespace Accounting.DataManager.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Oib { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mjesto { get; set; }
        public string Drzava { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string StrucnaSprema { get; set; }
        public string Zvanje { get; set; }
        public decimal Olaksica { get; set; }
        public string Iban { get; set; }
        public DateTime DatumDolaska { get; set; }
        public DateTime DatumOdlaska { get; set; }
    }
}
