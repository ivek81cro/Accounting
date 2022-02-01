using AccountingUI.Core.Validation;

namespace AccountingUI.Core.Models
{
    public class PayrollHours : ValidationBindableBase
    {

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _oib;
        public string Oib
        {
            get => _oib;
            set => SetProperty(ref _oib, value);
        }

        private int _payrollId;
        public int PayrollId
        {
            get => _payrollId;
            set => SetProperty(ref _payrollId, value);
        }

        private int _totalHours;
        public int TotalHours
        {
            get => _totalHours;
            set => SetProperty(ref _totalHours, value);
        }

        private int _regularHours = 0;
        public int RegularHours
        {
            get => _regularHours;
            set => SetProperty(ref _regularHours, value);
        }

        private int _sundayHours = 0;
        public int SundayHours
        {
            get => _sundayHours;
            set => SetProperty(ref _sundayHours, value);
        }

        private int _holidayHours = 0;
        public int HolidayHours
        {
            get => _holidayHours;
            set => SetProperty(ref _holidayHours, value);
        }

        private int _nightHours = 0;
        public int NightHours
        {
            get => _nightHours;
            set => SetProperty(ref _nightHours, value);
        }

        private int _overtime = 0;
        public int Overtime
        {
            get => _overtime;
            set => SetProperty(ref _overtime, value);
        }

        private int _overtimeSundayHours = 0;
        public int OvertimeSundayHours
        {
            get => _overtimeSundayHours;
            set => SetProperty(ref _overtimeSundayHours, value);
        }

        private int __overtimeHolidayHours = 0;
        public int OvertimeHolidayHours
        {
            get => __overtimeHolidayHours;
            set => SetProperty(ref __overtimeHolidayHours, value);
        }
        
        private int _overtimeNightHours = 0;
        public int OvertimeNightHours
        {
            get => _overtimeNightHours;
            set => SetProperty(ref _overtimeNightHours, value);
        }

        private int _standBy = 0;
        public int StandBy
        {
            get => _standBy;
            set => SetProperty(ref _standBy, value);
        }

        private int _vacationCompensation = 0;
        public int VacationCompensation
        {
            get => _vacationCompensation;
            set => SetProperty(ref _vacationCompensation, value);
        }

        private int _sickDays = 0;
        public int SickDays
        {
            get => _sickDays;
            set => SetProperty(ref _sickDays, value);
        }

        private int _sickDaysState = 0;
        public int SickDaysState
        {
            get => _sickDaysState;
            set => SetProperty(ref _sickDaysState, value);
        }

        private int _specialHolidayCompensation = 0;
        public int SpecialHolidayCompensation
        {
            get => _specialHolidayCompensation;
            set => SetProperty(ref _specialHolidayCompensation, value);
        }
    }
}
