namespace Accounting.DataManager.Models
{
    public class PayrollModel
    {
        public string Oib { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public decimal Bruto { get; set; }
        public decimal Mio1 { get; set; }
        public decimal Mio2 { get; set; }
        public decimal Dohodak { get; set; }
        public decimal Odbitak { get; set; }
        public decimal PoreznaOsnovica { get; set; }
        public decimal PoreznaStopa1 { get; set; }
        public decimal PoreznaStopa2 { get; set; }
        public decimal UkupnoPorez { get; set; }
        public decimal Prirez { get; set; }
        public decimal UkupnoPorezPrirez { get; set; }
        public decimal Neto { get; set; }
        public decimal DoprinosZdravstvo { get; set; }
    }
}
