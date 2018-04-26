CREATE TABLE [dbo].[posts_hashtags]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PostId] INT NOT NULL FOREIGN KEY REFERENCES posts(Id) ON DELETE CASCADE, 
    [HashtagId] INT NOT NULL FOREIGN KEY REFERENCES hashtags(id) ON DELETE CASCADE,
	CONSTRAINT [UQ_posts_hashtags] UNIQUE (PostId, HashtagId)
)
GO

CREATE INDEX [IX_posts_hashtags_Post] ON [dbo].[posts_hashtags] ([PostId])
GO

CREATE INDEX [IX_posts_hashtags_Hashtag] ON [dbo].[posts_hashtags] (HashtagId)
GO