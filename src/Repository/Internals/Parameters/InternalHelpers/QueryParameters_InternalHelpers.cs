using DevOvercome.EntityFramework.Repository.Internals.Utils;
using System.Data.Entity;
using System.Linq;

namespace DevOvercome.EntityFramework.Repository.Internals.Parameters.InternalHelpers
{
	internal static class QueryParameters_InternalHelpers
	{
		public static IQueryable<TModel> PerformIncludes<TModel>(this QueryParameters<TModel> target, IQueryable<TModel> items) where TModel : class
		{
			foreach (var expr in target.Includes.Where(x => x != null))
			{
				var path = "";
				if (DbHelpers.TryParsePath(expr.Body, out path))
				{
					items = items.Include(path);
				}
			}
			return items;
		}
	}
}
