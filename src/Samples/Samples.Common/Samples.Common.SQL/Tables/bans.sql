CREATE TABLE [dbo].[bans]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL FOREIGN KEY REFERENCES users(Id) ON DELETE CASCADE, 
    [ModeratorId] INT NULL FOREIGN KEY REFERENCES moderators(Id) ON DELETE SET NULL, 
    [DateBanned] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
	-- when ban expired. 
    [DaysBanned] INT NOT NULL,
	CONSTRAINT CK_DaysBanned_GR_Zero CHECK(DaysBanned > 0)
	-- gotta check moderator on insert but no.
	

)
