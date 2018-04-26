CREATE TABLE [dbo].[moderators]
(
	-- 1:0 relation. [users] record can exist w/o [moderators]. but not in reverse.
	[Id] INT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES users(Id),
	-- moderator can be assigned for specific topics
	[TargetCategoryId] INT NULL FOREIGN KEY REFERENCES [postCategories](Id),
	
)
