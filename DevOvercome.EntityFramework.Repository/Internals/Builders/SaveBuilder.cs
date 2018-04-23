using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals.Builders
{
	internal sealed class SaveBuilder : ISaveBuilder
	{
		private readonly BasicRepository repository;

		internal SaveBuilder(BasicRepository repository)
		{
			this.repository = repository;
		}

		public int Save()
		{
			var res = repository.SaveChanges();
			return res;
		}

		public async Task<int> SaveAsync()
		{
			return await repository.SaveChangesAsync();
		}
	}
}
