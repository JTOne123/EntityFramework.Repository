using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Fetching;
using DevOvercome.EntityFramework.Repository.Internals.Parameters;
using DevOvercome.EntityFramework.Repository.Internals.Parameters.Fetching;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Builders
{
	internal class FetchBuilder<TModel> 
		: IFetchBuilder<TModel> where TModel : class
	{
		private readonly FetchParameters<TModel> fetchParameters = new FetchParameters<TModel>();

		protected readonly BasicRepository repository;

		// this injection is fine since the design hides this behind internal
		internal FetchBuilder(BasicRepository repository)
		{
			this.repository = repository;
		}

		public IFetchBuilder<TModel> AddFilter(Expression<Func<TModel, bool>> predicate)
		{
			fetchParameters.AddFilter(predicate);
			return this;
		}

		public IFetchBuilder<TModel> AddSorting(string key, SortDirectionEnum sortDirection = SortDirectionEnum.Asc)
		{
			fetchParameters.AddSorting(key, sortDirection);
			return this;
		}

		public IFetchBuilder<TModel> AddSorting<TProperty>(Expression<Func<TModel, TProperty>> selector, SortDirectionEnum sortDirection = SortDirectionEnum.Asc)
		{
			fetchParameters.AddSorting(selector, sortDirection);
			return this;
		}

		public IFetchBuilder<TModel> AddSorting(SortingRule sortingRule)
		{
			fetchParameters.AddSorting(sortingRule.Key, sortingRule.SortDirection);
			return this;
		}

		

		public TModel FetchOne(bool throwIfNotFound = false)
		{
			var parameters = new FetchOneParameters<TModel>(fetchParameters)
			{
				ThrowIfNotFound = throwIfNotFound
			};
			return repository.FetchOne(parameters);
		}

		public Task<TModel> FetchOneAsync(bool throwIfNotFound = false)
		{
			
			var parameters = new FetchOneParameters<TModel>(fetchParameters)
			{
				ThrowIfNotFound = throwIfNotFound
			};
			return repository.FetchOneAsync(parameters);
		}

		public IFetchBuilder<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path)
		{
			fetchParameters.Include(path);
			return this;
		}

		public IFetchBuilder<TModel> SetNoTracking(bool noTracking)
		{
			fetchParameters.SetNoTracking(noTracking);
			return this;
		}
	

		public Task<List<TModel>> FetchAsync()
		{
			return repository.FetchAsync(fetchParameters);
		}

		public List<TModel> Fetch()
		{
			return repository.Fetch(fetchParameters);
		}

		public IPagingFetchBuilder<TModel> AsPagingBuilder()
		{
			return new PagingFetchBuilder<TModel>(repository, fetchParameters);
		}
	}
}
