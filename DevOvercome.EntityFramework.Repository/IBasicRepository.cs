namespace DevOvercome.EntityFramework.Repository
{
	public interface IBasicRepository
	{
		/// <summary>
		/// Make query settings via fluent api, then call .Query
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
			/// <returns></returns>
		IQueryBuilder<TModel> BuildQuery<TModel>() where TModel : class;

		/// <summary>
		/// Make fetch settings via fluent api, and then call .Fetch
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <returns></returns>
		IFetchBuilder<TModel> BuildFetch<TModel>() where TModel : class;

		/// <summary>
		/// If ID = 0 then adds a record. Otherwise - updates.
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="model"></param>
		/// <returns></returns>
		ISaveBuilder AddOrUpdateItem<TModel>(TModel model) where TModel : class;
		ISaveBuilder DeleteItem<TModel>(TModel model) where TModel : class;
		ISaveBuilder BuildSave();
	}

}
