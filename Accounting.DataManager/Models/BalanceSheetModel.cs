namespace Accounting.DataManager.Models
{
    public class BalanceSheetModel
    {
        public string Opis { get; set; }
        public string Konto { get; set; }
        public decimal Dugovna { get; set; }
        public decimal Potrazna { get; set; }
        public decimal Stanje { get; set; }
    }
}
