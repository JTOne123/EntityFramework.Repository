using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using System;
using System.Linq.Expressions;

namespace DevOvercome.EntityFramework.Repository.Fetching
{

	public interface IFetchBuilder<TModel> : IFetchable<TModel>
		where TModel : class
	{
		IFetchBuilder<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path);
		IFetchBuilder<TModel> SetNoTracking(bool noTracking);
		IFetchBuilder<TModel> AddFilter(Expression<Func<TModel, bool>> predicate);
		IFetchBuilder<TModel> AddSorting(SortingRule sortingRule);
		IFetchBuilder<TModel> AddSorting(string key, SortDirectionEnum sortDirection = SortDirectionEnum.Asc);
		IFetchBuilder<TModel> AddSorting<TProperty>(Expression<Func<TModel, TProperty>> selector, SortDirectionEnum sortDirection = SortDirectionEnum.Asc);

		IPagingFetchBuilder<TModel> AsPagingBuilder();

	}
}
