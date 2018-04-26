using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Fetching
{

	public interface IFetchable<TModel>
		where TModel : class
	{
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
}
