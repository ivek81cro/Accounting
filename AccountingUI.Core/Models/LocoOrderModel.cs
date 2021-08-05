using AccountingUI.Core.Validation;
using System;

namespace AccountingUI.Core.Models
{
    public class LocoOrderModel : ValidationBindableBase
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private int _startingKm;
        public int StartingKm
        {
            get { return _startingKm; }
            set { SetProperty(ref _startingKm, value); }
        }

        private int _finishKm;
        public int FinishKm
        {
            get { return _finishKm; }
            set { SetProperty(ref _finishKm, value); }
        }

        private string _destination;
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }

        private int _totalDistance;
        public int TotalDistance
        {
            get { return _totalDistance; }
            set { SetProperty(ref _totalDistance, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private int _calculationId;
        public int CalculationId
        {
            get { return _calculationId; }
            set { SetProperty(ref _calculationId, value); }
        }
    }
}
