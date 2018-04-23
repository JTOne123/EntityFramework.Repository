using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevOvercome.EntityFramework.Repository.Internals.Parameters
{
	public class QueryParameters<TModel>
		where TModel : class
	{
		private readonly List<LambdaExpression> lambdaExpressions = new List<LambdaExpression>();
		internal List<LambdaExpression> Includes { get { return lambdaExpressions; } }

		
		public QueryParameters<TModel> Include<TProperty>(Expression<Func<TModel, TProperty>> path)
		{
			lambdaExpressions.Add(path);
			return this;
		}
	}
}