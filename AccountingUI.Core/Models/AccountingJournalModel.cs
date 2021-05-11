using AccountingUI.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingUI.Core.Models
{
    public class AccountingJournalModel : ValidationBindableBase
    {
        public int Id { get; set; }
        private string _opis;
        [Required]
        public string Opis
        {
            get { return _opis; }
            set { SetProperty(ref _opis, value); }
        }

        private string _dokument;
        [Required]
        public string Dokument
        {
            get { return _dokument; }
            set { SetProperty(ref _dokument, value); }
        }
        public int Broj { get; set; }

        private string _konto;
        [Required]
        public string Konto
        {
            get { return _konto; }
            set { SetProperty(ref _konto, value); }
        }

        private DateTime? _datum;
        [Required]
        public DateTime? Datum
        {
            get { return _datum; }
            set { SetProperty(ref _datum, value); }
        }

        public string Valuta { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public string VrstaTemeljnice { get; set; }
        public int BrojTemeljnice { get; set; }
    }
}
