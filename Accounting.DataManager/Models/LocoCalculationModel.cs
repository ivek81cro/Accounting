using System;

namespace Accounting.DataManager.Models
{
    public class LocoCalculationModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleRegistration { get; set; }

        public DateTime? DateOfCalculation { get; set; }

        public DateTime? DateOfPayment { get; set; }

        public decimal TotalCost { get; set; }

        public bool Processed { get; set; }
    }
}
