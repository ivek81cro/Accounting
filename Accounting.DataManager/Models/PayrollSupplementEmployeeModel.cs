namespace Accounting.DataManager.Models
{
    public class PayrollSupplementEmployeeModel : PayrollSupplementModel
    {
        public int Id { get; set; }
        public string Oib { get; set; }
        public decimal Iznos { get; set; }

    }
}
