USE [TemplateDB]
GO
/****** Object:  StoredProcedure [dbo].[spGetUsers]    Script Date: 5/4/2021 1:43:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetUsers]
	-- Add the parameters for the stored procedure here
	@PageSize INT = 10,
	@PageIndex INT = 1,
	@DateFrom DATETIME = NULL,
	@DateTo DATETIME = NULL,
	@Desc BIT = NULL,
	@SearchName NVARCHAR(100) = NULL,
	@SearchEmail NVARCHAR(100) = NULL,
	@SearchAddress NVARCHAR(100) = NULL,
	@Age INT = NULL,
	@FieldName NVARCHAR(30) = 'CreatedDate'
AS
BEGIN
	--- Check column name input if not exists then set default ---
	IF NOT EXISTS
		(SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = @FieldName)
		BEGIN
			SET @FieldName = 'CreatedDate'
		END

	--- Query order by ----
	DECLARE @SortOrder NVARCHAR(4) = NULL
	IF @Desc = 1
		BEGIN
			SET @SortOrder = ' DESC '
		END
	ELSE
		BEGIN
			SET @SortOrder = ' ASC '
		END

	DECLARE @SortQuery NVARCHAR(512) = CONCAT(' ORDER BY ', @FieldName, @SortOrder)

	--- Pagination ----
	DECLARE @PagingQuery NVARCHAR(512) = NULL

	IF @PageIndex IS NOT NULL AND @PageSize IS NOT NULL
		BEGIN
			SET @PagingQuery = CONCAT(' OFFSET ', @PageSize, ' * (', @PageIndex, ' - 1) ROWS FETCH NEXT ', @PageSize, ' ROWS ONLY ')
		END

	--- Where search name ---
	DECLARE @NameQuery NVARCHAR(512) = NULL
	IF @SearchName IS NOT NULL
		BEGIN
			SET @NameQuery = CONCAT(' AND [Users].[FullName] LIKE N''%', @SearchName, '''')
		END

	--- Where search email ---
	DECLARE @EmailQuery NVARCHAR(512) = NULL
	IF @SearchEmail IS NOT NULL
		BEGIN
			SET @EmailQuery = CONCAT(' AND [Users].[Email] LIKE ''%', @EmailQuery, '%''')
		END

	--- Where search address ---
	DECLARE @AddressQuery NVARCHAR(512) = NULL
	IF @SearchAddress IS NOT NULL
		BEGIN
			SET @AddressQuery = CONCAT(' AND [Users].[HomeAddress] LIKE ''%', @EmailQuery, '%''')
		END

	--- Where search age ---
	DECLARE @AgeQuery NVARCHAR(512) = NULL
	IF @Age IS NOT NULL
		BEGIN
			SET @AgeQuery = CONCAT(' AND [Users].[Age] = ', @Age)
		END


	--- Where search date from ---
	DECLARE @DateFromQuery NVARCHAR(600) = NULL
	IF @DateFrom IS NOT NULL
		BEGIN
			SET @DateFromQuery = CONCAT(' AND [Users].[CreatedDate] >= ', '''',@DateFrom, '''')
		END

		
	--- Where search date to ---
	DECLARE @DateToQuery NVARCHAR(600) = NULL
	IF @DateTo IS NOT NULL
		BEGIN
			SET @DateToQuery = CONCAT(' AND [Users].[CreatedDate] <= ', @DateTo)
		END
	
	DECLARE @SqlCommand NVARCHAR(1000);
		
	SET @SqlCommand = 
			'SELECT
				[Users].[Id]
				,[Users].FullName 
				,[Users].HomeAddress
				,[Users].Username
				,[Users].Email
				,[Users].CreatedDate
			FROM dbo.Users WHERE 1 = 1 '

	--- Concat where queries ---
	IF @AgeQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @AgeQuery
		END

	IF @NameQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @NameQuery
		END

	IF @EmailQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @EmailQuery
		END

	IF @AddressQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @AddressQuery
		END

	IF @DateToQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @DateToQuery
		END

	IF @DateFromQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = @SqlCommand + @DateFromQuery
		END

	--- Concat pagination
	IF @PagingQuery IS NOT NULL
		BEGIN
			SET @SqlCommand = ' WITH Main_CTE AS ( ' +  @SqlCommand + ')
									,Count_CTE AS ( SELECT COUNT(*) AS [TotalCount]
								FROM Main_CTE)
								SELECT * FROM Main_CTE, Count_CTE ' + @SortQuery + @PagingQuery
		END
	ELSE
		BEGIN
			SET @SqlCommand = ' WITH Main_CTE AS ( ' +  @SqlCommand + ')
									,Count_CTE AS ( SELECT COUNT(*) AS [TotalCount]
								FROM Main_CTE)
								SELECT * FROM Main_CTE, Count_CTE ' + @SortQuery
		END

	PRINT @SqlCommand

	EXEC (@SqlCommand)

END