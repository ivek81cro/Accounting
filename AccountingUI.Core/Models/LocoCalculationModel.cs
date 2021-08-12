using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class LocoCalculationModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private int _employeeId;
        public int EmployeeId
        {
            get { return _employeeId; }
            set { SetProperty(ref _employeeId, value); }
        }

        private string _vehicleMake;
        public string VehicleMake
        {
            get { return _vehicleMake; }
            set { SetProperty(ref _vehicleMake, value); }
        }

        private string _vehicleRegistration;
        public string VehicleRegistration
        {
            get { return _vehicleRegistration; }
            set { SetProperty(ref _vehicleRegistration, value); }
        }

        private DateTime? _dateOfCalculation;
        public DateTime? DateOfCalculation
        {
            get { return _dateOfCalculation; }
            set { SetProperty(ref _dateOfCalculation, value); }
        }

        private DateTime? _dateOfPayment;
        public DateTime? DateOfPayment
        {
            get { return _dateOfPayment; }
            set { SetProperty(ref _dateOfPayment, value); }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get { return _totalCost; }
            set { SetProperty(ref _totalCost, value); }
        }

        private bool _processed;
        public bool Processed
        {
            get { return _processed; }
            set { SetProperty(ref _processed, value); }
        }
    }
}
