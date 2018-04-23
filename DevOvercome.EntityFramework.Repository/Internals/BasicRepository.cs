using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Internals.Builders;
using DevOvercome.EntityFramework.Repository.Internals.Parameters;
using DevOvercome.EntityFramework.Repository.Internals.Parameters.InternalHelpers;
using DevOvercome.EntityFramework.Repository.Internals.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DevOvercome.EntityFramework.Repository.Internals
{
	internal class BasicRepository : IBasicRepository
	{
		private readonly DbContext context;

		// HACK: ninject cant find internal ctor. okay since class is internal
		public BasicRepository(DbContext context)
		{

			this.context = context;
			// TODO:
			this.context.Database.Log = x => Trace.WriteLine(x);
		}

		public ISaveBuilder AddOrUpdateItem<TModel>(TModel model) where TModel : class
		{
			// http://www.michaelgmccarthy.com/2016/08/24/entity-framework-addorupdate-is-a-destructive-operation/#ef7coreaddorupdateismissingnowwhathintwriteyourown
			var entry = context.Entry(model);
			if (FetchParameters_InternalHelpers.HasKey(model))
			{
				entry.State = EntityState.Modified;
			}
			else
			{
				entry.State = EntityState.Added;
			}
			return BuildSave();
		}

		public ISaveBuilder BuildSave()
		{
			return new SaveBuilder(this);
		}

		public ISaveBuilder DeleteItem<TModel>(TModel model) where TModel : class
		{
			context.Entry(model).State = EntityState.Deleted;
			return BuildSave();
		}

		public IQueryBuilder<TModel> BuildQuery<TModel>() where TModel : class
		{
			return new QueryBuilder<TModel>(this);
		}

		public IFetchBuilder<TModel> BuildFetch<TModel>() where TModel : class
		{
			return new FetchBuilder<TModel>(this);
		}

		internal List<TModel> Fetch<TModel>(FetchParameters<TModel> fetchParameters)
			where TModel : class
		{
			return FormFetchQuery(fetchParameters).ToList();
		}

		internal Task<List<TModel>> FetchAsync<TModel>(FetchParameters<TModel> fetchParameters)
			where TModel : class
		{
			return FormFetchQuery(fetchParameters).ToListAsync();
		}

		internal Task<PagingResult<TModel>> FetchPagingAsync<TModel>(FetchParameters<TModel> fetchParameters) where TModel : class
		{
			var fq = FormFetchQuery(fetchParameters);
			// 4 Paging
			return PagingAsync(fq, fetchParameters.PagingRule);
		}

		internal PagingResult<TModel> FetchPaging<TModel>(FetchParameters<TModel> fetchParameters) where TModel : class
		{
			var fq = FormFetchQuery(fetchParameters);
			// 4 Paging
			return Paging(fq, fetchParameters.PagingRule);
		}

		private IQueryable<TModel> FormFetchQuery<TModel>(FetchParameters<TModel> fetchParameters) where TModel : class
		{

			Check.NotNull(fetchParameters, "fetchParameters");
			// !IMPORTANT! Call order DOES matter!
			// 1 Build Query
			var queriedItems = Query(fetchParameters);
			if (fetchParameters.NoTracking)
			{
				queriedItems = queriedItems.AsNoTracking();
			}

			// 2 Sorting  
			if (fetchParameters.HasSorting())
			{
				queriedItems = fetchParameters.PerformSorting(queriedItems);
			}
			else
			{
				// HACK: https://stackoverflow.com/a/14524571/3469518
				// this hack required for paging
				queriedItems = queriedItems.OrderBy(x => 0);
			}


			// 3 Filtering
			return fetchParameters.PerformFiltering(queriedItems);
		}

		private IQueryable<TModel> FormFetchOneQuery<TModel>(FetchOneParameters<TModel> fetchParameters) where TModel: class
		{
			Check.NotNull(fetchParameters, "fetchParameters");
			// !IMPORTANT! Call order DOES matter!
			// 1 Build Query
			var queriedItems = Query(fetchParameters);
			if (fetchParameters.NoTracking)
			{
				queriedItems = queriedItems.AsNoTracking();
			}
			// 2 Filtering
			queriedItems = fetchParameters.PerformFiltering(queriedItems);
			return queriedItems;
		}

		internal TModel FetchOne<TModel>(FetchOneParameters<TModel> fetchParameters) where TModel : class
		{
			var fq = FormFetchOneQuery(fetchParameters);
			// 3 Taking
			if (fetchParameters.ThrowIfNotFound)
			{
				// actually need to support 'single' but we dont use it.
				return fq.First();
			}
			else
			{
				return fq.FirstOrDefault();
			}

		}

		internal Task<TModel> FetchOneAsync<TModel>(FetchOneParameters<TModel> fetchParameters) where TModel : class
		{
			var fq = FormFetchOneQuery(fetchParameters);
			// 3 Taking
			if (fetchParameters.ThrowIfNotFound)
			{
				// TODO: actually need to support 'single' but we dont use it.
				return fq.FirstAsync();
			}
			else
			{
				return fq.FirstOrDefaultAsync();
			}


		}

		internal IQueryable<TModel> Query<TModel>(QueryParameters<TModel> parameters) where TModel : class
		{
			var set = context.Set<TModel>().AsQueryable();
			if (parameters != null)
			{
				set = parameters.PerformIncludes(set);
			}

			return set;
		}

		internal Task<int> SaveChangesAsync()
		{
			return context.SaveChangesAsync();
		}

	

		private IQueryable<TModel> FormPagingQuery<TModel>(IQueryable<TModel> items, PagingRule paging, out int total)
			where TModel : class
		{
			Check.NotNull(items, "items");

			total = items.Count();

			// unload everything if paging not defined.
			paging.PageSize = paging.PageSize <= 0 ?
				total
				: paging.PageSize;

			if ((paging.PageIndex - 1) * paging.PageSize >= total)
			{
				paging.PageSize = paging.PageSize == 0 ? int.MaxValue : paging.PageSize;
				paging.PageIndex = (total / paging.PageSize) - 1;
			}

			if (paging.PageIndex <= 0)
			{
				paging.PageIndex = 1;
			}

			var result = items
				.Skip(paging.PageSize * (paging.PageIndex - 1))
				.Take(paging.PageSize);

			return result;
		}

		// TODO: doublecheck deadlocks
		internal async Task<PagingResult<TModel>> PagingAsync<TModel>(IQueryable<TModel> items, PagingRule paging) where TModel : class
		{
			var total = 0;
			var result = await FormPagingQuery(items, paging, out total).ToListAsync().ConfigureAwait(false);
			var res = new PagingResult<TModel>()
			{
				Items = result,
				PageIndex = paging.PageIndex,
				PageSize = paging.PageSize,
				TotalItemsCount = total
			};
			if (res.Items == null)
			{
				// just in case
				res.Items = new List<TModel>();
			}
			return res;
		}

	

		internal int SaveChanges()
		{
			return context.SaveChanges();
		}

		internal PagingResult<TModel> Paging<TModel>(IQueryable<TModel> items, PagingRule paging) where TModel : class
		{
			var total = 0;
			var result = FormPagingQuery(items, paging, out total).ToList();
			var res = new PagingResult<TModel>()
			{
				Items = result,
				PageIndex = paging.PageIndex,
				PageSize = paging.PageSize,
				TotalItemsCount = total
			};
			if (res.Items == null)
			{
				// just in case
				res.Items = new List<TModel>();
			}
			return res;
		}
	}

}
