using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Fetching
{
	public interface IPagingFetchable<TModel> 
		where TModel : class
	{
		Task<PagingResult<TModel>> FetchPagingAsync();
		PagingResult<TModel> FetchPaging();
	}
}
