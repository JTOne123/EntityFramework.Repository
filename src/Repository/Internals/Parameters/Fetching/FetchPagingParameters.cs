using DevOvercome.EntityFramework.Repository.DataManipulationRules;

namespace DevOvercome.EntityFramework.Repository.Internals.Parameters.Fetching
{
	internal class FetchPagingParameters<TModel> : FetchParameters<TModel>
		where TModel : class
	{
		internal PagingRule PagingRule { get; private set; }

		public FetchPagingParameters(FetchParameters<TModel>model): base(model)
		{
			PagingRule = new PagingRule() { PageIndex = 1, PageSize = int.MaxValue };
		}

		public FetchParameters<TModel> SetPaging(int pageIndex, int pageSize)
		{
			PagingRule = new PagingRule() { PageIndex = pageIndex, PageSize = pageSize };
			return this;
		}
	}
}
