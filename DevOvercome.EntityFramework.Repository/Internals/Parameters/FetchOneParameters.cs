namespace DevOvercome.EntityFramework.Repository.Internals.Parameters
{
	// TODO: actually, paging not required here.
	public class FetchOneParameters<TModel> : FetchParameters<TModel> 
		where TModel : class
	{
		public bool ThrowIfNotFound { get; set; }
		public FetchOneParameters(FetchParameters<TModel> fetchParameters) : base(fetchParameters)
		{
			NoTracking = false;
		}
	}
}
