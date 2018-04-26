CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(64) NOT NULL, 
	-- just an example, no encryption
    [Password] NVARCHAR(64) NOT NULL, 
    [DateRegistered] DATETIME NOT NULL DEFAULT GETUTCDATE()
)
