using System;

namespace Accounting.DataManager.Models
{
    public class LocoOrderModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int StartingKm { get; set; }

        public int FinishKm { get; set; }

        public string Destination { get; set; }

        public int TotalDistance { get; set; }

        public string Description { get; set; }

        public int CalculationId { get; set; }
    }
}
