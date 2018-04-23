using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository
{
	public interface IFetchable<TModel>
		where TModel : class
	{
		/// <summary>
		/// Gets entity without tracking, by default
		/// </summary>
		/// <returns></returns>
		Task<PagingResult<TModel>> FetchPagingAsync();

		PagingResult<TModel> FetchPaging();

		Task<List<TModel>> FetchAsync();

		List<TModel> Fetch();

		/// <summary>
		/// Gets entity with tracking, by default
		/// </summary>
		/// <param name="throwIfNotFound"> First or FirstOrDefault. By default returns null </param>
		/// <returns></returns>
		Task<TModel> FetchOneAsync(bool throwIfNotFound = false);

		TModel FetchOne(bool throwIfNotFound = false);
	}

	public interface IFetchBuilder<TModel> : IFetchable<TModel>
		where TModel : class
	{
		IFetchBuilder<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path);
		IFetchBuilder<TModel> SetNoTracking(bool noTracking);
		IFetchBuilder<TModel> AddFilter(Expression<Func<TModel, bool>> predicate);
		IFetchBuilder<TModel> AddSorting(SortingRule sortingRule);
		IFetchBuilder<TModel> AddSorting(string key, SortDirectionEnum sortDirection = SortDirectionEnum.Asc);
		IFetchBuilder<TModel> AddSorting<TProperty>(Expression<Func<TModel, TProperty>> selector, SortDirectionEnum sortDirection = SortDirectionEnum.Asc);
		IFetchBuilder<TModel> SetPaging(int pageIndex, int pageSize);
		IFetchBuilder<TModel> SetPaging(PagingRule pagingRule);

		
	}
}
