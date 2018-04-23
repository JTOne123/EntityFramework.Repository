using System;
using System.Collections.Generic;

namespace DevOvercome.EntityFramework.Repository
{
	public class PagingResult<TModel>
		where TModel : class
	{
		public int TotalItemsCount { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get { return PageSize == 0 ? 0 : (int)Math.Ceiling((TotalItemsCount *1.0) / PageSize); } }
		public List<TModel> Items { get; set; }
	}
}
