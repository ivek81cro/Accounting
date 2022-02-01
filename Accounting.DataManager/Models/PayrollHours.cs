namespace Accounting.DataManager.Models
{
    public class PayrollHours
    {
        public int Id { get; set; }
        public string Oib { get; set; }
        public int PayrollId { get; set; }
        public int TotalHours { get; set; }
        public int RegularHours { get; set; }
        public int SundayHours { get; set; }
        public int HolidayHours { get; set; }
        public int NightHours { get; set; }
        public int Overtime { get; set; }
        public int OvertimeSundayHours { get; set; }
        public int OvertimeHolidayHours { get; set; }
        public int OvertimeNightHours { get; set; }
        public int StandBy { get; set; }
        public int VacationCompensation { get; set; }
        public int SickDays { get; set; }
        public int SpecialHolidayCompensation { get; set; }
        public int SickDaysState { get; set; }
    }
}
