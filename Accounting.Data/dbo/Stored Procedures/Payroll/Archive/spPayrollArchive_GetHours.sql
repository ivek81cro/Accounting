CREATE PROCEDURE [dbo].[spPayrollArchive_GetHours]
@Id int
AS
	SELECT Oib, PayrollId, TotalHours, RegularHours, SundayHours, HolidayHours, NightHours, Overtime,
	OvertimeSundayHours, OvertimeHolidayHours, OvertimeNightHours, StandBy, VacationCompensation, SickDays,
	SickDaysState, SpecialHolidayCompensation
	FROM PayrollHours
	WHERE PayrollId=@Id;
RETURN 0
