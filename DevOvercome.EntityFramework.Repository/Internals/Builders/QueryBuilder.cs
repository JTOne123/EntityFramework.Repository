using DevOvercome.EntityFramework.Repository.Internals.Parameters;
using System;
using System.Linq;

namespace DevOvercome.EntityFramework.Repository.Internals.Builders
{
	internal sealed class QueryBuilder<TModel> : IQueryBuilder<TModel> where TModel : class
	{
		private readonly BasicRepository repository;
		private readonly QueryParameters<TModel> queryParameters = new QueryParameters<TModel>();

		// this injection is fine since the design hides this behind internal
		internal QueryBuilder(BasicRepository repository)
		{
			this.repository = repository;
		}

		public IQueryBuilder<TModel> Include<TProperty>(System.Linq.Expressions.Expression<Func<TModel, TProperty>> path)
		{
			queryParameters.Include(path);
			return this;
		}

		public IQueryable<TModel> Query()
		{
			return repository.Query(queryParameters);
		}
	}
}
