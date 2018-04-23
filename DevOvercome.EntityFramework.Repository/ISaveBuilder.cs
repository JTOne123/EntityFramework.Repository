using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository
{
	public interface ISaveBuilder
	{
		Task<int> SaveAsync();
		int Save();
	}
}
