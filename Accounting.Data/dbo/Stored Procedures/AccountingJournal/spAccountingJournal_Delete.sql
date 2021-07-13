CREATE PROCEDURE [dbo].[spAccountingJournal_Delete]
@BrojTemeljnice int,
@VrstaTemeljnice nvarchar(125)
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM AccountingJournal WHERE BrojTemeljnice=@BrojTemeljnice AND VrstaTemeljnice=@VrstaTemeljnice;
END
