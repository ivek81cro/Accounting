CREATE TABLE [dbo].[PayrollHours]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[Oib] nvarchar(11) not null,
	[PayrollId] int,
	[TotalHours] int,
	[RegularHours] int,
	[SundayHours] int,
	[HolidayHours] int,
	[NightHours] int,
	[Overtime] int,
	[OvertimeSundayHours] int,
	[OvertimeHolidayHours] int,
	[OvertimeNightHours] int,
	[StandBy] int,
	[VacationCompensation] int,
	[SickDays] int,
	[SickDaysState] int,
	[SpecialHolidayCompensation] int
)
