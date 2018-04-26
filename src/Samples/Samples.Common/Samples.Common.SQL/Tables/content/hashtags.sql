CREATE TABLE [dbo].[hashtags]
(
	[Id] INT NOT NULL PRIMARY KEY,
	Name NVARCHAR(64) NOT NULL UNIQUE,
	--		AS Hitcount -- TODO: well...
)
