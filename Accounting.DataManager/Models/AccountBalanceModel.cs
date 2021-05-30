using System;

namespace Accounting.DataManager.Models
{
    public class AccountBalanceModel
    {
        public string Opis { get; set; }
        public string Dokument { get; set; }
        public DateTime Datum { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public decimal Stanje { get; set; }
    }
}
