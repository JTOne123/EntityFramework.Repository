using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Fetching;
using DevOvercome.EntityFramework.Repository.Internals.Parameters.Fetching;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Builders
{
	internal sealed class PagingFetchBuilder<TModel> : FetchBuilder<TModel>, IPagingFetchBuilder<TModel> 
		where TModel : class
	{
		private readonly FetchPagingParameters<TModel> fetchParameters;

		
		// this injection is fine since the design hides this behind internal
		internal PagingFetchBuilder(BasicRepository repository, FetchParameters<TModel> fetchParameters) : base(repository)
		{
			fetchParameters = new FetchPagingParameters<TModel>(fetchParameters);
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
