PRINT 'seed hashtags BEGIN'

-- important if seed crashed.
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_tmp_insert_Lookup]') AND type in (N'P', N'PC'))
DROP PROCEDURE sp_tmp_insert_Lookup
GO

CREATE PROCEDURE sp_tmp_insert_Lookup (@Name nvarchar(64))
AS
BEGIN

	IF NOT EXISTS (SELECT 1 FROM hashtags WHERE Name = @Name)
		INSERT INTO hashtags(Name) VALUES (@Name)

END
GO

EXEC sp_tmp_insert_Lookup 'Tip'
EXEC sp_tmp_insert_Lookup 'HeadShot'
EXEC sp_tmp_insert_Lookup 'Zombie swarm'
EXEC sp_tmp_insert_Lookup 'Bad'
EXEC sp_tmp_insert_Lookup 'dad'
EXEC sp_tmp_insert_Lookup 'company'
EXEC sp_tmp_insert_Lookup 'day'

EXEC sp_tmp_insert_Lookup 'HeadsUp'
EXEC sp_tmp_insert_Lookup 'WhereIsMyRocketLauncher'
EXEC sp_tmp_insert_Lookup 'Truth'
EXEC sp_tmp_insert_Lookup 'behind'
EXEC sp_tmp_insert_Lookup 'conspiracy'

DROP PROCEDURE sp_tmp_insert_Lookup
GO

PRINT 'seed hashtags END'