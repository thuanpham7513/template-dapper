-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetUsers]
	-- Add the parameters for the stored procedure here
	@PageSize INT = NULL,
	@PageIndex INT = NULL,
	@SelectColumns nvarchar(200) = NULL,
	@SearchColumn nvarchar(50) = NULL,
	@SortOrder NVARCHAR(20) = 'ASC',
	@SearchText NVARCHAR(100) = NULL,
	@Total INT OUTPUT
AS
BEGIN
	
	SELECT @Total = COUNT(*) FROM dbo.Users;
	
	DECLARE @SqlCommand NVARCHAR(1000);
	SET @SearchText = LTRIM(RTRIM(@SearchText))
		
	SET @SqlCommand = 
		'SELECT' + @SelectColumns + '
		 FROM dbo.Users '

	IF(@SearchText IS NOT NULL AND @SearchColumn Is NOT NULL)
	BEGIN
		SET @SqlCommand = CONCAT(@SqlCommand, 'WHERE', @SearchColumn, 'LIKE N''%', @SearchText, '''') 
	END

	SET @SqlCommand = CONCAT(@SqlCommand, ' ORDER BY Id ', @SortOrder)

	IF(@PageSize IS NOT NULL AND @PageSize >= 1 
		AND @PageSize IS NOT NULL AND @PageIndex >= 1)
	BEGIN
		SET @SqlCommand = CONCAT(@SqlCommand, ' OFFSET ', (@PageIndex - 1) * @PageSize, ' ROWS ', 'FETCH NEXT ', @PageSize, ' ROWS ONLY');
	END

	EXEC (@SqlCommand)

END