using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class BookIraModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public int RedniBroj { get; set; }
        public string BrojRacuna { get; set; }
        public bool Storno { get; set; }
        public int IzRacuna { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Dospijece { get; set; }
        public DateTime? DatumZadnjeUplate { get; set; }
        public string NazivISjedisteKupca { get; set; }
        public string Oib { get; set; }
        public decimal IznosSPdv { get; set; }
        public decimal OslobodjenoPdvEU { get; set; }
        public decimal OslobodjenoPdvOstalo { get; set; }
        public decimal ProlaznaStavka { get; set; }
        public decimal PoreznaOsnovica0 { get; set; }
        public decimal PoreznaOsnovica5 { get; set; }
        public decimal Pdv5 { get; set; }
        public decimal PoreznaOsnovica10 { get; set; }
        public decimal Pdv10 { get; set; }
        public decimal PoreznaOsnovica13 { get; set; }
        public decimal Pdv13 { get; set; }
        public decimal PoreznaOsnovica23 { get; set; }
        public decimal Pdv23 { get; set; }
        public decimal PoreznaOsnovica25 { get; set; }
        public decimal Pdv25 { get; set; }
        public decimal UkupniPdv { get; set; }
        public decimal UkupnoUplaceno { get; set; }
        public decimal PreostaloZaUplatit { get; set; }
        public string NapomenaORacunu { get; set; }
        public DateTime? ZaprimljenUHzzo { get; set; }
        public int DanaOdZaprimanja { get; set; }
        public int DanaNeplacanja { get; set; }
        private bool _knjizen;
        public bool Knjizen
        {
            get { return _knjizen; }
            set { SetProperty(ref _knjizen, value); }
        }
        public int TemeljnicaId { get; set; }
    }
}
