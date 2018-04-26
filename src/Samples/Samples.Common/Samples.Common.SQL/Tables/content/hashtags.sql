CREATE TABLE [dbo].[Hashtags]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(64) NOT NULL UNIQUE,
	-- DateCreate DateModified
	--		AS Hitcount -- TODO: well...
)
