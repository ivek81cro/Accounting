CREATE PROCEDURE [dbo].[spPartners_Delete]
@id int
AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE FROM Partners
	WHERE Id=@id;

END
