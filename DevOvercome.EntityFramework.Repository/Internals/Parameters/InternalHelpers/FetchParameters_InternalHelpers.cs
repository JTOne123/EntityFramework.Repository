using DevOvercome.EntityFramework.Repository.DataManipulationRules;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DevOvercome.EntityFramework.Repository.Internals.Parameters.InternalHelpers
{
	internal static class FetchParameters_InternalHelpers
	{
		internal static bool HasSorting<TModel>(this FetchParameters<TModel> target) where TModel : class
		{
			return target.SortingRules != null && target.SortingRules.Any(x => x != null);
		}

		internal static IQueryable<TModel> PerformFiltering<TModel>(this FetchParameters<TModel> target, IQueryable<TModel> queriedItems) where TModel : class
		{
			if (target.FilteringRules != null)
			{
				foreach (var filteringRule in target.FilteringRules.Where(x => x != null))
				{
					queriedItems = queriedItems.Where(filteringRule);
				}
			}
			return queriedItems;
		}

		internal static IQueryable<TModel> PerformSorting<TModel>(this FetchParameters<TModel> target, IQueryable<TModel> queriedItems) where TModel : class
		{
			if (target.HasSorting())
			{
				var rules = target.SortingRules.Where(x => x != null);
				if (!rules.Any())
				{
					return queriedItems; // sanity check
				}

				foreach (var sortingRule in rules)
				{
					queriedItems = queriedItems.OrderBy(sortingRule);
				}
			}
			return queriedItems;
		}

		public static bool HasKey<TSource>(TSource entity)
		{
			var entityType = typeof(TSource);

			foreach(var item in new string[] { "id", "ID", "Id" })
			{
				var property = entityType.GetProperty(item);
				if (property != null)
				{
					
					var value = property.GetValue(entity);
					if (value != null)
					{
						var id = 0;
						if (Int32.TryParse(value.ToString(), out id))
						{
							return id != 0;
						}
						else
						{
							return false;
						}
					} 
				}
			}
			return false;
			//Create x=>x.PropName
			
		}

		// https://stackoverflow.com/a/31959568/3469518
		private static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, SortingRule sortingRule)
		{
			var entityType = typeof(TSource);

			//Create x=>x.PropName
			var propertyInfo = entityType.GetProperty(sortingRule.Key);

			if (propertyInfo == null)
			{
				throw new ArgumentException(string.Format("Sorting rule - entity doesnt contain column ({0})"), sortingRule.Key);
			}
			ParameterExpression arg = Expression.Parameter(entityType, "x");
			MemberExpression property = Expression.Property(arg, sortingRule.Key);
			var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

			var desiredMethodName = sortingRule.SortDirection == SortDirectionEnum.Asc ? "OrderBy" : "OrderByDescending";

			//Get System.Linq.Queryable.OrderBy() method.
			var enumarableType = typeof(System.Linq.Queryable);
			var method = enumarableType.GetMethods()
				 .Where(m => m.Name == desiredMethodName && m.IsGenericMethodDefinition)
				 .Where(m =>
				 {
					 var parameters = m.GetParameters().ToList();
					 //Put more restriction here to ensure selecting the right overload                
					 return parameters.Count == 2;//overload that has 2 parameters
				 }).Single();
			//The linq's OrderBy<TSource, TKey> has two generic types, which provided here
			MethodInfo genericMethod = method
				 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

			/*Call query.OrderBy(selector), with query and selector: x=> x.PropName
			  Note that we pass the selector as Expression to the method and we don't compile it.
			  By doing so EF can extract "order by" columns and generate SQL for it.*/
			var newQuery = (IOrderedQueryable<TSource>)genericMethod
				 .Invoke(genericMethod, new object[] { query, selector });
			return newQuery;
		}


		internal static string GetPropertyNameBySelector<TModel, TProperty>(Expression<Func<TModel, TProperty>> propertyLambda)
		{
			// https://stackoverflow.com/a/672212/3469518
			{
				Type type = typeof(TModel);

				MemberExpression member = propertyLambda.Body as MemberExpression;
				if (member == null)
					throw new ArgumentException(string.Format(
						"Expression '{0}' refers to a method, not a property.",
						propertyLambda.ToString()));

				PropertyInfo propInfo = member.Member as PropertyInfo;
				if (propInfo == null)
					throw new ArgumentException(string.Format(
						"Expression '{0}' refers to a field, not a property.",
						propertyLambda.ToString()));

				if (type != propInfo.ReflectedType &&
					!type.IsSubclassOf(propInfo.ReflectedType))
					throw new ArgumentException(string.Format(
						"Expresion '{0}' refers to a property that is not from type {1}.",
						propertyLambda.ToString(),
						type));

				return propInfo.Name;
			}
		}
	}
}
