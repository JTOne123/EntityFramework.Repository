using DevOvercome.EntityFramework.Repository;
using DevOvercome.EntityFramework.Repository.Internals;
using System;
using System.Data.Entity;

namespace DevOvercome.EntityFramework.Repository
{
	/// <summary>
	/// since implementations are internal, just in case if your IoC cant locate internals.
	/// </summary>
	public class RepositoryFactory
    {
		protected readonly DbContext context;

		public RepositoryFactory(DbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		// yay! extention points!
		public virtual IBasicRepository CreateBasicRepository()
		{
			return new BasicRepository(context);
		}

		public virtual IBasicTableRepository<TModel> CreateBasicTableRepository<TModel>()
			where TModel : class
		{
			return new BasicTableRepository<TModel>(new BasicRepository(context));
		}
    }
}
