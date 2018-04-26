namespace Samples.Common.CodeFirst
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class BlogsContext : DbContext
	{
		public BlogsContext()
			: base("name=BlogsContext")
		{
		}

		public virtual DbSet<Ban> Bans { get; set; }
		public virtual DbSet<Hashtag> Hashtags { get; set; }
		public virtual DbSet<Moderator> Moderators { get; set; }
		public virtual DbSet<PostCategory> PostCategories { get; set; }
		public virtual DbSet<Post> Posts { get; set; }
		public virtual DbSet<Posts_hashtags> Posts_hashtags { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PostCategory>()
				.HasMany(e => e.Moderators)
				.WithOptional(e => e.PostCategory)
				.HasForeignKey(e => e.TargetCategoryId);

			modelBuilder.Entity<PostCategory>()
				.HasMany(e => e.Posts)
				.WithRequired(e => e.PostCategory)
				.HasForeignKey(e => e.CategoryId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Post>()
				.Property(e => e.Post1)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.HasOptional(e => e.Moderator)
				.WithRequired(e => e.User);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Posts)
				.WithRequired(e => e.User)
				.HasForeignKey(e => e.AutorId)
				.WillCascadeOnDelete(false);
		}
	}
}
