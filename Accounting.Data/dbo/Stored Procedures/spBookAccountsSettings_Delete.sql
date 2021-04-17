CREATE PROCEDURE [dbo].[spBookAccountsSettings_Delete]
@Id int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM BookAccountSettings WHERE Id=@Id;
END