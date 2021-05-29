using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class BookUraRestModel : ValidationBindableBase
    {
        public int Id { get; set; }
        public int RedniBroj { get; set; }
        public DateTime Datum { get; set; }
        public string BrojRacuna { get; set; }
        public bool Storno { get; set; }
        public int StornoBroja { get; set; }
        public DateTime DatumRacuna { get; set; }
        public int StarostRacuna { get; set; }
        public DateTime Dospijece { get; set; }
        public decimal PlaniranaUplata { get; set; }
        public DateTime? DatumUplate { get; set; }
        public decimal ZaUplatu { get; set; }
        public string NazivDobavljaca { get; set; }
        public int BrojPrimke { get; set; }
        public string NapomenaORacunu { get; set; }
        public decimal NettoNabavnaVrijednost { get; set; }
        public string SjedisteDobavljaca { get; set; }
        public string OIB { get; set; }
        public decimal IznosSPorezom { get; set; }
        public decimal PoreznaOsnovica0 { get; set; }
        public decimal PoreznaOsnovica5 { get; set; }
        public decimal PretporezT5 { get; set; }
        public decimal PoreznaOsnovica10 { get; set; }
        public decimal PretporezT10 { get; set; }
        public decimal PoreznaOsnovica13 { get; set; }
        public decimal PretporezT13 { get; set; }
        public decimal PoreznaOsnovica23 { get; set; }
        public decimal PretporezT23 { get; set; }
        public decimal PoreznaOsnovica25 { get; set; }
        public decimal PretporezT25 { get; set; }
        public decimal UkupniPretporez { get; set; }
        public decimal MozeSeOdbiti { get; set; }
        public decimal NeMozeSeOdbiti { get; set; }
        public decimal IznosBezPoreza { get; set; }
        public decimal ProlaznaStavka { get; set; }
        public decimal Neoporezivo { get; set; }
        public decimal CassaScontoPercent { get; set; }
        public decimal CassaSconto { get; set; }
        public string BrojOdobrenja { get; set; }
        public string OdobrenjaBezPDV { get; set; }
        public decimal OdobreniPDV { get; set; }
        public DateTime? DatumPodnosenja { get; set; }
        public DateTime? DatumIzvrsenja { get; set; }
        public decimal UkupnoUplaceno { get; set; }
        public decimal PreostaloZaUplatit { get; set; }
        public int DospijeceDana { get; set; }
        private bool _knjizen;
        public bool Knjizen
        {
            get { return _knjizen; }
            set { SetProperty(ref _knjizen, value); }
        }
        public int TemeljnicaId { get; set; }
    }
}
