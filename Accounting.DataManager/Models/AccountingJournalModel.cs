using System;

namespace Accounting.DataManager.Models
{
    public class AccountingJournalModel
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public string Dokument { get; set; }
        public int Broj { get; set; }
        public string Konto { get; set; }
        public DateTime? Datum { get; set; }
        public string Valuta { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public string VrstaTemeljnice { get; set; }
        public int BrojTemeljnice { get; set; }
        public DateTime? DatumKnjizenja { get; set; }
    }
}
