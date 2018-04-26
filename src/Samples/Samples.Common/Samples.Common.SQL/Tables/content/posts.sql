CREATE TABLE [dbo].posts
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Post] TEXT NOT NULL, 
    [DatePosted] DATETIME NULL DEFAULT GETUTCDATE(), 
    [CategoryId] INT NOT NULL FOREIGN KEY REFERENCES [postCategories](Id),
	DayPosted AS CONVERT (date, DatePosted),
	Topic nvarchar(64) NOT NULL,
	AutorId INT NOT NULL FOREIGN KEY REFERENCES users(Id)
)

GO

CREATE INDEX [IX_posts_CategoryId] ON [dbo].posts ([CategoryId])
GO

CREATE INDEX [IX_posts_DayPosted] ON [dbo].posts (DayPosted) 
GO
