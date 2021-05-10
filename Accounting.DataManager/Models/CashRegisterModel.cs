using System;

namespace Accounting.DataManager.Models
{
    public class CashRegisterModel
    {
        public int Id { get; set; }
        public int RedniBroj { get; set; }        
        public DateTime? Datum { get; set; }
        public decimal Gotovina { get; set; }
        public decimal KreditneKartice { get; set; }
        public decimal Sveukupno { get; set; }
        public decimal IznosSudjelovanja { get; set; }
    }
}
