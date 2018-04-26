CREATE TABLE [dbo].[Posts_hashtags]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PostId] INT NOT NULL FOREIGN KEY REFERENCES Posts(Id) ON DELETE CASCADE, 
    [HashtagId] INT NOT NULL FOREIGN KEY REFERENCES [Hashtags](id) ON DELETE CASCADE,
	CONSTRAINT [UQ_posts_hashtags] UNIQUE (PostId, HashtagId)
)
GO

CREATE INDEX [IX_posts_hashtags_Post] ON [dbo].[Posts_hashtags] ([PostId])
GO

CREATE INDEX [IX_posts_hashtags_Hashtag] ON [dbo].[Posts_hashtags] (HashtagId)
GO