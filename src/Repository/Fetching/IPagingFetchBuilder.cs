using DevOvercome.EntityFramework.Repository.DataManipulationRules;

namespace DevOvercome.EntityFramework.Repository.Fetching
{
	public interface IPagingFetchBuilder<TModel> : IPagingFetchable<TModel>
		where TModel : class
	{
		IPagingFetchBuilder<TModel> SetPaging(int pageIndex, int pageSize);
		IPagingFetchBuilder<TModel> SetPaging(PagingRule pagingRule);
	}
}
