PRINT 'seed postCategories BEGIN'

-- important if seed crashed.
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_tmp_insert_Lookup]') AND type in (N'P', N'PC'))
DROP PROCEDURE sp_tmp_insert_Lookup
GO

CREATE PROCEDURE sp_tmp_insert_Lookup (@Name nvarchar(64))
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM postCategories WHERE Name = @Name)
		INSERT INTO postCategories(Name) VALUES (@Name)

END
GO

EXEC sp_tmp_insert_Lookup 'Survival'
EXEC sp_tmp_insert_Lookup 'Shooting range'
EXEC sp_tmp_insert_Lookup 'Field expirience'
EXEC sp_tmp_insert_Lookup 'ZombieCount'

DROP PROCEDURE sp_tmp_insert_Lookup
GO

PRINT 'seed postCategories END'