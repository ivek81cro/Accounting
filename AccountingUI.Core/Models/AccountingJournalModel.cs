using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class AccountingJournalModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public string Opis { get; set; }
        public string Dokument { get; set; }
        public int Broj { get; set; }
        private string _konto;
        [Required]
        public string Konto
        {
            get { return _konto; }
            set { SetProperty(ref _konto, value); }
        }
        public DateTime? Datum { get; set; }
        public string Valuta { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public string VrstaTemeljnice { get; set; }
        public int BrojTemeljnice { get; set; }
    }
}
