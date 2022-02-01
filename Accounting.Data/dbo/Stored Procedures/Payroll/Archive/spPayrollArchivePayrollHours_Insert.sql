CREATE PROCEDURE [dbo].[spPayrollArchivePayrollHours_Insert]
@Id int,
@Oib nvarchar(11),
@PayrollId int,
@TotalHours int,
@RegularHours int,
@SundayHours int,
@HolidayHours int,
@NightHours int,
@Overtime int,
@OvertimeSundayHours int,
@OvertimeHolidayHours int,
@OvertimeNightHours int,
@StandBy int,
@VacationCompensation int,
@SickDays int,
@SickDaysState int,
@SpecialHolidayCompensation int
AS
BEGIN
	INSERT INTO PayrollHours (Oib, PayrollId, TotalHours, RegularHours, SundayHours, HolidayHours, NightHours, Overtime,
	OvertimeSundayHours, OvertimeHolidayHours, OvertimeNightHours, StandBy, VacationCompensation, SickDays,
	SickDaysState, SpecialHolidayCompensation)
	VALUES (@Oib, @PayrollId, @TotalHours, @RegularHours, @SundayHours, @HolidayHours, @NightHours, @Overtime,
	@OvertimeSundayHours, @OvertimeHolidayHours, @OvertimeNightHours, @StandBy, @VacationCompensation, @SickDays,
	@SickDaysState, @SpecialHolidayCompensation)
END