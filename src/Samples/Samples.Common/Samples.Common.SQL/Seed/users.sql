PRINT 'seed users BEGIN'

-- important if seed crashed.
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_tmp_insert_Lookup]') AND type in (N'P', N'PC'))
DROP PROCEDURE sp_tmp_insert_Lookup
GO

CREATE PROCEDURE sp_tmp_insert_Lookup (@Username nvarchar(64))
AS
BEGIN

	DECLARE @Password NVARCHAR(64) = 'Password'
	
	DECLARE @userID INT
	SELECT @userID = id 
	FROM [Users]
	WHERE Username = @Username

	IF @userID IS NULL
		INSERT INTO [Users](Username, Password)	
			VALUES	(@Username, @Password)
	ELSE
		UPDATE [Users] 
		SET Password = @Password -- let it be.
		WHERE id = @userID

END
GO

EXEC sp_tmp_insert_Lookup 'Chris'
EXEC sp_tmp_insert_Lookup 'Barry'
EXEC sp_tmp_insert_Lookup 'Jill'
EXEC sp_tmp_insert_Lookup 'Albert'
EXEC sp_tmp_insert_Lookup 'Rebeca'
EXEC sp_tmp_insert_Lookup 'Claire'
EXEC sp_tmp_insert_Lookup 'Leon'

EXEC sp_tmp_insert_Lookup 'Nemesis'
EXEC sp_tmp_insert_Lookup 'Umbrella'
EXEC sp_tmp_insert_Lookup 'Spencer'


DROP PROCEDURE sp_tmp_insert_Lookup
GO

PRINT 'seed users END'