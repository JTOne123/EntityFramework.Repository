using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using DevOvercome.EntityFramework.Repository.Internals.Parameters.InternalHelpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevOvercome.EntityFramework.Repository.Internals.Parameters
{
	internal class FetchParameters<TModel> : QueryParameters<TModel>
		where TModel : class
	{
		/// <summary>
		/// Determines is loaded object excluded from context
		/// i.e. no point to load multiple records if they just for display.
		/// Default = true, for Fetch(), and False, for FetchOne
		/// </summary>
		public bool NoTracking { get; protected set; }

		internal List<Expression<Func<TModel, bool>>> FilteringRules { get; private set; }
		
		internal PagingRule PagingRule { get; private set; }
		internal List<SortingRule> SortingRules { get; private set; }

		public FetchParameters()
		{
			NoTracking = true;
			FilteringRules = new List<Expression<Func<TModel, bool>>>();
			PagingRule = new PagingRule() { PageIndex = 1, PageSize = int.MaxValue };
			SortingRules = new List<SortingRule>();
		}

		public FetchParameters(FetchParameters<TModel> model)
			:this()
		{
			FilteringRules = model.FilteringRules;
			PagingRule = model.PagingRule;
			SortingRules = model.SortingRules;
		}

		public FetchParameters<TModel> SetNoTracking(bool noTracking)
		{
			NoTracking = noTracking;
			return this;
		}

		public FetchParameters<TModel> AddFilter(Expression<Func<TModel, bool>> predicate)
		{
			FilteringRules.Add(predicate);
			return this;
		}

		// TODO: http://www.albahari.com/nutshell/predicatebuilder.aspx
		//public FetchParameters<TModel> AddFilterRangeAll<TProperty>(Expression<Func<TModel, TProperty>> selector, params TProperty[] values)
		//{

		//	foreach (string keyword in keywords)
		//	{
		//		string temp = keyword;
		//		query = query.Where(p => p.Description.Contains(temp));
		//	}
		//}

		//public FetchParameters<TModel> AddFilterRangeAny<TProperty>(Expression<Func<TModel, TProperty>> selector, params TProperty[] values)
		//{
		//	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
		//	// https://stackoverflow.com/a/13181067/3469518
		//}

		/// <summary>
		/// Can add only ONE sorting rule! Multiple rules will be added later.
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public FetchParameters<TModel> AddSorting(string key, SortDirectionEnum sortDirection = SortDirectionEnum.Asc)
		{
			if (SortingRules.Count > 0)
			{
				throw new NotSupportedException("Unable to make multple sorting, sorry. You have to define own separate repository and perform multiple sorting there by LINQ directly"); // TODO:
			}
			SortingRules.Add(new SortingRule() { Key = key, SortDirection = sortDirection });
			return this;
		}

		/// <summary>
		/// Can add only ONE sorting rule! Multiple rules will be added later.
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public FetchParameters<TModel> AddSorting<TProperty>(Expression<Func<TModel, TProperty>> selector, SortDirectionEnum sortDirection = SortDirectionEnum.Asc)
		{
			var name = FetchParameters_InternalHelpers.GetPropertyNameBySelector(selector);
			return AddSorting(name, sortDirection);
		}

		public FetchParameters<TModel> SetPaging(int pageIndex, int pageSize)
		{
			PagingRule = new PagingRule() { PageIndex = pageIndex, PageSize = pageSize };
			return this;
		}

		// We wont support this hell. Just use fluent api for base classes later than derived ones
		//// HACK: bad idea. every new XXXParameters supposed to do this over and over again in order to support fluent api
		//public new FetchParameters<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path)
		//{
		//	base.Include(path);
		//	return this;
		//}
	}
}
