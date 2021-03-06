using DevOvercome.EntityFramework.Repository.Fetching;
using System.Linq;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository
{
	public interface IBasicTableRepository<TModel>
		where TModel : class
	{
		IQueryBuilder<TModel> BuildQuery();
		IFetchBuilder<TModel> BuildFetch();
		ISaveBuilder AddOrUpdateItem(TModel model);
		ISaveBuilder AddItem(TModel model);
		ISaveBuilder UpdateItem(TModel model);
		ISaveBuilder DeleteItem(TModel model);
		ISaveBuilder BuildSave();
	}

}
