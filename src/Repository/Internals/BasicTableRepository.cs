using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Fetching;
using System.Linq;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals
{
	internal class BasicTableRepository<TModel> : IBasicTableRepository<TModel>
		where TModel : class
	{
		private readonly BasicRepository repository;

		// HACK: ninject cant find internal ctor. okay since class is internal
		public BasicTableRepository(BasicRepository repository)
		{
			this.repository = repository;
		}

		public IFetchBuilder<TModel> BuildFetch()
		{
			return repository.BuildFetch<TModel>();
		}

		public IQueryBuilder<TModel> BuildQuery()
		{
			return repository.BuildQuery<TModel>();
		}

		public ISaveBuilder DeleteItem(TModel model)
		{
			return repository.DeleteItem(model);
		}

		public Task<PagingResult<TModel>> PagingAsync(IQueryable<TModel> items, PagingRule paging)
		{
			return repository.PagingAsync(items, paging);
		}

		public Task<int> SaveChangesAsync()
		{
			return repository.SaveChangesAsync();
		}

		public ISaveBuilder AddOrUpdateItem(TModel model)
		{
			return repository.AddOrUpdateItem(model);
		}

		public ISaveBuilder BuildSave()
		{
			return repository.BuildSave();
		}
	}

}
