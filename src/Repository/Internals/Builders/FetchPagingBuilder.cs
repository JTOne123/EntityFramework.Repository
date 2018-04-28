using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Fetching;
using DevOvercome.EntityFramework.Repository.Internals.Parameters.Fetching;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Builders
{
	internal sealed class PagingFetchBuilder<TModel> : IPagingFetchBuilder<TModel> 
		where TModel : class
	{
		private readonly FetchPagingParameters<TModel> fetchParameters;
		private readonly BasicRepository repository;


		// this injection is fine since the design hides this behind internal
		internal PagingFetchBuilder(BasicRepository repository, FetchParameters<TModel> fetchParameters) 
		{
			if (fetchParameters == null)
			{
				throw new System.ArgumentNullException(nameof(fetchParameters));
			}

			this.fetchParameters = new FetchPagingParameters<TModel>(fetchParameters);
			this.repository = repository;
		}

		public IPagingFetchBuilder<TModel> SetPaging(int pageIndex, int pageSize)
		{
			fetchParameters.SetPaging(pageIndex, pageSize);
			return this;
		}

		public IPagingFetchBuilder<TModel> SetPaging(PagingRule pagingRule)
		{
			fetchParameters.SetPaging(pagingRule.PageIndex, pagingRule.PageSize);
			return this;
		}

		public PagingResult<TModel> FetchPaging()
		{
			return repository.FetchPaging(fetchParameters);
		}

		public Task<PagingResult<TModel>> FetchPagingAsync()
		{
			return repository.FetchPagingAsync(fetchParameters);
		}
	}
}
