using System;
using System.Linq;
using System.Linq.Expressions;

namespace DevOvercome.EntityFramework.Repository
{
	public interface IQueryBuilder<TModel>
		where TModel : class
	{
		IQueryBuilder<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path);
		IQueryable<TModel> Query();
	}
}
