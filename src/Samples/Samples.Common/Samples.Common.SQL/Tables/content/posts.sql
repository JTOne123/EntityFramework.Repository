CREATE TABLE [dbo].Posts
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Post] TEXT NOT NULL, 
    [DatePosted] DATETIME NULL DEFAULT GETUTCDATE(), 
    [CategoryId] INT NOT NULL FOREIGN KEY REFERENCES [PostCategories](Id),
	DayPosted AS CONVERT (date, DatePosted),
	Topic nvarchar(64) NOT NULL,
	AutorId INT NOT NULL FOREIGN KEY REFERENCES [Users](Id)
)

GO

CREATE INDEX [IX_posts_CategoryId] ON [dbo].Posts ([CategoryId])
GO

CREATE INDEX [IX_posts_DayPosted] ON [dbo].Posts (DayPosted) 
GO
